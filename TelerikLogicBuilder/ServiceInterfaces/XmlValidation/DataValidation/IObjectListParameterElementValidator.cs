using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation
{
    internal interface IObjectListParameterElementValidator
    {
        void Validate(ListOfObjectsParameter parameter, XmlElement parameterElement, ApplicationTypeInfo application, List<string> validationErrors);
    }
}
