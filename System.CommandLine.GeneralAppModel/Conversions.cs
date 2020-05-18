namespace System.CommandLine.GeneralAppModel
{
    public static class Conversions
    {

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
    }
}
