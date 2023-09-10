using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Floatingman.CommandLineParser;
using Floatingman.CommandLineParser.Parser;

namespace AppWithPlugin;

class Program : ConsoleDecorator
{
   static void Main(string[] args)
   {
      Run(args);
   }

}
