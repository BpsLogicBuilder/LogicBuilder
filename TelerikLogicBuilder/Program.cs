using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

[assembly: InternalsVisibleTo("TelerikLogicBuilder.Tests")]
[assembly: InternalsVisibleTo("TelerikLogicBuilder.IntegrationTests")]
namespace ABIS.LogicBuilder.FlowBuilder
{
    internal static class Program
    {
        static Program()
        {
            ServiceProvider = ServiceCollection
                .BuildServiceProvider();
        }

        #region Variables
        private static MDIParent? mdiParent;
        #endregion Variables

        public static IServiceProvider ServiceProvider { get; set; }
        public static IServiceCollection ServiceCollection => new ServiceCollection()
            .AddMdiParentCommands()
            .AddFlowBuilder()
            .AddConfiguration()
            .AddData()
            .AddDataParsers()
            .AddIntellisense()
            .AddReflection()
            .AddRulesGenerator()
            .AddTreeViewBuiilders()
            .AddUserControls()
            .AddXmlValidation();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("fr-FR");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr-FR");
            System.Windows.Forms.Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            System.Windows.Forms.Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Telerik.WinControls.ThemeResolutionService.ApplicationThemeName = Properties.Settings.Default.themeName;

            mdiParent = ServiceProvider.GetRequiredService<MDIParent>();

            Application.Run(mdiParent);
        }

        #region EventHandlers
        internal static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                if (e.ExceptionObject is Exception ex)
                {
                    DisplayMessage.Show(mdiParent!, ex.Message, GetRightToLeft());
                    EventLogger.WriteEntry(ApplicationProperties.Name, ex);
                }
                else
                {
                    DisplayMessage.Show(mdiParent!, e.ExceptionObject.ToString()!, GetRightToLeft());
                    EventLogger.WriteEntry(ApplicationProperties.Name, e.ExceptionObject.ToString()!, System.Diagnostics.EventLogEntryType.Error);
                }
            }
            finally
            {
                System.Windows.Forms.Application.Exit();
            }
        }

        internal static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
                DisplayMessage.Show(mdiParent!, e.Exception.Message, GetRightToLeft());
                EventLogger.WriteEntry(ApplicationProperties.Name, e.Exception);
            }
            finally
            {
                System.Windows.Forms.Application.Exit();
            }
        }

        private static RightToLeft GetRightToLeft()
            => mdiParent?.RightToLeft ?? RightToLeft.No;
        #endregion EventHandlers
    }
}