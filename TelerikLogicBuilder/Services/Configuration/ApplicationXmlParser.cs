using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class ApplicationXmlParser : IApplicationXmlParser
    {
        private readonly IContextProvider _contextProvider;
        private readonly IWebApiDeploymentXmlParser _webApiDeploymentXmlParser;

        public ApplicationXmlParser(IContextProvider contextProvider, IWebApiDeploymentXmlParser webApiDeploymentXmlParser)
        {
            _contextProvider = contextProvider;
            _webApiDeploymentXmlParser = webApiDeploymentXmlParser;
        }

        public Application Parse(XmlElement xmlElement) 
            => new ApplicationXmlParserUtility(xmlElement, _contextProvider, _webApiDeploymentXmlParser).Application;
    }
}
