using System.Linq;
using System.Reflection;

namespace System.CommandLine.ReflectionModel.Tests.ModelCodeForTests
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

    }
}
