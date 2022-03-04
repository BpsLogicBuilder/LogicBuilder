using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration.Initialization
{
    internal class ConstructorDictionaryBuilder : IConstructorDictionaryBuilder
    {
        private readonly IConstructorXmlParser _constructorXmlParser;

        public ConstructorDictionaryBuilder(IConstructorXmlParser constructorXmlParser)
        {
            _constructorXmlParser = constructorXmlParser;
        }

        public IDictionary<string, Constructor> GetDictionary(XmlDocument xmlDocument) 
            => xmlDocument
                .SelectNodes($"//{XmlDataConstants.CONSTRUCTORELEMENT}")!/*Never null when SelectNodes is called on an XmlDocument*/
                .OfType<XmlElement>()
                .ToDictionary
                (
                    e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                    e => _constructorXmlParser.Parse(e)
                );
    }
}
