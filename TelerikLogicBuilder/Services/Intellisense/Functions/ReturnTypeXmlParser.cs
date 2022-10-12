using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using XmlElement = System.Xml.XmlElement;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Functions
{
    internal class ReturnTypeXmlParser : IReturnTypeXmlParser
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IReturnTypeFactory _returnTypeFactory;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ReturnTypeXmlParser(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IReturnTypeFactory returnTypeFactory,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _returnTypeFactory = returnTypeFactory;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        #region Methods
        public ReturnTypeBase Parse(XmlElement xmlElement)
        {
            ReturnTypeCategory returnTypeTypeCategory = _enumHelper.GetReturnTypeCategory(xmlElement.Name);
            return returnTypeTypeCategory switch
            {
                ReturnTypeCategory.Literal => BuildLiteralReturnType(_xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)),
                ReturnTypeCategory.Object => BuildConstructorReturnType(_xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)),
                ReturnTypeCategory.Generic => BuildGenericReturnType(_xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)),
                ReturnTypeCategory.LiteralList => BuildLiteralListReturnType(_xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)),
                ReturnTypeCategory.ObjectList => BuildConstructorListReturnType(_xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)),
                ReturnTypeCategory.GenericList => BuildGenericListReturnType(_xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)),
                _ => throw _exceptionHelper.CriticalException("{A50B026F-EE71-4CDC-9C4C-8BEEC776C9C1}"),
            };
        }

        private ReturnTypeBase BuildLiteralReturnType(Dictionary<string, XmlElement> elements)
            => _returnTypeFactory.GetLiteralReturnType
            (
                (LiteralFunctionReturnType)Enum.Parse
                (
                    typeof(LiteralFunctionReturnType), 
                    elements[XmlDataConstants.LITERALTYPEELEMENT].InnerText
                )
            );

        private ReturnTypeBase BuildConstructorReturnType(Dictionary<string, XmlElement> elements)
            => _returnTypeFactory.GetObjectReturnType
            (
                elements[XmlDataConstants.OBJECTTYPEELEMENT].InnerText
            );

        private ReturnTypeBase BuildGenericReturnType(Dictionary<string, XmlElement> elements)
            => _returnTypeFactory.GetGenericReturnType
            (
                elements[XmlDataConstants.GENERICARGUMENTNAMEELEMENT].InnerText
            );

        private ReturnTypeBase BuildLiteralListReturnType(Dictionary<string, XmlElement> elements)
            => _returnTypeFactory.GetListOfLiteralsReturnType
            (
                (LiteralFunctionReturnType)Enum.Parse(typeof(LiteralFunctionReturnType), elements[XmlDataConstants.LITERALTYPEELEMENT].InnerText),
                (ListType)Enum.Parse(typeof(ListType), elements[XmlDataConstants.LISTTYPEELEMENT].InnerText)
            );

        private ReturnTypeBase BuildConstructorListReturnType(Dictionary<string, XmlElement> elements)
            => _returnTypeFactory.GetListOfObjectsReturnType
            (
                elements[XmlDataConstants.OBJECTTYPEELEMENT].InnerText,
                (ListType)Enum.Parse(typeof(ListType), elements[XmlDataConstants.LISTTYPEELEMENT].InnerText)
            );

        private ReturnTypeBase BuildGenericListReturnType(Dictionary<string, XmlElement> elements)
            => _returnTypeFactory.GetListOfGenericsReturnType
            (
                elements[XmlDataConstants.GENERICARGUMENTNAMEELEMENT].InnerText,
                (ListType)Enum.Parse(typeof(ListType), elements[XmlDataConstants.LISTTYPEELEMENT].InnerText)
            );
        #endregion Methods
    }
}
