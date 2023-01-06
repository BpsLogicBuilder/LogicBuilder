using System;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.CustomConfiguration.Factories
{
    internal class IntellisenseCustomConfigurationControlFactory : IIntellisenseCustomConfigurationControlFactory
    {
        private readonly Func<IConfiguredItemHelperForm, IArrayIndexerConfigurationControl> _getArrayIndexerConfigurationControl;
        private readonly Func<IConfiguredItemHelperForm, IFieldConfigurationControl> _getFieldConfigurationControl;
        private readonly IFunctionConfigurationControl _functionConfigurationControl;
        private readonly Func<IConfiguredItemHelperForm, IIndexerConfigurationControl> _getIndexerConfigurationControl;
        private readonly Func<IConfiguredItemHelperForm, IPropertyConfigurationControl> _getPropertyConfigurationControl;

        public IntellisenseCustomConfigurationControlFactory(
            Func<IConfiguredItemHelperForm, IArrayIndexerConfigurationControl> getArrayIndexerConfigurationControl,
            Func<IConfiguredItemHelperForm, IFieldConfigurationControl> getFieldConfigurationControl,
            IFunctionConfigurationControl functionConfigurationControl,
            Func<IConfiguredItemHelperForm, IIndexerConfigurationControl> getIndexerConfigurationControl,
            Func<IConfiguredItemHelperForm, IPropertyConfigurationControl> getPropertyConfigurationControl)
        {
            _getArrayIndexerConfigurationControl = getArrayIndexerConfigurationControl;
            _getFieldConfigurationControl = getFieldConfigurationControl;
            _functionConfigurationControl = functionConfigurationControl;
            _getIndexerConfigurationControl = getIndexerConfigurationControl;
            _getPropertyConfigurationControl = getPropertyConfigurationControl;
        }

        public IArrayIndexerConfigurationControl GetArrayIndexerConfigurationControl(IConfiguredItemHelperForm configuredItemHelperForm)
            => _getArrayIndexerConfigurationControl(configuredItemHelperForm);

        public IFieldConfigurationControl GetFieldConfigurationControl(IConfiguredItemHelperForm configuredItemHelperForm)
            => _getFieldConfigurationControl(configuredItemHelperForm);

        public IFunctionConfigurationControl GetFunctionConfigurationControl()
            => _functionConfigurationControl;

        public IIndexerConfigurationControl GetIndexerConfigurationControl(IConfiguredItemHelperForm configuredItemHelperForm)
            => _getIndexerConfigurationControl(configuredItemHelperForm);

        public IPropertyConfigurationControl GetPropertyConfigurationControl(IConfiguredItemHelperForm configuredItemHelperForm)
            => _getPropertyConfigurationControl(configuredItemHelperForm);
    }
}
