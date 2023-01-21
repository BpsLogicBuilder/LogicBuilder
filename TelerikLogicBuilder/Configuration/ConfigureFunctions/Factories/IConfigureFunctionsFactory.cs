using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Helpers;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Factories
{
    internal interface IConfigureFunctionsFactory
    {
        IConfigureFunctionsDragDropHandler GetConfigureFunctionsDragDropHandler(IConfigureFunctionsForm configureFunctionsForm);
        ConfigureFunctionsTreeView GetConfigureFunctionsTreeView(IConfigureFunctionsForm configureFunctionsForm);
    }
}
