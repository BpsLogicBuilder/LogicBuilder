using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Factories
{
    internal class ConfigurationControlFactory : IConfigurationControlFactory
    {
        private readonly Func<IConfigureProjectProperties, IApplicationControl> _getApplicationControl;
        private readonly Func<IConfigureLoadAssemblyPaths, ILoadAssemblyPathsControl> _getLoadAssemblyPathsControl;

        public ConfigurationControlFactory(
            Func<IConfigureProjectProperties, IApplicationControl> getApplicationControl,
            Func<IConfigureLoadAssemblyPaths, ILoadAssemblyPathsControl> getLoadAssemblyPathsControl)
        {
            _getApplicationControl = getApplicationControl;
            _getLoadAssemblyPathsControl = getLoadAssemblyPathsControl;
        }

        public IApplicationControl GetApplicationControl(IConfigureProjectProperties configureProjectProperties)
            => _getApplicationControl(configureProjectProperties);

        public ILoadAssemblyPathsControl GetLoadAssemblyPathsControl(IConfigureLoadAssemblyPaths configureLoadAssemblyPaths)
            => _getLoadAssemblyPathsControl(configureLoadAssemblyPaths);
    }
}
