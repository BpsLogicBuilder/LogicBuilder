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
    internal class ObjectListParameterElementValidator : IObjectListParameterElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ObjectListParameterElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
            _typeLoadHelper = xmlElementValidator.TypeLoadHelper;
            _enumHelper = xmlElementValidator.ContextProvider.EnumHelper;
            _exceptionHelper = xmlElementValidator.ContextProvider.ExceptionHelper;
            _xmlDocumentHelpers = xmlElementValidator.ContextProvider.XmlDocumentHelpers;
        }

        //ElementValidator properties are created in the XmlElementValidator constructor and may be null in the constructor
        private ICallElementValidator CallElementValidator => _xmlElementValidator.CallElementValidator;

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
