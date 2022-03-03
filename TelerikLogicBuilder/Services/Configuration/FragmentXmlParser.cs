using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class FragmentXmlParser : IFragmentXmlParser
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IContextProvider _contextProvider;

        public FragmentXmlParser(IContextProvider contextProvider)
        {
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _exceptionHelper = contextProvider.ExceptionHelper;
            _contextProvider = contextProvider;
        }

        public Fragment Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.FRAGMENTELEMENT)
                throw _exceptionHelper.CriticalException("{B0C817E4-F83C-4140-9850-ECE0F5BE3506}");

            return new Fragment
            (
                xmlElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value,/*Attribute is required by schema definition*/
                _xmlDocumentHelpers.GetXmlString
                (
                    _xmlDocumentHelpers.ToXmlDocument(xmlElement.FirstChild!)/*Fragment element always has a first child by schema definition*/
                ),
                _contextProvider
            ); ;
        }
    }
}
