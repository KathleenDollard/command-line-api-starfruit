using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.Linq;
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
                Type t => GetTypeChildren(strategy, commandDescriptor, t, c => GetCandidate(c.Item)),
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
                _ => null
            };

        public override Candidate GetCandidate(object item)
           => item switch
           {
               Type typeItem => GetCandidateInternal(typeItem),
               MethodInfo methodItem => GetCandidateInternal(methodItem),
               ParameterInfo parameterInfo => GetCandidateInternal(parameterInfo),
               PropertyInfo propertyInfo => GetCandidateInternal(propertyInfo),
               _ => null
           };

        private static Candidate GetCandidateInternal(PropertyInfo propertyInfo)
        {
            var name = propertyInfo.Name;
            var candidate = new Candidate(propertyInfo, propertyInfo.Name);
            candidate.AddTraitRange(propertyInfo.GetCustomAttributes(Context.IncludeBaseClassAttributes));
            candidate.AddTrait(name);
            candidate.AddTrait(new IdentityWrapper<string>(name));
            return candidate;
        }

        private static Candidate GetCandidateInternal(ParameterInfo parameterInfo)
        {
            var name = parameterInfo.Name;
            var candidate = new Candidate(parameterInfo, parameterInfo.Name);
            candidate.AddTraitRange(parameterInfo.GetCustomAttributes(Context.IncludeBaseClassAttributes));
            candidate.AddTrait(name);
            candidate.AddTrait(new IdentityWrapper<string>(name));
            return candidate;
        }

        private static Candidate GetCandidateInternal(MethodInfo methodInfo)
        {
            var name = methodInfo.Name;
            var candidate = new Candidate(methodInfo, methodInfo.Name);
            candidate.AddTraitRange(methodInfo.GetCustomAttributes(Context.IncludeBaseClassAttributes));
            candidate.AddTrait(name);
            candidate.AddTrait(new IdentityWrapper<string>(name));
            return candidate;
        }

        private static Candidate GetCandidateInternal(Type type)
        {
            var name = type.Name;
            var candidate = new Candidate(type, type.Name);
            candidate.AddTraitRange(type.GetCustomAttributes(Context.IncludeBaseClassAttributes));
            candidate.AddTrait(name);
            candidate.AddTrait(new IdentityWrapper<string>(name));
            return candidate;
        }
    }
}
