using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data
{
    internal interface IGetValidConfigurationFromData
    {
        bool TryGetFunction(FunctionData functionData, ApplicationTypeInfo application, out Function? function);
        bool TryGetConstructor(ConstructorData constructorData, ApplicationTypeInfo application, out Constructor? constructor);
        bool TryGetVariable(VariableData variableData, ApplicationTypeInfo application, out VariableBase? variable);
        bool TryGetVariable(DecisionData decisionData, ApplicationTypeInfo application, out VariableBase? variable);
    }
}
