using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class LiteralListVariableElementValidator : ILiteralListVariableElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;

        public LiteralListVariableElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
        }
    }
}
