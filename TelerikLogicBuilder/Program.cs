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
            LoadThemes();
            ServiceProvider = ServiceCollection
                .BuildServiceProvider();
        }

        #region Variables
        private static MDIParent? mdiParent;
        #endregion Variables

        public static IServiceProvider ServiceProvider { get; set; }

        private static IServiceCollection? _serviceCollection;
        public static IServiceCollection ServiceCollection
        {
            get
            {
                if (_serviceCollection == null)
                {
                    _serviceCollection = new ServiceCollection()
                        .AddMdiParentCommands()
                        .AddEditingControls()
                        .AddFlowBuilder()
                        .AddComponents()
                        .AddConfiguration()
                        .AddData()
                        .AddDataParsers()
                        .AddIntellisense()
                        .AddReflection()
                        .AddRulesGenerator()
                        .AddTreeViewBuiilders()
                        .AddUserControls()
                        .AddXmlValidation();
                }

                return _serviceCollection;
            }
        }

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