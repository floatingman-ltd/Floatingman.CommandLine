using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CaseWare.CommandLineParser
{
    public class CommandLine<TArgs> : ICommandLine<TArgs> where TArgs : CommandArgs, new()
    {
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
                    // we are only supporting IEnumerable<> as the collection type
                    {typeof(int[]), v => v.Split(',').Select(i => int.Parse(i)).ToArray()},
                    {typeof(double[]), v => v.Split(',').Select(double.Parse).ToArray()},
                    {typeof(string[]), v => v.Split(',').Select(i => i).ToArray()},
                });
            }
        }

        public TArgs Parse(string[] args)
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
                        option.Property.SetValue(parameters, CoersionMap[option.Property.PropertyType](arg));
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
                            if (option.Option.IsSet) parameters.Errors.Add($"{token} is duplicated");
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
                                option.Option.IsSet = true;
                            }
                        }
                    }
                    else // this is a positional argument
                    {
                        // get the first not set positional
                        // this will explode if there is not positional argument to put this in
                        var positional = arguments.First(a => !a.Argument.IsSet);
                        positional.Property.SetValue(parameters, CoersionMap[positional.Property.PropertyType](arg));
                        positional.Argument.IsSet = true;
                    }
                }
            }
            // we've gone through the args and set what we could - let's look for the required and default values
            // rather then just exploding it would be nice to form a list of reasons to explode, and then explode
            parameters.Errors.AddRange(options.Where(o => !o.Option.IsSet && o.Option.IsRequired && o.Property.PropertyType != typeof(bool)).Select(t => $"{t.Option.Name} is required, but not set"));
            foreach (var optionWithDefault in options.Where(o => !o.Option.IsSet && o.Option.Default != null)) optionWithDefault.Property.SetValue(parameters, optionWithDefault.Option.Default);

            foreach (var argumentsWithRequired in arguments.Where(o => !o.Argument.IsSet && o.Argument.IsRequired && o.Property.PropertyType != typeof(bool))) throw new Exception("kaboom?");
            foreach (var argumentsWithDefault in arguments.Where(o => !o.Argument.IsSet && o.Argument.Default != null)) argumentsWithDefault.Property.SetValue(parameters, argumentsWithDefault.Argument.Default);

            return parameters;
        }

        private List<(PropertyInfo Property, ArgumentAttribute Argument)> GetArgumentsFromAttributes(TArgs parameters) =>
            parameters
                .GetType()
                .GetProperties()
                .Where(p => p.GetCustomAttributes(true).OfType<ArgumentAttribute>().Any())
                .Select(p => (Property: p, Argument: p.GetCustomAttributes(true).OfType<ArgumentAttribute>().Single()))
                .ToList();

        private (Type Class, CommandAttribute Command) GetCommandFromAttributes(TArgs parameters) =>
            parameters
                .GetType()
                .GetCustomAttributes(true)
                .OfType<CommandAttribute>()
                .Select(c => (Class: parameters.GetType(), Command: c))
                .SingleOrDefault();

        private List<(PropertyInfo Property, OptionAttribute Option)> GetOptionsFromAttributes(TArgs parameters) =>
            parameters
                .GetType()
                .GetProperties()
                .Where(p => p.GetCustomAttributes(true).OfType<OptionAttribute>().Any())
                .Select(p => (Property: p, Option: p.GetCustomAttributes(true).OfType<OptionAttribute>().Single()))
                .ToList();
    }
}