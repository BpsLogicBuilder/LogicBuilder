using System;
using System.Windows.Forms;
using Telerik.WinControls;

namespace TelerikLogicBuilder.FormsPreviewer
{
    internal static class Program
    {
        static Program()
        {
            LoadThemes();
        }

        public const string Office2019Dark = "Office2019Dark";
        public const string Office2019Dark10 = "Office2019Dark10";
        public const string Office2019Dark11 = "Office2019Dark11";
        public const string Office2019Dark12 = "Office2019Dark12";
        public const string Office2019Dark14 = "Office2019Dark14";
        public const string Office2019Gray = "Office2019Gray";
        public const string Office2019Gray10 = "Office2019Gray10";
        public const string Office2019Gray11 = "Office2019Gray11";
        public const string Office2019Gray12 = "Office2019Gray12";
        public const string Office2019Gray14 = "Office2019Gray14";
        public const string Office2019Light = "Office2019Light";
        public const string Office2019Light10 = "Office2019Light10";
        public const string Office2019Light11 = "Office2019Light11";
        public const string Office2019Light12 = "Office2019Light12";
        public const string Office2019Light14 = "Office2019Light14";

        private const string defaultNamespace = "TelerikLogicBuilder.FormsPreviewer";
        public static readonly string Office2019Dark10_PackageResource = $"{defaultNamespace}.{Office2019Dark10}.tssp";
        public static readonly string Office2019Dark11_PackageResource = $"{defaultNamespace}.{Office2019Dark11}.tssp";
        public static readonly string Office2019Dark12_PackageResource = $"{defaultNamespace}.{Office2019Dark12}.tssp";
        public static readonly string Office2019Dark14_PackageResource = $"{defaultNamespace}.{Office2019Dark14}.tssp";
        public static readonly string Office2019Gray10_PackageResource = $"{defaultNamespace}.{Office2019Gray10}.tssp";
        public static readonly string Office2019Gray11_PackageResource = $"{defaultNamespace}.{Office2019Gray11}.tssp";
        public static readonly string Office2019Gray12_PackageResource = $"{defaultNamespace}.{Office2019Gray12}.tssp";
        public static readonly string Office2019Gray14_PackageResource = $"{defaultNamespace}.{Office2019Gray14}.tssp";
        public static readonly string Office2019Light10_PackageResource = $"{defaultNamespace}.{Office2019Light10}.tssp";
        public static readonly string Office2019Light11_PackageResource = $"{defaultNamespace}.{Office2019Light11}.tssp";
        public static readonly string Office2019Light12_PackageResource = $"{defaultNamespace}.{Office2019Light12}.tssp";
        public static readonly string Office2019Light14_PackageResource = $"{defaultNamespace}.{Office2019Light14}.tssp";


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Telerik.WinControls.ThemeResolutionService.ApplicationThemeName = "Office2019Dark";
            Application.Run(new RadForm1());
        }

        internal static void LoadThemes()
        {
            //_ = new Telerik.WinControls.Themes.ControlDefault();
            _ = new Telerik.WinControls.Themes.Office2007BlackTheme();
            _ = new Telerik.WinControls.Themes.Office2007SilverTheme();
            _ = new Telerik.WinControls.Themes.Office2010BlackTheme();
            _ = new Telerik.WinControls.Themes.Office2010BlueTheme();
            _ = new Telerik.WinControls.Themes.Office2010SilverTheme();
            _ = new Telerik.WinControls.Themes.Office2013DarkTheme();
            _ = new Telerik.WinControls.Themes.Office2013LightTheme();
            _ = new Telerik.WinControls.Themes.Office2019GrayTheme();
            _ = new Telerik.WinControls.Themes.Office2019LightTheme();
            _ = new Telerik.WinControls.Themes.Office2019DarkTheme();
            ThemeResolutionService.LoadPackageResource(Program.Office2019Dark10_PackageResource);
            ThemeResolutionService.LoadPackageResource(Program.Office2019Dark11_PackageResource);
            ThemeResolutionService.LoadPackageResource(Program.Office2019Dark12_PackageResource);
            ThemeResolutionService.LoadPackageResource(Program.Office2019Dark14_PackageResource);
            ThemeResolutionService.LoadPackageResource(Program.Office2019Gray10_PackageResource);
            ThemeResolutionService.LoadPackageResource(Program.Office2019Gray11_PackageResource);
            ThemeResolutionService.LoadPackageResource(Program.Office2019Gray12_PackageResource);
            ThemeResolutionService.LoadPackageResource(Program.Office2019Gray14_PackageResource);
            ThemeResolutionService.LoadPackageResource(Program.Office2019Light10_PackageResource);
            ThemeResolutionService.LoadPackageResource(Program.Office2019Light11_PackageResource);
            ThemeResolutionService.LoadPackageResource(Program.Office2019Light12_PackageResource);
            ThemeResolutionService.LoadPackageResource(Program.Office2019Light14_PackageResource);
        }
    }
}