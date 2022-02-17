using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables
{
    internal interface IVariableHelper
    {
        bool CanBeInteger(VariableBase variable);
    }
}
