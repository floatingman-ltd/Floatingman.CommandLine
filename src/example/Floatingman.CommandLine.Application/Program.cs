using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Floatingman.CommandLineParser;
using Floatingman.CommandLineParser.Parser;

namespace Floatingman.CommandLine.Application
{
    class Program:ConsoleDecorator
    {
        static async Task Main(string[] args)
        {
           await RunAsync(args); 
        }

    }
}
