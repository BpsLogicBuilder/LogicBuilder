using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration.Initialization
{
    internal class VariableDictionaryBuilder : IVariableDictionaryBuilder
    {
        private readonly IVariablesXmlParser _variablesXmlParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public VariableDictionaryBuilder(IVariablesXmlParser variablesXmlParser, IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _variablesXmlParser = variablesXmlParser; 
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public IDictionary<string, VariableBase> GetDictionary(XmlDocument xmlDocument)
            => _xmlDocumentHelpers.SelectElements
                (
                    xmlDocument,
                    $"//{XmlDataConstants.LITERALVARIABLEELEMENT}|//{XmlDataConstants.OBJECTVARIABLEELEMENT}|//{XmlDataConstants.LITERALLISTVARIABLEELEMENT}|//{XmlDataConstants.OBJECTLISTVARIABLEELEMENT}"
                )
                .ToDictionary
                (
                    e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                    e => _variablesXmlParser.Parse(e)
                );
    }
}
