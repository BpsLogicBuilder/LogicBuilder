using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Constructors
{
    internal class ConstructorXmlParser : IConstructorXmlParser
    {
        private readonly IContextProvider _contextProvider;
        private readonly IParametersXmlParser _parametersXmlParser;

        public ConstructorXmlParser(IContextProvider contextProvider, IParametersXmlParser parametersXmlParser)
        {
            _contextProvider = contextProvider;
            _parametersXmlParser = parametersXmlParser;
        }

        public Constructor Parse(XmlElement xmlElement) 
            => new ConstructorXmlParserUtility(xmlElement, _contextProvider, _parametersXmlParser).Constructor;
    }
}
