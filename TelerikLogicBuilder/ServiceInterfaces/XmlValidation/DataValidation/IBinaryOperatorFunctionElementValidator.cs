using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation
{
    internal interface IBinaryOperatorFunctionElementValidator
    {
        void Validate(Function function, IList<XmlElement> parameterElementsList, ApplicationTypeInfo application, List<string> validationErrors);
    }
}
