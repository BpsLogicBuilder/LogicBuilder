using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration.Initialization
{
    internal class FragmentDictionaryBuilder : IFragmentDictionaryBuilder
    {
        private readonly IFragmentXmlParser _fragmentXmlParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public FragmentDictionaryBuilder(IFragmentXmlParser fragmentXmlParser, IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _fragmentXmlParser = fragmentXmlParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public IDictionary<string, Fragment> GetDictionary(XmlDocument xmlDocument)
            => _xmlDocumentHelpers.SelectElements
            (
                xmlDocument, 
                $"//{XmlDataConstants.FRAGMENTELEMENT}"
            )
            .ToDictionary
            (
                e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                e => _fragmentXmlParser.Parse(e)
            );
    }
}
