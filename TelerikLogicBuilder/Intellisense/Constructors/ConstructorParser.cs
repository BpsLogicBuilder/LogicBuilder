using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors
{
    internal class ConstructorParser
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IParametersXmlParser _parametersXmlParser;
        private readonly IContextProvider _contextProvider;

        internal ConstructorParser(XmlElement xmlElement, IContextProvider contextProvider)
        {
            this.xmlElement = xmlElement;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _parametersXmlParser = contextProvider.ParametersXmlParser;
            _contextProvider = contextProvider;
        }

        #region Fields
        private readonly XmlElement xmlElement;
        #endregion Fields

        #region Properties
        internal Constructor Constructor
        {
            get
            {
                return GetConstructor
                (
                    _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)
                );

                Constructor GetConstructor(Dictionary<string, XmlElement> elements)
                    => new                    (
                        xmlElement.Attributes[XmlDataConstants.NAMEATTRIBUTE].Value,
                        elements[XmlDataConstants.TYPENAMEELEMENT].InnerText,
                        _xmlDocumentHelpers.GetChildElements(elements[XmlDataConstants.PARAMETERSELEMENT]).Select(e => _parametersXmlParser.Parse(e)).ToList(),
                        new List<string>(_xmlDocumentHelpers.GetChildElements(elements[XmlDataConstants.GENERICARGUMENTSELEMENT]).Select(e => e.InnerText)),
                        elements[XmlDataConstants.SUMMARYELEMENT].InnerText,
                        this._contextProvider
                    );
            }
        }
        #endregion Properties
    }
}
