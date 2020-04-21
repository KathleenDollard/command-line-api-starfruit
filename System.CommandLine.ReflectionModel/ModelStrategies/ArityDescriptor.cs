namespace System.CommandLine.GeneralAppModel
{
    public class ArityDescriptor
    {
        public ArityDescriptor(int min, int max)
        {
            Min = min;
            Max = max;
            IsSet = true;
        }
        public ArityDescriptor()
        { }

        public int Min { get; }
        public int Max { get; }
        public bool IsSet { get; set; }


    }
}