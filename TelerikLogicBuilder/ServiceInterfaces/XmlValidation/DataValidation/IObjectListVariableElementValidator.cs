using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation
{
    internal interface IObjectListVariableElementValidator
    {
        void Validate(XmlElement variableElement, ListOfObjectsVariable variable, ApplicationTypeInfo application, List<string> validationErrors);
    }
}
