using System.CommandLine.ReflectionModel.XmlDocument;
using System.IO;
using System.Reflection;
using System.CommandLine.GeneralAppModel;

namespace System.CommandLine.ReflectionModel.Strategies
{
    public class XmlDocStrategy : StrategyBase
    {
        public XmlDocStrategy(string xmlDocsFilePath = null, SymbolType symbolType = SymbolType.All)
        : base(symbolType)
            => this.xmlDocsFilePath = xmlDocsFilePath;

        private readonly string xmlDocsFilePath;
        private XmlDocReader _docReader;
        private bool _loadAttempted = false;
        private XmlDocReader DocReader
        {
            get
            {
                if (!_loadAttempted)
                {
                    _docReader = GetNewDocReader();
                }
                return _docReader;
            }
        }

        private XmlDocReader GetNewDocReader()
        {
            _loadAttempted = true;
            return XmlDocReader.TryLoad(xmlDocsFilePath ?? GetDefaultXmlDocsFileLocation(Assembly.GetExecutingAssembly()), out var xmlDocs)
                           ? xmlDocs
                           : null;
        }

        private static string GetDefaultXmlDocsFileLocation(Assembly assembly)
          => Path.Combine(
              Path.GetDirectoryName(assembly.Location),
              Path.GetFileNameWithoutExtension(assembly.Location) + ".xml");

        private (bool found, string description) GetDescriptionInternal(MethodInfo methodInfo, string name)
        => DocReader is null
            ? ((bool found, string description))(false, null)
            : DocReader.TryGetMethodDescription(methodInfo, out MethodHelpMetadata commandHelpMetdata)
                        && commandHelpMetdata.ParameterDescriptions.TryGetValue(name, out string parameterDescription)
                ? (true, parameterDescription)
                : ((bool found, string description))(false, null);

        private (bool found, string description) GetDescriptionInternal(PropertyInfo propertyInfo, string name) 
        => DocReader is null
            ? ((bool found, string description))(false, null)
            : DocReader.TryGetTypeDescription(propertyInfo.DeclaringType, out TypeHelpMetadata typeHelpMetdata)
                            && typeHelpMetdata.PropertyDescriptions.TryGetValue(name, out string propertyDescription)
                ? (true, propertyDescription)
                : ((bool found, string description))(false, null);

        private (bool found, string description) GetDescriptionInternal(Type type, string name)
        => DocReader is null
            ? ((bool found, string description))(false, null)
            : DocReader.TryGetTypeDescription(type, out TypeHelpMetadata typeHelpMetdata)
                ? (true, typeHelpMetdata.Description)
                : ((bool found, string description))(false, null);

        internal (bool found, string description) GetDescription(object info, string name)
        => info switch
        {
            MethodInfo methodInfo => GetDescriptionInternal(methodInfo, name),
            PropertyInfo propertyInfo => GetDescriptionInternal(propertyInfo, name),
            Type type => GetDescriptionInternal(type, name),
            _ => (false, null)
        };

        public override string StrategyDescription => "XML Documentation Strategy";
    }
}
