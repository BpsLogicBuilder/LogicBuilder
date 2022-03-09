using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.GenericArguments;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.GenericArguments
{
    internal class GenericConfigXmlParser : IGenericConfigXmlParser
    {
        private readonly IContextProvider _contextProvider;

        public GenericConfigXmlParser(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public GenericConfigBase Parse(XmlElement xmlElement)
            => new GenericConfigXmlParserUtility(xmlElement, _contextProvider).GenericConfig;
    }
}
