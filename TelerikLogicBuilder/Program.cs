using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

[assembly: InternalsVisibleTo("TelerikLogicBuilder.Tests")]
namespace ABIS.LogicBuilder.FlowBuilder
{
    internal static class Program
    {
        public static IServiceProvider ServiceProvider { get; set; }

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
            
            BuildServices();

            var form = ServiceProvider.GetRequiredService<MDIParent>();

            ServiceProvider.GetRequiredService<IFormInitializer>().SetCenterScreen(form);

            Application.Run(form);
        }

        static void BuildServices()
        {
            var services = new ServiceCollection();
            ServiceProvider = services.AddSingleton<IExceptionHelper, ExceptionHelper>()
                .AddSingleton<IFormInitializer, FormInitializer>()
                .AddSingleton<IPathHelper, PathHelper>()
                .AddSingleton<MDIParent, MDIParent>()
                .BuildServiceProvider();  
        }
    }
}