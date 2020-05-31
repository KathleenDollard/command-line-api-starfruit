namespace System.CommandLine.GeneralAppModel
{
    public static class Conversions
    {
        // Jon: Does System.CommandLine have any conversion stuff I should use. 

        // Don't remove this until we settle on how to convert values
        public static T To<T>(object rawValue)
        {
            return rawValue switch
            {
                bool b => (T)(object)b,
                int i => (T)(object)i,
                string s => (T)(object)s,
                _ => Force(rawValue)
            };

            static T Force(object raw)
            {
                try
                {
                    return (T)raw;
                }
                catch
                {
                    return default;
                }
            }
        }

        public static (bool success, T value) TryTo<T>(object rawValue)
        {
            return rawValue switch
            {
                bool b => (true, (T)(object)b),
                int i => (true, (T)(object)i),
                string s => (true, (T)(object)s),
                _ => Force(rawValue)
            };

            static (bool, T) Force(object raw)
            {
                try
                {
                    return (true, (T)raw);
                }
                catch
                {
                    return (false, default);
                }
            }
        }

        // ******************
        // This is more conversions stuff taken from ReflecionSpecificSource. 
        // ******************
        ///// <summary>
        ///// Adust the type. We work at this so traits like attributes can be flexible. 
        ///// For exmple, a design might be to put each alias in a separate attribute. 
        ///// </summary>
        ///// <typeparam name="TValue"></typeparam>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //private TValue FixType<TValue>(object value)
        //{
        //    // We can just cast
        //    if (value is TValue tValue)
        //    {
        //        // no further conversion needed
        //        return tValue;
        //    }

        //    // We need to move a single value into an IEnumerable - this happens for aliases
        //    if (typeof(IEnumerable<>).IsAssignableFrom(typeof(TValue)) && !typeof(IEnumerable<>).IsAssignableFrom(value.GetType()))
        //    {
        //        // Wrap the value in an Enumerable. This could use some type checking
        //        var innerType = typeof(TValue).GenericTypeArguments.First();
        //        return (TValue)MakeEnumerable(typeof(TValue), innerType, value);
        //    }

        //    // throwing an exception here so during alpha we can figure out what is missing
        //    throw new InvalidOperationException("Unhandled attribute propertytype");
        //}

        //private object MakeEnumerable(Type type, Type innerType, object value)
        //{
        //    var bindingFlags = BindingFlags.NonPublic | BindingFlags.Static;
        //    var method = GetType().GetMethod(nameof(MakeGenericEnumerable), bindingFlags);
        //    var _ = method ?? throw new InvalidOperationException("Expected internal method not found");
        //    var constructed = method.MakeGenericMethod(innerType);
        //    return constructed.Invoke(null, new object[] { value });
        //}
    }
}
