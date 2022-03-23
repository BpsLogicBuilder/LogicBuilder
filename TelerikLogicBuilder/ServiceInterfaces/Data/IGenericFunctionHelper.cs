using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data
{
    internal interface IGenericFunctionHelper
    {
        Function ConvertGenericTypes(Function function, IList<GenericConfigBase> genericParameters, ApplicationTypeInfo application);
        Type MakeGenericType(Function function, IList<GenericConfigBase> genericParameters, ApplicationTypeInfo application);
    }
}
