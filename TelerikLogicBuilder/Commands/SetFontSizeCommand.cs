using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class SetFontSizeCommand : ClickCommandBase
    {
        private readonly IThemeManager _themeManager;
        private readonly RadMenuItem colorThemeMenuItem;
        private readonly RadMenuItem fontSizeMenuItem;
        private readonly int fontSize;

        public SetFontSizeCommand(
            IThemeManager themeManager,
            RadMenuItem colorThemeMenuItem,
            RadMenuItem fontSizeMenuItem,
            int fontSize)
        {
            _themeManager = themeManager;
            this.colorThemeMenuItem = colorThemeMenuItem;
            this.fontSizeMenuItem = fontSizeMenuItem;
            this.fontSize = fontSize;
        }

        public override void Execute()
        {
            _themeManager.SetFontSize(fontSize);
            _themeManager.CheckMenuItemsForCurrentSettings(colorThemeMenuItem.Items, fontSizeMenuItem.Items);
        }
    }
}
