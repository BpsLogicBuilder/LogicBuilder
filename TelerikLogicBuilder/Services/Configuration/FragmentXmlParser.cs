using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class FragmentXmlParser : IFragmentXmlParser
    {
        private readonly IConfigurationItemFactory _configurationItemFactory;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public FragmentXmlParser(
            IConfigurationItemFactory configurationItemFactory,
            IExceptionHelper exceptionHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _configurationItemFactory = configurationItemFactory;
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public Fragment Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.FRAGMENTELEMENT)
                throw _exceptionHelper.CriticalException("{B0C817E4-F83C-4140-9850-ECE0F5BE3506}");

            return _configurationItemFactory.GetFragment
            (
                xmlElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                _xmlDocumentHelpers.GetXmlString
                (
                    _xmlDocumentHelpers.ToXmlDocument
                    (
                        _xmlDocumentHelpers.GetSingleChildElement(xmlElement)
                    )
                )
            );
        }
    }
}
