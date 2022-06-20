using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class SetThemeCommand : ClickCommandBase
    {
        private readonly IThemeManager _themeManager;
        private readonly RadMenuItem themeMenuItem;
        private readonly string themeName;

        public SetThemeCommand(IThemeManager themeManager, RadMenuItem themeMenuItem, string themeName)
        {
            _themeManager = themeManager;
            this.themeMenuItem = themeMenuItem;
            this.themeName = themeName;
        }

        public override void Execute()
        {
            _themeManager.SetTheme(this.themeName);
            _themeManager.CheckMenuItemForCurrentTheme(themeMenuItem.Items);
        }
    }
}
