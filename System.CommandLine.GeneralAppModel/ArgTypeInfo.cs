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
    public class ArgTypeInfo
    {
        public ArgTypeInfo(object typeRepresentation)
        {
            TypeRepresentation = typeRepresentation;
        }

        public object TypeRepresentation { get; }

        public  T GetArgumentType<T>()
            where T : class
        {
            if (TypeRepresentation is T t)
            {
                return t;
            }
            throw new NotImplementedException("Add other outputs like a Roslyn SyntaxNode");
        }
    }
}
