using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Playground
{
    public static class Utils
    {
        public static MethodInfo MethodInfo<T>(string methodName)
        {
            var type = typeof(T);
            var methodInfo = type.GetMethod(methodName);
            return methodInfo;
        }

        public static int MapAndRun(this ManageGlobalJson args, MethodInfo methodInfo, bool errorOnMisMatch = false)
        {
            var argsType = args.GetType();
            var parameters = methodInfo.GetParameters();
            var argsProperties = argsType.GetProperties();
            var objectProperties = typeof(object).GetProperties();
            if (errorOnMisMatch && parameters.Count() != (argsProperties.Count() - objectProperties.Count()))
            {
                throw new InvalidOperationException("Method parameter count doesn't match args for MapAndRun");
            }

            var paramValues = new List<object>();
            foreach (var parameter in parameters)
            {
                var argsProp = argsProperties.FirstOrDefault(p => p.NameMatches(parameter.Name));
                if (argsProp is null)
                {
                    if (errorOnMisMatch)
                    {
                        throw new InvalidOperationException("Method parameter doesn't have corresponding args property");
                    }
                    continue;
                }
                if (!parameter.ParameterType.IsAssignableFrom(argsProp.PropertyType))// whether or not Error on mismatch set to true
                {
                    throw new InvalidOperationException("Method parameter doesn't have same type as args property");
                }
                paramValues.Add(argsProp.GetValue(args));
            }
            return (int)methodInfo.Invoke(null, paramValues.ToArray());
        }

        private static bool NameMatches(this PropertyInfo prop, string parameterName)
        {
            return prop.Name == parameterName ||
                    prop.Name == parameterName + "Arg" ||
                    prop.Name == parameterName + "Argument";

        }
    }
}
