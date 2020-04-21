//using System.Collections.Generic;
//using System.CommandLine.GeneralAppModel;
//using System.Reflection;

//namespace System.CommandLine.GeneralAppModel
//{
//    public class NameRules : ModelRules
//    {
//        public override void UseStandard()
//        {
//            Add(new AttributeRule("Name", "string", "Name"));
//            Add(new AttributeRule("Argument", "string", "Name", SymbolType.Argument));
//            Add(new AttributeRule("Option", "string", "Name", SymbolType.Option));
//            Add(new AttributeRule("Command", "string", "Name", SymbolType.Command));
//            Add(new ItemNameRule());
//        }
//    }
//}