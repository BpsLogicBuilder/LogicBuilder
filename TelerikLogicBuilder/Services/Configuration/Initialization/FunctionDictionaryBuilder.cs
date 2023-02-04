using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration.Initialization
{
    internal class FunctionDictionaryBuilder : IFunctionDictionaryBuilder
    {
        private readonly IFunctionListMatcher _functionListMatcher;
        private readonly IFunctionXmlParser _functionXmlParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public FunctionDictionaryBuilder(
            IFunctionListMatcher functionListMatcher,
            IFunctionXmlParser functionXmlParser,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _functionListMatcher = functionListMatcher;
            _functionXmlParser = functionXmlParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public IDictionary<string, Function> GetBooleanFunctionDictionary(XmlDocument xmlDocument)
            => GetElements(xmlDocument)
                .Select(_functionXmlParser.Parse)
                .Where(_functionListMatcher.IsBoolFunction)
                .ToDictionary(e => e.Name);

        public IDictionary<string, Function> GetDialogFunctionDictionary(XmlDocument xmlDocument)
            => GetElements(xmlDocument)
                .Select(_functionXmlParser.Parse)
                .Where(_functionListMatcher.IsDialogFunction)
                .ToDictionary(e => e.Name);

        public IDictionary<string, Function> GetDictionary(XmlDocument xmlDocument)
            => GetElements(xmlDocument)
                .Select(_functionXmlParser.Parse)
                .ToDictionary(e => e.Name);

        public IDictionary<string, Function> GetTableFunctionDictionary(XmlDocument xmlDocument)
            => GetElements(xmlDocument)
                .Select(_functionXmlParser.Parse)
                .Where(_functionListMatcher.IsTableFunction)
                .ToDictionary(e => e.Name);

        public IDictionary<string, Function> GetValueFunctionDictionary(XmlDocument xmlDocument)
            => GetElements(xmlDocument)
                .Select(_functionXmlParser.Parse)
                .Where(_functionListMatcher.IsValueFunction)
                .ToDictionary(e => e.Name);

        public IDictionary<string, Function> GetVoidFunctionDictionary(XmlDocument xmlDocument)
            => GetElements(xmlDocument)
                .Select(_functionXmlParser.Parse)
                .Where(_functionListMatcher.IsVoidFunction)
                .ToDictionary(e => e.Name);

        private IList<XmlElement> GetElements(XmlDocument xmlDocument) 
            => _xmlDocumentHelpers.SelectElements
            (
                xmlDocument,
                $"//{XmlDataConstants.FUNCTIONELEMENT}"
            );
    }
}
