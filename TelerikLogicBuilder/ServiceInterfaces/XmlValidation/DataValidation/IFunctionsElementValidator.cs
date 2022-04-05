using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation
{
    internal interface IFunctionsElementValidator
    {
        void Validate(XmlElement functionsElement, ApplicationTypeInfo application, List<string> validationErrors);
    }
}
