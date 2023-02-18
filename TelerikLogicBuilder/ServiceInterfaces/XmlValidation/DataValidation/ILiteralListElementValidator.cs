using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation
{
    internal interface ILiteralListElementValidator
    {
        void Validate(XmlElement literalListElement, Type assignedTo, ApplicationTypeInfo application, List<string> validationErrors);
        void ValidateTypeOnly(XmlElement literalListElement, Type assignedTo, ApplicationTypeInfo application, List<string> validationErrors);
    }
}
