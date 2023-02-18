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

            Validate(literalListElement, assignedTo, application, validationErrors, true);
        }

        public void ValidateTypeOnly(XmlElement literalListElement, Type assignedTo, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (literalListElement.Name != XmlDataConstants.LITERALLISTELEMENT)
                throw _exceptionHelper.CriticalException("{47F201BE-71CE-476A-9EFB-F4694DE5C2B7}");

            Validate(literalListElement, assignedTo, application, validationErrors, false);
        }

        private void Validate(XmlElement literalListElement, Type assignedTo, ApplicationTypeInfo application, List<string> validationErrors, bool validateChildElements)
        {
            Type elementType = _enumHelper.GetSystemType
            (
                _enumHelper.ParseEnumText<LiteralParameterType>
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

            if (!validateChildElements)
                return;//During editing we want to allow the user to fix changes in the UI i.e. if validateChildElements == false then only reset the control if the above validations fail.

            _xmlDocumentHelpers.GetChildElements(literalListElement).ForEach
            (
                element => LiteralElementValidator.Validate(element, elementType, application, validationErrors)
            );
        }
    }
}
