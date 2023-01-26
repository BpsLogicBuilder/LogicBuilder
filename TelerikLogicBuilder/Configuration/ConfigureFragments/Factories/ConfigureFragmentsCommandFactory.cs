using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Factories
{
    internal class ConfigureFragmentsCommandFactory : IConfigureFragmentsCommandFactory
    {
        private readonly Func<IConfigureFragmentsForm, ConfigureFragmentsAddFolderCommand> _getConfigureFragmentsAddFolderCommand;
        private readonly Func<IConfigureFragmentsForm, ConfigureFragmentsAddFragmentCommand> _getConfigureFragmentsAddFragmentCommand;
        private readonly Func<IConfigureFragmentsForm, ConfigureFragmentsCopyXmlCommand> _getConfigureFragmentsCopyXmlCommand;
        private readonly Func<IConfigureFragmentsForm, ConfigureFragmentsCutCommand> _getConfigureFragmentsCutCommand;
        private readonly Func<IConfigureFragmentsForm, ConfigureFragmentsDeleteCommand> _getConfigureFragmentsDeleteCommand;
        private readonly Func<IConfigureFragmentsForm, ConfigureFragmentsImportCommand> _getConfigureFragmentsImportCommand;
        private readonly Func<IConfigureFragmentsForm, ConfigureFragmentsPasteCommand> _getConfigureFragmentsPasteCommand;

        public ConfigureFragmentsCommandFactory(
            Func<IConfigureFragmentsForm, ConfigureFragmentsAddFolderCommand> getConfigureFragmentsAddFolderCommand,
            Func<IConfigureFragmentsForm, ConfigureFragmentsAddFragmentCommand> getConfigureFragmentsAddFragmentCommand,
            Func<IConfigureFragmentsForm, ConfigureFragmentsCopyXmlCommand> getConfigureFragmentsCopyXmlCommand,
            Func<IConfigureFragmentsForm, ConfigureFragmentsCutCommand> getConfigureFragmentsCutCommand,
            Func<IConfigureFragmentsForm, ConfigureFragmentsDeleteCommand> getConfigureFragmentsDeleteCommand,
            Func<IConfigureFragmentsForm, ConfigureFragmentsImportCommand> getConfigureFragmentsImportCommand,
            Func<IConfigureFragmentsForm, ConfigureFragmentsPasteCommand> getConfigureFragmentsPasteCommand)
        {
            _getConfigureFragmentsAddFolderCommand = getConfigureFragmentsAddFolderCommand;
            _getConfigureFragmentsAddFragmentCommand = getConfigureFragmentsAddFragmentCommand;
            _getConfigureFragmentsCopyXmlCommand = getConfigureFragmentsCopyXmlCommand;
            _getConfigureFragmentsCutCommand = getConfigureFragmentsCutCommand;
            _getConfigureFragmentsDeleteCommand = getConfigureFragmentsDeleteCommand;
            _getConfigureFragmentsImportCommand = getConfigureFragmentsImportCommand;
            _getConfigureFragmentsPasteCommand = getConfigureFragmentsPasteCommand;
        }

        public ConfigureFragmentsAddFolderCommand GetConfigureFragmentsAddFolderCommand(IConfigureFragmentsForm configureFragmentsForm)
            => _getConfigureFragmentsAddFolderCommand(configureFragmentsForm);

        public ConfigureFragmentsAddFragmentCommand GetConfigureFragmentsAddFragmentCommand(IConfigureFragmentsForm configureFragmentsForm)
            => _getConfigureFragmentsAddFragmentCommand(configureFragmentsForm);

        public ConfigureFragmentsCopyXmlCommand GetConfigureFragmentsCopyXmlCommand(IConfigureFragmentsForm configureFragmentsForm)
            => _getConfigureFragmentsCopyXmlCommand(configureFragmentsForm);

        public ConfigureFragmentsCutCommand GetConfigureFragmentsCutCommand(IConfigureFragmentsForm configureFragmentsForm)
            => _getConfigureFragmentsCutCommand(configureFragmentsForm);

        public ConfigureFragmentsDeleteCommand GetConfigureFragmentsDeleteCommand(IConfigureFragmentsForm configureFragmentsForm)
            => _getConfigureFragmentsDeleteCommand(configureFragmentsForm);

        public ConfigureFragmentsImportCommand GetConfigureFragmentsImportCommand(IConfigureFragmentsForm configureFragmentsForm)
            => _getConfigureFragmentsImportCommand(configureFragmentsForm);

        public ConfigureFragmentsPasteCommand GetConfigureFragmentsPasteCommand(IConfigureFragmentsForm configureFragmentsForm)
            => _getConfigureFragmentsPasteCommand(configureFragmentsForm);
    }
}
