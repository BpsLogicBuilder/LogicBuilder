using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Variables
{
    internal class VariablesXmlParser : IVariablesXmlParser
    {
        private readonly IContextProvider _contextProvider;

        public VariablesXmlParser(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public IDictionary<string, VariableBase> GetVariablesDictionary(XmlDocument xmlDocument)
            => xmlDocument.SelectNodes
            (
                string.Format
                (
                    CultureInfo.InvariantCulture, 
                    "//{0}|//{1}|//{2}|//{3}", 
                    XmlDataConstants.LITERALVARIABLEELEMENT, 
                    XmlDataConstants.OBJECTVARIABLEELEMENT, 
                    XmlDataConstants.LITERALLISTVARIABLEELEMENT, 
                    XmlDataConstants.OBJECTLISTVARIABLEELEMENT
                )
            )!/*Never null when SelectNodes is called on an XmlDocument*/
            .OfType<XmlElement>()
            .ToDictionary
            (
                e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                e => Parse(e)
            );

        public VariableBase Parse(XmlElement xmlElement) 
            => new VariablesXmlParserUtility(xmlElement, _contextProvider).Variable;
    }
}
