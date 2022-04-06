using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class ObjectListVariableElementValidator : IObjectListVariableElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ObjectListVariableElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
            _typeLoadHelper = xmlElementValidator.TypeLoadHelper;
            _enumHelper = xmlElementValidator.ContextProvider.EnumHelper;
            _exceptionHelper = xmlElementValidator.ContextProvider.ExceptionHelper;
            _xmlDocumentHelpers = xmlElementValidator.ContextProvider.XmlDocumentHelpers;
        }

        //ElementValidator properties are created in the XmlElementValidator constructor and may be null in the constructor
        private ICallElementValidator CallElementValidator => _xmlElementValidator.CallElementValidator;

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
