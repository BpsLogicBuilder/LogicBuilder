using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration
{
    internal class WebApiDeploymentXmlParserUtility
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IContextProvider _contextProvider;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly XmlElement xmlElement;

        public WebApiDeploymentXmlParserUtility(XmlElement xmlElement, IContextProvider contextProvider)
        {
            this.xmlElement = xmlElement;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _exceptionHelper = contextProvider.ExceptionHelper;
            _contextProvider = contextProvider;
        }

        internal WebApiDeployment WebApiDeployment
        {
            get
            {
                if (xmlElement.Name != XmlDataConstants.WEBAPIDEPLOYMENTELEMENT)
                    throw _exceptionHelper.CriticalException("{634152FE-E383-4A37-A65E-21E206D7F6A7}");

                return GetWebApiDeployment
                (
                    _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)
                );

                WebApiDeployment GetWebApiDeployment(Dictionary<string, XmlElement> elements)
                    => new                    
                    (
                        elements[XmlDataConstants.POSTFILEDATAURLELEMENT].InnerText,
                        elements[XmlDataConstants.POSTVARIABLESMETADATAURLELEMENT].InnerText,
                        elements[XmlDataConstants.DELETERULESURLELEMENT].InnerText,
                        elements[XmlDataConstants.DELETEALLRULESURLELEMENT].InnerText,
                        _contextProvider
                    );
            }
        }
    }
}
