using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class SetColorThemeCommand : ClickCommandBase
    {
        private readonly IThemeManager _themeManager;
        private readonly RadMenuItem colorThemeMenuItem;
        private readonly RadMenuItem fontSizeMenuItem;
        private readonly string colorTheme;

        public SetColorThemeCommand(
            IThemeManager themeManager,
            RadMenuItem colorThemeMenuItem,
            RadMenuItem fontSizeMenuItem,
            string colorTheme)
        {
            _themeManager = themeManager;
            this.colorThemeMenuItem = colorThemeMenuItem;
            this.fontSizeMenuItem = fontSizeMenuItem;
            this.colorTheme = colorTheme;
        }

        public override void Execute()
        {
            _themeManager.SetColorTheme(colorTheme);
            _themeManager.CheckMenuItemsForCurrentSettings(colorThemeMenuItem.Items, fontSizeMenuItem.Items);
        }
    }
}
