using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunction;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Helpers.Factories
{
    internal interface IFunctionControlValidatorFactory
    {
        IFunctionControlsValidator GetFunctionControlsValidator(IConfigureFunctionControl configureFunctionControl);
    }
}
