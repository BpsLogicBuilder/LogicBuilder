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
                .SelectNodes($"//{XmlDataConstants.CONSTRUCTORELEMENT}")
                .OfType<XmlElement>()
                .ToDictionary
                (
                    e => e.Attributes[XmlDataConstants.NAMEATTRIBUTE].Value,
                    e => _constructorXmlParser.Parse(e)
                );
    }
}
