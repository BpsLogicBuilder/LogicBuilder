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
        private readonly Func<RadMenuItem, RadMenuItem, string, SetColorThemeCommand> _getSetColorThemeCommand;
        private readonly Func<RadMenuItem, RadMenuItem, int, SetFontSizeCommand> _getSetFontSizeCommand;
        private readonly Func<RadMenuItem, string, SetSelectedApplicationCommand> _getSetSelectedApplicationCommand;
        private readonly Func<string, ValidateSelectedRulesCommand> _getValidateSelectedRulesCommand;

        public ApplicationCommandsFactory(
            Func<string, DeleteSelectedFilesFromApiCommand> getDeleteSelectedFilesFromApiCommand,
            Func<string, DeleteSelectedFilesFromFileSystemCommand> getDeleteSelectedFilesFromFileSystemCommand,
            Func<string, DeploySelectedFilesToApiCommand> getDeploySelectedFilesToApiCommand,
            Func<string, DeploySelectedFilesToFileSystemCommand> getDeploySelectedFilesToFileSystemCommand,
            Func<RadMenuItem, RadMenuItem, string, SetColorThemeCommand> getSetColorThemeCommand,
            Func<RadMenuItem, RadMenuItem, int, SetFontSizeCommand> getSetFontSizeCommand,
            Func<RadMenuItem, string, SetSelectedApplicationCommand> getSetSelectedApplicationCommand,
            Func<string, ValidateSelectedRulesCommand> getValidateSelectedRulesCommand)
        {
            _getDeleteSelectedFilesFromApiCommand = getDeleteSelectedFilesFromApiCommand;
            _getDeleteSelectedFilesFromFileSystemCommand = getDeleteSelectedFilesFromFileSystemCommand;
            _getDeploySelectedFilesToApiCommand = getDeploySelectedFilesToApiCommand;
            _getDeploySelectedFilesToFileSystemCommand = getDeploySelectedFilesToFileSystemCommand;
            _getSetColorThemeCommand = getSetColorThemeCommand;
            _getSetSelectedApplicationCommand = getSetSelectedApplicationCommand;
            _getSetFontSizeCommand = getSetFontSizeCommand;
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

        public SetColorThemeCommand GetSetColorThemeCommand(RadMenuItem colorThemeMenuItem, RadMenuItem fontSizeMenuItems, string colorTheme)
            => _getSetColorThemeCommand(colorThemeMenuItem, fontSizeMenuItems, colorTheme);

        public SetFontSizeCommand GetSetFontSizeCommand(RadMenuItem colorThemeMenuItem, RadMenuItem fontSizeMenuItems, int fontSize)
            => _getSetFontSizeCommand(colorThemeMenuItem, fontSizeMenuItems, fontSize);

        public SetSelectedApplicationCommand GetSetSelectedApplicationCommand(RadMenuItem menuItem, string applicationName)
            => _getSetSelectedApplicationCommand(menuItem, applicationName);

        public ValidateSelectedRulesCommand GetValidateSelectedRulesCommand(string applicationName)
            => _getValidateSelectedRulesCommand(applicationName);
    }
}
