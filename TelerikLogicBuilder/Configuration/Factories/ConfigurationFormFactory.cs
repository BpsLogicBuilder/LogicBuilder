using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Factories
{
    internal class ConfigurationFormFactory : IConfigurationFormFactory
    {
        private Form? _scopedService;
        private readonly Func<bool, ConfigureProjectProperties> _getConfigureProjectProperties;
        private readonly IServiceScope _scope;

        public ConfigurationFormFactory(
            IServiceScopeFactory serviceScopeFactory,
            Func<bool, ConfigureProjectProperties> getConfigureProjectProperties)
        {
            _scope = serviceScopeFactory.CreateScope();
            _getConfigureProjectProperties = getConfigureProjectProperties;
        }

        public ConfigureProjectProperties GetConfigureProjectProperties(bool openedAsReadOnly)
        {
            _scopedService = _getConfigureProjectProperties(openedAsReadOnly);
            return (ConfigureProjectProperties)_scopedService;
        }

        public void Dispose()
        {
            //The factory methods uses new() (outside the container) because of the parameter
            //so we have to dispose of the service manually (_scope.Dispose() will not dispose _scopedService).
            _scopedService?.Dispose();
            _scope.Dispose();
        }
    }
}
