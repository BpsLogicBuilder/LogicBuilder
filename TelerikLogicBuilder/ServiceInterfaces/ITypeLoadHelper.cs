using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface ITypeLoadHelper
    {
        bool TryGetSystemType(GenericConfigBase config, ApplicationTypeInfo application, out Type type);
        bool TryGetSystemType(ParameterBase paramter, ApplicationTypeInfo application, out Type type);
        bool TryGetSystemType(ReturnTypeBase returnType, IList<GenericConfigBase> GenericArguments, ApplicationTypeInfo application, out Type? type);
        bool TryGetSystemType(string typeName, ApplicationTypeInfo application, out Type? type);
        bool TryGetSystemType(VariableBase variable, ApplicationTypeInfo application, out Type? variableType);
        bool TryGetSystemTypeForNonGeneric(ReturnTypeBase returnType, ApplicationTypeInfo application, out Type? type);
    }
}
