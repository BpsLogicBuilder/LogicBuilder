using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation
{
    internal interface IDecisionsElementValidator
    {
        void Validate(XmlElement decisionsElement, ApplicationTypeInfo application, List<string> validationErrors);
    }
}
