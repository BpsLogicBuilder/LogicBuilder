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
        private readonly IEnumHelper _enumHelper;
        private readonly IStringHelper _stringHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public VariablesXmlValidator(IVariablesXmlParser variablesXmlParser,
            IVariableValidationHelper variableValidationHelper,
            IEnumHelper enumHelper,
            IStringHelper stringHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _variablesXmlParser = variablesXmlParser;
            _variableValidationHelper = variableValidationHelper;
            _enumHelper = enumHelper;
            _stringHelper = stringHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public XmlValidationResponse Validate(string xmlString) 
            => new VariablesXmlValidatorUtility
            (
                xmlString, 
                _variablesXmlParser, 
                _variableValidationHelper, 
                _enumHelper, 
                _stringHelper, 
                _xmlDocumentHelpers
            ).ValidateXmlDocument();
    }
}
