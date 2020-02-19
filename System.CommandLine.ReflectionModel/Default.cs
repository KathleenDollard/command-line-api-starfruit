namespace System.CommandLine.ReflectionModel
{
    public class Default
    {
        public static Default<T> Create<T>(T defaultValue)
            => new Default<T>()
            {
                Value = defaultValue
            };
    }

    public class Default<T> : Default
    {
        private T defaultValue;

        public bool HasDefaultValue { get; set; }

        public T Value
        {
            get => defaultValue;
            internal set
            {
                defaultValue = value;
                HasDefaultValue = true;
            }
        }
    }

}
