using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureVariablesHelper
{
    internal interface IConfigureVariablesHelperForm : IConfiguredItemHelperForm
    {
        VariableBase Variable { get; }
    }
}
