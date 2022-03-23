using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation
{
    internal interface IFunctionGenericsConfigrationValidator
    {
        bool Validate(Function function, List<GenericConfigBase> genericArguments, ApplicationTypeInfo application, List<string> validationErrors);
    }
}
