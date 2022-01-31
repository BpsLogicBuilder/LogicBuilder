using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class ParametersXmlParser : IParametersXmlParser
    {
        private readonly IContextProvider _contextProvider;

        public ParametersXmlParser(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public ParameterBase Parse(XmlElement xmlElement) 
            => new ParameterParser(xmlElement, _contextProvider).Parameter;
    }
}
