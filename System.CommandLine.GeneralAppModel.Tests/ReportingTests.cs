using FluentAssertions;
using FluentAssertions.Equivalency;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public class ReportingTests
    {
        private static Func<EquivalencyAssertionOptions<Symbol>, EquivalencyAssertionOptions<Symbol>> symbolOptions;
        private readonly Strategy strategy;


        public ReportingTests()
        {
            AssertionOptions.AssertEquivalencyUsing(o => o.ExcludingFields().IgnoringCyclicReferences());
            //argumentOptions = o => o.Excluding(ctx => ctx.SelectedMemberInfo.MemberName =="TryConvertArgument");
            symbolOptions = o => o.Excluding(ctx => ctx.SelectedMemberPath.EndsWith("ConvertArguments"));
            strategy = new Strategy().SetStandardRules();
        }

        [Fact]
        public void CanMakeSimpleCommand()
        {
            var report = strategy.Report();
            var expected = @"
Strategy: 
   SelectSymbolRules:
      Name Pattern Rule: Suffix - 'Command'
      AttributeName Rule: Command PropertyName: Name
      Name Pattern Rule: Suffix - 'Argument'
      Name Pattern Rule: Suffix - 'Arg'
      AttributeName Rule: Argument PropertyName: Name
      RemainingSymbolRule: All not yet matched
   ArgumentRules:
      NameRules:  
         AttributeName Rule: Name PropertyName: Name
         AttributeName Rule: Argument PropertyName: Name
         Name Pattern Rule: Suffix - 'Arg'
         Name Pattern Rule: Suffix - 'Argument'
         Identity Rule: 
                     
      DescriptionRules:  
         AttributeName Rule: Description PropertyName: Description
         AttributeName Rule: Argument PropertyName: Description
                     
      AliasesRules:  
                     
      IsHiddenRules:  
      Required Rules: 
         Bool Attribute Rule: Required PropertyName: 
         AttributeName Rule: Argument PropertyName: Required
                      
      Arity Rules:  
                      
      SpecialArgumentType Rule:  
         Bool Attribute Rule: Required PropertyName: 
         AttributeName Rule: Argument PropertyName: Required
   OptionRules:
      NameRules:  
         AttributeName Rule: Name PropertyName: Name
         Name Pattern Rule: Suffix - 'Option'
         AttributeName Rule: Option PropertyName: Name
                     
      DescriptionRules:  
         AttributeName Rule: Description PropertyName: Description
         AttributeName Rule: Option PropertyName: Description
                     
      AliasesRules:  
                     
      IsHiddenRules:  
      Required Rules: 
         Bool Attribute Rule: Required PropertyName: 
         AttributeName Rule: Option PropertyName: OptionRequired
   CommandRules:
      NameRules:  
         AttributeName Rule: Name PropertyName: Name
         Name Pattern Rule: Suffix - 'Command'
         AttributeName Rule: Command PropertyName: Name
         Identity Rule: 
                     
      DescriptionRules:  
         AttributeName Rule: Description PropertyName: Description
         AttributeName Rule: Command PropertyName: Description
                     
      AliasesRules:  
                     
      IsHiddenRules:  
";

            report.Should().Be(expected);
        }
    }
}
