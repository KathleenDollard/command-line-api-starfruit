using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.CommandLine.GeneralAppModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace System.CommandLine.ReflectionAppModel
{
    public class ReflectionSpecificSource : SpecificSource
    {
        public override IEnumerable<Candidate> GetChildCandidates(Strategy strategy, SymbolDescriptor commandDescriptor)
        {
            var item = commandDescriptor.Raw;
            return item switch
            {
                MethodInfo m => GetMethodChildCandidates(m, commandDescriptor),
                Type t => GetTypeChildren(strategy, commandDescriptor, t, c => CreateCandidate(c.Item)),
                _ => new List<Candidate>(),

            };

            static IEnumerable<Candidate> GetTypeChildren(Strategy strategy, SymbolDescriptor commandDescriptor, Type t, Func<Candidate, Candidate> fillCandidate)
            {
                var derivedTypes = strategy.GetCandidateRules.GetCandidates(commandDescriptor).Select(c => fillCandidate(c));
                var flags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;
                return derivedTypes.Union(t.GetProperties().Select(p => GetCandidateInternal(p)));
            }
        }

        private static IEnumerable<Candidate> GetMethodChildCandidates(MethodInfo methodInfo, SymbolDescriptor parentSymbolDescriptor)
        {
            return methodInfo.GetParameters().Select(p => GetCandidateInternal(p));
        }

        public override IEnumerable<InvokeMethodInfo> GetAvailableInvokeMethodInfos(object? raw, SymbolDescriptor parentSymbolDescriptor, bool treatParametersAsSymbols)
        {
            if (raw is null || !(raw is Type type))
            {
                return new List<InvokeMethodInfo>();
            }
            return type.GetMethods()
                            .Select(x => CreateInvokeMethodInfo(x, parentSymbolDescriptor, treatParametersAsSymbols)); // by default public instance

            static InvokeMethodInfo CreateInvokeMethodInfo(MethodInfo methodInfo, SymbolDescriptor parentSymbolDescriptor, bool treatParametersAsSymbols)
            {
                var invokeMethodInfo = new InvokeMethodInfo(methodInfo, methodInfo.Name, methodInfo.GetParameters().Count());
                if (treatParametersAsSymbols)
                {
                    invokeMethodInfo.ChildCandidates.AddRange(GetMethodChildCandidates(methodInfo, parentSymbolDescriptor));
                }
                return invokeMethodInfo;
            }
        }

        public override ArgTypeInfo GetArgTypeInfo(Candidate candidate)
            => candidate.Item switch
            {
                PropertyInfo prop => new ArgTypeInfo(prop.PropertyType),
                ParameterInfo param => new ArgTypeInfo(param.ParameterType),
                Type type => new ArgTypeInfo(type),
                _ => throw new InvalidOperationException("Argument result is of an unexpected type")
            };

        public override Candidate CreateCandidate(object item)
           => item switch
           {
               Type typeItem => GetCandidateInternal(typeItem),
               MethodInfo methodItem => GetCandidateInternal(methodItem),
               ParameterInfo parameterInfo => GetCandidateInternal(parameterInfo),
               PropertyInfo propertyInfo => GetCandidateInternal(propertyInfo),
               _ => throw new InvalidOperationException("Unexpected candidate type")
           };

        public override bool DoesTraitMatch<TTraitType>(string attributeName,
                                                        string propertyName,
                                                        ISymbolDescriptor symbolDescriptor,
                                                        TTraitType trait,
                                                        ISymbolDescriptor parentSymbolDescriptor)
        {
            if (!(trait is Attribute attribute))
            {
                return false;
            }
            var itemName = attribute.GetType().Name;
            return itemName.Equals(attributeName, StringComparison.OrdinalIgnoreCase)
                || itemName.Equals(attributeName + "Attribute", StringComparison.OrdinalIgnoreCase);
        }

        public override (bool success, TValue value) GetValue<TValue>(string attributeName,
                                                                      string propertyName,
                                                                      ISymbolDescriptor symbolDescriptor,
                                                                      object trait,
                                                                      ISymbolDescriptor parentSymbolDescriptor)
        {
            if (!(trait is Attribute attribute) ||
                !DoesTraitMatch(attributeName, propertyName, symbolDescriptor, trait, parentSymbolDescriptor))
            {
                return (false, default);
            }
            var propInfo = attribute.GetType().GetProperties().Where(p => p.Name == propertyName).FirstOrDefault();
            if (propInfo is null)
            {
                return (false, default);
            }
            if (!typeof(TValue).IsAssignableFrom(propInfo.PropertyType))
            {
                // This is a bit strict as it doesn't handle numeric widening conversions
                return (false, default);
            }
            var obj = propInfo.GetValue(attribute);
            return (obj is TValue tValue)
                    ? (true, tValue)
                    : (false, default);
        }

        public override IEnumerable<TValue> GetAllValues<TValue>(string attributeName,
                                                                 string propertyName,
                                                                 ISymbolDescriptor symbolDescriptor,
                                                                 object trait,
                                                                 ISymbolDescriptor parentSymbolDescriptor)
        {
            var (singleSuccess, single) = GetValue<TValue>(attributeName, propertyName, symbolDescriptor, trait, parentSymbolDescriptor);
            if (singleSuccess)
            {
                return new List<TValue> { single };
            }
            var (multipleSuccess, multiple) = GetValue<IEnumerable<TValue>>(attributeName, propertyName, symbolDescriptor, trait, parentSymbolDescriptor);
            if (multipleSuccess)
            {
                return multiple;
            }
            return new List<TValue>();
        }



        internal static IEnumerable<TValue> MakeGenericEnumerable<TValue>(object value)
        {
            var list = new List<TValue> { (TValue)value };
            return list;
        }

        public override IEnumerable<(string key, TValue value)> GetComplexValue<TValue>(
                                                                      string attributeName,
                                                                      ISymbolDescriptor symbolDescriptor,
                                                                      object trait,
                                                                      ISymbolDescriptor parentSymbolDescriptor)
        {
            if (!(trait is Attribute attribute) ||
                !DoesTraitMatch(attributeName, symbolDescriptor, trait, parentSymbolDescriptor))
            {
                return new List<(string, TValue)>();
            }

            var attributeType = attribute.GetType();
            var pairs = attributeType.GetProperties()
                                .Where(prop => prop.Name != "TypeId")
                                .Select(prop => new { prop.Name, Value = prop.GetValue(trait) });
            return pairs.Select(pair => (pair.Name, (TValue)pair.Value));
        }

        private static Candidate GetCandidateInternal(PropertyInfo propertyInfo)
        {
            var candidate = new Candidate(propertyInfo);
            candidate.AddTraitRange(propertyInfo.GetCustomAttributes(Context.IncludeBaseClassAttributes));
            candidate.AddTrait(new IdentityWrapper<string>(propertyInfo.Name));
            return candidate;
        }

        private static Candidate GetCandidateInternal(ParameterInfo parameterInfo)
        {
            var candidate = new Candidate(parameterInfo);
            candidate.AddTraitRange(parameterInfo.GetCustomAttributes(Context.IncludeBaseClassAttributes));
            var _ = parameterInfo.Name ?? throw new InvalidOperationException("How is a parameter name null?");
            candidate.AddTrait(new IdentityWrapper<string>(parameterInfo.Name));
            return candidate;
        }

        private static Candidate GetCandidateInternal(MethodInfo methodInfo)
        {
            var candidate = new Candidate(methodInfo);
            candidate.AddTraitRange(methodInfo.GetCustomAttributes(Context.IncludeBaseClassAttributes));
            candidate.AddTrait(new IdentityWrapper<string>(methodInfo.Name));
            return candidate;
        }

        private static Candidate GetCandidateInternal(Type type)
        {
            var candidate = new Candidate(type);
            candidate.AddTraitRange(type.GetCustomAttributes(Context.IncludeBaseClassAttributes));
            candidate.AddTrait(new IdentityWrapper<string>(type.Name));
            return candidate;
        }

        public override bool DoesTraitMatch<TAttribute, TTraitType>(string propertyName,
                                                                    ISymbolDescriptor symbolDescriptor,
                                                                    TTraitType trait,
                                                                    ISymbolDescriptor parentSymbolDescriptor)
            => trait is TAttribute;

        public override IEnumerable<TValue> GetAllValues<TAttribute, TValue>(string propertyName,
                                                                             ISymbolDescriptor symbolDescriptor,
                                                                             object trait,
                                                                             ISymbolDescriptor parentSymbolDescriptor)
        {
            var (singleSuccess, single) = GetValue<TAttribute, TValue>(propertyName, symbolDescriptor, trait, parentSymbolDescriptor);
            if (singleSuccess)
            {
                return new List<TValue> { single };
            }
            var (multipleSuccess, multiple) = GetValue<TAttribute, IEnumerable<TValue>>(propertyName, symbolDescriptor, trait, parentSymbolDescriptor);
            if (multipleSuccess)
            {
                return multiple;
            }
            return new List<TValue>();
        }

        public override (bool success, TValue value) GetValue<TAttribute, TValue>(string propertyName,
                                                                                  ISymbolDescriptor symbolDescriptor,
                                                                                  object trait,
                                                                                  ISymbolDescriptor parentSymbolDescriptor)
        {
            if (!(trait is Attribute attribute) ||
                !DoesTraitMatch<TAttribute, object>(propertyName, symbolDescriptor, trait, parentSymbolDescriptor))
            {
                return (false, default);
            }
            var propInfo = attribute.GetType().GetProperties().Where(p => p.Name == propertyName).FirstOrDefault();
            if (propInfo is null)
            {
                return (false, default);
            }
            if (!typeof(TValue).IsAssignableFrom(propInfo.PropertyType))
            {
                // This is a bit strict as it doesn't handle numeric widening conversions
                return (false, default);
            }
            var obj = propInfo.GetValue(attribute);
            return (obj is TValue tValue)
                    ? (true, tValue)
                    : (false, default);
        }


        public override IEnumerable<(string key, TValue value)> GetComplexValue<TAttribute, TValue>(ISymbolDescriptor symbolDescriptor,
                                                                                                    object trait,
                                                                                                    ISymbolDescriptor parentSymbolDescriptor)
        {
            if (!(trait is Attribute attribute) ||
                 !DoesTraitMatch<TAttribute, object>(symbolDescriptor, trait, parentSymbolDescriptor))
            {
                return new List<(string, TValue)>();
            }

            var attributeType = attribute.GetType();
            var pairs = attributeType.GetProperties()
                                .Where(prop => prop.Name != "TypeId")
                                .Select(prop => new { prop.Name, Value = prop.GetValue(trait) });
            return pairs.Select(pair => (pair.Name, (TValue)pair.Value));
        }


    }
}
