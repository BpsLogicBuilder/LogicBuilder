using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation
{
    internal interface IObjectListElementValidator
    {
        void Validate(XmlElement objectListElement, Type assignedTo, ApplicationTypeInfo application, List<string> validationErrors);
    }
}
