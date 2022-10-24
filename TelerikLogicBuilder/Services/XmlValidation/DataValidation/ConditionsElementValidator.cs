using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class ConditionsElementValidator : IConditionsElementValidator
    {
        private readonly IConditionsDataParser _conditionsDataParser;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlElementValidatorFactory _xmlElementValidatorFactory;

        public ConditionsElementValidator(
            IConditionsDataParser conditionsDataParser,
            IExceptionHelper exceptionHelper,
            IXmlElementValidatorFactory xmlElementValidatorFactory)
        {
            _conditionsDataParser = conditionsDataParser;
            _exceptionHelper = exceptionHelper;
            _xmlElementValidatorFactory = xmlElementValidatorFactory;
        }

        //Element validators cannot be injected because of cyclic dependencies.
        private IFunctionElementValidator? _functionElementValidator;
		private IFunctionElementValidator FunctionElementValidator => _functionElementValidator ??= _xmlElementValidatorFactory.GetFunctionElementValidator();

        public void Validate(XmlElement condtionsElement, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (condtionsElement.Name != XmlDataConstants.CONDITIONSELEMENT)
                throw _exceptionHelper.CriticalException("{4C53E295-8C5E-4FD4-9340-BF2BC9BC8F11}");

            _conditionsDataParser.Parse(condtionsElement).FunctionElements.ForEach
            (
                functionElement => FunctionElementValidator.Validate
                (
                    functionElement, 
                    typeof(bool), 
                    application, 
                    validationErrors
                )
            );
        }
    }
}
