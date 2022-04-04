using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation
{
    internal interface IConnectorElementValidator
    {
        void Validate(XmlElement connectorElement, ApplicationTypeInfo application, List<string> validationErrors);
    }
}
