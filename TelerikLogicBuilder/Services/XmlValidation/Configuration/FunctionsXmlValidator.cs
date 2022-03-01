using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.Configuration;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Configuration;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.Configuration
{
    internal class FunctionsXmlValidator : IFunctionsXmlValidator
    {
        private readonly IFunctionValidationHelper _functionValidationHelper;
        private readonly IContextProvider _contextProvider;

        public FunctionsXmlValidator(IFunctionValidationHelper functionValidationHelper, IContextProvider contextProvider)
        {
            _functionValidationHelper = functionValidationHelper;
            _contextProvider = contextProvider;
        }

        public XmlValidationResponse Validate(string xmlString)
            => new FunctionsXmlValidatorUtility
            (
                xmlString,
                _functionValidationHelper,
                _contextProvider
            ).ValidateXmlDocument();
    }
}
