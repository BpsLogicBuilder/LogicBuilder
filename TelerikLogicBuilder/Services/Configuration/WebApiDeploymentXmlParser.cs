using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class WebApiDeploymentXmlParser : IWebApiDeploymentXmlParser
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IWebApiDeploymentItemFactory _webApiDeploymentItemFactory;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public WebApiDeploymentXmlParser(
            IExceptionHelper exceptionHelper,
            IWebApiDeploymentItemFactory webApiDeploymentItemFactory,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _exceptionHelper = exceptionHelper;
            _webApiDeploymentItemFactory = webApiDeploymentItemFactory;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public WebApiDeployment Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.WEBAPIDEPLOYMENTELEMENT)
                throw _exceptionHelper.CriticalException("{634152FE-E383-4A37-A65E-21E206D7F6A7}");

            return GetWebApiDeployment
            (
                _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)
            );

            WebApiDeployment GetWebApiDeployment(Dictionary<string, XmlElement> elements)
                => _webApiDeploymentItemFactory.GetWebApiDeployment
                (
                    elements[XmlDataConstants.POSTFILEDATAURLELEMENT].InnerText,
                    elements[XmlDataConstants.POSTVARIABLESMETADATAURLELEMENT].InnerText,
                    elements[XmlDataConstants.DELETERULESURLELEMENT].InnerText,
                    elements[XmlDataConstants.DELETEALLRULESURLELEMENT].InnerText
                );
        }
    }
}
