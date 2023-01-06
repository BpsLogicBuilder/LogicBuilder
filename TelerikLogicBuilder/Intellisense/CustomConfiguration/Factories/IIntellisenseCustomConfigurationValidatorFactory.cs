namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.CustomConfiguration.Factories
{
    internal interface IIntellisenseCustomConfigurationValidatorFactory
    {
        IIntellisenseVariableControlsValidator GetIntellisenseVariableControlsValidator(IIntellisenseVariableConfigurationControl intellisenseVariableConfigurationControl);
    }
}
