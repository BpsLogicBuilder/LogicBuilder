using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class ThemeManager : IThemeManager
    {
        private readonly IExceptionHelper _exceptionHelper;

        public ThemeManager(IExceptionHelper exceptionHelper)
        {
            _exceptionHelper = exceptionHelper;
        }

        public void CheckMenuItemForCurrentTheme(RadItemOwnerCollection themeMenuItems)
        {
            string currentTheme = ThemeResolutionService.ApplicationThemeName;
            foreach (Telerik.WinControls.UI.RadMenuItem menuItem in themeMenuItems)
                menuItem.IsChecked = (string)menuItem.Tag == currentTheme;
        }

        public void SetTheme(string themeName)
        {
            if (!ThemeCollections.OfficeThemes.Contains(themeName))
                throw _exceptionHelper.CriticalException("{22FDC293-A727-4845-AF68-A3275B239D4E}");

            ThemeResolutionService.ApplicationThemeName = themeName;
            Properties.Settings.Default.themeName = ThemeResolutionService.ApplicationThemeName;
            Properties.Settings.Default.Save();
        }
    }
}
