using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
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
    internal class ObjectListVariableElementValidator : IObjectListVariableElementValidator
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IXmlElementValidatorFactory _xmlElementValidatorFactory;

        public ObjectListVariableElementValidator(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            ITypeLoadHelper typeLoadHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlElementValidatorFactory xmlElementValidatorFactory)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _typeLoadHelper = typeLoadHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _xmlElementValidatorFactory = xmlElementValidatorFactory;
        }

        //Element validators cannot be injected because of cyclic dependencies.
        private ICallElementValidator? _callElementValidator;
		private ICallElementValidator CallElementValidator => _callElementValidator ??= _xmlElementValidatorFactory.GetCallElementValidator();

        public void Validate(XmlElement variableElement, ListOfObjectsVariable variable, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (variableElement.Name != XmlDataConstants.OBJECTLISTVARIABLEELEMENT)
                throw _exceptionHelper.CriticalException("{18D38FF4-CE3D-49A2-A8D7-19655D7E011F}");

            if (!_typeLoadHelper.TryGetSystemType(variable.ObjectType, application, out Type? elementType))
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeFormat2, variable.ObjectType));
                return;
            }

            CallElementValidator.Validate
            (
                _xmlDocumentHelpers.GetSingleChildElement(variableElement),
                _enumHelper.GetSystemType
                (
                    variable.ListType,
                    elementType
                ),
                application,
                validationErrors
            );
        }
    }
}
