using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class FragmentXmlParser : IFragmentXmlParser
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFragmentItemFactory _fragmentItemFactory;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public FragmentXmlParser(
            IExceptionHelper exceptionHelper,
            IFragmentItemFactory fragmentItemFactory,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _fragmentItemFactory = fragmentItemFactory;
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public Fragment Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.FRAGMENTELEMENT)
                throw _exceptionHelper.CriticalException("{B0C817E4-F83C-4140-9850-ECE0F5BE3506}");

            return _fragmentItemFactory.GetFragment
            (
                xmlElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value,
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
