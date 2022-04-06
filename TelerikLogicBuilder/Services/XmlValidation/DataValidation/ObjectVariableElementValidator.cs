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
    internal class ObjectVariableElementValidator : IObjectVariableElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;

        public ObjectVariableElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
            _exceptionHelper = xmlElementValidator.ContextProvider.ExceptionHelper;
            _typeLoadHelper = xmlElementValidator.TypeLoadHelper;
        }

        //ElementValidator properties are created in the XmlElementValidator constructor and may be null in the constructor
        private IObjectElementValidator ObjectElementValidator => _xmlElementValidator.ObjectElementValidator;

        public void Validate(XmlElement variableElement, ObjectVariable variable, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (variableElement.Name != XmlDataConstants.OBJECTVARIABLEELEMENT)
                throw _exceptionHelper.CriticalException("{D4A2A138-DEC9-4B91-B490-2C2A8D6DC1D4}");

            if (!_typeLoadHelper.TryGetSystemType(variable.ObjectType, application, out Type? assignedTo))
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeForVariableFormat, variable.ObjectTypeString, variable.Name));
                return;
            }

            ObjectElementValidator.Validate(variableElement, assignedTo, application, validationErrors);
        }
    }
}
