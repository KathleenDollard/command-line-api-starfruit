using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests
{
    public static class ReflectionSupport
    {

        internal static object GetPropertyInfo<T>(string name)
            => typeof(T)
                    .GetProperties()
                    .Where(x => x.Name == name)
                    .FirstOrDefault();


    }
}
