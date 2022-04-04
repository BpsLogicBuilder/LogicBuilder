using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class LiteralListElementValidator : ILiteralListElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;

        public LiteralListElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
        }

        public void Validate(XmlElement literalListElement, Type assignedTo, ApplicationTypeInfo application, List<string> validationErrors)
        {
            //throw new NotImplementedException();
        }
    }
}
