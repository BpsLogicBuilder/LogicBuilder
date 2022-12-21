using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Factories
{
    internal class ConfigureVariablesCommandFactory : IConfigureVariablesCommandFactory
    {
        private readonly Func<IConfigureVariablesForm, AddLiteralListVariableCommand> _getAddLiteralListVariableCommand;
        private readonly Func<IConfigureVariablesForm, AddLiteralVariableCommand> _getAddLiteralVariableCommand;
        private readonly Func<IConfigureVariablesForm, AddObjectListVariableCommand> _getAddObjectListVariableCommand;
        private readonly Func<IConfigureVariablesForm, AddObjectVariableCommand> _getAddObjectVariableCommand;
        private readonly Func<IConfigureVariablesForm, ConfigureVariablesAddFolderCommand> _getConfigureVariablesAddFolderCommand;
        private readonly Func<IConfigureVariablesForm, ConfigureVariablesAddMembersCommand> _getConfigureVariablesAddMembersCommand;
        private readonly Func<IConfigureVariablesForm, ConfigureVariablesCopyXmlCommand> _getConfigureVariablesCopyXmlCommand;
        private readonly Func<IConfigureVariablesForm, ConfigureVariablesCutCommand> _getConfigureVariablesCutCommand;
        private readonly Func<IConfigureVariablesForm, ConfigureVariablesDeleteCommand> _getConfigureVariablesDeleteCommand;
        private readonly Func<IConfigureVariablesForm, ConfigureVariablesHelperCommand> _getConfigureVariablesHelperCommand;
        private readonly Func<IConfigureVariablesForm, ConfigureVariablesImportCommand> _getConfigureVariablesImportCommand;
        private readonly Func<IConfigureVariablesForm, ConfigureVariablesPasteCommand> _getConfigureVariablesPasteCommand;

        public ConfigureVariablesCommandFactory(
            Func<IConfigureVariablesForm, AddLiteralListVariableCommand> getAddLiteralListVariableCommand,
            Func<IConfigureVariablesForm, AddLiteralVariableCommand> getAddLiteralVariableCommand,
            Func<IConfigureVariablesForm, AddObjectListVariableCommand> getAddObjectListVariableCommand,
            Func<IConfigureVariablesForm, AddObjectVariableCommand> getAddObjectVariableCommand,
            Func<IConfigureVariablesForm, ConfigureVariablesAddFolderCommand> getConfigureVariablesAddFolderCommand,
            Func<IConfigureVariablesForm, ConfigureVariablesAddMembersCommand> getConfigureVariablesAddMembersCommand,
            Func<IConfigureVariablesForm, ConfigureVariablesCopyXmlCommand> getConfigureVariablesCopyXmlCommand,
            Func<IConfigureVariablesForm, ConfigureVariablesCutCommand> getConfigureVariablesCutCommand,
            Func<IConfigureVariablesForm, ConfigureVariablesDeleteCommand> getConfigureVariablesDeleteCommand,
            Func<IConfigureVariablesForm, ConfigureVariablesHelperCommand> getConfigureVariablesHelperCommand,
            Func<IConfigureVariablesForm, ConfigureVariablesImportCommand> getConfigureVariablesImportCommand,
            Func<IConfigureVariablesForm, ConfigureVariablesPasteCommand> getConfigureVariablesPasteCommand)
        {
            _getAddLiteralListVariableCommand = getAddLiteralListVariableCommand;
            _getAddLiteralVariableCommand = getAddLiteralVariableCommand;
            _getAddObjectListVariableCommand = getAddObjectListVariableCommand;
            _getAddObjectVariableCommand = getAddObjectVariableCommand;
            _getConfigureVariablesAddFolderCommand = getConfigureVariablesAddFolderCommand;
            _getConfigureVariablesAddMembersCommand = getConfigureVariablesAddMembersCommand;
            _getConfigureVariablesCopyXmlCommand = getConfigureVariablesCopyXmlCommand;
            _getConfigureVariablesCutCommand = getConfigureVariablesCutCommand;
            _getConfigureVariablesDeleteCommand = getConfigureVariablesDeleteCommand;
            _getConfigureVariablesHelperCommand = getConfigureVariablesHelperCommand;
            _getConfigureVariablesImportCommand = getConfigureVariablesImportCommand;
            _getConfigureVariablesPasteCommand = getConfigureVariablesPasteCommand;
        }

        public AddLiteralListVariableCommand GetAddLiteralListVariableCommand(IConfigureVariablesForm configureVariablesForm)
            => _getAddLiteralListVariableCommand(configureVariablesForm);

        public AddLiteralVariableCommand GetAddLiteralVariableCommand(IConfigureVariablesForm configureVariablesForm)
            => _getAddLiteralVariableCommand(configureVariablesForm);

        public AddObjectListVariableCommand GetAddObjectListVariableCommand(IConfigureVariablesForm configureVariablesForm)
            => _getAddObjectListVariableCommand(configureVariablesForm);

        public AddObjectVariableCommand GetAddObjectVariableCommand(IConfigureVariablesForm configureVariablesForm)
            => _getAddObjectVariableCommand(configureVariablesForm);

        public ConfigureVariablesAddFolderCommand GetConfigureVariablesAddFolderCommand(IConfigureVariablesForm configureVariablesForm)
            => _getConfigureVariablesAddFolderCommand(configureVariablesForm);

        public ConfigureVariablesAddMembersCommand GetConfigureVariablesAddMembersCommand(IConfigureVariablesForm configureVariablesForm)
            => _getConfigureVariablesAddMembersCommand(configureVariablesForm);

        public ConfigureVariablesCopyXmlCommand GetConfigureVariablesCopyXmlCommand(IConfigureVariablesForm configureVariablesForm)
            => _getConfigureVariablesCopyXmlCommand(configureVariablesForm);

        public ConfigureVariablesCutCommand GetConfigureVariablesCutCommand(IConfigureVariablesForm configureVariablesForm)
            => _getConfigureVariablesCutCommand(configureVariablesForm);

        public ConfigureVariablesDeleteCommand GetConfigureVariablesDeleteCommand(IConfigureVariablesForm configureVariablesForm)
            => _getConfigureVariablesDeleteCommand(configureVariablesForm);

        public ConfigureVariablesHelperCommand GetConfigureVariablesHelperCommand(IConfigureVariablesForm configureVariablesForm)
            => _getConfigureVariablesHelperCommand(configureVariablesForm);

        public ConfigureVariablesImportCommand GetConfigureVariablesImportCommand(IConfigureVariablesForm configureVariablesForm)
            => _getConfigureVariablesImportCommand(configureVariablesForm);

        public ConfigureVariablesPasteCommand GetConfigureVariablesPasteCommand(IConfigureVariablesForm configureVariablesForm)
            => _getConfigureVariablesPasteCommand(configureVariablesForm);
    }
}
