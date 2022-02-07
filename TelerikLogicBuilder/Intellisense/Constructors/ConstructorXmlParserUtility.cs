using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors
{
    internal class ConstructorXmlParserUtility
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IParametersXmlParser _parametersXmlParser;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IContextProvider _contextProvider;

        internal ConstructorXmlParserUtility(XmlElement xmlElement, IContextProvider contextProvider)
        {
            this.xmlElement = xmlElement;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _parametersXmlParser = contextProvider.ParametersXmlParser;
            _exceptionHelper = contextProvider.ExceptionHelper;
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
                if (xmlElement.Name != XmlDataConstants.CONSTRUCTORELEMENT)
                    throw _exceptionHelper.CriticalException("{09D4676D-0042-46FF-AF14-BF4555B8AE41}");

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
