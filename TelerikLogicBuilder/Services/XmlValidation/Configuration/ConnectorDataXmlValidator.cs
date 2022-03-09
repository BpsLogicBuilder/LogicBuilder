using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.Configuration;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Configuration;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.Configuration
{
    internal class ConnectorDataXmlValidator : IConnectorDataXmlValidator
    {
        private readonly IContextProvider _contextProvider;

        public ConnectorDataXmlValidator(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public XmlValidationResponse Validate(string xmlString)
            => new ConnectorDataXmlValidatorUtility(xmlString, _contextProvider).ValidateXmlDocument();
    }
}
