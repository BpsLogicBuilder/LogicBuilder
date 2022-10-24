using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class LiteralListVariableElementValidator : ILiteralListVariableElementValidator
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IXmlElementValidatorFactory _xmlElementValidatorFactory;

        public LiteralListVariableElementValidator(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlElementValidatorFactory xmlElementValidatorFactory)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _xmlElementValidatorFactory = xmlElementValidatorFactory;
        }

        //Element validators cannot be injected because of cyclic dependencies.
        private ICallElementValidator? _callElementValidator;
		private ICallElementValidator CallElementValidator => _callElementValidator ??= _xmlElementValidatorFactory.GetCallElementValidator();

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
