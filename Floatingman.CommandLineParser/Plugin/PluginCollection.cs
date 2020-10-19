using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Floatingman.CommandLineParser.Parser;

namespace Floatingman.CommandLineParser.Plugin
{
    public class PluginCollection : Dictionary<string,IPlugin>
    {

        public void Add(IPlugin plugin)
        {
            this.Add(plugin.Name,plugin);
        }
        private IArgs _args;

        public IPlugin GetPluginCommand(IArgs rawArgs)
        {
            _args = rawArgs;
            return this[_args[0]];
        }
    }
}
