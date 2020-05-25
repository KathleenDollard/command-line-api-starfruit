using System;
using System.Collections;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Linq;
using System.Text;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public abstract class MakerCommandTestDataBase
    {

        public MakerCommandTestDataBase(CommandDescriptor descriptor)
        {
            Descriptor = descriptor;
        }

        public CommandDescriptor Descriptor { get; }
        public abstract void Check(Command command);

    }

    public abstract class MakerOptionTestDataBase
    {

        public MakerOptionTestDataBase(OptionDescriptor descriptor)
        {
            Descriptor = descriptor;
        }

        public OptionDescriptor Descriptor { get; }
        public abstract void Check(Option actual);
    }

    public abstract class MakerArgumentTestDataBase
    {

        public MakerArgumentTestDataBase(ArgumentDescriptor descriptor)
        {
            Descriptor = descriptor;
        }

        public ArgumentDescriptor Descriptor { get; }
        public abstract void Check(Argument actual);
    }

    //public class MakerClassDataForCommand : IEnumerable<object[]>
    //{
    //    private readonly Func<Command> check;
    //    private readonly CommandDescriptor descriptor;

    //    public MakerClassDataForCommand(CommandDescriptor  descriptor, Func<Command> check)
    //    {
    //        this.descriptor = descriptor;
    //        this.check = check;
    //    }

    //    public IEnumerable<object[]> AsObjectArray
    //        => _data.Select(forSource => new object[] { MakeId(forSource), forSource, _commandData });


    //    public IEnumerator<object[]> GetEnumerator() => AsObjectArray.GetEnumerator();

    //    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


    //}

}
