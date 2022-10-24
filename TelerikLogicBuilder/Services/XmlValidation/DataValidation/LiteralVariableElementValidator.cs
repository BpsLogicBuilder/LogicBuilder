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
    internal class LiteralVariableElementValidator : ILiteralVariableElementValidator
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlElementValidatorFactory _xmlElementValidatorFactory;

        public LiteralVariableElementValidator(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IXmlElementValidatorFactory xmlElementValidatorFactory)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _xmlElementValidatorFactory = xmlElementValidatorFactory;
        }

        //Element validators cannot be injected because of cyclic dependencies.
        private ILiteralElementValidator? _literalElementValidator;
		private ILiteralElementValidator LiteralElementValidator => _literalElementValidator ??= _xmlElementValidatorFactory.GetLiteralElementValidator();

        public void Validate(XmlElement variableElement, LiteralVariable variable, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (variableElement.Name != XmlDataConstants.LITERALVARIABLEELEMENT)
                throw _exceptionHelper.CriticalException("{CCF7F7F8-EA8D-484E-80A3-FFE79C0FDD3F}");

            LiteralElementValidator.Validate
            (
                variableElement,
                _enumHelper.GetSystemType(variable.LiteralType),
                application,
                validationErrors
            );
        }
    }
}
