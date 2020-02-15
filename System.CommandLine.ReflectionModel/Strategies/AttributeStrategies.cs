﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.CommandLine.ReflectionModel
{

    public class AttributeStrategies<T>
    {
        protected readonly List<Func<IEnumerable<Attribute>, T>> attributeStrategies = new List<Func<IEnumerable<Attribute>, T>>();
        public void Add(Func<IEnumerable<Attribute>, T> strategy)
                            => attributeStrategies.Add(strategy);


    }

    public class AttributeStrategiesWithSymbolFilter<T>
    {
        protected readonly List<Func<IEnumerable<Attribute>, SymbolType, T>> attributeStrategies = new List<Func<IEnumerable<Attribute>, SymbolType, T>>();
        public void Add(Func<IEnumerable<Attribute>, SymbolType, T> strategy)
                            => attributeStrategies.Add(strategy);


    }

    public static class AttributeStrategyExtensions
    {
   
        internal static bool Filter(this Func<SymbolType, bool> filterFunc, SymbolType symbolType)
            => filterFunc is null
                ? true
                : filterFunc(symbolType);
    }
}