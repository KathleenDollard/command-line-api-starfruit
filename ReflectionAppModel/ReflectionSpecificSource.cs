using System;
using System.Collections;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace System.CommandLine.ReflectionAppModel
{
    public class ReflectionSpecificSource : SpecificSource
    {
        public override IEnumerable<Candidate> GetChildCandidates(Strategy strategy, SymbolDescriptorBase commandDescriptor)
        {
            var item = commandDescriptor.Raw;
            return item switch
            {
                MethodInfo m => m.GetParameters().Select(p => GetCandidateInternal(p)),
                Type t => GetTypeChildren(strategy, commandDescriptor, t, c => CreateCandidate(c.Item)),
                _ => new List<Candidate>(),

            };

            static IEnumerable<Candidate> GetTypeChildren(Strategy strategy, SymbolDescriptorBase commandDescriptor, Type t, Func<Candidate, Candidate> fillCandidate)
            {
                var derivedTypes = strategy.GetCandidateRules.GetCandidates(commandDescriptor).Select(c => fillCandidate(c));
                return derivedTypes.Union(t.GetProperties().Select(p => GetCandidateInternal(p)));
            }
        }

        public override Type GetArgumentType(Candidate candidate)
            => candidate.Item switch
            {
                PropertyInfo prop => prop.PropertyType,
                ParameterInfo param => param.ParameterType,
                Type type => type,
                _ => throw new InvalidOperationException("There must be an argument type")
            };

        public override Candidate CreateCandidate(object item)
           => item switch
           {
               Type typeItem => GetCandidateInternal(typeItem),
               MethodInfo methodItem => GetCandidateInternal(methodItem),
               ParameterInfo parameterInfo => GetCandidateInternal(parameterInfo),
               PropertyInfo propertyInfo => GetCandidateInternal(propertyInfo),
               _ => null
           };

        public override bool DoesTraitMatch<TTraitType>(string attributeName,
                                                        string propertyName,
                                                        SymbolDescriptorBase symbolDescriptor,
                                                        TTraitType trait,
                                                        SymbolDescriptorBase parentSymbolDescriptor)
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
                                                                      SymbolDescriptorBase symbolDescriptor,
                                                                      object trait,
                                                                      SymbolDescriptorBase parentSymbolDescriptor)
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
                                                                 SymbolDescriptorBase symbolDescriptor,
                                                                 object trait,
                                                                 SymbolDescriptorBase parentSymbolDescriptor)
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

        /// <summary>
        /// Adust the type. We work at this so traits like attributes can be flexible. 
        /// For exmple, a design might be to put each alias in a separate attribute. 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        private TValue FixType<TValue>(object value)
        {
            // We can just cast
            if (value is TValue tValue)
            {
                // no further conversion needed
                return tValue;
            }

            // We need to move a single value into an IEnumerable - this happens for aliases
            if (typeof(IEnumerable<>).IsAssignableFrom(typeof(TValue)) && !typeof(IEnumerable<>).IsAssignableFrom(value.GetType()))
            {
                // Wrap the value in an Enumerable. This could use some type checking
                var innerType = typeof(TValue).GenericTypeArguments.First();
                return (TValue)MakeEnumerable(typeof(TValue), innerType, value);
            }

            // throwing an exception here so during alpha we can figure out what is missing
            throw new InvalidOperationException("Unhandled attribute propertytype");
        }

        private object MakeEnumerable(Type type, Type innerType, object value)
        {
            var bindingFlags = BindingFlags.NonPublic | BindingFlags.Static;
            var method = GetType().GetMethod(nameof(MakeGenericEnumerable), bindingFlags);
            var constructed = method.MakeGenericMethod(innerType);
            return constructed.Invoke(null, new object[] { value });
        }

        internal static IEnumerable<TValue> MakeGenericEnumerable<TValue>(object value)
        {
            var list = new List<TValue> { (TValue)value };
            return list;
        }

        public override IEnumerable<(string key, TValue value)> GetComplexValue<TValue>(
                                                                      string attributeName,
                                                                      SymbolDescriptorBase symbolDescriptor,
                                                                      object trait,
                                                                      SymbolDescriptorBase parentSymbolDescriptor)
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

    }
}
