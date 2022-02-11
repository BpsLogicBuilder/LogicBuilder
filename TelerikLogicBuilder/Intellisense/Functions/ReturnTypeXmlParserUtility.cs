using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions
{
    internal class ReturnTypeXmlParserUtility
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IContextProvider _contextProvider;

        public ReturnTypeXmlParserUtility(XmlElement xmlElement, IContextProvider contextProvider)
        {
            _exceptionHelper = contextProvider.ExceptionHelper;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _contextProvider = contextProvider;

            this.xmlElement = xmlElement;
            this.returnTypeTypeCategory = contextProvider.EnumHelper.GetReturnTypeCategory(this.xmlElement.Name);
        }

        #region Fields
        private readonly XmlElement xmlElement;
        private readonly ReturnTypeCategory returnTypeTypeCategory;
        #endregion Fields

        #region Properties
        internal ReturnTypeBase ReturnType
            => this.returnTypeTypeCategory switch
            {
                ReturnTypeCategory.Literal => BuildLiteralReturnType(_xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)),
                ReturnTypeCategory.Object => BuildConstructorReturnType(_xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)),
                ReturnTypeCategory.Generic => BuildGenericReturnType(_xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)),
                ReturnTypeCategory.LiteralList => BuildLiteralListReturnType(_xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)),
                ReturnTypeCategory.ObjectList => BuildConstructorListReturnType(_xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)),
                ReturnTypeCategory.GenericList => BuildGenericListReturnType(_xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)),
                _ => throw _exceptionHelper.CriticalException("{A50B026F-EE71-4CDC-9C4C-8BEEC776C9C1}"),
            };
        #endregion Properties

        #region Methods
        private ReturnTypeBase BuildLiteralReturnType(Dictionary<string, XmlElement> elements)
            => new LiteralReturnType
            (
                (LiteralFunctionReturnType)Enum.Parse(typeof(LiteralFunctionReturnType),
                elements[XmlDataConstants.LITERALTYPEELEMENT].InnerText),
                _contextProvider
            );

        private ReturnTypeBase BuildConstructorReturnType(Dictionary<string, XmlElement> elements)
            => new ObjectReturnType
            (
                elements[XmlDataConstants.OBJECTTYPEELEMENT].InnerText,
                _contextProvider
            );

        private ReturnTypeBase BuildGenericReturnType(Dictionary<string, XmlElement> elements)
            => new GenericReturnType
            (
                elements[XmlDataConstants.GENERICARGUMENTNAMEELEMENT].InnerText,
                _contextProvider
            );

        private ReturnTypeBase BuildLiteralListReturnType(Dictionary<string, XmlElement> elements)
            => new ListOfLiteralsReturnType
            (
                (LiteralFunctionReturnType)Enum.Parse(typeof(LiteralFunctionReturnType), elements[XmlDataConstants.LITERALTYPEELEMENT].InnerText),
                (ListType)Enum.Parse(typeof(ListType), elements[XmlDataConstants.LISTTYPEELEMENT].InnerText),
                _contextProvider
            );

        private ReturnTypeBase BuildConstructorListReturnType(Dictionary<string, XmlElement> elements)
            => new ListOfObjectsReturnType
            (
                elements[XmlDataConstants.OBJECTTYPEELEMENT].InnerText,
                (ListType)Enum.Parse(typeof(ListType), elements[XmlDataConstants.LISTTYPEELEMENT].InnerText),
                _contextProvider
            );

        private ReturnTypeBase BuildGenericListReturnType(Dictionary<string, XmlElement> elements)
            => new ListOfGenericsReturnType
            (
                elements[XmlDataConstants.GENERICARGUMENTNAMEELEMENT].InnerText,
                (ListType)Enum.Parse(typeof(ListType), elements[XmlDataConstants.LISTTYPEELEMENT].InnerText),
                _contextProvider
            );
        #endregion Methods
    }
}
