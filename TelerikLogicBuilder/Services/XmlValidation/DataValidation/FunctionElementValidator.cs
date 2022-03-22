using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class FunctionElementValidator : IFunctionElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;

        public FunctionElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
        }

        public void Validate(XmlElement functionElement, Type assignedTo, ApplicationTypeInfo application, List<string> validationErrors)
        {
            //throw new NotImplementedException();
        }
    }
}
