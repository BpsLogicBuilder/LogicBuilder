using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class ApplicationXmlParser : IApplicationXmlParser
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IProjectPropertiesItemFactory _projectPropertiesItemFactory;
        private readonly IWebApiDeploymentXmlParser _webApiDeploymentXmlParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ApplicationXmlParser(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IProjectPropertiesItemFactory projectPropertiesItemFactory,
            IWebApiDeploymentXmlParser webApiDeploymentXmlParser,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _projectPropertiesItemFactory = projectPropertiesItemFactory;
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _webApiDeploymentXmlParser = webApiDeploymentXmlParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public Application Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.APPLICATIONELEMENT)
                throw _exceptionHelper.CriticalException("{2DD6B7A0-86AD-43BF-8D09-C1B0CB546DF4}");

            return GetApplication
            (
                _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)
            );

            Application GetApplication(Dictionary<string, XmlElement> elements)
                => _projectPropertiesItemFactory.GetApplication
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
                    _webApiDeploymentXmlParser.Parse(elements[XmlDataConstants.WEBAPIDEPLOYMENTELEMENT])
                );
        }
    }
}
