using ABIS.LogicBuilder.FlowBuilder.Enums;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation
{
    internal interface IXmlValidator
    {
        void Validate(SchemaName schemaName, string xmlString);
    }
}
