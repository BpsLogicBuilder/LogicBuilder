using ABIS.LogicBuilder.FlowBuilder.XmlValidation;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.Configuration
{
    internal interface IVariablesXmlValidator
    {
        XmlValidationResponse Validate(string xmlString);
    }
}
