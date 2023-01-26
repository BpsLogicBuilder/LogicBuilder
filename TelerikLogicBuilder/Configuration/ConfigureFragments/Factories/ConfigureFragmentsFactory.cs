using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Helpers;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Factories
{
    internal class ConfigureFragmentsFactory : IConfigureFragmentsFactory
    {
        private readonly Func<IConfigureFragmentsForm, IConfigureFragmentsDragDropHandler> _getConfigureFragmentsDragDropHandler;
        private readonly Func<IConfigureFragmentsForm, ConfigureFragmentsTreeView> _getConfigureFragmentsTreeView;

        public ConfigureFragmentsFactory(
            Func<IConfigureFragmentsForm, IConfigureFragmentsDragDropHandler> getConfigureFragmentsDragDropHandler,
            Func<IConfigureFragmentsForm, ConfigureFragmentsTreeView> getConfigureFragmentsTreeView)
        {
            _getConfigureFragmentsDragDropHandler = getConfigureFragmentsDragDropHandler;
            _getConfigureFragmentsTreeView = getConfigureFragmentsTreeView;
        }

        public IConfigureFragmentsDragDropHandler GetConfigureFragmentsDragDropHandler(IConfigureFragmentsForm configureFragmentsForm)
            => _getConfigureFragmentsDragDropHandler(configureFragmentsForm);

        public ConfigureFragmentsTreeView GetConfigureFragmentsTreeView(IConfigureFragmentsForm configureFragmentsForm)
            => _getConfigureFragmentsTreeView(configureFragmentsForm);
    }
}
