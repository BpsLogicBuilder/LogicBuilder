using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.Configuration;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Configuration;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.Configuration
{
    internal class ConstructorsXmlValidator : IConstructorsXmlValidator
    {
        private readonly IContextProvider _contextProvider;

        public ConstructorsXmlValidator(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public XmlValidationResponse Validate(string xmlString) 
            => new ConstructorsXmlValidatorUtility(xmlString, _contextProvider).ValidateXmlDocument();
    }
}
