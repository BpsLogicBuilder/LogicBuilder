using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
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
    internal class ObjectListParameterElementValidator : IObjectListParameterElementValidator
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IXmlElementValidatorFactory _xmlElementValidatorFactory;

        public ObjectListParameterElementValidator(
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

        public void Validate(XmlElement parameterElement, ListOfObjectsParameter parameter, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (parameterElement.Name != XmlDataConstants.OBJECTLISTPARAMETERELEMENT)
                throw _exceptionHelper.CriticalException("{B3303DFB-4BB3-4836-B900-ACA2666D5706}");

            if (!_typeLoadHelper.TryGetSystemType(parameter.ObjectType, application, out Type? elementType))
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeFormat2, parameter.ObjectType));
                return;
            }

            CallElementValidator.Validate
            (
                _xmlDocumentHelpers.GetSingleChildElement(parameterElement),
                _enumHelper.GetSystemType(parameter.ListType, elementType), 
                application, 
                validationErrors
            );
        }
    }
}
