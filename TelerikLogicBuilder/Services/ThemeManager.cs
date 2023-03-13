using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Globalization;
using System.Linq;
using Telerik.WinControls;
using Telerik.WinControls.UI;

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
            foreach (RadMenuItem menuItem in themeMenuItems.Cast<RadMenuItem>())
                menuItem.IsChecked = (string)menuItem.Tag == currentTheme;
        }

        public void CheckMenuItemsForCurrentSettings(RadItemOwnerCollection colorThemeMenuItems, RadItemOwnerCollection fontSizeMenuItems)
        {
            ThemeSelector selectorFromSettings = new(Properties.Settings.Default.colorTheme, Properties.Settings.Default.fontSize);
            if (ThemeCollections.ThemeToSelector.TryGetValue(ThemeResolutionService.ApplicationThemeName, out ThemeSelector? selectorFromTheme) 
                && selectorFromSettings == selectorFromTheme)
            {
                CheckItems(colorThemeMenuItems, menuItem => (string)menuItem.Tag == selectorFromSettings.ColorTheme);
                CheckItems
                (
                    fontSizeMenuItems, 
                    menuItem => int.Parse
                    (
                        menuItem.Tag.ToString() ?? throw _exceptionHelper.CriticalException("{47E2C449-1518-4AF9-A9EE-959D2E5AF7C7}"), 
                        CultureInfo.InvariantCulture
                    ) == selectorFromSettings.FontSize
                );
            }
            else
            {
                CheckItems(colorThemeMenuItems, menuItem => false);
                CheckItems(fontSizeMenuItems, menuItem => false);
            }

            static void CheckItems(RadItemOwnerCollection menuItems, System.Func<RadMenuItem, bool> shouldCheck)
            {
                foreach (RadMenuItem menuItem in menuItems.Cast<RadMenuItem>())
                    menuItem.IsChecked = shouldCheck(menuItem);
            }
        }

        public void SetColorTheme(string colorTheme)
        {
            ThemeResolutionService.ApplicationThemeName = ThemeCollections.SelectorToTheme.TryGetValue(new ThemeSelector(colorTheme, Properties.Settings.Default.fontSize), out string? theme)
                ? theme 
                : ThemeCollections.ControlDefault;

            Properties.Settings.Default.colorTheme = colorTheme;
            Properties.Settings.Default.Save();
        }

        public void SetFontSize(int fontSize)
        {
            ThemeResolutionService.ApplicationThemeName = ThemeCollections.SelectorToTheme.TryGetValue(new ThemeSelector(Properties.Settings.Default.colorTheme, fontSize), out string? theme)
                ? theme 
                : ThemeCollections.ControlDefault;

            Properties.Settings.Default.fontSize = fontSize;
            Properties.Settings.Default.Save();
        }

        public void SetTheme(string themeName)
        {
            if (!ThemeCollections.ThemeNames.Contains(themeName))
                throw _exceptionHelper.CriticalException("{22FDC293-A727-4845-AF68-A3275B239D4E}");

            ThemeResolutionService.ApplicationThemeName = themeName;
            Properties.Settings.Default.themeName = ThemeResolutionService.ApplicationThemeName;
            Properties.Settings.Default.Save();
        }
    }
}
