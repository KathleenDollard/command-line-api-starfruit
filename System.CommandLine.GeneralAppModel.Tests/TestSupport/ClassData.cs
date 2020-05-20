using System.Collections;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
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
            => _data.Select(x => new object[] { _commandData, x });

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
            public Wrapper<object> RawWrapper { get; private set; }
            public Wrapper<SymbolType> SymbolTypeWrapper { get; private set; }
            public Wrapper<IEnumerable<string>> AliasesWrapper { get; private set; }
            public Wrapper<string> DescriptionWrapper { get; private set; }
            public Wrapper<bool> IsHiddenWrapper { get; private set; }
            public Wrapper<string> NameWrapper { get; private set; }

            public object Raw { set { RawWrapper = Wrap(value); } }
            public SymbolType SymbolType { set { SymbolTypeWrapper = Wrap(value); } }
            public IEnumerable<string> Aliases { set { AliasesWrapper = Wrap(value); } }
            public string Description { set { DescriptionWrapper = Wrap(value); } }
            public bool IsHidden { set { IsHiddenWrapper = Wrap(value); } }
            public string Name { set { NameWrapper = Wrap(value); } }

            public string AltName { get; private set; }
            protected void SetAltName(string altName)
            {
                AltName = altName;
            }
        }

        public class CommandData : SymbolData
        {
            public Wrapper<IEnumerable<Option>> OptionsWrapper { get; private set; }
            public Wrapper<IEnumerable<Argument>> ArgumentsWrapper { get; private set; }
            public Wrapper<IEnumerable<Command>> SubCommandsWrapper { get; private set; }
            public Wrapper<bool> TreatUnmatchedTokensAsErrorsWrapper { get; private set; }

            public IEnumerable<Option> Options { set { OptionsWrapper = Wrap(value); } }
            public IEnumerable<Argument> Arguments { set { ArgumentsWrapper = Wrap(value); } }
            public IEnumerable<Command> SubCommands { set { SubCommandsWrapper = Wrap(value); } }
            public bool TreatUnmatchedTokensAsErrors { set { TreatUnmatchedTokensAsErrorsWrapper = Wrap(value); } }

            public CommandData WithAltName(string altName)
            {
                SetAltName(altName);
                return this;
            }
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

