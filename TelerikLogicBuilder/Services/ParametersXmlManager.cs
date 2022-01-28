using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class ParametersXmlManager : IParametersXmlManager
    {
        private readonly IContextProvider _contextProvider;

        public ParametersXmlManager(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public ParameterBase BuildParameter(XmlElement xmlElement) 
            => new ParameterBuilder(xmlElement, _contextProvider).Parameter;
    }
}
