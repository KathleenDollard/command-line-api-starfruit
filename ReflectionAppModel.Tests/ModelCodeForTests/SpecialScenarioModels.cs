using System.ComponentModel;

namespace System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests
{
    namespace MissingPropertyNotBlockingOtherAttributes
    {
        /// <summary>
        /// This class does not have a Description property
        /// </summary>
        public class CommandAttribute : Attribute
        { }

        [Command]
        [Description("Boo!")]
        public class SpecialScenarioModels
        { }
    }


}
