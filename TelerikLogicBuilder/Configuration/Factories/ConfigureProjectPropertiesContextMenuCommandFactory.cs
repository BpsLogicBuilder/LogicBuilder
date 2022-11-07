using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Factories
{
    internal class ConfigureProjectPropertiesContextMenuCommandFactory : IConfigureProjectPropertiesContextMenuCommandFactory
    {
        private readonly Func<IConfigureProjectProperties, AddApplicationCommand> _getAddApplicationCommand;
        private readonly Func<IConfigureProjectProperties, DeleteApplicationCommand> _getDeleteApplicationCommand;

        public ConfigureProjectPropertiesContextMenuCommandFactory(
            Func<IConfigureProjectProperties, AddApplicationCommand> getAddApplicationCommand,
            Func<IConfigureProjectProperties, DeleteApplicationCommand> getDeleteApplicationCommand)
        {
            _getAddApplicationCommand = getAddApplicationCommand;
            _getDeleteApplicationCommand = getDeleteApplicationCommand;
        }

        public AddApplicationCommand GetAddApplicationCommand(IConfigureProjectProperties configureProjectProperties)
            => _getAddApplicationCommand(configureProjectProperties);

        public DeleteApplicationCommand GetDeleteApplicationCommand(IConfigureProjectProperties configureProjectProperties)
            => _getDeleteApplicationCommand(configureProjectProperties);
    }
}
