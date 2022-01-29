﻿using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters
{
    internal class ParameterBuilder
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IEnumHelper _enumHelper;
        private readonly IContextProvider _contextProvider;
        internal ParameterBuilder(XmlElement xmlElement, IContextProvider contextProvider)
        {
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _exceptionHelper = contextProvider.ExceptionHelper;
            _enumHelper = contextProvider.EnumHelper;
            _contextProvider = contextProvider;
            this.xmlElement = xmlElement;
            this.parameterCategory = contextProvider.EnumHelper.GetParameterCategory(this.xmlElement.Name);
        }

        #region Fields
        private readonly XmlElement xmlElement;
        private readonly ParameterCategory parameterCategory;
        #endregion Fields

        #region Properties
        internal ParameterBase Parameter 
            => this.parameterCategory switch
            {
                ParameterCategory.Literal => BuildLiteralParameter(xmlElement.Attributes[XmlDataConstants.NAMEATTRIBUTE].Value, _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)),
                ParameterCategory.Object => BuildObjectParameter(xmlElement.Attributes[XmlDataConstants.NAMEATTRIBUTE].Value, _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)),
                ParameterCategory.Generic => BuildGenericParameter(xmlElement.Attributes[XmlDataConstants.NAMEATTRIBUTE].Value, _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)),
                ParameterCategory.LiteralList => BuildLiteralListParameter(xmlElement.Attributes[XmlDataConstants.NAMEATTRIBUTE].Value, _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)),
                ParameterCategory.ObjectList => BuildObjectListParameter(xmlElement.Attributes[XmlDataConstants.NAMEATTRIBUTE].Value, _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)),
                ParameterCategory.GenericList => BuildGenericListParameter(xmlElement.Attributes[XmlDataConstants.NAMEATTRIBUTE].Value, _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)),
                _ => throw _exceptionHelper.CriticalException("{A5B2C8CE-DAED-4D27-A7BA-0B90AFF23F25}"),
            };
        #endregion Properties

        #region Methods
        private ParameterBase BuildLiteralParameter(string nameAttribute, Dictionary<string, XmlElement> elements) 
            => new LiteralParameter
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
                elements[XmlDataConstants.DOMAINELEMENT].ChildNodes.OfType<XmlElement>().Select(e => e.InnerText).ToList(),
                _contextProvider
            );

        private ParameterBase BuildObjectParameter(string nameAttribute, Dictionary<string, XmlElement> elements) 
            => new ObjectParameter
            (
                nameAttribute,
                bool.Parse(elements[XmlDataConstants.OPTIONALELEMENT].InnerText),
                elements[XmlDataConstants.COMMENTSELEMENT].InnerText,
                elements[XmlDataConstants.OBJECTTYPEELEMENT].InnerText,
                bool.Parse(elements[XmlDataConstants.USEFOREQUALITYELEMENT].InnerText),
                bool.Parse(elements[XmlDataConstants.USEFORHASHCODEELEMENT].InnerText),
                bool.Parse(elements[XmlDataConstants.USEFORTOSTRINGELEMENT].InnerText),
                _contextProvider
            );

        private ParameterBase BuildGenericParameter(string nameAttribute, Dictionary<string, XmlElement> elements) 
            => new GenericParameter
            (
                nameAttribute,
                bool.Parse(elements[XmlDataConstants.OPTIONALELEMENT].InnerText),
                elements[XmlDataConstants.COMMENTSELEMENT].InnerText,
                elements[XmlDataConstants.GENERICARGUMENTNAMEELEMENT].InnerText,
                _contextProvider
            );

        private ParameterBase BuildLiteralListParameter(string nameAttribute, Dictionary<string, XmlElement> elements) 
            => new ListOfLiteralsParameter
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
                elements[XmlDataConstants.DOMAINELEMENT].ChildNodes.OfType<XmlElement>().Select(e => e.InnerText).ToList(),
                _contextProvider
            );

        private ParameterBase BuildObjectListParameter(string nameAttribute, Dictionary<string, XmlElement> elements) 
            => new ListOfObjectsParameter
            (
                nameAttribute,
                bool.Parse(elements[XmlDataConstants.OPTIONALELEMENT].InnerText),
                elements[XmlDataConstants.COMMENTSELEMENT].InnerText,
                elements[XmlDataConstants.OBJECTTYPEELEMENT].InnerText,
                (ListType)Enum.Parse(typeof(ListType), elements[XmlDataConstants.LISTTYPEELEMENT].InnerText),
                (ListParameterInputStyle)Enum.Parse(typeof(ListParameterInputStyle), elements[XmlDataConstants.CONTROLELEMENT].InnerText),
                _contextProvider
            );

        private ParameterBase BuildGenericListParameter(string nameAttribute, Dictionary<string, XmlElement> elements) 
            => new ListOfGenericsParameter
            (
                nameAttribute,
                bool.Parse(elements[XmlDataConstants.OPTIONALELEMENT].InnerText),
                elements[XmlDataConstants.COMMENTSELEMENT].InnerText,
                elements[XmlDataConstants.GENERICARGUMENTNAMEELEMENT].InnerText,
                _enumHelper.ParseEnumText<ListType>(elements[XmlDataConstants.LISTTYPEELEMENT].InnerText),
                _enumHelper.ParseEnumText<ListParameterInputStyle>(elements[XmlDataConstants.CONTROLELEMENT].InnerText),
                _contextProvider
            );
        #endregion Methods
    }
}
