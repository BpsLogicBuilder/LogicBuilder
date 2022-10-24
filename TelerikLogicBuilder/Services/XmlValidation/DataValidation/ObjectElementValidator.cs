using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class ObjectElementValidator : IObjectElementValidator
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IXmlElementValidatorFactory _xmlElementValidatorFactory;

        public ObjectElementValidator(
            IExceptionHelper exceptionHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlElementValidatorFactory xmlElementValidatorFactory)
        {
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _xmlElementValidatorFactory = xmlElementValidatorFactory;
        }

        //Element validators cannot be injected because of cyclic dependencies.
        private ICallElementValidator? _callElementValidator;
		private ICallElementValidator CallElementValidator => _callElementValidator ??= _xmlElementValidatorFactory.GetCallElementValidator();

        public void Validate(XmlElement objectElement, Type assignedTo, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (objectElement.Name != XmlDataConstants.OBJECTELEMENT
                && objectElement.Name != XmlDataConstants.OBJECTPARAMETERELEMENT
                && objectElement.Name != XmlDataConstants.OBJECTVARIABLEELEMENT)
                throw _exceptionHelper.CriticalException("{54007C41-31CD-49BD-99E6-03C198E8B6AB}");

            if (objectElement.ChildNodes.Count == 0 || objectElement.ChildNodes.Count > 1)
                throw _exceptionHelper.CriticalException("{DB68CE86-4BA8-4ADF-B8FB-796E7FF73F40}");

            CallElementValidator.Validate
            (
                _xmlDocumentHelpers.GetSingleChildElement(objectElement),
                assignedTo,
                application,
                validationErrors
            );
        }
    }
}
