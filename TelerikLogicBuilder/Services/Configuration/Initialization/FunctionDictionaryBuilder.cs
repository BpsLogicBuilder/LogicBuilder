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
        private readonly IFunctionXmlParser _functionXmlParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public FunctionDictionaryBuilder(IFunctionXmlParser functionXmlParser, IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _functionXmlParser = functionXmlParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public IDictionary<string, Function> GetDictionary(XmlDocument xmlDocument)
            => _xmlDocumentHelpers.SelectElements
            (
                xmlDocument, 
                $"//{XmlDataConstants.FUNCTIONELEMENT}"
            )
            .ToDictionary
            (
                e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                e => _functionXmlParser.Parse(e)
            );
    }
}
