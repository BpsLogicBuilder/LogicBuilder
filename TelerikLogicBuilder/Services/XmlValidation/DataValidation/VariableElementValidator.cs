using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class VariableElementValidator : IVariableElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;

        public VariableElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
        }

        public void Validate(XmlElement variableElement, Type assignedTo, ApplicationTypeInfo application, List<string> validationErrors)
        {
            //throw new NotImplementedException();
        }
    }
}
