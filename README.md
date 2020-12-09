# Command-Parser

![.NET Core](https://github.com/64-Code-Amusements/Floatingman.CommandLine/workflows/.NET%20Core/badge.svg)

A command line parser - **console is the new UI**.

## Usage:

From the dotnet cli

```ps
dotnet add package Floatingman.CommandLineParser --version 0.1.0
```

---

## Anatomy of a command Line

```cmd
:> sample usage c:/temp/Animals.txt --type file -m "There are dog about" --shouldBeCareful
// ^      ^     ^                   ^      ^    ^  ^                     ^
// |      verb  |                   |      |    |  quoted option value   boolean option key
// driver       positional argument |      |    short option key
//                                  |      option value
//                                  long option
```

### The Parts Defined

There are three parts to the _command_ line implementation:

1. The **driver** is the executable that runs the process, this tool does not affect this.
2. The **verb** is the argument and command selector.
3. The remaining items are the arguments, either named or positional.
   1. Positional arguments are order dependent
   2. Option arguments are key-value pairs
   3. Any argument containing a space must be enclosed in quotes
   4. Boolean option arguments do not require a value and are true if present and false if not present
   5. Arguments can be the simple types _boolean_, _integer_, _floating point_ or _string_
   6. Arguments can be arrays of the above simple types, arrays contain values separated by commas
   7. Option arguments can be either short or long form
      1. Long form are preceded by `--`
      2. Short form are preceded by `-`

### Create a DI Container

> The plugin functionality requires (at this point) an _Dependency Injector (DI)_ (a.k.a. _Inversion of Control (IoC)_).  For the purposes of this example _Simple Injector_ is used, however this should work with any injector or even custom tooling.

We are using package or module registration of the _DI_ container to create plugins.

- [SimpleInjector - Packages](https://simpleinjector.readthedocs.io/en/latest/howto.html#package-registrations)

This can also be achieved using plugin registration if the _DI_ has that.

- [SimpleInjector - Plugins](https://simpleinjector.readthedocs.io/en/latest/advanced.html#registering-plugins-dynamically)

```csharp
    class Program
    {
        static void Main(string[] args)
        {
            // add a DI container
            var di = ConfigureDI(args);

            var verb = args != null && args.Length > 0 ? args[0] : String.Empty;

            var commands = di.GetAllInstances<IPlugin>().ToList();
            var command = commands
                .Where(p => string.Equals(p.Name, verb, StringComparison.CurrentCultureIgnoreCase))
                //.DefaultIfEmpty(new HelpWithVerbs(commands))
                .Single();
            var result = command.Execute();

            Console.WriteLine(result);
        }

        private static Container ConfigureDI(string [] args)
        {
            var container = new Container();
            var pluginAssemblies = PluginAssemblies("");
            container.RegisterPackages(pluginAssemblies);
            container.Collection.Register<IPlugin>(pluginAssemblies);
            container.RegisterInstance<IArgs>(new Args(args));
#if DEBUG
            container.Verify();
#endif
            return container;
        }

        private static List<Assembly> PluginAssemblies(string pluginDirectory)
        {
            var binDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pluginDirectory);
            var assemblyNames = Directory.GetFiles(binDirectory)
                .Where(f => Path.GetExtension(f) == ".dll")
                .Select(f => Path.GetFullPath(f));

            var assemblies =
                from assembly in assemblyNames
                select Assembly.LoadFile(assembly);

            var pluginAssemblies = assemblies.ToList();
            return pluginAssemblies;
        }

    }
```

### Building Plugin Modules

There are four principal parts to a plugin module:

1. The _plugin_ class which inherits and implements the abstract class `Plugin<TArgs>`, by convention this class is called **verb**_Plugin_.
2. The _args_ class which inherits the `CommandArgs` class.  This class is the commandline arguments as a plain data object.
3. The _command_ class which inherits for `ICommand<Targs>`, the actual functionality of the plugin is contained within the `Execute()` method.
4. The _package_ class which contains the _DI_ container module or package configuration.

## ToDo

- [ ] document the various deployment and development strategies for plugins
