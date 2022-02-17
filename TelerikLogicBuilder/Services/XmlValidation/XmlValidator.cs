using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.Configuration;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation
{
    internal class XmlValidator : IXmlValidator
    {
        private readonly IConstructorsXmlValidator _constructorsXmlValidator;
        private readonly IFunctionsXmlValidator _functionsXmlValidator;
        private readonly IVariablesXmlValidator _variablesXmlValidator;

        public XmlValidator(IConstructorsXmlValidator constructorsXmlValidator, IFunctionsXmlValidator functionsXmlValidator, IVariablesXmlValidator variablesXmlValidator)
        {
            _constructorsXmlValidator = constructorsXmlValidator;
            _functionsXmlValidator = functionsXmlValidator;
            _variablesXmlValidator = variablesXmlValidator;
        }

        public XmlValidationResponse Validate(SchemaName schemaName, string xmlString) 
            => schemaName switch
            {
                SchemaName.ConstructorSchema => _constructorsXmlValidator.Validate(xmlString),
                SchemaName.FunctionsSchema => _functionsXmlValidator.Validate(xmlString),
                SchemaName.VariablesSchema => _variablesXmlValidator.Validate(xmlString),
                _ => XmlValidatorUtility.GetXmlValidator(schemaName, xmlString).ValidateXmlDocument(),
            };
    }
}
