using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.ReflectionModel.XmlDocument
{
    public static class HelpFromXmlComments
    {
        public static CommandLineBuilder ConfigureHelpFromXmlComments(
             this CommandLineBuilder builder,
             MethodInfo method,
             string xmlDocsFilePath)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            if (XmlDocReader.TryLoad(xmlDocsFilePath ?? GetDefaultXmlDocsFileLocation(method.DeclaringType.Assembly), out var xmlDocs))
            {
                if (xmlDocs.TryGetMethodDescription(method, out MethodHelpMetadata metadata) &&
                    metadata.Description != null)
                {
                    builder.Command.Description = metadata.Description;
                    var options = builder.Options.ToArray();

                    foreach (var parameterDescription in metadata.ParameterDescriptions)
                    {
                        var kebabCasedParameterName = parameterDescription.Key.ToKebabCase();

                        var option = options.FirstOrDefault(o => o.HasAlias(kebabCasedParameterName));

                        if (option != null)
                        {
                            option.Description = parameterDescription.Value;
                        }
                        else
                        {
                            foreach (var argument in builder.Command.Arguments)
                            {
                                if (string.Equals(
                                        argument.Name,
                                        kebabCasedParameterName,
                                        StringComparison.OrdinalIgnoreCase))
                                {
                                    argument.Description = parameterDescription.Value;
                                }
                            }
                        }
                    }

                    metadata.Name = method.DeclaringType.Assembly.GetName().Name;
                }
            }

            return builder;
        }


        private static string GetDefaultXmlDocsFileLocation(Assembly assembly) 
            => Path.Combine(
                Path.GetDirectoryName(assembly.Location),
                Path.GetFileNameWithoutExtension(assembly.Location) + ".xml");
    }
}
