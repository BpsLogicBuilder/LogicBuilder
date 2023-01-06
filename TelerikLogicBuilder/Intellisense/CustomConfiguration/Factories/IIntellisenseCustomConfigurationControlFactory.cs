namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.CustomConfiguration.Factories
{
    internal interface IIntellisenseCustomConfigurationControlFactory
    {
        IArrayIndexerConfigurationControl GetArrayIndexerConfigurationControl(IConfiguredItemHelperForm configuredItemHelperForm);
        IFieldConfigurationControl GetFieldConfigurationControl(IConfiguredItemHelperForm configuredItemHelperForm);
        IFunctionConfigurationControl GetFunctionConfigurationControl();
        IIndexerConfigurationControl GetIndexerConfigurationControl(IConfiguredItemHelperForm configuredItemHelperForm);
        IPropertyConfigurationControl GetPropertyConfigurationControl(IConfiguredItemHelperForm configuredItemHelperForm);
    }
}
