using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class DecisionElementValidator : IDecisionElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;
        private readonly IConfigurationService _configurationService;
        private readonly IDecisionDataParser _decisionDataParser;

        public DecisionElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
            _decisionDataParser = xmlElementValidator.DecisionDataParser;
            _configurationService = xmlElementValidator.ContextProvider.ConfigurationService;
        }

        //ElementValidator properties are created in the XmlElementValidator constructor.
        //These fields may be null in the constructor i.e. when new AssertFunctionElementValidator((XmlElementValidator)this) runs
        //therefore they must be properties.
        private IFunctionElementValidator FunctionElementValidator => _xmlElementValidator.FunctionElementValidator;

        public void Validate(XmlElement decisionElement, ApplicationTypeInfo application, List<string> validationErrors)
        {
            DecisionData decisionData = _decisionDataParser.Parse(decisionElement);
            if(!_configurationService.VariableList.Variables.TryGetValue(decisionData.Name, out var variable))
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.cannotEvaluateVariableFormat, decisionData.Name));
                return;
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
