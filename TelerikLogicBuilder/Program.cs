using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            //Telerik.WinControls.ThemeResolutionService.ApplicationThemeName = Properties.Settings.Default.ThemeName;
            var form = new MDIParent();

            Services.IFormInitializer formInitializer = new Services.FormInitializer();
            formInitializer.SetCenterScreen(form);

            Application.Run(form);
            
        }
    }
}