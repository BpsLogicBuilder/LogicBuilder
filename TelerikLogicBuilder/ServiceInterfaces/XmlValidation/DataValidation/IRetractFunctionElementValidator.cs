using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation
{
    internal interface IRetractFunctionElementValidator
    {
        void Validate(XmlElement functionElement, ApplicationTypeInfo application, List<string> validationErrors);
    }
}
