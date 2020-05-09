using System;
using System.Collections.Generic;
using System.Text;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public  class TestData
    {
        public static readonly ArgumentTestData ArgData1 = new ArgumentTestData
            {
                Aliases = new string[] { "a", "b", "c" },
                Name = "Fred",
                Description = "This is a description",
                IsHidden = false,
                //Arity  = asdf,
                //AllowedValues { get; } = new HashSet<string>();
                ArgumentType = typeof(string),
                //DefaultValue  = asdf,
                Required = false,
            };
        public static readonly ArgumentTestData ArgData2= new ArgumentTestData
            {
                Name = "George",
                Description = "Another description",
                IsHidden = true,
                //Arity  = asdf,
                //AllowedValues { get; } = new HashSet<string>();
                ArgumentType = typeof(int),
                //DefaultValue  = asdf,
                Required = true,
            };
        public static readonly ArgumentTestData ArgData3= new ArgumentTestData
            {
                Name = "Ron",
                IsHidden = false,
                //Arity  = asdf,
                //AllowedValues { get; } = new HashSet<string>();
                ArgumentType = typeof(bool),
                //DefaultValue  = asdf,
                Required = false,
            };
                
        public static readonly CommandTestData CommandData1= new CommandTestData
            {
                Aliases = new string[] { "a", "b", "c" },
                Name = "Spot",
                Description = "This is a description",
            };
        public static readonly CommandTestData CommandData2 = new CommandTestData
            {
                Name = "Rover",
                Description = "Another description",
                IsHidden = true
            };
        public static readonly CommandTestData CommandData3 = new CommandTestData
            {
                Name = "Aspen",
                IsHidden = false,
            };
                
        public static readonly OptionTestData OptionData1= new OptionTestData
            {
                Aliases = new string[] { "a", "b", "c" },
                Name = "Charlotte",
                Description = "This is a description",
                IsHidden = false,
                Required = false,
            };
        public static readonly OptionTestData OptionData2 = new OptionTestData
            {
                Name = "Violet",
                Description = "Another description",
                IsHidden = true,
                Required = true,
            };
        public static readonly OptionTestData OptionData3 = new OptionTestData
            {
                Name = "Amber",
                IsHidden = false,
                Required = false,
            };
    }
}
