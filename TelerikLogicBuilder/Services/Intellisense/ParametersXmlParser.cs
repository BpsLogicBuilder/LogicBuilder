using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense
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
