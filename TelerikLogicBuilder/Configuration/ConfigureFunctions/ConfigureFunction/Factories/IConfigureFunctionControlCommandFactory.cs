using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunction.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunction.Factories
{
    internal interface IConfigureFunctionControlCommandFactory
    {
        ConfigureFunctionReturnTypeCommand GetConfigureFunctionReturnTypeCommand(IConfigureFunctionControl configureFunctionControl);
        EditFunctionGenericArgumentsCommand GetEditFunctionGenericArgumentsCommand(IConfigureFunctionControl configureFunctionControl);
    }
}
