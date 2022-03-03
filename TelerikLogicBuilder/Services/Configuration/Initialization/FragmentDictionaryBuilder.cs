using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
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

        public FragmentDictionaryBuilder(IFragmentXmlParser fragmentXmlParser)
        {
            _fragmentXmlParser = fragmentXmlParser;
        }

        public IDictionary<string, Fragment> GetDictionary(XmlDocument xmlDocument)
            => xmlDocument
                .SelectNodes($"//{XmlDataConstants.FRAGMENTELEMENT}")!/*Never null when SelectNodes is called on an XmlDocument*/
                .OfType<XmlElement>()
                .ToDictionary
                (
                    e => e.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value,/*Attribute is required by schema definition*/
                    e => _fragmentXmlParser.Parse(e)
                );
    }
}
