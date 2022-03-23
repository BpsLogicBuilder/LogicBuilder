using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data
{
    internal interface IGenericReturnTypeHelper
    {
        ReturnTypeBase GetConvertedReturnType(ReturnTypeBase returnType, IList<GenericConfigBase> genericParametersDictionary, ApplicationTypeInfo application);
    }
}
