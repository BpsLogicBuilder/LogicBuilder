using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class FragmentXmlParser : IFragmentXmlParser
    {
        private readonly IContextProvider _contextProvider;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public FragmentXmlParser(
            IContextProvider contextProvider,
            IExceptionHelper exceptionHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _contextProvider = contextProvider;
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public Fragment Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.FRAGMENTELEMENT)
                throw _exceptionHelper.CriticalException("{B0C817E4-F83C-4140-9850-ECE0F5BE3506}");

            return new Fragment
            (
                xmlElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                _xmlDocumentHelpers.GetXmlString
                (
                    _xmlDocumentHelpers.ToXmlDocument
                    (
                        _xmlDocumentHelpers.GetSingleChildElement(xmlElement)
                    )
                ),
                _contextProvider
            ); ;
        }
    }
}
