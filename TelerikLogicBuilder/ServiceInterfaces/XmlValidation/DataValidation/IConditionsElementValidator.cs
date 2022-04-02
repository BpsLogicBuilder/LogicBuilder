using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation
{
    internal interface IConditionsElementValidator
    {
        void Validate(XmlElement condtionsElement, ApplicationTypeInfo application, List<string> validationErrors);
    }
}
