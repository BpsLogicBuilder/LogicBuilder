using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class LiteralListVariableElementValidator : ILiteralListVariableElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public LiteralListVariableElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
            _enumHelper = xmlElementValidator.ContextProvider.EnumHelper;
            _exceptionHelper = xmlElementValidator.ContextProvider.ExceptionHelper;
            _xmlDocumentHelpers = xmlElementValidator.ContextProvider.XmlDocumentHelpers;
        }

        //ElementValidator properties are created in the XmlElementValidator constructor and may be null in the constructor
        private ICallElementValidator CallElementValidator => _xmlElementValidator.CallElementValidator;

        public void Validate(XmlElement variableElement, ListOfLiteralsVariable variable, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (variableElement.Name != XmlDataConstants.LITERALLISTVARIABLEELEMENT)
                throw _exceptionHelper.CriticalException("{6688FD56-7D82-4CE0-B7A9-72288BED0B72}");

            //used in assert function therfore validation ensures that the element (list, constructor, function, other variable) is assignable to the configured list type.
            //The castAS only applies when the variable is being assigned to something else - not when its value is being set.
            CallElementValidator.Validate
            (
                _xmlDocumentHelpers.GetSingleChildElement(variableElement),
                _enumHelper.GetSystemType
                (
                    variable.ListType,
                    _enumHelper.GetSystemType
                    (
                        variable.LiteralType
                    )
                ),
                application,
                validationErrors
            );
        }
    }
}
