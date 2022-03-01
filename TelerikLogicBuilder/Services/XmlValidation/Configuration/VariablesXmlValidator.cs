using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.Configuration;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Configuration;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.Configuration
{
    internal class VariablesXmlValidator : IVariablesXmlValidator
    {
        private readonly IVariablesXmlParser _variablesXmlParser;
        private readonly IVariableValidationHelper _variableValidationHelper;
        private readonly IContextProvider _contextProvider;

        public VariablesXmlValidator(IVariablesXmlParser variablesXmlParser,
            IVariableValidationHelper variableValidationHelper,
            IContextProvider contextProvider)
        {
            _variablesXmlParser = variablesXmlParser;
            _variableValidationHelper = variableValidationHelper;
            _contextProvider = contextProvider;
        }

        public XmlValidationResponse Validate(string xmlString) 
            => new VariablesXmlValidatorUtility
            (
                xmlString,
                _variablesXmlParser,
                _variableValidationHelper,
                _contextProvider
            ).ValidateXmlDocument();
    }
}
