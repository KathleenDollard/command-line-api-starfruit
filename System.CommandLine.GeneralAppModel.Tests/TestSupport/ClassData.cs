using System.Collections;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Linq;

namespace System.CommandLine.GeneralAppModel.Tests.ModelCodeForTests
{
    public class ClassData : IEnumerable<object[]>
    {
        private readonly List<For> _data = new List<For>();
        private readonly CommandData _commandData;

        public ClassData(CommandData commandData, params For[] sources)
        {
            _data.AddRange(sources);
            _commandData = commandData;
        }

        public IEnumerable<object[]> AsObjectArray
            => _data.Select(forSource => new object[] { MakeId(forSource), forSource, _commandData });

        private string MakeId(For forSource)
        {
            var name = forSource.Type.FullName;
            var posLast = name.LastIndexOf(".");
            return name[(posLast + 1)..];
        }

        public IEnumerator<object[]> GetEnumerator() => AsObjectArray.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public class Wrapper<T>
        {
            public T Value { get; }

            public Wrapper(T value)
            {
                Value = value;
            }
        }

        public static Wrapper<T> Wrap<T>(T value)
        {
            return new Wrapper<T>(value);
        }

        public class SymbolData
        {
            public Wrapper<object>? RawWrapper { get; private set; }
            public Wrapper<SymbolType>? SymbolTypeWrapper { get; private set; }
            public Wrapper<IEnumerable<string>>? AliasesWrapper { get; private set; }
            public Wrapper<string>? DescriptionWrapper { get; private set; }
            public Wrapper<bool>? IsHiddenWrapper { get; private set; }
            public Wrapper<string>? NameWrapper { get; private set; }

            public string? AltName { get; private set; }
            protected void SetAltName(string altName)
            {
                AltName = altName;
            }

            public string? Id { get; private set; }
            protected void SetId(string id)
            {
                Id = id;
            }
        }

        public class CommandData : SymbolData
        {
            public Wrapper<IEnumerable<OptionData>>? OptionsWrapper { get; private set; }
            public Wrapper<IEnumerable<ArgumentData>>? ArgumentsWrapper { get; private set; }
            public Wrapper<IEnumerable<CommandData>>? SubCommandsWrapper { get; private set; }
            public Wrapper<bool>? TreatUnmatchedTokensAsErrorsWrapper { get; private set; }

            public IEnumerable<OptionData>? Options { set { OptionsWrapper = Wrap(value); } }
            public IEnumerable<ArgumentData>? Arguments { set { ArgumentsWrapper = Wrap(value); } }
            public IEnumerable<CommandData>? SubCommands { set { SubCommandsWrapper = Wrap(value); } }
            public bool TreatUnmatchedTokensAsErrors { set { TreatUnmatchedTokensAsErrorsWrapper = Wrap(value); } }

            // This allows return type to be specifically CommandData. We could probably 
            // clean this up, but extension methods would require expanding scope of property
            // and this is a stop gap until we can use C# 9 features
            public CommandData WithAltName(string altName)
            {
                SetAltName(altName);
                return this;
            }
            public CommandData WithId(string id)
            {
                SetId(id);
                return this;
            }
        }

        public class ArgumentData : SymbolData
        {
            public Wrapper<int>? ArityMinWrapper { get; private set; }
            public Wrapper<int>? ArityMaxWrapper { get; private set; }
            public Wrapper<bool>? HasArityWrapper { get; private set; }
            public Wrapper<object>? DefaultValueWrapper { get; private set; }
            public Wrapper<bool>? HasDefaultWrapper { get; private set; }
            public Wrapper<Type>? ArgumentTypeWrapper { get; private set; }
            public Wrapper<HashSet<string>>? AllowedValuesWrapper { get; private set; }
            public Wrapper<bool>? RequiredWrapper { get; private set; }

            public int ArityMin
            {
                set
                {
                    ArityMinWrapper = Wrap(value);
                    HasArityWrapper = Wrap(true);
                }
            }
            public int ArityMax
            {
                set
                {
                    ArityMaxWrapper = Wrap(value);
                    HasArityWrapper = Wrap(true);
                }
            }
            public object DefaultValue
            {
                set
                {
                    DefaultValueWrapper = Wrap(value);
                    HasDefaultWrapper = Wrap(true);
                }
            }
            public Type ArgumentType { set { ArgumentTypeWrapper = Wrap(value); } }
            public HashSet<string> AllowedValues { set { AllowedValuesWrapper = Wrap(value); } }
            public bool Required { set { RequiredWrapper = Wrap(value); } }
        }

        public class OptionData : SymbolData
        {
            public Wrapper<IEnumerable<ArgumentData>>? ArgumentsWrapper { get; private set; }
            public Wrapper<bool>? RequiredWrapper { get; private set; }


            public IEnumerable<ArgumentData> Arguments { set { ArgumentsWrapper = Wrap(value); } }
            public bool Required { set { RequiredWrapper = Wrap(value); } }
        }

        public class For
        {
            public Type Type { get; }
            public string AltName { get; }

            public For(string altName, Type type)
            {
                Type = type;
                AltName = altName;
            }
        }

        public class ForMethod : For
        {

            public ForMethod(Type type, string methodName)
                : base(methodName, type)
            {
                MethodName = methodName;
            }

            public string MethodName;
        }

        public class ForType : For
        {
            public ForType(Type type)
                : base(type.Name, type)
            { }

        }
    }
}

