using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests
{
    public static class ReflectionSupport
    {
        public static MethodInfo GetMethodInfo<T>(string name)
            => typeof(T)
                    .GetMethods()
                    .Where(x => x.Name == name)
                    .FirstOrDefault();

        internal static object GetPropertyInfo<T>(string name)
            => typeof(T)
                    .GetProperties()
                    .Where(x => x.Name == name)
                    .FirstOrDefault();

        internal static object GetParameterInfo<T>(string methodName, string parameterName)
            => GetMethodInfo<T>(methodName)
                    .GetParameters()
                    .Where(p => p.Name == parameterName)
                    .FirstOrDefault();

        public static IEnumerable<MethodInfo> GetMethodsOnDeclaredType(this Type type)
            => type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);


    }
}
