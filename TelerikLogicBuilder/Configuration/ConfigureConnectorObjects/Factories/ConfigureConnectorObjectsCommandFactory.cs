using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects.Factories
{
    internal class ConfigureConnectorObjectsCommandFactory : IConfigureConnectorObjectsCommandFactory
    {
        public AddConnectorObjectListBoxItemCommand GetAddConnectorObjectListBoxItemCommand(IConfigureConnectorObjectsControl configureConnectorObjectsControl)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IConnectorObjectsItemFactory>(),
                configureConnectorObjectsControl
            );

        public UpdateConnectorObjectListBoxItemCommand GetUpdateConnectorObjectListBoxItemCommand(IConfigureConnectorObjectsControl configureConnectorObjectsControl)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IConnectorObjectsItemFactory>(),
                configureConnectorObjectsControl
            );
    }
}
