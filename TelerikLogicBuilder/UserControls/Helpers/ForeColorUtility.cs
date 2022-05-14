using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers
{
    internal class ForeColorUtility
    {
        private Dictionary<string, string> ErrorRepositoryTable => new()
        {
            [ThemeCollections.Office2007Black] = "BlackForeColor",
            [ThemeCollections.Office2007Silver] = "BlackForeColor",
            [ThemeCollections.Office2010Black] = "ForeColorBlack",
            [ThemeCollections.Office2010Blue] = "ForeColorBlack",
            [ThemeCollections.Office2010Silver] = "BlackText",
            [ThemeCollections.Office2013Dark] = "ForeColor(0;0;0)",
            [ThemeCollections.Office2013Light] = "ForeColor(0;0;0)",
            [ThemeCollections.Office2019Dark] = "MainForeColor",
            [ThemeCollections.Office2019Gray] = "MainForeColor",
            [ThemeCollections.Office2019Light] = "MainForeColor",
        };

        private Dictionary<string, string> OkRepositoryTable => new()
        {
            [ThemeCollections.Office2007Black] = "ForeColor(0;50;208)",
            [ThemeCollections.Office2007Silver] = "ForeColor(0;50;208)",
            [ThemeCollections.Office2010Black] = "ForeColorWhite",
            [ThemeCollections.Office2010Blue] = "ForeColorOrange",
            [ThemeCollections.Office2010Silver] = "ForeColor(214;121;3)",
            [ThemeCollections.Office2013Dark] = "ForeColor(0;114;198)",
            [ThemeCollections.Office2013Light] = "ForeColor(0;114;198)",
            [ThemeCollections.Office2019Dark] = "AccentMouseOverBorderForeColor",
            [ThemeCollections.Office2019Gray] = "AccentForeColor",
            [ThemeCollections.Office2019Light] = "AccentForeColor"
        };

        public Color GetErrorForeColor(string themeName)
        {
            if (!ThemeCollections.OfficeThemes.Contains(themeName))
                throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{E49A3619-879C-4DCF-9575-E77F97F31E0E}"));

            return GetForeColorFromRepository(themeName, ErrorRepositoryTable);
        }

        public Color GetOkForeColor(string themeName)
        {
            if (!ThemeCollections.OfficeThemes.Contains(themeName))
                throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{53746E6D-A654-4818-A81D-303295E6346C}"));

            return GetForeColorFromRepository(themeName, OkRepositoryTable);
        }

        private Color GetForeColorFromRepository(string themeName, IDictionary<string, string> colorRepositoryTable)
        {
            var theme = ThemeResolutionService.GetTheme(themeName);
            StyleRepository repositoryFromTheme = theme.FindRepository(colorRepositoryTable[themeName]);
            PropertySetting styleForeColor = repositoryFromTheme.FindSetting(MessageSettings.ForeColorSetting);

            return (Color)styleForeColor.Value;
        }

        private struct MessageSettings
        {
            internal const string ForeColorSetting = "ForeColor";
        }
    }
}
