using System;
using System.Collections.Generic;
using System.Text;

namespace System.CommandLine.GeneralAppModel
{
    /// <summary>
    /// Derived classes supply specific technology implementation for the 
    /// ArgumentType. 
    /// <br/>
    /// For exmple: In Reflection, this provides a Type object. In Roslyn it provides
    /// a syntax node that represents the type.
    /// </summary>
    public class InvokeMethodInfo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodRepresentation"></param>
        /// <param name="name"></param>
        /// <param name="score">Usually the parameter count</param>
        public InvokeMethodInfo(object methodRepresentation, string name, int score)
        {
            MethodRepresentation = methodRepresentation;
            Name = name;
            Score = score;
        }

        public object MethodRepresentation { get; }
        public string Name { get; }
        public int Score { get; }
        public List<Candidate> ChildCandidates { get; } = new List<Candidate>();

        public  T GetInvokeMethod<T>()
            where T : class
        {
            if (!(MethodRepresentation is null) && MethodRepresentation is T t)
            {
                return t;
            }
            throw new NotImplementedException("Do we need anything else here");
        }
    }
}
