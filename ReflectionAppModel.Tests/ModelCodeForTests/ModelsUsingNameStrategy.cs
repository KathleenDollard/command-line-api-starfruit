using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Tests;
using System.CommandLine.GeneralAppModel.Tests.ModelCodeForTests;
using System.ComponentModel;

namespace System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests
{
    // In this file, each class represents an outcome - a Descriptor that might
    // be achieved by different means. This file is for reflection, so the additional
    // data in each class are nested classes and nested classes with methods. 
    // Other AppModel sources may have different "For" types, and may set them in some
    // manner other than by referring to types and methods. 

    // To test a new strategy: Copy this file
    public class EmptyCommand : ClassData
    {
        public EmptyCommand()
            : base(
                  new CommandData
                  { },
                  new For[]
                  {
                      new ForType(typeof(FromEmptyType)),
                      new ForMethod(typeof(FromMethodNoParameters),nameof(FromMethodNoParameters.DoSomething ))
                  })
        { }

        public class FromMethodNoParameters
        {
            public void DoSomething() { }
        }

        public class FromEmptyType { }
    }

    public class CommandWithDescription : ClassData
    {
        const string desc = "This is a great description";
        public CommandWithDescription()
            : base(
                  new CommandData
                  {
                      Description = desc
                  },
                  new For[]
                  {
                      new ForType(typeof(FromTypeWithDescriptionAttribute)),
                      new ForMethod(typeof(FromMethodWithDescriptionAttribute),nameof(FromMethodWithDescriptionAttribute.DoSomething )),
                      new ForType(typeof(FromTypeWithCommandAttribute)),
                      new ForMethod(typeof(FromMethodWithCommandAttribute),nameof(FromMethodWithCommandAttribute.DoSomething ))
                 })
        { }

        public class FromMethodWithDescriptionAttribute
        {
            [Description(desc)]
            public void DoSomething() { }
        }

        [Description(desc)]
        public class FromTypeWithDescriptionAttribute { }

        public class FromMethodWithCommandAttribute
        {
            [Command(Description = desc)]
            public void DoSomething() { }
        }

        [Command(Description = desc)]
        public class FromTypeWithCommandAttribute { }
    }

    public class CommandWithSpecifiedName : ClassData
    {
        const string name = "Fred";
        public CommandWithSpecifiedName()
            : base(
                  new CommandData
                  {
                      Name = name
                  },
                  new For[]
                  {
                      new ForType(typeof(FromTypeWithCommandAttribute)),
                      new ForMethod(typeof(FromMethodWithCommandAttribute),nameof(FromMethodWithCommandAttribute.DoSomething ))
                  })
        { }

        public class FromMethodWithCommandAttribute
        {
            [Command(Name = name)]
            public void DoSomething() { }
        }

        [Command(Name = name)]
        public class FromTypeWithCommandAttribute { }
    }

    public class CommandWithTreatUnmatchedTokensAsErrors : ClassData
    {
        public CommandWithTreatUnmatchedTokensAsErrors()
            : base(
                  new CommandData
                  {
                      TreatUnmatchedTokensAsErrors = true
                  },
                  new For[]
                  {
                      new ForType(typeof(FromTypeWithAttribute)),
                      new ForMethod(typeof(FromMethodWithAttribute),nameof(FromMethodWithAttribute.DoSomething )),
                      new ForType(typeof(FromTypeWithAttributeValue)),
                      new ForMethod(typeof(FromMethodWithAttributeValue),nameof(FromMethodWithAttributeValue.DoSomething )),
                      new ForType(typeof(FromTypeWithCommandAttribute)),
                      new ForMethod(typeof(FromMethodWithCommandAttribute),nameof(FromMethodWithCommandAttribute.DoSomething ))
                  })
        { }

        public class FromMethodWithAttribute
        {
            [TreatUnmatchedTokensAsErrors]
            public void DoSomething() { }
        }

        [TreatUnmatchedTokensAsErrors]
        public class FromTypeWithAttribute { }

        public class FromMethodWithAttributeValue
        {
            [TreatUnmatchedTokensAsErrors(Value = true)]
            public void DoSomething() { }
        }

        [TreatUnmatchedTokensAsErrors(Value = true)]
        public class FromTypeWithAttributeValue { }

        public class FromMethodWithCommandAttribute
        {
            [Command(TreatUnmatchedTokensAsErrors = true)]
            public void DoSomething() { }
        }

        [Command(TreatUnmatchedTokensAsErrors = true)]
        public class FromTypeWithCommandAttribute { }
    }

    public class CommandWithIsHidden : ClassData
    {
        public CommandWithIsHidden()
            : base(
                  new CommandData
                  {
                      IsHidden = true
                  },
                  new For[]
                  {
                      // TODO: Reenable these tests after BoolAttributes are complete
                      //new ForType(typeof(FromTypeWithAttribute)),
                      //new ForMethod(typeof(FromMethodWithAttribute),nameof(FromMethodWithAttribute.DoSomething )),
                      new ForType(typeof(FromTypeWithAttributeValue)),
                      new ForMethod(typeof(FromMethodWithAttributeValue),nameof(FromMethodWithAttributeValue.DoSomething )),
                      new ForType(typeof(FromTypeWithCommandAttribute)),
                      new ForMethod(typeof(FromMethodWithCommandAttribute),nameof(FromMethodWithCommandAttribute.DoSomething ))
                  })
        { }

        public class FromMethodWithAttribute
        {
            [Hidden]
            public void DoSomething() { }
        }

        [Hidden]
        public class FromTypeWithAttribute { }

        public class FromMethodWithAttributeValue
        {
            [Hidden(Value = true)]
            public void DoSomething() { }
        }

        [Hidden(Value = true)]
        public class FromTypeWithAttributeValue { }

        public class FromMethodWithCommandAttribute
        {
            [Command(IsHidden = true)]
            public void DoSomething() { }
        }

        [Command(IsHidden = true)]
        public class FromTypeWithCommandAttribute { }
    }

    public class CommandWithOneAlias : ClassData
    {
        public CommandWithOneAlias()
            : base(
                  new CommandData
                  {
                      Aliases = new string[] { "x" }
                  },
                  new For[]
                  {
                      new ForType(typeof(FromTypeWithCommandAttributeValue)),
                      new ForMethod(typeof(FromMethodWithCommandAttributeValue),nameof(FromMethodWithCommandAttributeValue.DoSomething )),
                      new ForType(typeof(FromTypeWithAttribute)),
                      new ForMethod(typeof(FromMethodWithAttribute),nameof(FromMethodWithAttribute.DoSomething ))
                  })
        { }

        public class FromMethodWithAttribute
        {
            [Aliases("x")]
            public void DoSomething() { }
        }

        [Aliases("x")]
        public class FromTypeWithAttribute { }

        public class FromMethodWithCommandAttributeValue
        {
            [Command(Aliases = new string[] { "x" })]
            public void DoSomething() { }
        }

        [Command(Aliases = new string[] { "x" })]
        public class FromTypeWithCommandAttributeValue { }

    }

    public class CommandWithMultipleAliases : ClassData
    {
        public CommandWithMultipleAliases()
             : base(
                   new CommandData
                   {
                       Aliases = new string[] { "x", "y", "zed" }
                   },
                   new For[]
                   {
                      new ForType(typeof(FromTypeWithCommandAttributeValue)),
                      new ForMethod(typeof(FromMethodWithCommandAttributeValue),nameof(FromMethodWithCommandAttributeValue.DoSomething )),
                      new ForType(typeof(FromTypeWithAttribute)),
                      new ForMethod(typeof(FromMethodWithAttribute),nameof(FromMethodWithAttribute.DoSomething ))
                   })
        { }

        public class FromMethodWithAttribute
        {
            [Aliases("x", "y", "zed")]
            public void DoSomething() { }
        }

        [Aliases("x", "y", "zed")]
        public class FromTypeWithAttribute { }

        public class FromMethodWithCommandAttributeValue
        {
            [Command(Aliases = new string[] { "x", "y", "zed" })]
            public void DoSomething() { }
        }

        [Command(Aliases = new string[] { "x", "y", "zed" })]
        public class FromTypeWithCommandAttributeValue { }
    }

    public class CommandWithOneArg : ClassData
    {
        public CommandWithOneArg()
            : base(
                  new CommandData
                  {
                      Arguments = new List<ArgumentData>
                      {
                          new ArgumentData
                          { }
                      }
                  },
                  new For[]
                  {
                      new ForType(typeof(FromPropertyWithAttribute)),
                      new ForMethod(typeof(FromParameterWithAttribute),nameof(FromParameterWithAttribute.DoSomething )),
                      new ForType(typeof(FromPropertyWithArgName)),
                      new ForMethod(typeof(FromParameterWithArgName),nameof(FromParameterWithArgName.DoSomething )),
                      new ForType(typeof(FromPropertyWithArgumentName)),
                      new ForMethod(typeof(FromParameterWithArgumentName),nameof(FromParameterWithArgumentName.DoSomething )),
                 })
        { }

        public class FromParameterWithArgName
        {
            public void DoSomething(string stringValueArg) { }
        }

        public class FromPropertyWithArgName
        {
            public string stringValueArg { get; set; }
        }

        public class FromParameterWithArgumentName
        {
            public void DoSomething(string stringValueArgument) { }
        }

        public class FromPropertyWithArgumentName
        {
            public string stringValueArgument { get; set; }
        }

        public class FromParameterWithAttribute
        {
            public void DoSomething([Argument] string stringValue)
            { }
        }

        public class FromPropertyWithAttribute
        {
            [Argument]
            public string stringValue { get; set; }
        }
    }

    public class CommandWithOneOption : ClassData
    {
        public CommandWithOneOption()
            : base(
                  new CommandData
                  {
                      Options = new List<OptionData>
                      {
                          new OptionData
                          { }
                      }
                  },
                  new For[]
                  {
                      new ForType(typeof(FromType)),
                      new ForMethod(typeof(FromMethod),nameof(FromMethod.DoSomething )),
                  })
        { }

        public class FromMethod
        {
            public void DoSomething(string stringValue) { }
        }

        public class FromType
        {
            public string stringValue { get; set; }
        }
    }

    public class CommandWithOneSubCommand : ClassData
    {
        public CommandWithOneSubCommand()
            : base(
                  new CommandData
                  {
                      SubCommands = new List<CommandData>
                      {
                          new CommandData()
                      }
                  },
                  new For[]
                  {
                      new ForType(typeof(FromType)),
                      new ForMethod(typeof(FromMethod),nameof(FromMethod.DoSomething )),
                  })
        { }

        public class FromMethod
        {
            public void DoSomething(SampleSubCommand sample) { }
        }

        public class FromType
        {
        }

        public class SampleSubCommand : FromType
        {
            public string intValue { get; set; }
        }
    }




    public class OptionWithSpecifiedName : ClassData
    {
        const string name = "George";
        public OptionWithSpecifiedName()
            : base(
                  new CommandData
                  {
                      Options = new List<OptionData>
                      {
                          new OptionData
                          {
                              Name = name
                          }
                      }
                  },
                  new For[]
                  {
                      new ForType(typeof(FromPropertyName)),
                      new ForMethod(typeof(FromParameterName),nameof(FromParameterName.DoSomething )),
                      new ForType(typeof(FromPropertyWithNameAttribute)),
                      new ForMethod(typeof(FromParameterWithNameAttribute),nameof(FromParameterWithOptionAttribute.DoSomething )),
                      new ForType(typeof(FromPropertyWithOptionAttribute)),
                      new ForMethod(typeof(FromParameterWithOptionAttribute),nameof(FromParameterWithOptionAttribute.DoSomething ))
                 })
        { }

        public class FromParameterName
        {
            public void DoSomething(string george) { }
        }

        public class FromPropertyName

        {
            public string George { get; set; }

        }

        public class FromParameterWithNameAttribute
        {
            public void DoSomething([Name(name)] string george) { }
        }

        public class FromPropertyWithNameAttribute

        {
            [Name(name)]

            public string George { get; set; }

        }

        public class FromParameterWithOptionAttribute
        {
            public void DoSomething([Option(Name = name)] string george) { }
        }

        public class FromPropertyWithOptionAttribute

        {
            [Option(Name = name)]

            public string George { get; set; }

        }
    }

    public class OptionWithDescription : ClassData
    {
        const string desc = "This is a great description";
        public OptionWithDescription()
            : base(
                  new CommandData
                  {
                      Options = new List<OptionData>
                      {
                          new OptionData
                          {
                              Description = desc
                          }
                      }
                  },
                  new For[]
                  {
                      new ForType(typeof(FromPropertyWithDescriptionAttribute)),
                      new ForMethod(typeof(FromParameterWithDescriptionAttribute),nameof(FromParameterWithDescriptionAttribute.DoSomething )),
                      new ForType(typeof(FromPropertyWithOptionAttribute)),
                      new ForMethod(typeof(FromParameterWithOptionAttribute),nameof(FromParameterWithOptionAttribute.DoSomething ))
                 })
        { }

        public class FromParameterWithDescriptionAttribute
        {
            
            public void DoSomething([Description(desc)]string stringValue) {  }
        }

        public class FromPropertyWithDescriptionAttribute
        {
            [Description(desc)]
            public string StringProperty { get; set; }
        }

        public class FromParameterWithOptionAttribute
        {
            
            public void DoSomething([Option(Description = desc)]string stringValue) { }
        }

        public class FromPropertyWithOptionAttribute
        {
            [Option(Description = desc)]
            public string StringProperty { get; set; }
        }
    }

    public class OptionWithIsHidden : ClassData
    {
        public OptionWithIsHidden()
            : base(
                  new CommandData
                  {
                      Options = new List<OptionData>
                      {
                          new OptionData
                          {
                              IsHidden = true
                          }
                      }
                  },
                  new For[]
                  {
                      // TODO: Reenable these tests after BoolAttributes are complete
                      //new ForType(typeof(FromPropertyWithAttribute)),
                      //new ForMethod(typeof(FromParameterWithAttribute),nameof(FromParameterWithAttribute.DoSomething )),
                      new ForType(typeof(FromPropertyWithAttributeValue)),
                      new ForMethod(typeof(FromParameterWithAttributeValue),nameof(FromParameterWithAttributeValue.DoSomething )),
                      new ForType(typeof(FromPropertyWithOptionAttribute)),
                      new ForMethod(typeof(FromParameterWithOptionAttribute),nameof(FromParameterWithOptionAttribute.DoSomething ))
                  })
        { }

        public class FromParameterWithAttribute
        {

            public void DoSomething([Hidden] string stringValue) { }
        }

        public class FromPropertyWithAttribute
        {
            [Hidden]
            public string StringProperty { get; set; }
        }

        public class FromParameterWithAttributeValue
        {
            public void DoSomething([Hidden(Value = true)] string stringValue) { }
        }

        public class FromPropertyWithAttributeValue
        {
            [Hidden(Value = true)]
            public string StringProperty { get; set; }
        }

        public class FromParameterWithOptionAttribute
        {
            public void DoSomething([Option(IsHidden = true)] string stringValue) { }
        }

        public class FromPropertyWithOptionAttribute
        {
            [Option(IsHidden = true)]
            public string StringProperty { get; set; }
        }
    }

    public class OptionWithRequired : ClassData
    {
        public OptionWithRequired()
            : base(
                  new CommandData
                  {
                      Options = new List<OptionData>
                      {
                          new OptionData
                          {
                              Required = true
                          }
                      }
                  },
                  new For[]
                  {
                      // TODO: Reenable these tests after BoolAttributes are complete
                      //new ForType(typeof(FromPropertyWithAttribute)),
                      //new ForMethod(typeof(FromParameterWithAttribute),nameof(FromParameterWithAttribute.DoSomething )),
                      new ForType(typeof(FromPropertyWithAttributeValue)),
                      new ForMethod(typeof(FromParameterWithAttributeValue),nameof(FromParameterWithAttributeValue.DoSomething )),
                      new ForType(typeof(FromPropertyWithOptionAttribute)),
                      new ForMethod(typeof(FromParameterWithOptionAttribute),nameof(FromParameterWithOptionAttribute.DoSomething ))
                  })
        { }

        public class FromParameterWithAttribute
        {

            public void DoSomething([Required] string stringValue) { }
        }

        public class FromPropertyWithAttribute
        {
            [Required]
            public string StringProperty { get; set; }
        }

        public class FromParameterWithAttributeValue
        {
            public void DoSomething([Required(Value = true)] string stringValue) { }
        }

        public class FromPropertyWithAttributeValue
        {
            [Required(Value = true)]
            public string StringProperty { get; set; }
        }

        public class FromParameterWithOptionAttribute
        {
            public void DoSomething([Option(OptionRequired = true)] string stringValue) { }
        }

        public class FromPropertyWithOptionAttribute
        {
            [Option(OptionRequired = true)]
            public string StringProperty { get; set; }
        }
    }
   
    public class OptionWithOneAlias : ClassData
    {
        public OptionWithOneAlias()
            : base(
                  new CommandData
                  {
                      Options = new List<OptionData>
                      {
                          new OptionData
                          {
                              Aliases = new string[] { "x" }
                          }
                      }
                  },
                  new For[]
                  {
                      new ForType(typeof(FromPropertyWithAttributeValue)),
                      new ForMethod(typeof(FromParameterWithAttributeValue),nameof(FromParameterWithAttributeValue.DoSomething )),
                      new ForType(typeof(FromPropertyWithOptionAttribute)),
                      new ForMethod(typeof(FromParameterWithOptionAttribute),nameof(FromParameterWithOptionAttribute.DoSomething ))
                  })
        { }

        public class FromParameterWithAttributeValue
        {
            public void DoSomething([Aliases("x")] string stringValue) { }
        }

        public class FromPropertyWithAttributeValue
        {
            [Aliases("x")]
            public string StringProperty { get; set; }
        }

        public class FromParameterWithOptionAttribute
        {
            public void DoSomething([Option(Aliases = new string[] { "x" })] string stringValue) { }
        }

        public class FromPropertyWithOptionAttribute
        {
            [Option(Aliases = new string[] { "x" })]
            public string StringProperty { get; set; }
        }
    }

    public class OptionWithMultipleAliases : ClassData
    {
        public OptionWithMultipleAliases()
            : base(
                  new CommandData
                  {
                      Options = new List<OptionData>
                      {
                          new OptionData
                          {
                              Aliases = new string[] { "x" , "y", "zed" }
                          }
                      }
                  },
                  new For[]
                  {
                      new ForType(typeof(FromPropertyWithAttributeValue)),
                      new ForMethod(typeof(FromParameterWithAttributeValue),nameof(FromParameterWithAttributeValue.DoSomething )),
                      new ForType(typeof(FromPropertyWithOptionAttribute)),
                      new ForMethod(typeof(FromParameterWithOptionAttribute),nameof(FromParameterWithOptionAttribute.DoSomething ))
                  })
        { }


        public class FromParameterWithAttributeValue
        {
            public void DoSomething([Aliases("x", "y", "zed")] string stringValue) { }
        }

        public class FromPropertyWithAttributeValue
        {
            [Aliases("x", "y", "zed")]
            public string StringProperty { get; set; }
        }

        public class FromParameterWithOptionAttribute
        {
            public void DoSomething([Option(Aliases = new string[] { "x", "y", "zed" })] string stringValue) { }
        }

        public class FromPropertyWithOptionAttribute
        {
            [Option(Aliases = new string[] { "x", "y", "zed" })]
            public string StringProperty { get; set; }
        }
    }




    public class ArgumentWithSpecifiedName : ClassData
    {
        const string name = "Ron";
        public ArgumentWithSpecifiedName()
            : base(
                  new CommandData
                  {
                      Arguments = new List<ArgumentData>
                      {
                          new ArgumentData
                          {
                              Name = name
                          }
                      }
                  },
                  new For[]
                  {
                      new ForType(typeof(FromPropertyName)),
                      new ForMethod(typeof(FromParameterName),nameof(FromParameterName.DoSomething )),
                      new ForType(typeof(FromPropertyWithArgumentAttribute)),
                      new ForMethod(typeof(FromParameterWithArgumentAttribute),nameof(FromParameterWithArgumentAttribute.DoSomething ))
                  })
        { }

        public class FromParameterName
        {
            public void DoSomething(string ronArg) { }
        }

        public class FromPropertyName

        {
            public string RonArg { get; set; }

        }

        public class FromParameterWithArgumentAttribute
        {

            public void DoSomething([Argument(Name = name)] string stringValue) { }
        }

        public class FromPropertyWithArgumentAttribute
        {
            [Argument(Name = name)]
            public string StringProperty { get; set; }
        }
    }

    public class ArgumentWithDescription : ClassData
    {
        const string desc = "This is a great description";
        public ArgumentWithDescription()
            : base(
                  new CommandData
                  {
                      Arguments = new List<ArgumentData>
                      {
                          new ArgumentData
                          {
                              Description = desc
                          }
                      }
                  },
                  new For[]
                  {
                      new ForType(typeof(FromPropertyWithDescriptionAttribute)),
                      new ForMethod(typeof(FromParameterWithDescriptionAttribute),nameof(FromParameterWithDescriptionAttribute.DoSomething )),
                      new ForType(typeof(FromPropertyWithArgumentAttribute)),
                      new ForMethod(typeof(FromParameterWithArgumentAttribute),nameof(FromParameterWithArgumentAttribute.DoSomething ))
                 })
        { }

        public class FromParameterWithDescriptionAttribute
        {
            
            public void DoSomething([Description(desc)]string stringValueArg) { }
        }

        public class FromPropertyWithDescriptionAttribute
        {
            [Description(desc)]
            public string StringPropertyArg { get; set; }
        }

        public class FromParameterWithArgumentAttribute
        {
            
            public void DoSomething([Argument(Description = desc)]string stringValueArg) { }
        }

        public class FromPropertyWithArgumentAttribute
        {
            [Argument(Description = desc)]
            public string StringPropertyArg { get; set; }
        }
    }

    public class ArgumentWithIsHidden : ClassData
    {
        public ArgumentWithIsHidden()
            : base(
                  new CommandData
                  {
                      Arguments = new List<ArgumentData>
                      {
                          new ArgumentData
                          {
                              IsHidden = true
                          }
                      }
                  },
                  new For[]
                  {
                      // TODO: Reenable these tests after BoolAttributes are complete
                      //new ForType(typeof(FromPropertyWithAttribute)),
                      //new ForMethod(typeof(FromParameterWithAttribute),nameof(FromParameterWithAttribute.DoSomething )),
                      new ForType(typeof(FromPropertyWithAttributeValue)),
                      new ForMethod(typeof(FromParameterWithAttributeValue),nameof(FromParameterWithAttributeValue.DoSomething )),
                      new ForType(typeof(FromPropertyWithArgumentAttribute)),
                      new ForMethod(typeof(FromParameterWithArgumentAttribute),nameof(FromParameterWithArgumentAttribute.DoSomething ))
                  })
        { }

        public class FromParameterWithAttribute
        {

            public void DoSomething([Hidden] string stringValueArg) { }
        }

        public class FromPropertyWithAttribute
        {
            [Hidden]
            public string StringPropertyArg { get; set; }
        }

        public class FromParameterWithAttributeValue
        {
            public void DoSomething([Hidden(Value = true)] string stringValueArg) { }
        }

        public class FromPropertyWithAttributeValue
        {
            [Hidden(Value = true)]
            public string StringPropertyArg { get; set; }
        }

        public class FromParameterWithArgumentAttribute
        {
            public void DoSomething([Argument(IsHidden = true)] string stringValueArg) { }
        }

        public class FromPropertyWithArgumentAttribute
        {
            [Argument(IsHidden = true)]
            public string StringPropertyArg { get; set; }
        }
    }

    public class ArgumentWithRequired : ClassData
    {
        public ArgumentWithRequired()
            : base(
                  new CommandData
                  {
                      Arguments = new List<ArgumentData>
                      {
                          new ArgumentData
                          {
                              Required = true
                          }
                      }
                  },
                  new For[]
                  {
                      // TODO: Reenable these tests after BoolAttributes are complete
                      //new ForType(typeof(FromPropertyWithAttribute)),
                      //new ForMethod(typeof(FromParameterWithAttribute),nameof(FromParameterWithAttribute.DoSomething )),
                      new ForType(typeof(FromPropertyWithAttributeValue)),
                      new ForMethod(typeof(FromParameterWithAttributeValue),nameof(FromParameterWithAttributeValue.DoSomething )),
                      new ForType(typeof(FromPropertyWithArgumentAttribute)),
                      new ForMethod(typeof(FromParameterWithArgumentAttribute),nameof(FromParameterWithArgumentAttribute.DoSomething ))
                  })
        { }

        public class FromParameterWithAttribute
        {

            public void DoSomething([Required] string stringValueArg) { }
        }

        public class FromPropertyWithAttribute
        {
            [Required]
            public string StringPropertyArg { get; set; }
        }

        public class FromParameterWithAttributeValue
        {
            public void DoSomething([Required(Value = true)] string stringValueArg) { }
        }

        public class FromPropertyWithAttributeValue
        {
            [Required(Value = true)]
            public string StringPropertyArg { get; set; }
        }

        public class FromParameterWithArgumentAttribute
        {
            public void DoSomething([Argument(Required = true)] string stringValueArg) { }
        }

        public class FromPropertyWithArgumentAttribute
        {
            [Argument(Required = true)]
            public string StringPropertyArg { get; set; }
        }
    }

    public class ArgumentWithNonStringArgumentType : ClassData
    {
        public ArgumentWithNonStringArgumentType()
            : base(
                  new CommandData
                  {
                      Arguments = new List<ArgumentData>
                      {
                          new ArgumentData
                          {
                              ArgumentType =  typeof(System.IO.FileInfo)
                          }
                      }
                  },
                  new For[]
                  {
                      new ForType(typeof(FromProperty)),
                      new ForMethod(typeof(FromParameter),nameof(FromParameter.DoSomething )),
                  })
        { }

        public class FromParameter
        {
            public void DoSomething(System.IO.FileInfo fileValueArg) { }
        }

        public class FromProperty
        {
            public System.IO.FileInfo filePropertyArg { get; set; }
        }

    }

    public class ArgumentWithArity : ClassData
    {
        private const int min = 1;
        private const int max = 5;

        public ArgumentWithArity()
            : base(
                  new CommandData
                  {
                      Arguments = new List<ArgumentData>
                      {
                          new ArgumentData
                          {
                              ArityMin=min,
                              ArityMax=max
                          }
                      }
                  },
                  new For[]
                  {
                      new ForType(typeof(FromPropertyWithAttributeValue)),
                      new ForMethod(typeof(FromParameterWithAttributeValue),nameof(FromParameterWithAttributeValue.DoSomething ))
                  })
        { }

        public class FromParameterWithAttributeValue
        {
            public void DoSomething([Arity(MinimumCount = min, MaximumCount = max)] string stringValueArg) { }
        }

        public class FromPropertyWithAttributeValue
        {
            [Arity(MinimumCount = min, MaximumCount = max)]
            public string StringPropertyArg { get; set; }
        }

  
    }

    public class ArgumentWithDefaultValue : ClassData
    {
        private const string value = "Percy";
        public ArgumentWithDefaultValue()
            : base(
                  new CommandData
                  {
                      Arguments = new List<ArgumentData>
                      {
                          new ArgumentData
                          {
                              DefaultValue=value
                          }
                      }
                  },
                  new For[]
                  {
                      new ForType(typeof(FromPropertyWithAttributeValue)),
                      new ForMethod(typeof(FromParameterWithAttributeValue),nameof(FromParameterWithAttributeValue.DoSomething ))
                  })
        { }

        public class FromParameterWithAttributeValue
        {
            public void DoSomething([Default(value)] string stringValueArg) { }
        }

        public class FromPropertyWithAttributeValue
        {
            [Default(value)]
            public string StringPropertyArg { get; set; }
        }
    }

    public class ArgumentWithOneAlias : ClassData
    {
        public ArgumentWithOneAlias()
            : base(
                  new CommandData
                  {
                      Arguments = new List<ArgumentData>
                      {
                          new ArgumentData
                          {
                              Aliases = new string[] { "x" }
                          }
                      }
                  },
                  new For[]
                  {
                      new ForType(typeof(FromPropertyWithAttributeValue)),
                      new ForMethod(typeof(FromParameterWithAttributeValue),nameof(FromParameterWithAttributeValue.DoSomething )),
                      new ForType(typeof(FromPropertyWithArgumentAttribute)),
                      new ForMethod(typeof(FromParameterWithArgumentAttribute),nameof(FromParameterWithArgumentAttribute.DoSomething ))
                  })
        { }

        public class FromParameterWithAttributeValue
        {
            public void DoSomething([Aliases("x")] string stringValueArg) { }
        }

        public class FromPropertyWithAttributeValue
        {
            [Aliases("x")]
            public string StringPropertyArg { get; set; }
        }

        public class FromParameterWithArgumentAttribute
        {
            public void DoSomething([Argument(Aliases = new string[] { "x" })] string stringValueArg) { }
        }

        public class FromPropertyWithArgumentAttribute
        {
            [Argument(Aliases = new string[] { "x" })]
            public string StringPropertyArg { get; set; }
        }
    }

    public class ArgumentWithMultipleAliases : ClassData
    {
        public ArgumentWithMultipleAliases()
            : base(
                  new CommandData
                  {
                      Arguments = new List<ArgumentData>
                      {
                          new ArgumentData
                          {

                              Aliases = new string[] { "x", "y", "zed" }
                          }
                      }
                  },
                  new For[]
                  {
                      new ForType(typeof(FromPropertyWithAttributeValue)),
                      new ForMethod(typeof(FromParameterWithAttributeValue),nameof(FromParameterWithAttributeValue.DoSomething )),
                      new ForType(typeof(FromPropertyWithArgumentAttribute)),
                      new ForMethod(typeof(FromParameterWithArgumentAttribute),nameof(FromParameterWithArgumentAttribute.DoSomething ))
                  })
        { }

        public class FromParameterWithAttributeValue
        {
            public void DoSomething([Aliases("x", "y", "zed")] string stringValueArg) { }
        }

        public class FromPropertyWithAttributeValue
        {
            [Aliases("x", "y", "zed")]
            public string StringPropertyArg { get; set; }
        }

        public class FromParameterWithArgumentAttribute
        {
            public void DoSomething([Argument(Aliases = new string[] { "x", "y", "zed" })] string stringValueArg) { }
        }

        public class FromPropertyWithArgumentAttribute
        {
            [Argument(Aliases = new string[] { "x", "y", "zed" })]
            public string StringPropertyArg { get; set; }
        }
    }

}

