using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation
{
    internal interface IDecisionElementValidator
    {
        void Validate(XmlElement decisionElement, ApplicationTypeInfo application, List<string> validationErrors);
    }
}
