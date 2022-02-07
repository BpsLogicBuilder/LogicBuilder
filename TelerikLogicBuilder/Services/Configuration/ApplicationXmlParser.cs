using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class ApplicationXmlParser : IApplicationXmlParser
    {
        private readonly IContextProvider _contextProvider;

        public ApplicationXmlParser(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public Application Parse(XmlElement xmlElement) 
            => new ApplicationXmlParserUtility(xmlElement, _contextProvider).Application;
    }
}
