using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense
{
    internal class ConstructorXmlParser : IConstructorXmlParser
    {
        private readonly IContextProvider _contextProvider;

        public ConstructorXmlParser(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public Constructor Parse(XmlElement xmlElement) 
            => new ConstructorXmlParserUtility(xmlElement, _contextProvider).Constructor;
    }
}
