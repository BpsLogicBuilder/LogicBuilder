using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Parameters
{
    internal class ParametersXmlParser : IParametersXmlParser
    {
        private readonly IContextProvider _contextProvider;

        public ParametersXmlParser(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public ParameterBase Parse(XmlElement xmlElement) 
            => new ParametersXmlParserUtility(xmlElement, _contextProvider).Parameter;
    }
}
