using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface ITypeLoadHelper
    {
        bool TryGetSystemType(GenericConfigBase config, ApplicationTypeInfo application, [NotNullWhen(true)] out Type? type);
        bool TryGetSystemType(ParameterBase parameter, ApplicationTypeInfo application, [NotNullWhen(true)] out Type? type);
        bool TryGetSystemType(ReturnTypeBase returnType, IList<GenericConfigBase> GenericArguments, ApplicationTypeInfo application, [NotNullWhen(true)] out Type? type);
        bool TryGetSystemType(string typeName, ApplicationTypeInfo application, [NotNullWhen(true)] out Type? type);
        bool TryGetSystemType(VariableBase variable, ApplicationTypeInfo application, [NotNullWhen(true)] out Type? variableType);
    }
}
