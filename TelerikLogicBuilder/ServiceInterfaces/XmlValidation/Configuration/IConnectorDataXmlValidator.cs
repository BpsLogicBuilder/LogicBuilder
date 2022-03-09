using ABIS.LogicBuilder.FlowBuilder.XmlValidation;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.Configuration
{
    internal interface IConnectorDataXmlValidator
    {
        XmlValidationResponse Validate(string xmlString);
    }
}
