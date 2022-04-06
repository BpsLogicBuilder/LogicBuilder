using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class ObjectElementValidator : IObjectElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ObjectElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
            _exceptionHelper = xmlElementValidator.ContextProvider.ExceptionHelper;
            _xmlDocumentHelpers = xmlElementValidator.ContextProvider.XmlDocumentHelpers;
        }

        //ElementValidator properties are created in the XmlElementValidator constructor and may be null in the constructor
        private ICallElementValidator CallElementValidator => _xmlElementValidator.CallElementValidator;

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
