using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments
{
    internal class GenericConfigXmlParserUtility
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IEnumHelper _enumHelper;
        private readonly IContextProvider _contextProvider;

        public GenericConfigXmlParserUtility(XmlElement xmlElement, IContextProvider contextProvider)
        {
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _exceptionHelper = contextProvider.ExceptionHelper;
            _enumHelper = contextProvider.EnumHelper;
            _contextProvider = contextProvider;
            this.xmlElement = xmlElement;
            this.genericConfigCategory = _enumHelper.GetGenericConfigCategory(this.xmlElement.Name);
        }

        #region Fields
        private readonly XmlElement xmlElement;
        private readonly GenericConfigCategory genericConfigCategory;
        #endregion Fields

        #region Properties
        internal GenericConfigBase GenericConfig 
            => this.genericConfigCategory switch
            {
                GenericConfigCategory.Literal => GetLiteralGenericConfig
                (
                    xmlElement.GetAttribute(XmlDataConstants.GENERICARGUMENTNAMEATTRIBUTE),
                    _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)
                ),
                GenericConfigCategory.Object => GetObjectGenericConfig
                (
                    xmlElement.GetAttribute(XmlDataConstants.GENERICARGUMENTNAMEATTRIBUTE),
                    _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)
                ),
                GenericConfigCategory.LiteralList => GetLiteralListGenericConfig
                (
                    xmlElement.GetAttribute(XmlDataConstants.GENERICARGUMENTNAMEATTRIBUTE),
                    _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)
                ),
                GenericConfigCategory.ObjectList => GetObjectListGenericConfig
                (
                    xmlElement.GetAttribute(XmlDataConstants.GENERICARGUMENTNAMEATTRIBUTE),
                    _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)
                ),
                _ => throw _exceptionHelper.CriticalException("{F8179AFE-CC33-4B89-97FA-725E59D4D530}"),
            };
        #endregion Properties

        #region Methods
        private GenericConfigBase GetLiteralGenericConfig(string genericArgumentName, Dictionary<string, XmlElement> elements) 
            => new LiteralGenericConfig
            (
                genericArgumentName,
                _enumHelper.ParseEnumText<LiteralParameterType>(elements[XmlDataConstants.LITERALTYPEELEMENT].InnerText),
                _enumHelper.ParseEnumText<LiteralParameterInputStyle>(elements[XmlDataConstants.CONTROLELEMENT].InnerText),
                bool.Parse(elements[XmlDataConstants.USEFOREQUALITYELEMENT].InnerText),
                bool.Parse(elements[XmlDataConstants.USEFORHASHCODEELEMENT].InnerText),
                bool.Parse(elements[XmlDataConstants.USEFORTOSTRINGELEMENT].InnerText),
                elements[XmlDataConstants.PROPERTYSOURCEELEMENT].InnerText,
                elements[XmlDataConstants.PROPERTYSOURCEPARAMETERELEMENT].InnerText,
                elements[XmlDataConstants.DEFAULTVALUEELEMENT].InnerText,
                _xmlDocumentHelpers
                    .GetChildElements(elements[XmlDataConstants.DOMAINELEMENT])
                    .Select(e => e.InnerText)
                    .ToList(),
                _contextProvider
            );

        private GenericConfigBase GetObjectGenericConfig(string genericArgumentName, Dictionary<string, XmlElement> elements) 
            => new ObjectGenericConfig
            (
                genericArgumentName,
                elements[XmlDataConstants.OBJECTTYPEELEMENT].InnerText,
                bool.Parse(elements[XmlDataConstants.USEFOREQUALITYELEMENT].InnerText),
                bool.Parse(elements[XmlDataConstants.USEFORHASHCODEELEMENT].InnerText),
                bool.Parse(elements[XmlDataConstants.USEFORTOSTRINGELEMENT].InnerText),
                _contextProvider
            );

        private GenericConfigBase GetLiteralListGenericConfig(string genericArgumentName, Dictionary<string, XmlElement> elements) 
            => new LiteralListGenericConfig
            (
                genericArgumentName,
                _enumHelper.ParseEnumText<LiteralParameterType>(elements[XmlDataConstants.LITERALTYPEELEMENT].InnerText),
                _enumHelper.ParseEnumText<ListType>(elements[XmlDataConstants.LISTTYPEELEMENT].InnerText),
                _enumHelper.ParseEnumText<ListParameterInputStyle>(elements[XmlDataConstants.CONTROLELEMENT].InnerText),
                _enumHelper.ParseEnumText<LiteralParameterInputStyle>(elements[XmlDataConstants.ELEMENTCONTROLELEMENT].InnerText),
                elements[XmlDataConstants.PROPERTYSOURCEELEMENT].InnerText,
                elements[XmlDataConstants.PROPERTYSOURCEPARAMETERELEMENT].InnerText,
                _xmlDocumentHelpers
                    .GetChildElements(elements[XmlDataConstants.DEFAULTVALUEELEMENT])
                    .Select(e => e.InnerText)
                    .ToList(),
                _xmlDocumentHelpers
                    .GetChildElements(elements[XmlDataConstants.DOMAINELEMENT])
                    .Select(e => e.InnerText)
                    .ToList(),
                _contextProvider
            );

        private GenericConfigBase GetObjectListGenericConfig(string genericArgumentName, Dictionary<string, XmlElement> elements) 
            => new ObjectListGenericConfig
            (
                genericArgumentName,
                elements[XmlDataConstants.OBJECTTYPEELEMENT].InnerText,
                _enumHelper.ParseEnumText<ListType>(elements[XmlDataConstants.LISTTYPEELEMENT].InnerText),
                _enumHelper.ParseEnumText<ListParameterInputStyle>(elements[XmlDataConstants.CONTROLELEMENT].InnerText),
                _contextProvider
            );
        #endregion Methods
    }
}
