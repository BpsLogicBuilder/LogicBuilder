using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IThemeManager
    {
        void CheckMenuItemForCurrentTheme(RadItemOwnerCollection themeMenuItems);
        void CheckMenuItemsForCurrentSettings(RadItemOwnerCollection colorThemeMenuItems, RadItemOwnerCollection fontSizeMenuItems);
        void SetColorTheme(string colorTheme);
        void SetFontSize(int fontSize);
        void SetTheme(string themeName);
    }
}
