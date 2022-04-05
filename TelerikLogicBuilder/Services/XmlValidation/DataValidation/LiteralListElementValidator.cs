using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class LiteralListElementValidator : ILiteralListElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITypeHelper _typeHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public LiteralListElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
            _enumHelper = xmlElementValidator.ContextProvider.EnumHelper;
            _exceptionHelper = xmlElementValidator.ContextProvider.ExceptionHelper;
            _typeHelper = xmlElementValidator.ContextProvider.TypeHelper;
            _xmlDocumentHelpers = xmlElementValidator.ContextProvider.XmlDocumentHelpers;
        }

        //ElementValidator properties are created in the XmlElementValidator constructor and may be null in the constructor
        private ILiteralElementValidator LiteralElementValidator => _xmlElementValidator.LiteralElementValidator;

        public void Validate(XmlElement literalListElement, Type assignedTo, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (literalListElement.Name != XmlDataConstants.LITERALLISTELEMENT)
                throw _exceptionHelper.CriticalException("{5B0D0D9D-0DA3-4941-88E5-22989811F1D1}");

            Type elementType = _enumHelper.GetSystemType
            (
                _enumHelper.ParseEnumText<LiteralListElementType>
                (
                    literalListElement.GetAttribute(XmlDataConstants.LITERALTYPEATTRIBUTE)
                )
            );

            ListType listType = _enumHelper.ParseEnumText<ListType>
            (
                literalListElement.GetAttribute(XmlDataConstants.LISTTYPEATTRIBUTE)
            );


            Type listSystemType = _enumHelper.GetSystemType(listType, elementType);
            if (!_typeHelper.AssignableFrom(assignedTo, listSystemType))
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.typeNotAssignableFormat, listSystemType.ToString(), assignedTo.ToString()));
                return;
            }

            _xmlDocumentHelpers.GetChildElements(literalListElement).ForEach
            (
                element => LiteralElementValidator.Validate(element, elementType, application, validationErrors)
            );
        }
    }
}
