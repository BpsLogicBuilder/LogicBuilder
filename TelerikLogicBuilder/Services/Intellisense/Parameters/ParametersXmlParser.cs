using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Parameters
{
    internal class ParametersXmlParser : IParametersXmlParser
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IEnumHelper _enumHelper;
        private readonly IParameterFactory _parameterFactory;

        public ParametersXmlParser(
            IXmlDocumentHelpers xmlDocumentHelpers,
            IExceptionHelper exceptionHelper,
            IEnumHelper enumHelper,
            IParameterFactory parameterFactory)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _exceptionHelper = exceptionHelper;
            _enumHelper = enumHelper;
            _parameterFactory = parameterFactory;
        }

        #region Methods
        public ParameterBase Parse(XmlElement xmlElement)
        {
            ParameterCategory parameterCategory = _enumHelper.GetParameterCategory(xmlElement.Name);

            return parameterCategory switch
            {
                ParameterCategory.Literal => GetLiteralParameter(xmlElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value, _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)),
                ParameterCategory.Object => GetObjectParameter(xmlElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value, _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)),
                ParameterCategory.Generic => GetGenericParameter(xmlElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value, _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)),
                ParameterCategory.LiteralList => GetLiteralListParameter(xmlElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value, _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)),
                ParameterCategory.ObjectList => GetObjectListParameter(xmlElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value, _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)),
                ParameterCategory.GenericList => GetGenericListParameter(xmlElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value, _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)),
                _ => throw _exceptionHelper.CriticalException("{A5B2C8CE-DAED-4D27-A7BA-0B90AFF23F25}"),
            };
        }

        private ParameterBase GetLiteralParameter(string nameAttribute, Dictionary<string, XmlElement> elements)
            => _parameterFactory.GetLiteralParameter
            (
                nameAttribute,
                bool.Parse(elements[XmlDataConstants.OPTIONALELEMENT].InnerText),
                elements[XmlDataConstants.COMMENTSELEMENT].InnerText,
                (LiteralParameterType)Enum.Parse(typeof(LiteralParameterType), elements[XmlDataConstants.LITERALTYPEELEMENT].InnerText),
                (LiteralParameterInputStyle)Enum.Parse(typeof(LiteralParameterInputStyle), elements[XmlDataConstants.CONTROLELEMENT].InnerText),
                bool.Parse(elements[XmlDataConstants.USEFOREQUALITYELEMENT].InnerText),
                bool.Parse(elements[XmlDataConstants.USEFORHASHCODEELEMENT].InnerText),
                bool.Parse(elements[XmlDataConstants.USEFORTOSTRINGELEMENT].InnerText),
                elements[XmlDataConstants.PROPERTYSOURCEELEMENT].InnerText,
                elements[XmlDataConstants.PROPERTYSOURCEPARAMETERELEMENT].InnerText,
                elements[XmlDataConstants.DEFAULTVALUEELEMENT].InnerText,
                elements[XmlDataConstants.DOMAINELEMENT].ChildNodes.OfType<XmlElement>().Select(e => e.InnerText).ToList()
            );

        private ParameterBase GetObjectParameter(string nameAttribute, Dictionary<string, XmlElement> elements)
            => _parameterFactory.GetObjectParameter
            (
                nameAttribute,
                bool.Parse(elements[XmlDataConstants.OPTIONALELEMENT].InnerText),
                elements[XmlDataConstants.COMMENTSELEMENT].InnerText,
                elements[XmlDataConstants.OBJECTTYPEELEMENT].InnerText,
                bool.Parse(elements[XmlDataConstants.USEFOREQUALITYELEMENT].InnerText),
                bool.Parse(elements[XmlDataConstants.USEFORHASHCODEELEMENT].InnerText),
                bool.Parse(elements[XmlDataConstants.USEFORTOSTRINGELEMENT].InnerText)
            );

        private ParameterBase GetGenericParameter(string nameAttribute, Dictionary<string, XmlElement> elements)
            => _parameterFactory.GetGenericParameter
            (
                nameAttribute,
                bool.Parse(elements[XmlDataConstants.OPTIONALELEMENT].InnerText),
                elements[XmlDataConstants.COMMENTSELEMENT].InnerText,
                elements[XmlDataConstants.GENERICARGUMENTNAMEELEMENT].InnerText
            );

        private ParameterBase GetLiteralListParameter(string nameAttribute, Dictionary<string, XmlElement> elements)
            => _parameterFactory.GetListOfLiteralsParameter
            (
                nameAttribute,
                bool.Parse(elements[XmlDataConstants.OPTIONALELEMENT].InnerText),
                elements[XmlDataConstants.COMMENTSELEMENT].InnerText,
                (LiteralParameterType)Enum.Parse(typeof(LiteralParameterType), elements[XmlDataConstants.LITERALTYPEELEMENT].InnerText),
                (ListType)Enum.Parse(typeof(ListType), elements[XmlDataConstants.LISTTYPEELEMENT].InnerText),
                (ListParameterInputStyle)Enum.Parse(typeof(ListParameterInputStyle), elements[XmlDataConstants.CONTROLELEMENT].InnerText),
                (LiteralParameterInputStyle)Enum.Parse(typeof(LiteralParameterInputStyle), elements[XmlDataConstants.ELEMENTCONTROLELEMENT].InnerText),
                elements[XmlDataConstants.PROPERTYSOURCEELEMENT].InnerText,
                elements[XmlDataConstants.PROPERTYSOURCEPARAMETERELEMENT].InnerText,
                elements[XmlDataConstants.DEFAULTVALUEELEMENT].ChildNodes.OfType<XmlElement>().Select(e => e.InnerText).ToList(),
                MiscellaneousConstants.DEFAULT_PARAMETER_DELIMITERS,
                elements[XmlDataConstants.DOMAINELEMENT].ChildNodes.OfType<XmlElement>().Select(e => e.InnerText).ToList()
            );

        private ParameterBase GetObjectListParameter(string nameAttribute, Dictionary<string, XmlElement> elements)
            => _parameterFactory.GetListOfObjectsParameter
            (
                nameAttribute,
                bool.Parse(elements[XmlDataConstants.OPTIONALELEMENT].InnerText),
                elements[XmlDataConstants.COMMENTSELEMENT].InnerText,
                elements[XmlDataConstants.OBJECTTYPEELEMENT].InnerText,
                (ListType)Enum.Parse(typeof(ListType), elements[XmlDataConstants.LISTTYPEELEMENT].InnerText),
                (ListParameterInputStyle)Enum.Parse(typeof(ListParameterInputStyle), elements[XmlDataConstants.CONTROLELEMENT].InnerText)
            );

        private ParameterBase GetGenericListParameter(string nameAttribute, Dictionary<string, XmlElement> elements)
            => _parameterFactory.GetListOfGenericsParameter
            (
                nameAttribute,
                bool.Parse(elements[XmlDataConstants.OPTIONALELEMENT].InnerText),
                elements[XmlDataConstants.COMMENTSELEMENT].InnerText,
                elements[XmlDataConstants.GENERICARGUMENTNAMEELEMENT].InnerText,
                _enumHelper.ParseEnumText<ListType>(elements[XmlDataConstants.LISTTYPEELEMENT].InnerText),
                _enumHelper.ParseEnumText<ListParameterInputStyle>(elements[XmlDataConstants.CONTROLELEMENT].InnerText)
            );
        #endregion Methods
    }
}
