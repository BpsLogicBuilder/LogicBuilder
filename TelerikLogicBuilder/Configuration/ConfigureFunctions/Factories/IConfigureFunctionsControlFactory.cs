using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunction;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunctionsFolder;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunctionsRootNode;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Factories
{
    internal interface IConfigureFunctionsControlFactory
    {
        IConfigureFunctionControl GetConfigureFunctionControl(IConfigureFunctionsForm configureFunctionsForm);
        IConfigureFunctionsFolderControl GetConfigureFunctionsFolderControl(IConfigureFunctionsForm configureFunctionsForm);
        IConfigureFunctionsRootNodeControl GetConfigureFunctionsRootNodeControl();
    }
}
