using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Tests;
using System.Linq;
using System.Text;

namespace System.CommandLine.ReflectionModel.Tests
{
    internal static class ModelData
    {
        public static CommandTestData FromFirstMethod<T>()
            where T : new()
            => (Activator.CreateInstance<T>() as IHasTestData)
                    .CommandDataFromMethods
                    .First();

        public static IEnumerable<CommandTestData> FromAllMethod<T>()
            where T : new()
            => (Activator.CreateInstance<T>() as IHasTestData)
                    .CommandDataFromMethods;

        public static CommandTestData FromMethod<T>(string methodName)
            where T : new()
            => (Activator.CreateInstance<T>() as IHasTestData)
                    .CommandDataFromMethods
                    .Where(x=>x.Name == methodName)
                    .FirstOrDefault();

        internal static CommandTestData FromType<T>()
           where T : IHasTestData, new()
           => (Activator.CreateInstance<T>() as IHasTestData)
                    .CommandDataFromType;
    }
}
