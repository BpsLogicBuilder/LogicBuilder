using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.ConfigureFragment;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.ConfigureFragmentsFolder;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.ConfigureFragmentsRootNode;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Factories
{
    internal class ConfigureFragmentsControlFactory : IConfigureFragmentsControlFactory
    {
        private readonly Func<IConfigureFragmentsForm, IConfigureFragmentControl> _getConfigureFragmentControl;
        private readonly Func<IConfigureFragmentsForm, IConfigureFragmentsFolderControl> _getConfigureFragmentsFolderControl;

        public ConfigureFragmentsControlFactory(
            Func<IConfigureFragmentsForm, IConfigureFragmentControl> getConfigureFragmentControl,
            Func<IConfigureFragmentsForm, IConfigureFragmentsFolderControl> getConfigureFragmentsFolderControl)
        {
            _getConfigureFragmentControl = getConfigureFragmentControl;
            _getConfigureFragmentsFolderControl = getConfigureFragmentsFolderControl;
        }

        public IConfigureFragmentControl GetConfigureFragmentControl(IConfigureFragmentsForm configureFragmentsForm)
            => _getConfigureFragmentControl(configureFragmentsForm);

        public IConfigureFragmentsFolderControl GetConfigureFragmentsFolderControl(IConfigureFragmentsForm configureFragmentsForm)
            => _getConfigureFragmentsFolderControl(configureFragmentsForm);

        public IConfigureFragmentsRootNodeControl GetConfigureFragmentsRootNodeControl()
            => new ConfigureFragmentsRootNodeControl();
    }
}
