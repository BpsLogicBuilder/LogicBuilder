using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructor;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Helpers.Factories
{
    internal class ConstructorControlValidatorFactory : IConstructorControlValidatorFactory
    {
        private readonly Func<IConfigureConstructorControl, IConstructorControlsValidator> _getConstructorControlsValidator;

        public ConstructorControlValidatorFactory(
            Func<IConfigureConstructorControl, IConstructorControlsValidator> getConstructorControlsValidator)
        {
            _getConstructorControlsValidator = getConstructorControlsValidator;
        }

        public IConstructorControlsValidator GetConstructorControlsValidator(IConfigureConstructorControl configureConstructorControl)
            => _getConstructorControlsValidator(configureConstructorControl);
    }
}
