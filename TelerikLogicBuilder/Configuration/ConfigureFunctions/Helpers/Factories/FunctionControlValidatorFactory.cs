using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunction;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Helpers.Factories
{
    internal class FunctionControlValidatorFactory : IFunctionControlValidatorFactory
    {
        private readonly Func<IConfigureFunctionControl, IFunctionControlsValidator> _getFunctionControlsValidator;

        public FunctionControlValidatorFactory(
            Func<IConfigureFunctionControl, IFunctionControlsValidator> getFunctionControlsValidator)
        {
            _getFunctionControlsValidator = getFunctionControlsValidator;
        }

        public IFunctionControlsValidator GetFunctionControlsValidator(IConfigureFunctionControl configureFunctionControl)
            => _getFunctionControlsValidator(configureFunctionControl);
    }
}
