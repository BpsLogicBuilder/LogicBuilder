using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class LiteralListElementValidator : ILiteralListElementValidator
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITypeHelper _typeHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IXmlElementValidatorFactory _xmlElementValidatorFactory;

        public LiteralListElementValidator(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            ITypeHelper typeHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlElementValidatorFactory xmlElementValidatorFactory)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _typeHelper = typeHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _xmlElementValidatorFactory = xmlElementValidatorFactory;
        }

        //Element validators cannot be injected because of cyclic dependencies.
        private ILiteralElementValidator? _literalElementValidator;
		private ILiteralElementValidator LiteralElementValidator => _literalElementValidator ??= _xmlElementValidatorFactory.GetLiteralElementValidator();

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
