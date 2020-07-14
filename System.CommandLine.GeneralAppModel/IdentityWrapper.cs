namespace System.CommandLine.GeneralAppModel
{
    /// <summary>
    /// The identity wrapper distinguishes the intent of supporting the identity rule from the intent of 
    /// supporting StringContentRules and supports non-string identity types. "Identity" here is the sense 
    /// of "Just use this value" not the sense of "This value is the name", although it often is the name.
    /// </summary>
    public abstract class IdentityWrapper
    {
        public abstract object? ValueAsObject { get; }
    }

    public class IdentityWrapper<T> : IdentityWrapper
    {
        public IdentityWrapper(T value)
        {
            Value = value;
        }
        public T Value { get; }
        public override object? ValueAsObject
            => Value;
    }
}
