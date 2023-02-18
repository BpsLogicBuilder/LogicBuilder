using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation
{
    internal interface IFunctionElementValidator
    {
        void Validate(XmlElement functionElement, Type assignedTo, ApplicationTypeInfo application, List<string> validationErrors);
        void ValidateTypeOnly(XmlElement functionElement, Type assignedTo, ApplicationTypeInfo application, List<string> validationErrors);
    }
}
