using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories
{
    internal class ConfigureProjectPropertiesContextMenuCommandFactory : IConfigureProjectPropertiesContextMenuCommandFactory
    {
        private readonly Func<IConfigureProjectPropertiesForm, AddApplicationCommand> _getAddApplicationCommand;
        private readonly Func<IConfigureProjectPropertiesForm, DeleteApplicationCommand> _getDeleteApplicationCommand;

        public ConfigureProjectPropertiesContextMenuCommandFactory(
            Func<IConfigureProjectPropertiesForm, AddApplicationCommand> getAddApplicationCommand,
            Func<IConfigureProjectPropertiesForm, DeleteApplicationCommand> getDeleteApplicationCommand)
        {
            _getAddApplicationCommand = getAddApplicationCommand;
            _getDeleteApplicationCommand = getDeleteApplicationCommand;
        }

        public AddApplicationCommand GetAddApplicationCommand(IConfigureProjectPropertiesForm configureProjectProperties)
            => _getAddApplicationCommand(configureProjectProperties);

        public DeleteApplicationCommand GetDeleteApplicationCommand(IConfigureProjectPropertiesForm configureProjectProperties)
            => _getDeleteApplicationCommand(configureProjectProperties);
    }
}
