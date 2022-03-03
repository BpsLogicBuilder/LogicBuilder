using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
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

        public VariableDictionaryBuilder(IVariablesXmlParser variablesXmlParser)
        {
            _variablesXmlParser = variablesXmlParser;
        }

        public IDictionary<string, VariableBase> GetDictionary(XmlDocument xmlDocument)
            => xmlDocument
                .SelectNodes
                (
                    $"//{XmlDataConstants.LITERALVARIABLEELEMENT}|//{XmlDataConstants.OBJECTVARIABLEELEMENT}|//{XmlDataConstants.LITERALLISTVARIABLEELEMENT}|//{XmlDataConstants.OBJECTLISTVARIABLEELEMENT}"
                )!/*Never null when SelectNodes is called on an XmlDocument*/
                .OfType<XmlElement>()
                .ToDictionary
                (
                    e => e.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value,/*Attribute is required by schema definition*/
                    e => _variablesXmlParser.Parse(e)
                );
    }
}
