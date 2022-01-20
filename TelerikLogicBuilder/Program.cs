using System;
using System.Linq;
using System.Windows.Forms;

namespace TelerikLogicBuilder
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
            Telerik.WinControls.ThemeResolutionService.ApplicationThemeName = "Office2019Dark";
            Application.Run(new MDIParent());
            
        }
    }
}