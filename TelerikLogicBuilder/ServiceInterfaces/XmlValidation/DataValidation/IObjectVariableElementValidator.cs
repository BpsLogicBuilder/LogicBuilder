using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation
{
    internal interface IObjectVariableElementValidator
    {
        void Validate(XmlElement variableElement, ObjectVariable variable, ApplicationTypeInfo application, List<string> validationErrors);
    }
}
