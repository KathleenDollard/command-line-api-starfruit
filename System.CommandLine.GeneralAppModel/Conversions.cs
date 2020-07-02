using System.Diagnostics.CodeAnalysis;

namespace System.CommandLine.GeneralAppModel
{
    public static class Conversions
    {
        // Jon: Does System.CommandLine have any conversion stuff I should use. 

        [return: MaybeNull]
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
#pragma warning disable CS8603 // Possible null reference return.
                    return default;
#pragma warning restore CS8603 // Possible null reference return.
                }
            }
        }
    }
}
