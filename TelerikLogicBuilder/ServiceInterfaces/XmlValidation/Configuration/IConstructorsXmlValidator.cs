using ABIS.LogicBuilder.FlowBuilder.XmlValidation;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.Configuration
{
    internal interface IConstructorsXmlValidator
    {
        XmlValidationResponse Validate(string xmlString);
    }
}
