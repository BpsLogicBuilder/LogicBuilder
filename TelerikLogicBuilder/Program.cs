using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.Services;
using ABIS.LogicBuilder.FlowBuilder.Services.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Services.Intellisense;
using ABIS.LogicBuilder.FlowBuilder.Services.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation;
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
        public static IServiceCollection ServiceCollection => new ServiceCollection()
            .AddSingleton<IEnumHelper, EnumHelper>()
            .AddSingleton<IExceptionHelper, ExceptionHelper>()
            .AddSingleton<IFormInitializer, FormInitializer>()
            .AddSingleton<IExceptionHelper, ExceptionHelper>()
            .AddSingleton<IMemberAttributeReader, MemberAttributeReader>()
            .AddSingleton<IParameterAttributeReader, ParameterAttributeReader>()
            .AddSingleton<IStringHelper, StringHelper>()
            .AddSingleton<IPathHelper, PathHelper>()
            .AddSingleton<IXmlDocumentHelpers, XmlDocumentHelpers>()
            .AddSingleton<IReflectionHelper, ReflectionHelper>()
            .AddSingleton<ITypeHelper, TypeHelper>()
            .AddSingleton<IEncryption, Encryption>()
            .AddSingleton<IFileIOHelper, FileIOHelper>()
            .AddSingleton<IMessageBoxOptionsHelper, MessageBoxOptionsHelper>()
            .AddSingleton<IXmlValidator, XmlValidator>()
            .AddSingleton<IContextProvider, ContextProvider>()
            .AddSingleton<IChildConstructorFinder, ChildConstructorFinder>()
            .AddSingleton<IAssemblyLoadContextManager, AssemblyLoadContextManager>()
            .AddSingleton<IApplicationTypeInfoManager, ApplicationTypeInfoManager>()
            .AddSingleton<IConfigurationService, ConfigurationService>()
            .AddSingleton<IWebApiDeploymentXmlParser, WebApiDeploymentXmlParser>()
            .AddSingleton<IUpdateProjectProperties, UpdateProjectProperties>()
            .AddSingleton<ICreateProjectProperties, CreateProjectProperties>()
            .AddSingleton<ILoadProjectProperties, LoadProjectProperties>()
            .AddSingleton<IProjectPropertiesXmlParser, ProjectPropertiesXmlParser>()
            .AddSingleton<IApplicationXmlParser, ApplicationXmlParser>()
            .AddSingleton<ILoadContextSponsor, LoadContextSponsor>()
            .AddSingleton<IAssemblyLoader, AssemblyLoader>()
            .AddSingleton<IAssemblyHelper, AssemblyHelper>()
            .AddTransient<MDIParent, MDIParent>();

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

        internal static void BuildServices()
        {
            ServiceProvider = ServiceCollection
                .BuildServiceProvider();  
        }
    }
}