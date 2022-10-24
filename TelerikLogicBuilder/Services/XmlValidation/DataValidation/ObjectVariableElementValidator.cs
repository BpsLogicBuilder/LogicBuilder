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
    internal class ObjectVariableElementValidator : IObjectVariableElementValidator
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IXmlElementValidatorFactory _xmlElementValidatorFactory;

        public ObjectVariableElementValidator(
            IExceptionHelper exceptionHelper,
            ITypeLoadHelper typeLoadHelper,
            IXmlElementValidatorFactory xmlElementValidatorFactory)
        {
            _exceptionHelper = exceptionHelper;
            _typeLoadHelper = typeLoadHelper;
            _xmlElementValidatorFactory = xmlElementValidatorFactory;
        }

        //Element validators cannot be injected because of cyclic dependencies.
        private IObjectElementValidator? _objectElementValidator;
		private IObjectElementValidator ObjectElementValidator => _objectElementValidator ??= _xmlElementValidatorFactory.GetObjectElementValidator();

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
