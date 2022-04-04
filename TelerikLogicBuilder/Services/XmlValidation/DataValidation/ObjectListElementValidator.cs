using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class ObjectListElementValidator : IObjectListElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;

        public ObjectListElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
        }

        public void Validate(XmlElement objectListElement, Type assignedTo, ApplicationTypeInfo application, List<string> validationErrors)
        {
            //throw new NotImplementedException();
        }
    }
}
