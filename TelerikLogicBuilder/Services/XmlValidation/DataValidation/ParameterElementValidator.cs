using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class ParameterElementValidator : IParameterElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;

        public ParameterElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
        }

        public void Validate(ParameterBase parameter, XmlElement parameterElement, ApplicationTypeInfo application, List<string> validationErrors)
        {
            //throw new System.NotImplementedException();
        }
    }
}
