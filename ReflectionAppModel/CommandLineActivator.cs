using System.Diagnostics.CodeAnalysis;
using System.CommandLine.Binding;
using System.CommandLine.Builder;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.Parsing;
using System.CommandLine.ReflectionAppModel;
using System.Linq;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Dynamic;
using System.CommandLine;

namespace System.CommandLine.ReflectionAppModel
{
    public class CommandLineActivator : CommandLineActivatorBase
    {
        public override CommandDescriptor GetCommandDescriptor<TRoot>(Strategy? strategy = null)
        {
            strategy ??= Strategy.Standard;
            return ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeof(TRoot));
        }
    }
}

