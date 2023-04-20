using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.GenericArguments;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.GenericArguments
{
    internal class GenericConfigXmlParser : IGenericConfigXmlParser
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IEnumHelper _enumHelper;
        private readonly IGenericConfigFactory _genericConfigFactory;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public GenericConfigXmlParser(
            IExceptionHelper exceptionHelper,
            IEnumHelper enumHelper,
            IGenericConfigFactory genericConfigFactory,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _exceptionHelper = exceptionHelper;
            _enumHelper = enumHelper;
            _genericConfigFactory = genericConfigFactory;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        #region Methods
        public GenericConfigBase Parse(XmlElement xmlElement)
        {
            GenericConfigCategory genericConfigCategory = _enumHelper.GetGenericConfigCategory(xmlElement.Name);

            return genericConfigCategory switch
            {
                GenericConfigCategory.Literal => GetLiteralGenericConfig
                (
                    xmlElement.Attributes[XmlDataConstants.GENERICARGUMENTNAMEATTRIBUTE]!.Value,
                    _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)
                ),
                GenericConfigCategory.Object => GetObjectGenericConfig
                (
                    xmlElement.Attributes[XmlDataConstants.GENERICARGUMENTNAMEATTRIBUTE]!.Value,
                    _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)
                ),
                GenericConfigCategory.LiteralList => GetLiteralListGenericConfig
                (
                    xmlElement.Attributes[XmlDataConstants.GENERICARGUMENTNAMEATTRIBUTE]!.Value,
                    _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)
                ),
                GenericConfigCategory.ObjectList => GetObjectListGenericConfig
                (
                    xmlElement.Attributes[XmlDataConstants.GENERICARGUMENTNAMEATTRIBUTE]!.Value,
                    _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)
                ),
                _ => throw _exceptionHelper.CriticalException("{F8179AFE-CC33-4B89-97FA-725E59D4D530}"),
            };
        }

        private GenericConfigBase GetLiteralGenericConfig(string genericArgumentName, Dictionary<string, XmlElement> elements)
            => _genericConfigFactory.GetLiteralGenericConfig
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
                    .ToList()
            );

        private GenericConfigBase GetObjectGenericConfig(string genericArgumentName, Dictionary<string, XmlElement> elements)
            => _genericConfigFactory.GetObjectGenericConfig
            (
                genericArgumentName,
                elements[XmlDataConstants.OBJECTTYPEELEMENT].InnerText,
                bool.Parse(elements[XmlDataConstants.USEFOREQUALITYELEMENT].InnerText),
                bool.Parse(elements[XmlDataConstants.USEFORHASHCODEELEMENT].InnerText),
                bool.Parse(elements[XmlDataConstants.USEFORTOSTRINGELEMENT].InnerText)
            );

        private GenericConfigBase GetLiteralListGenericConfig(string genericArgumentName, Dictionary<string, XmlElement> elements)
            => _genericConfigFactory.GetLiteralListGenericConfig
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
                    .ToList()
            );

        private GenericConfigBase GetObjectListGenericConfig(string genericArgumentName, Dictionary<string, XmlElement> elements)
            => _genericConfigFactory.GetObjectListGenericConfig
            (
                genericArgumentName,
                elements[XmlDataConstants.OBJECTTYPEELEMENT].InnerText,
                _enumHelper.ParseEnumText<ListType>(elements[XmlDataConstants.LISTTYPEELEMENT].InnerText),
                _enumHelper.ParseEnumText<ListParameterInputStyle>(elements[XmlDataConstants.CONTROLELEMENT].InnerText)
            );
        #endregion Methods
    }
}
