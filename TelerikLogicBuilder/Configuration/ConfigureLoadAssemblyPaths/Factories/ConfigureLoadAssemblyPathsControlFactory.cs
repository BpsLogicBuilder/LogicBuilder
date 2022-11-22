using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths.Factories
{
    internal class ConfigureLoadAssemblyPathsControlFactory : IConfigureLoadAssemblyPathsControlFactory
    {
        private readonly Func<IConfigureLoadAssemblyPathsForm, IConfigureLoadAssemblyPathsControl> _getConfigureLoadAssemblyPathsControl;

        public ConfigureLoadAssemblyPathsControlFactory(
            Func<IConfigureLoadAssemblyPathsForm, IConfigureLoadAssemblyPathsControl> getConfigureLoadAssemblyPathsControl)
        {
            _getConfigureLoadAssemblyPathsControl = getConfigureLoadAssemblyPathsControl;
        }

        public IConfigureLoadAssemblyPathsControl GetLoadAssemblyPathsControl(IConfigureLoadAssemblyPathsForm configureLoadAssemblyPathsForm)
            => _getConfigureLoadAssemblyPathsControl(configureLoadAssemblyPathsForm);
    }
}
