using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Commands.Factories
{
    internal interface IApplicationCommandsFactory
    {
        DeleteSelectedFilesFromApiCommand GetDeleteSelectedFilesFromApiCommand(string applicationName);
        DeleteSelectedFilesFromFileSystemCommand GetDeleteSelectedFilesFromFileSystemCommand(string applicationName);
        DeploySelectedFilesToApiCommand GetDeploySelectedFilesToApiCommand(string applicationName);
        DeploySelectedFilesToFileSystemCommand GetDeploySelectedFilesToFileSystemCommand(string applicationName);
        SetColorThemeCommand GetSetColorThemeCommand(RadMenuItem colorThemeMenuItem, RadMenuItem fontSizeMenuItems, string colorTheme);
        SetFontSizeCommand GetSetFontSizeCommand(RadMenuItem colorThemeMenuItem, RadMenuItem fontSizeMenuItems, int fontSize);
        SetSelectedApplicationCommand GetSetSelectedApplicationCommand(RadMenuItem menuItem, string applicationName);
        SetThemeCommand GetSetThemeCommand(RadMenuItem menuItem, string applicationName);
        ValidateSelectedRulesCommand GetValidateSelectedRulesCommand(string applicationName);
    }
}
