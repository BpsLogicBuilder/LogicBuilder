using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructor;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructorsFolder;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructorsRootNode;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Factories
{
    internal class ConfigureConstructorsControlFactory : IConfigureConstructorsControlFactory
    {
        private readonly Func<IConfigureConstructorsForm, IConfigureConstructorControl> _getConfigureConstructorControl;
        private readonly Func<IConfigureConstructorsForm, IConfigureConstructorsFolderControl> _getConfigureConstructorsFolderControl;

        public ConfigureConstructorsControlFactory(
            Func<IConfigureConstructorsForm, IConfigureConstructorControl> getConfigureConstructorControl,
            Func<IConfigureConstructorsForm, IConfigureConstructorsFolderControl> getConfigureConstructorsFolderControl)
        {
            _getConfigureConstructorControl = getConfigureConstructorControl;
            _getConfigureConstructorsFolderControl = getConfigureConstructorsFolderControl;
        }

        public IConfigureConstructorControl GetConfigureConstructorControl(IConfigureConstructorsForm configureConstructorsForm)
            => _getConfigureConstructorControl(configureConstructorsForm);

        public IConfigureConstructorsFolderControl GetConfigureConstructorsFolderControl(IConfigureConstructorsForm configureConstructorsForm)
            => _getConfigureConstructorsFolderControl(configureConstructorsForm);

        public IConfigureConstructorsRootNodeControl GetConfigureConstructorsRootNodeControl()
            => new ConfigureConstructorsRootNodeControl();
    }
}
