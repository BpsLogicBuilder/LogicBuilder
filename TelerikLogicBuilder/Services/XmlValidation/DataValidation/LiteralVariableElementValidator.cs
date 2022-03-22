using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class LiteralVariableElementValidator : ILiteralVariableElementValidator
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ILiteralElementValidator _literalElementValidator;

        public LiteralVariableElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _enumHelper = xmlElementValidator.ContextProvider.EnumHelper;
            _exceptionHelper = xmlElementValidator.ContextProvider.ExceptionHelper;
            _literalElementValidator = xmlElementValidator.LiteralElementValidator;
        }

        public void Validate(XmlElement variableElement, LiteralVariable variable, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (variableElement.Name != XmlDataConstants.LITERALVARIABLEELEMENT)
                throw _exceptionHelper.CriticalException("{CCF7F7F8-EA8D-484E-80A3-FFE79C0FDD3F}");

            _literalElementValidator.Validate
            (
                variableElement,
                _enumHelper.GetSystemType(variable.LiteralType),
                application,
                validationErrors
            );
        }
    }
}
