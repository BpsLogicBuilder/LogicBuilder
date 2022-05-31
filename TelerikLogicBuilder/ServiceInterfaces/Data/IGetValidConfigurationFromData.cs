using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data
{
    internal interface IGetValidConfigurationFromData
    {
        bool TryGetFunction(FunctionData functionData, ApplicationTypeInfo application, [NotNullWhen(true)] out Function? function);
        bool TryGetConstructor(ConstructorData constructorData, ApplicationTypeInfo application, [NotNullWhen(true)] out Constructor? constructor);
        bool TryGetVariable(VariableData variableData, ApplicationTypeInfo application, [NotNullWhen(true)] out VariableBase? variable);
        bool TryGetVariable(DecisionData decisionData, ApplicationTypeInfo application, [NotNullWhen(true)] out VariableBase? variable);
    }
}
