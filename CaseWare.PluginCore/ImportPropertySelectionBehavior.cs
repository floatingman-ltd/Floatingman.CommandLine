using SimpleInjector.Advanced;
using System;
using System.Linq;
using System.Reflection;

namespace CaseWare.IdeaCommandLine.Plugins
{
    public class ImportPropertySelectionBehavior : IPropertySelectionBehavior
    {
        public bool SelectProperty(Type implementationType, PropertyInfo prop) =>
            prop.GetCustomAttributes(typeof(ImportAttribute)).Any();
    }
}