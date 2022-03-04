using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration
{
    internal class ApplicationXmlParserUtility
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEnumHelper _enumHelper;
        private readonly IWebApiDeploymentXmlParser _webApiDeploymentXmlParser;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IContextProvider _contextProvider;
        private readonly XmlElement xmlElement;

        public ApplicationXmlParserUtility(XmlElement xmlElement, IContextProvider contextProvider, IWebApiDeploymentXmlParser webApiDeploymentXmlParser)
        {
            this.xmlElement = xmlElement;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _enumHelper = contextProvider.EnumHelper;
            _exceptionHelper = contextProvider.ExceptionHelper;
            _contextProvider = contextProvider;
            _webApiDeploymentXmlParser = webApiDeploymentXmlParser;
        }

        internal Application Application
        {
            get
            {
                if (xmlElement.Name != XmlDataConstants.APPLICATIONELEMENT)
                    throw _exceptionHelper.CriticalException("{2DD6B7A0-86AD-43BF-8D09-C1B0CB546DF4}");

                return GetApplication
                (
                    _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)
                );

                Application GetApplication(Dictionary<string, XmlElement> elements)
                    => new
                    (
                        xmlElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                        xmlElement.GetAttribute(XmlDataConstants.NICKNAMEATTRIBUTE),
                        elements[XmlDataConstants.ACTIVITYASSEMBLYELEMENT].InnerText.Trim(),
                        elements[XmlDataConstants.ACTIVITYASSEMBLYPATHELEMENT].InnerText.Trim(),
                        _enumHelper.ParseEnumText<RuntimeType>(elements[XmlDataConstants.RUNTIMEELEMENT].InnerText.Trim()),
                        _xmlDocumentHelpers.GetChildElements(elements[XmlDataConstants.LOADASSEMBLYPATHSELEMENT]).Select(e => e.InnerText).ToList(),
                        elements[XmlDataConstants.ACTIVITYCLASSELEMENT].InnerText.Trim(),
                        elements[XmlDataConstants.APPLICATIONEXECUTABLEELEMENT].InnerText.Trim(),
                        elements[XmlDataConstants.APPLICATIONEXECUTABLEPATHELEMENT].InnerText.Trim(),
                        _xmlDocumentHelpers.GetChildElements(elements[XmlDataConstants.STARTUPARGUMENTSELEMENT]).Select(e => e.InnerText).ToList(),
                        elements[XmlDataConstants.RESOURCEFILEELEMENT].InnerText.Trim(),
                        elements[XmlDataConstants.RESOURCEFILEDEPLOYMENTPATHELEMENT].InnerText.Trim(),
                        elements[XmlDataConstants.RULESFILEELEMENT].InnerText.Trim(),
                        elements[XmlDataConstants.RULESDEPLOYMENTPATHELEMENT].InnerText.Trim(),
                        _xmlDocumentHelpers.GetChildElements(elements[XmlDataConstants.EXCLUDEDMODULESELEMENT]).Select(e => e.InnerText).ToList(),
                        _webApiDeploymentXmlParser.Parse(elements[XmlDataConstants.WEBAPIDEPLOYMENTELEMENT]),
                        _contextProvider
                    );
            }
        }
    }
}
