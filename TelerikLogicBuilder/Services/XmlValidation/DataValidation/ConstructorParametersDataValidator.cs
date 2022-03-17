using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class ConstructorParametersDataValidator : IConstructorParametersDataValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;

        public ConstructorParametersDataValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
        }
    }
}
