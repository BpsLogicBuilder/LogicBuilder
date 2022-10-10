using System;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Commands.Factories
{
    internal class ApplicationCommandsFactory : IApplicationCommandsFactory
    {
        private readonly Func<string, DeleteSelectedFilesFromApiCommand> _getDeleteSelectedFilesFromApiCommand;
        private readonly Func<string, DeleteSelectedFilesFromFileSystemCommand> _getDeleteSelectedFilesFromFileSystemCommand;
        private readonly Func<string, DeploySelectedFilesToApiCommand> _getDeploySelectedFilesToApiCommand;
        private readonly Func<string, DeploySelectedFilesToFileSystemCommand> _getDeploySelectedFilesToFileSystemCommand;
        private readonly Func<RadMenuItem, string, SetSelectedApplicationCommand> _getSetSelectedApplicationCommand;
        private readonly Func<RadMenuItem, string, SetThemeCommand> _getSetThemeCommand;
        private readonly Func<string, ValidateSelectedRulesCommand> _getValidateSelectedRulesCommand;

        public ApplicationCommandsFactory(
            Func<string, DeleteSelectedFilesFromApiCommand> getDeleteSelectedFilesFromApiCommand,
            Func<string, DeleteSelectedFilesFromFileSystemCommand> getDeleteSelectedFilesFromFileSystemCommand,
            Func<string, DeploySelectedFilesToApiCommand> getDeploySelectedFilesToApiCommand,
            Func<string, DeploySelectedFilesToFileSystemCommand> getDeploySelectedFilesToFileSystemCommand,
            Func<RadMenuItem, string, SetSelectedApplicationCommand> getSetSelectedApplicationCommand,
            Func<RadMenuItem, string, SetThemeCommand> getSetThemeCommand,
            Func<string, ValidateSelectedRulesCommand> getValidateSelectedRulesCommand)
        {
            _getDeleteSelectedFilesFromApiCommand = getDeleteSelectedFilesFromApiCommand;
            _getDeleteSelectedFilesFromFileSystemCommand = getDeleteSelectedFilesFromFileSystemCommand;
            _getDeploySelectedFilesToApiCommand = getDeploySelectedFilesToApiCommand;
            _getDeploySelectedFilesToFileSystemCommand = getDeploySelectedFilesToFileSystemCommand;
            _getSetSelectedApplicationCommand = getSetSelectedApplicationCommand;
            _getSetThemeCommand = getSetThemeCommand;
            _getValidateSelectedRulesCommand = getValidateSelectedRulesCommand;
        }

        public DeleteSelectedFilesFromApiCommand GetDeleteSelectedFilesFromApiCommand(string applicationName)
            => _getDeleteSelectedFilesFromApiCommand(applicationName);

        public DeleteSelectedFilesFromFileSystemCommand GetDeleteSelectedFilesFromFileSystemCommand(string applicationName)
            => _getDeleteSelectedFilesFromFileSystemCommand(applicationName);

        public DeploySelectedFilesToApiCommand GetDeploySelectedFilesToApiCommand(string applicationName)
            => _getDeploySelectedFilesToApiCommand(applicationName);

        public DeploySelectedFilesToFileSystemCommand GetDeploySelectedFilesToFileSystemCommand(string applicationName)
            => _getDeploySelectedFilesToFileSystemCommand(applicationName);

        public SetSelectedApplicationCommand GetSetSelectedApplicationCommand(RadMenuItem menuItem, string applicationName)
            => _getSetSelectedApplicationCommand(menuItem, applicationName);

        public SetThemeCommand GetSetThemeCommand(RadMenuItem menuItem, string applicationName)
            => _getSetThemeCommand(menuItem, applicationName);

        public ValidateSelectedRulesCommand GetValidateSelectedRulesCommand(string applicationName)
            => _getValidateSelectedRulesCommand(applicationName);
    }
}
