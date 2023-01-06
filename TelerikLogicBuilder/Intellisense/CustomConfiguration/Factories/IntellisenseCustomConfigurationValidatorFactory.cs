using System;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.CustomConfiguration.Factories
{
    internal class IntellisenseCustomConfigurationValidatorFactory : IIntellisenseCustomConfigurationValidatorFactory
    {
        private readonly Func<IIntellisenseVariableConfigurationControl, IIntellisenseVariableControlsValidator> _getIntellisenseVariableControlsValidator;

        public IntellisenseCustomConfigurationValidatorFactory(
            Func<IIntellisenseVariableConfigurationControl, IIntellisenseVariableControlsValidator> getIntellisenseVariableControlsValidator)
        {
            _getIntellisenseVariableControlsValidator = getIntellisenseVariableControlsValidator;
        }

        public IIntellisenseVariableControlsValidator GetIntellisenseVariableControlsValidator(IIntellisenseVariableConfigurationControl intellisenseVariableConfigurationControl)
            => _getIntellisenseVariableControlsValidator(intellisenseVariableConfigurationControl);
    }
}
