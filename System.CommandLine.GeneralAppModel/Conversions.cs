using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace System.CommandLine.GeneralAppModel
{
    public static class Conversions
    {

        public static T To<T>(object rawValue)
        {
            if (typeof(T) == typeof(bool))
            {
                // surely there is a better way to do this
                return (T)(object)Conversions.ToBool(rawValue);
            }
            if (typeof(T) == typeof(int))
            {
                // surely there is a better way to do this
                return (T)(object)Conversions.ToInt(rawValue);
            }
            if (typeof(T) == typeof(string))
            {
                // surely there is a better way to do this
                return (T)(object)rawValue.ToString();
            }
            throw new InvalidOperationException("Unexpected type requestd");
        }

        public static  bool ToBool(object rawValue)
            => rawValue switch
            {
                bool value => value,
                int intValue => intValue != 0,
                string stringValue => bool.TryParse(stringValue, out var value)
                                                    ? value
                                                    : false,
                _ => false
            };

        public static int ToInt(object rawValue)
             => rawValue switch
             {
                 bool _ => 1,
                 int value => value,
                 string stringValue => int.TryParse(stringValue, out var value)
                                                     ? value
                                                     : 0,
                 _ => 0
             };
    }
}
