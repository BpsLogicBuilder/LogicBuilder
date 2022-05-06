using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IThemeManager
    {
        void CheckMenuItemForCurrentTheme(RadItemOwnerCollection themeMenuItems);
        void SetTheme(string themeName);
    }
}
