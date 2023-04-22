using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class DecisionElementValidator : IDecisionElementValidator
    {
        private readonly IConfigurationService _configurationService;
        private readonly IDecisionDataParser _decisionDataParser;
        private readonly IXmlElementValidatorFactory _xmlElementValidatorFactory;

        public DecisionElementValidator(
            IConfigurationService configurationService,
            IDecisionDataParser decisionDataParser,
            IXmlElementValidatorFactory xmlElementValidatorFactory)
        {
            _configurationService = configurationService;
            _decisionDataParser = decisionDataParser;
            _xmlElementValidatorFactory = xmlElementValidatorFactory;
        }

        //Element validators cannot be injected because of cyclic dependencies.
        private IFunctionElementValidator? _functionElementValidator;
		private IFunctionElementValidator FunctionElementValidator => _functionElementValidator ??= _xmlElementValidatorFactory.GetFunctionElementValidator();

        public void Validate(XmlElement decisionElement, ApplicationTypeInfo application, List<string> validationErrors)
        {
            DecisionData decisionData = _decisionDataParser.Parse(decisionElement);
            if(!_configurationService.VariableList.Variables.TryGetValue(decisionData.Name, out var variable))
            {
                //Decisions are no longer tied to variables.
                //validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.cannotEvaluateVariableFormat, decisionData.Name));
                //return;
            }

            decisionData.FunctionElements.ForEach
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
