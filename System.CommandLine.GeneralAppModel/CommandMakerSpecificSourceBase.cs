using System.CommandLine.GeneralAppModel.Descriptors;

namespace System.CommandLine.GeneralAppModel
{
    public abstract class CommandMakerSpecificSourceBase
    {
        private static CommandMakerSpecificSourceBase? tools;

        /// <summary>
        /// This provides a singleton for access to the methods technology specific layer (like Roslyn, Reflection or JSON)
        /// </summary>
        public static CommandMakerSpecificSourceBase Tools
        {
            get
            {
                var _ = tools ?? throw new InvalidOperationException("Tools cannot be used before they are set");
                return tools;
            }
        }

        /// <summary>
        /// This provides a singleton for access to the methods technology specific layer (like Roslyn, Reflection or JSON)
        /// </summary>
        internal static void SetTools(CommandMakerSpecificSourceBase value)
        {
            tools = value;
        }

        // Basic logic:
        //    Set modelbinder when the binding context is created.
        //    Create and check the parse result
        //    Create an instance from the parse result
        //    Use that instance to call invoke or the method. Check if Invocatin handles this better. 

        public abstract BinderWrapper MakeModelBinder(CommandDescriptor commandDescriptor);
    }

    public abstract class BinderWrapper
    { }

    public class BinderWrapper<TBinder> : BinderWrapper 
    {
        public BinderWrapper(TBinder binder)
        {
            Binder = binder;
        }

        public TBinder Binder { get; }

    }
}
