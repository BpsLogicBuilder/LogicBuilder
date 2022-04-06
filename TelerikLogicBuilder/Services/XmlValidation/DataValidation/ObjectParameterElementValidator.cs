using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class ObjectParameterElementValidator : IObjectParameterElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;

        public ObjectParameterElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
            _exceptionHelper = xmlElementValidator.ContextProvider.ExceptionHelper;
            _typeLoadHelper = xmlElementValidator.TypeLoadHelper;
        }

        //ElementValidator properties are created in the XmlElementValidator constructor and may be null in the constructor
        private IObjectElementValidator ObjectElementValidator => _xmlElementValidator.ObjectElementValidator;

        public void Validate(XmlElement parameterElement, ObjectParameter parameter, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (parameterElement.Name != XmlDataConstants.OBJECTPARAMETERELEMENT)
                throw _exceptionHelper.CriticalException("{322C612B-F905-48D2-BFAC-1C40D451ED10}");

            if (!_typeLoadHelper.TryGetSystemType(parameter.ObjectType, application, out Type? assignedTo))
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeFormat2, parameter.ObjectType));
                return;
            }

            ObjectElementValidator.Validate(parameterElement, assignedTo, application, validationErrors);
        }
    }
}
