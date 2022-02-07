using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation
{
    internal class XmlValidator : IXmlValidator
    {
        public void Validate(SchemaName schemaName, string xmlString)
        {
            XmlValidatorUtility.GetXmlValidator(schemaName, xmlString).ValidateXmlDocument();
        }
    }
}
