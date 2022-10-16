using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;

namespace ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories
{
    internal interface IXmlValidatorFactory
    {
        IXmlValidator GetXmlValidator(SchemaName xmlSchema);
    }
}
