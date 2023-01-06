using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureFunctionsHelper
{
    internal interface IConfigureFunctionsHelperForm : IConfiguredItemHelperForm
    {
        Function Function { get; }
    }
}
