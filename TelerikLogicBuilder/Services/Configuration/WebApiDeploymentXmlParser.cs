using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class WebApiDeploymentXmlParser : IWebApiDeploymentXmlParser
    {
        private readonly IContextProvider _contextProvider;

        public WebApiDeploymentXmlParser(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public WebApiDeployment Parse(XmlElement xmlElement) 
            => new WebApiDeploymentXmlParserUtility(xmlElement, _contextProvider).WebApiDeployment;
    }
}
