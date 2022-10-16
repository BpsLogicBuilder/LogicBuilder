using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation
{
    internal interface IXmlValidator
    {
        XmlValidationResponse Validate(string xmlString);
    }
}
