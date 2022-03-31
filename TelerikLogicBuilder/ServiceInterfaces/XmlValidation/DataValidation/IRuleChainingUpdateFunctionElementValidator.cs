using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation
{
    internal interface IRuleChainingUpdateFunctionElementValidator
    {
        void Validate(Function function, IList<XmlElement> parameterElementsList, List<string> validationErrors);
    }
}
