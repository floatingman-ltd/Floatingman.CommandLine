using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Floatingman.CommandLineParser
{
  public sealed class CommandLine : ICommandLine
  {

    private static readonly Lazy<CommandLine> _instance =
        new Lazy<CommandLine>(() => new CommandLine());

    public static CommandLine Instance { get; } = _instance.Value;

    private CommandLine()
    {
    }

    // can we convertthis to a Lazy<T>?
    private IDictionary<Type, Func<string, object>> _coersionMap;

    private IDictionary<Type, Func<string, object>> CoersionMap
    {
      get
      {
        return _coersionMap ?? (_coersionMap = new Dictionary<Type, Func<string, object>>
                {
                    {typeof(int), v => int.Parse(v)},
                    {typeof(double), v => double.Parse(v)},
                    {typeof(string), v => v},
                    // we are only supporting arrays as the collection type
                    {typeof(int[]), v => v.Split(',').Select(i => int.Parse(i)).ToArray()},
                    {typeof(double[]), v => v.Split(',').Select(double.Parse).ToArray()},
                    {typeof(string[]), v => v.Split(',').Select(i => i).ToArray()},
                });
      }
    }

    public TArgs Parse<TArgs>(string[] args) where TArgs : ICommandArgs, new()
    {
      var parameters = new TArgs();
      // is there a command
      var command = GetCommandFromAttributes(parameters);

      // this gives us a list of (property,option) tuples
      var options = GetOptionsFromAttributes(parameters);

      // this gives us a list of (property,argument) tuples
      var arguments = GetArgumentsFromAttributes(parameters);

      // tokenize the args, a token is going to consist of an option or an argument
      (PropertyInfo Property, OptionAttribute Option) option = (null, null);
      bool isValue = false;
      foreach (var arg in args)
      {
        if (command.Command != null && !command.Command.IsSet)
        {
          parameters.Command = arg;
          command.Command.IsSet = true;
        }
        else if (isValue)
        {
          // coerce the value into the right type
          try
          {
            // how do we coerce a multi-parameter into an array?
            if (option.Property.PropertyType.IsArray && option.Option.IsSet)
            {
              var value = option.Property.GetValue(parameters);
              var ArrayMap = new Dictionary<Type, Func<string, object, object>>{
                    {typeof(double[]), (v,e) => {
                       var x = ((double[])e).ToList();
                       x.Add(double.Parse(v));
                       return x.ToArray();
                    }},
                    {typeof(string[]), (v,e) => {
                       var x = ((string[])e).ToList();
                       x.Add(v);
                       return x.ToArray();
                    }}
                    };
              // we have an array property and an existing value let's use something like the coersion map again
              option.Property.SetValue(parameters, ArrayMap[option.Property.PropertyType](arg, value));
            }
            else
            {
              option.Property.SetValue(parameters, CoersionMap[option.Property.PropertyType](arg));
            }
            option.Option.IsSet = true;

          }
          catch (FormatException)
          {
            // two problems here:
            // 1) The deferred execution of the linq means that the exception is not
            //    thrown until the values are examined; this means that the ienumerable
            //    of values would explode on first use in the application, outside of the
            //    commandline tooling; that would be a frustrating error to find. It would
            //    be best to ToList or ToArray the value before we exit the lamdba.
            // 2) For values like "1.21,3.14" being converted to a List<int> will only
            //    explode on the first exception, while in reality I'd like to see both
            //    messages in my list of errors.
            parameters.Errors.Add($"[{arg}] cannot be converted to <{option.Property.PropertyType}>");
          }

          isValue = false;
        }
        else
        {
          if (arg.StartsWith("-")) // this is an option
          {
            var longForm = arg.StartsWith("--");
            var token = arg.TrimStart('-');
            // this explodes if an argument is passed that is not in the list of options
            if (!options.Any(p => p.Option.LongForm == token || p.Option.ShortForm.ToString() == token))
            {
              parameters.Errors.Add($"{token} is not recognized");
            }
            else
            {
              option = longForm
                 ? options.Single(p => p.Option.LongForm == token)
                 // are compound options allowable? like -rm?
                 : options.Single(p => p.Option.ShortForm.ToString() == token);
              if (option.Option.IsSet && !option.Option.AllowMultiple) parameters.Errors.Add($"{option.Option.LongForm ?? option.Option.ShortForm.ToString()} is duplicated");
              if (option.Property.PropertyType != typeof(bool))
              {
                // possibly split by [':'. '=']
                isValue = true;
              }
              else
              {
                // what about setting a flag to false with a trailing -
                // what about setting a flag to true (explicitly) with a trailing +
                option.Property.SetValue(parameters, true);
                // we set the `IsSet` here because there is no value
                option.Option.IsSet = true;
              }
            }
          }
          else // this is a positional argument
          {
            // get the first not set positional
            // this will explode if there is not positional argument to put this in
            var (propertyInfo, argumentAttribute) = arguments.First(a => !a.Argument.IsSet);
            propertyInfo.SetValue(parameters, CoersionMap[propertyInfo.PropertyType](arg));
            argumentAttribute.IsSet = true;
          }
        }
      }

      // rather then just exploding it would be nice to form a list of reasons to explode, and then explode
      // test for required options
      parameters.Errors.AddRange(options.Where(o => !o.Option.IsSet && o.Option.IsRequired && o.Property.PropertyType != typeof(bool)).Select(t => $"{t.Option.Name} is required, but not set"));

      // set default values
      foreach (var (propertyInfo, optionAttribute) in options.Where(o => !o.Option.IsSet && o.Option.Default != null))
        propertyInfo.SetValue(parameters, optionAttribute.Default);

      // test for required arguments
      if (arguments.Any(o => !o.Argument.IsSet && o.Argument.IsRequired && o.Property.PropertyType != typeof(bool)))
        throw new Exception("kaboom?");

      // set default values
      foreach (var (propertyInfo, argumentAttribute) in arguments.Where(o => !o.Argument.IsSet && o.Argument.Default != null))
        propertyInfo.SetValue(parameters, argumentAttribute.Default);

      return parameters;
    }

    private List<(PropertyInfo Property, ArgumentAttribute Argument)> GetArgumentsFromAttributes<TArgs>(TArgs parameters) =>
        parameters
            .GetType()
            .GetProperties()
            .Where(p => p.GetCustomAttributes(true).OfType<ArgumentAttribute>().Any())
            .Select(p => (Property: p, Argument: p.GetCustomAttributes(true).OfType<ArgumentAttribute>().Single()))
            .ToList();

    private (Type Class, CommandAttribute Command) GetCommandFromAttributes<TArgs>(TArgs parameters) =>
        parameters
            .GetType()
            .GetCustomAttributes(true)
            .OfType<CommandAttribute>()
            .Select(c => (Class: parameters.GetType(), Command: c))
            .SingleOrDefault();

    private List<(PropertyInfo Property, OptionAttribute Option)> GetOptionsFromAttributes<TArgs>(TArgs parameters) =>
        parameters
            .GetType()
            .GetProperties()
            .Where(p => p.GetCustomAttributes(true).OfType<OptionAttribute>().Any())
            .Select(p => (Property: p, Option: p.GetCustomAttributes(true).OfType<OptionAttribute>().Single()))
            .ToList();
  }
}
