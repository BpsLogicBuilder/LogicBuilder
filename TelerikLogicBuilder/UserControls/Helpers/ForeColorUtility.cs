using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers
{
    internal static class ForeColorUtility
    {
        private const string DefaultTheme = ThemeCollections.ControlDefault;

        private static Dictionary<string, string> ErrorRepositoryTable => new()
        {
            [ThemeCollections.ControlDefault] = "CalculatorButtonForeColorHover",
            [ThemeCollections.Office2019Dark] = "AccentMouseOverBorderForeColor",
            [ThemeCollections.Office2019Dark10] = "AccentMouseOverBorderForeColor",
            [ThemeCollections.Office2019Dark11] = "AccentMouseOverBorderForeColor",
            [ThemeCollections.Office2019Dark12] = "AccentMouseOverBorderForeColor",
            [ThemeCollections.Office2019Dark14] = "AccentMouseOverBorderForeColor",
            [ThemeCollections.Office2019Gray] = "AccentForeColor",
            [ThemeCollections.Office2019Gray10] = "AccentForeColor",
            [ThemeCollections.Office2019Gray11] = "AccentForeColor",
            [ThemeCollections.Office2019Gray12] = "AccentForeColor",
            [ThemeCollections.Office2019Gray14] = "AccentForeColor",
            [ThemeCollections.Office2019Light] = "AccentForeColor",
            [ThemeCollections.Office2019Light10] = "AccentForeColor",
            [ThemeCollections.Office2019Light11] = "AccentForeColor",
            [ThemeCollections.Office2019Light12] = "AccentForeColor",
            [ThemeCollections.Office2019Light14] = "AccentForeColor"
        };

        private static Dictionary<string, string> GroupBoxBorderColorRepositoryTable => new()
        {
            [ThemeCollections.ControlDefault] = "EditorBorder",
            [ThemeCollections.Office2019Dark] = "MainBorder",
            [ThemeCollections.Office2019Dark10] = "MainBorder",
            [ThemeCollections.Office2019Dark11] = "MainBorder",
            [ThemeCollections.Office2019Dark12] = "MainBorder",
            [ThemeCollections.Office2019Dark14] = "MainBorder",
            [ThemeCollections.Office2019Gray] = "MainBorder",
            [ThemeCollections.Office2019Gray10] = "MainBorder",
            [ThemeCollections.Office2019Gray11] = "MainBorder",
            [ThemeCollections.Office2019Gray12] = "MainBorder",
            [ThemeCollections.Office2019Gray14] = "MainBorder",
            [ThemeCollections.Office2019Light] = "MainBorder",
            [ThemeCollections.Office2019Light10] = "MainBorder",
            [ThemeCollections.Office2019Light11] = "MainBorder",
            [ThemeCollections.Office2019Light12] = "MainBorder",
            [ThemeCollections.Office2019Light14] = "MainBorder"
        };

        private static Dictionary<string, string> GroupBoxForeColorRepositoryTable => new()
        {
            [ThemeCollections.ControlDefault] = "BlackText",
            [ThemeCollections.Office2019Dark] = "MainForeColor",
            [ThemeCollections.Office2019Dark10] = "MainForeColor",
            [ThemeCollections.Office2019Dark11] = "MainForeColor",
            [ThemeCollections.Office2019Dark12] = "MainForeColor",
            [ThemeCollections.Office2019Dark14] = "MainForeColor",
            [ThemeCollections.Office2019Gray] = "MainForeColor",
            [ThemeCollections.Office2019Gray10] = "MainForeColor",
            [ThemeCollections.Office2019Gray11] = "MainForeColor",
            [ThemeCollections.Office2019Gray12] = "MainForeColor",
            [ThemeCollections.Office2019Gray14] = "MainForeColor",
            [ThemeCollections.Office2019Light] = "MainForeColor",
            [ThemeCollections.Office2019Light10] = "MainForeColor",
            [ThemeCollections.Office2019Light11] = "MainForeColor",
            [ThemeCollections.Office2019Light12] = "MainForeColor",
            [ThemeCollections.Office2019Light14] = "MainForeColor"
        };

        private static Dictionary<string, string> OkRepositoryTable => new()
        {
            [ThemeCollections.ControlDefault] = "NormalButtonForeColor",
            [ThemeCollections.Office2019Dark] = "MainForeColor",
            [ThemeCollections.Office2019Dark10] = "MainForeColor",
            [ThemeCollections.Office2019Dark11] = "MainForeColor",
            [ThemeCollections.Office2019Dark12] = "MainForeColor",
            [ThemeCollections.Office2019Dark14] = "MainForeColor",
            [ThemeCollections.Office2019Gray] = "MainForeColor",
            [ThemeCollections.Office2019Gray10] = "MainForeColor",
            [ThemeCollections.Office2019Gray11] = "MainForeColor",
            [ThemeCollections.Office2019Gray12] = "MainForeColor",
            [ThemeCollections.Office2019Gray14] = "MainForeColor",
            [ThemeCollections.Office2019Light] = "MainForeColor",
            [ThemeCollections.Office2019Light10] = "MainForeColor",
            [ThemeCollections.Office2019Light11] = "MainForeColor",
            [ThemeCollections.Office2019Light12] = "MainForeColor",
            [ThemeCollections.Office2019Light14] = "MainForeColor"
        };

        private static Dictionary<string, string> TextBoxForeColorRepositoryTable => new()
        {
            [ThemeCollections.ControlDefault] = "BlackText",
            [ThemeCollections.Office2019Dark] = "MainForeColor",
            [ThemeCollections.Office2019Dark10] = "MainForeColor",
            [ThemeCollections.Office2019Dark11] = "MainForeColor",
            [ThemeCollections.Office2019Dark12] = "MainForeColor",
            [ThemeCollections.Office2019Dark14] = "MainForeColor",
            [ThemeCollections.Office2019Gray] = "MainForeColor",
            [ThemeCollections.Office2019Gray10] = "MainForeColor",
            [ThemeCollections.Office2019Gray11] = "MainForeColor",
            [ThemeCollections.Office2019Gray12] = "MainForeColor",
            [ThemeCollections.Office2019Gray14] = "MainForeColor",
            [ThemeCollections.Office2019Light] = "MainForeColor",
            [ThemeCollections.Office2019Light10] = "MainForeColor",
            [ThemeCollections.Office2019Light11] = "MainForeColor",
            [ThemeCollections.Office2019Light12] = "MainForeColor",
            [ThemeCollections.Office2019Light14] = "MainForeColor"
        };

        private static Dictionary<string, string> TextBoxBackColorRepositoryTable => new()
        {
            [ThemeCollections.ControlDefault] = "EditorFill",
            [ThemeCollections.Office2019Dark] = "MainFill",
            [ThemeCollections.Office2019Dark10] = "MainFill",
            [ThemeCollections.Office2019Dark11] = "MainFill",
            [ThemeCollections.Office2019Dark12] = "MainFill",
            [ThemeCollections.Office2019Dark14] = "MainFill",
            [ThemeCollections.Office2019Gray] = "MainFill",
            [ThemeCollections.Office2019Gray10] = "MainFill",
            [ThemeCollections.Office2019Gray11] = "MainFill",
            [ThemeCollections.Office2019Gray12] = "MainFill",
            [ThemeCollections.Office2019Gray14] = "MainFill",
            [ThemeCollections.Office2019Light] = "MainFill",
            [ThemeCollections.Office2019Light10] = "MainFill",
            [ThemeCollections.Office2019Light11] = "MainFill",
            [ThemeCollections.Office2019Light12] = "MainFill",
            [ThemeCollections.Office2019Light14] = "MainFill"
        };

        private static Dictionary<string, string> TextBoxBorderColorRepositoryTable => new()
        {
            [ThemeCollections.ControlDefault] = "EditorBorder",
            [ThemeCollections.Office2019Dark] = "MainBorder",
            [ThemeCollections.Office2019Dark10] = "MainBorder",
            [ThemeCollections.Office2019Dark11] = "MainBorder",
            [ThemeCollections.Office2019Dark12] = "MainBorder",
            [ThemeCollections.Office2019Dark14] = "MainBorder",
            [ThemeCollections.Office2019Gray] = "MainBorder",
            [ThemeCollections.Office2019Gray10] = "MainBorder",
            [ThemeCollections.Office2019Gray11] = "MainBorder",
            [ThemeCollections.Office2019Gray12] = "MainBorder",
            [ThemeCollections.Office2019Gray14] = "MainBorder",
            [ThemeCollections.Office2019Light] = "MainBorder",
            [ThemeCollections.Office2019Light10] = "MainBorder",
            [ThemeCollections.Office2019Light11] = "MainBorder",
            [ThemeCollections.Office2019Light12] = "MainBorder",
            [ThemeCollections.Office2019Light14] = "MainBorder"
        };

        private static Dictionary<string, string> TreeViewNormalBorderColorRepositoryTable => new()
        {
            [ThemeCollections.ControlDefault] = "TreeViewControlBorder",
            [ThemeCollections.Office2019Dark] = "MainBorder",
            [ThemeCollections.Office2019Dark10] = "MainBorder",
            [ThemeCollections.Office2019Dark11] = "MainBorder",
            [ThemeCollections.Office2019Dark12] = "MainBorder",
            [ThemeCollections.Office2019Dark14] = "MainBorder",
            [ThemeCollections.Office2019Gray] = "MainBorder",
            [ThemeCollections.Office2019Gray10] = "MainBorder",
            [ThemeCollections.Office2019Gray11] = "MainBorder",
            [ThemeCollections.Office2019Gray12] = "MainBorder",
            [ThemeCollections.Office2019Gray14] = "MainBorder",
            [ThemeCollections.Office2019Light] = "MainBorder",
            [ThemeCollections.Office2019Light10] = "MainBorder",
            [ThemeCollections.Office2019Light11] = "MainBorder",
            [ThemeCollections.Office2019Light12] = "MainBorder",
            [ThemeCollections.Office2019Light14] = "MainBorder"
        };

        private static Dictionary<string, string> DefaultFontTable => new()
        {
            [ThemeCollections.ControlDefault] = "SegoeUIDefaultText",
            [ThemeCollections.Office2019Dark] = "DefaultFont",
            [ThemeCollections.Office2019Dark10] = "DefaultFont",
            [ThemeCollections.Office2019Dark11] = "DefaultFont",
            [ThemeCollections.Office2019Dark12] = "DefaultFont",
            [ThemeCollections.Office2019Dark14] = "DefaultFont",
            [ThemeCollections.Office2019Gray] = "DefaultFont",
            [ThemeCollections.Office2019Gray10] = "DefaultFont",
            [ThemeCollections.Office2019Gray11] = "DefaultFont",
            [ThemeCollections.Office2019Gray12] = "DefaultFont",
            [ThemeCollections.Office2019Gray14] = "DefaultFont",
            [ThemeCollections.Office2019Light] = "DefaultFont",
            [ThemeCollections.Office2019Light10] = "DefaultFont",
            [ThemeCollections.Office2019Light11] = "DefaultFont",
            [ThemeCollections.Office2019Light12] = "DefaultFont",
            [ThemeCollections.Office2019Light14] = "DefaultFont"
        };

        public static Font GetDefaultFont(string themeName)
        {
            themeName ??= DefaultTheme;
            if (!ThemeCollections.ThemeNames.Contains(themeName))
                throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{E49A3619-879C-4DCF-9575-E77F97F31E0E}"));

            return GetFontFromRepository(themeName);
        }

        public static Color GetErrorForeColor(string themeName)
        {
            themeName ??= DefaultTheme;
            if (!ThemeCollections.ThemeNames.Contains(themeName))
                throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{E49A3619-879C-4DCF-9575-E77F97F31E0E}"));

            return GetForeColorFromRepository(themeName, ErrorRepositoryTable);
        }

        public static Color GetGroupBoxBorderColor(string themeName)
        {
            themeName ??= DefaultTheme;
            if (!ThemeCollections.ThemeNames.Contains(themeName))
                throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{A0BBBD23-9C5D-45B3-8700-37CE0E064961}"));

            return GetForeColorFromRepository(themeName, GroupBoxBorderColorRepositoryTable);
        }

        public static Color GetGroupBoxBorderErrorColor()
        {
            return Color.Red;
        }

        public static Color GetGroupBoxForeColor(string themeName)
        {
            themeName ??= DefaultTheme;
            if (!ThemeCollections.ThemeNames.Contains(themeName))
                throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{E7C95FE0-605B-4104-BA76-5E2FDFC99D69}"));

            return GetForeColorFromRepository(themeName, GroupBoxForeColorRepositoryTable);
        }

        public static Color GetLinkBoundaryColor(string themeName)
        {
            if (themeName == ThemeCollections.Office2019Dark)
                return GetTextBoxForeColor(themeName);

            return Color.Blue;
        }

        public static Color GetOkForeColor(string themeName)
        {
            themeName ??= DefaultTheme;
            if (!ThemeCollections.ThemeNames.Contains(themeName))
                throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{53746E6D-A654-4818-A81D-303295E6346C}"));

            return GetForeColorFromRepository(themeName, OkRepositoryTable);
        }

        public static Color GetTextBoxBackColor(string themeName)
        {
            themeName ??= DefaultTheme;
            if (!ThemeCollections.ThemeNames.Contains(themeName))
                throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{B990227F-B257-4812-B686-FB02854828B7}"));

            return GetBackColorFromRepository(themeName, TextBoxBackColorRepositoryTable);
        }

        public static Color GetTextBoxBorderColor(string themeName)
        {
            themeName ??= DefaultTheme;
            if (!ThemeCollections.ThemeNames.Contains(themeName))
                throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{AF8A7831-BB55-47E4-AB00-0E4E8DA220EB}"));

            return GetForeColorFromRepository(themeName, TextBoxBorderColorRepositoryTable);
        }

        public static Color GetTextBoxForeColor(string themeName)
        {
            themeName ??= DefaultTheme;
            if (!ThemeCollections.ThemeNames.Contains(themeName))
                throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{ECCF7199-0B6A-4608-9350-83870591B104}"));

            return GetForeColorFromRepository(themeName, TextBoxForeColorRepositoryTable);
        }

        public static Color GetTreeViewBorderErrorColor()
        {
            return Color.Red;
        }

        public static Color GetTreeViewBorderColor(string themeName)
        {
            themeName ??= DefaultTheme;
            if (!ThemeCollections.ThemeNames.Contains(themeName))
                throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{85F5E92B-C561-4B7B-A2D1-46631248D62D}"));

            return GetForeColorFromRepository(themeName, TreeViewNormalBorderColorRepositoryTable);
        }

        private static Color GetBackColorFromRepository(string themeName, IDictionary<string, string> colorRepositoryTable)
            => GetColorFromRepository(themeName, colorRepositoryTable, MessageSettings.BackColorSetting);

        private static Color GetForeColorFromRepository(string themeName, IDictionary<string, string> colorRepositoryTable) 
            => GetColorFromRepository(themeName, colorRepositoryTable, MessageSettings.ForeColorSetting);

        private static Color GetColorFromRepository(string themeName, IDictionary<string, string> colorRepositoryTable, string setting)
        {
            var theme = ThemeResolutionService.GetTheme(themeName);
            StyleRepository repositoryFromTheme = theme.FindRepository(colorRepositoryTable[themeName]);
            PropertySetting styleForeColor = repositoryFromTheme.FindSetting(setting);

            return (Color)(styleForeColor.Value ?? styleForeColor.EndValue);
        }

        private static Font GetFontFromRepository(string themeName)
        {
            var theme = ThemeResolutionService.GetTheme(themeName);
            StyleRepository repositoryFromTheme = theme.FindRepository(DefaultFontTable[themeName]);
            PropertySetting font = repositoryFromTheme.FindSetting(MessageSettings.FontSetting);

            return (Font)(font.Value ?? font.EndValue ?? throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{6B0663F4-BF7C-4E30-8389-83567523B739}")));
        }

        private struct MessageSettings
        {
            internal const string BackColorSetting = "BackColor";
            internal const string FontSetting = "Font";
            internal const string ForeColorSetting = "ForeColor";
        }
    }
}
