using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class LiteralParameterElementValidator : ILiteralParameterElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;

        public LiteralParameterElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
        }

        public void Validate(LiteralParameter parameter, XmlElement parameterElement, ApplicationTypeInfo application, List<string> validationErrors)
        {
            //throw new System.NotImplementedException();
        }
    }
}
