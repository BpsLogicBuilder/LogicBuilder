using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class ConstructorXmlParser : IConstructorXmlParser
    {
        private readonly IContextProvider _contextProvider;

        public ConstructorXmlParser(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public Constructor Parse(XmlElement xmlElement) 
            => new ConstructorParser(xmlElement, _contextProvider).Constructor;
    }
}
