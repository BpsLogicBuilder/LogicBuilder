using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
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

        public FunctionDictionaryBuilder(IFunctionXmlParser functionXmlParser)
        {
            _functionXmlParser = functionXmlParser;
        }

        public IDictionary<string, Function> GetDictionary(XmlDocument xmlDocument)
            => xmlDocument
                .SelectNodes($"//{XmlDataConstants.FUNCTIONELEMENT}")!/*Never null when SelectNodes is called on an XmlDocument*/
                .OfType<XmlElement>()
                .ToDictionary
                (
                    e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                    e => _functionXmlParser.Parse(e)
                );
    }
}
