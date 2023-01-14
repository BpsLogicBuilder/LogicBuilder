using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Helpers;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Factories
{
    internal class ConfigureConstructorsFactory : IConfigureConstructorsFactory
    {
        private readonly Func<IConfigureConstructorsForm, IConfigureConstructorsDragDropHandler> _getConfigureConstructorsDragDropHandler;
        private readonly Func<IConfigureConstructorsForm, ConfigureConstructorsTreeView> _getConfigureConstructorsTreeView;

        public ConfigureConstructorsFactory(
            Func<IConfigureConstructorsForm, IConfigureConstructorsDragDropHandler> getConfigureConstructorsDragDropHandler,
            Func<IConfigureConstructorsForm, ConfigureConstructorsTreeView> getConfigureConstructorsTreeView)
        {
            _getConfigureConstructorsDragDropHandler = getConfigureConstructorsDragDropHandler;
            _getConfigureConstructorsTreeView = getConfigureConstructorsTreeView;
        }

        public IConfigureConstructorsDragDropHandler GetConfigureConstructorsDragDropHandler(IConfigureConstructorsForm configureConstructorsForm)
            => _getConfigureConstructorsDragDropHandler(configureConstructorsForm);

        public ConfigureConstructorsTreeView GetConfigureConstructorsTreeView(IConfigureConstructorsForm configureConstructorsForm)
            => _getConfigureConstructorsTreeView(configureConstructorsForm);
    }
}
