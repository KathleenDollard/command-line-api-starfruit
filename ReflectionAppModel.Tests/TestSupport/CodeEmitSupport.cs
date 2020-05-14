using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.CommandLine;
using System.CommandLine.GeneralAppModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace ReflectionAppModel.Tests.TestSupport
{
    class CodeEmitSupport
    {
        /// <summary>
        /// Create code from strings for testing
        /// </summary>
        /// <param name="code">The source code to emit</param>
        /// <returns>The assembly created</returns>
        /// <example>
        /// var code = @"
        ///            using System; 
        ///            namespace test
        ///            {
        ///                public class firstCommand
        ///                { }
        ///            
        ///            }";
        ///            var assem = CodeEmitSupport.GetCode(code);
        /// </example>
        public static Assembly GetCode(string code)
        {
            var dd = typeof(Enumerable).GetTypeInfo().Assembly.Location;
            var coreDir = Directory.GetParent(dd);

            var compilation = CSharpCompilation.Create(Guid.NewGuid().ToString() + ".dll")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(
                MetadataReference.CreateFromFile(typeof(Object).GetTypeInfo().Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Uri).GetTypeInfo().Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Command).GetTypeInfo().Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Strategy).GetTypeInfo().Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.CommandLine.ReflectionAppModel.ReflectionAppModel).GetTypeInfo().Assembly.Location),
                MetadataReference.CreateFromFile(coreDir.FullName + Path.DirectorySeparatorChar + "mscorlib.dll"),
                MetadataReference.CreateFromFile(coreDir.FullName + Path.DirectorySeparatorChar + "System.Runtime.dll")
                )
                .AddSyntaxTrees(CSharpSyntaxTree.ParseText(code));

            string fileName = string.Format("{0}.dll", compilation.AssemblyName);
            if (null != compilation && !string.IsNullOrEmpty(compilation.AssemblyName))
            {
                using var stream = new MemoryStream();
                var result = compilation.Emit(stream);
                if (result.Success)
                {

                    using FileStream file = File.Create(fileName);
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(file);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

            var a = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.GetFullPath(fileName));

            return a;
        }
    }
}
