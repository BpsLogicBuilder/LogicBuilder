using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Variables
{
    internal class VariablesXmlParser : IVariablesXmlParser
    {
        private readonly IContextProvider _contextProvider;

        public VariablesXmlParser(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public VariableBase Parse(XmlElement xmlElement) 
            => new VariablesXmlParserUtility(xmlElement, _contextProvider).Variable;
    }
}
