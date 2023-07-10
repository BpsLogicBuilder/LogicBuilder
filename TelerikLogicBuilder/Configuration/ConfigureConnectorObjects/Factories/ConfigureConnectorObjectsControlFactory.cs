using ABIS.LogicBuilder.FlowBuilder.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects.Factories
{
    internal class ConfigureConnectorObjectsControlFactory : IConfigureConnectorObjectsControlFactory
    {
        public IConfigureConnectorObjectsControl GetConfigureConnectorObjectsControl(IConfigureConnectorObjectsForm configureConnectorObjectsForm)
            => new ConfigureConnectorObjectsControl
            (
                Program.ServiceProvider.GetRequiredService<IConfigureConnectorObjectsCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IConnectorObjectsItemFactory>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                configureConnectorObjectsForm
            );
    }
}
