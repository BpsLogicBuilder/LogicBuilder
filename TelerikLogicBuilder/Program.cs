using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.Services;
using ABIS.LogicBuilder.FlowBuilder.Services.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Variables;
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
            .AddTransient<MDIParent, MDIParent>()

            //Services
            .AddSingleton<IAssemblyLoadContextManager, AssemblyLoadContextManager>()
            .AddSingleton<IContextProvider, ContextProvider>()
            .AddSingleton<IEncryption, Encryption>()
            .AddSingleton<IEnumHelper, EnumHelper>()
            .AddSingleton<IExceptionHelper, ExceptionHelper>()
            .AddSingleton<IFileIOHelper, FileIOHelper>()
            .AddSingleton<IFormInitializer, FormInitializer>()
            .AddSingleton<IMemberAttributeReader, MemberAttributeReader>()
            .AddSingleton<IMessageBoxOptionsHelper, MessageBoxOptionsHelper>()
            .AddSingleton<IParameterAttributeReader, ParameterAttributeReader>()
            .AddSingleton<IPathHelper, PathHelper>()
            .AddSingleton<IReflectionHelper, ReflectionHelper>()
            .AddSingleton<IStringHelper, StringHelper>()
            .AddSingleton<ITypeHelper, TypeHelper>()
            .AddSingleton<IXmlDocumentHelpers, XmlDocumentHelpers>()

            //Configuration
            .AddSingleton<IApplicationXmlParser, ApplicationXmlParser>()
            .AddSingleton<IConfigurationService, ConfigurationService>()
            .AddSingleton<ICreateProjectProperties, CreateProjectProperties>()
            .AddSingleton<ILoadProjectProperties, LoadProjectProperties>()
            .AddSingleton<IProjectPropertiesXmlParser, ProjectPropertiesXmlParser>()
            .AddSingleton<IUpdateProjectProperties, UpdateProjectProperties>()
            .AddSingleton<IWebApiDeploymentXmlParser, WebApiDeploymentXmlParser>()

            //Intellisense.Constructors
            .AddSingleton<IChildConstructorFinder, ChildConstructorFinder>()
            .AddSingleton<IConstructorManager, ConstructorManager>()
            .AddSingleton<IConstructorXmlParser, ConstructorXmlParser>()
            .AddSingleton<IExistingConstructorFinder, ExistingConstructorFinder>()

            //Intellisense.Parameters
            .AddSingleton<IParametersManager, ParametersManager>()
            .AddSingleton<IParametersMatcher, ParametersMatcher>()
            .AddSingleton<IParametersXmlParser, ParametersXmlParser>()

            //Intellisense.Variables
            .AddSingleton<IVariablesManager, VariablesManager>()
            .AddSingleton<IVariablesXmlParser, VariablesXmlParser>()

            //Reflection
            .AddSingleton<IApplicationTypeInfoManager, ApplicationTypeInfoManager>()
            .AddSingleton<IAssemblyHelper, AssemblyHelper>()
            .AddSingleton<IAssemblyLoader, AssemblyLoader>()
            .AddSingleton<ILoadContextSponsor, LoadContextSponsor>()

            //XmlValidation
            .AddSingleton<IXmlValidator, XmlValidator>();

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