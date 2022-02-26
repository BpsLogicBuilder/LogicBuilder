using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Services;
using ABIS.LogicBuilder.FlowBuilder.Services.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Services.Configuration.Initialization;
using ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Services.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.Configuration;
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
            .AddSingleton<IBuiltInFunctionsLoader, BuiltInFunctionsLoader>()
            .AddSingleton<IConfigurationService, ConfigurationService>()
            .AddSingleton<ICreateConstructors, CreateConstructors>()
            .AddSingleton<ICreateFragments, CreateFragments>()
            .AddSingleton<ICreateFunctions, CreateFunctions>()
            .AddSingleton<ICreateProjectProperties, CreateProjectProperties>()
            .AddSingleton<ICreateVariables, CreateVariables>()
            .AddSingleton<IFragmentXmlParser, FragmentXmlParser>()
            .AddSingleton<ILoadConstructors, LoadConstructors>()
            .AddSingleton<ILoadFragments, LoadFragments>()
            .AddSingleton<ILoadFunctions, LoadFunctions>()
            .AddSingleton<ILoadProjectProperties, LoadProjectProperties>()
            .AddSingleton<ILoadVariables, LoadVariables>()
            .AddSingleton<IProjectPropertiesXmlParser, ProjectPropertiesXmlParser>()
            .AddSingleton<IUpdateConstructors, UpdateConstructors>()
            .AddSingleton<IUpdateFragments, UpdateFragments>()
            .AddSingleton<IUpdateFunctions, UpdateFunctions>()
            .AddSingleton<IUpdateProjectProperties, UpdateProjectProperties>()
            .AddSingleton<IUpdateVariables, UpdateVariables>()
            .AddSingleton<IWebApiDeploymentXmlParser, WebApiDeploymentXmlParser>()

            //Configuration.Initialization
            .AddSingleton<IConstructorDictionaryBuilder, ConstructorDictionaryBuilder>()
            .AddSingleton<IConstructorListInitializer, ConstructorListInitializer>()
            .AddSingleton<IConstructorTreeFolderBuilder, ConstructorTreeFolderBuilder>()
            .AddSingleton<IFragmentDictionaryBuilder, FragmentDictionaryBuilder>()
            .AddSingleton<IFragmentListInitializer, FragmentListInitializer>()
            .AddSingleton<IFragmentTreeFolderBuilder, FragmentTreeFolderBuilder>()
            .AddSingleton<IFunctionDictionaryBuilder, FunctionDictionaryBuilder>()
            .AddSingleton<IFunctionListInitializer, FunctionListInitializer>()
            .AddSingleton<IFunctionTreeFolderBuilder, FunctionTreeFolderBuilder>()
            .AddSingleton<IVariableDictionaryBuilder, VariableDictionaryBuilder>()
            .AddSingleton<IVariableListInitializer, VariableListInitializer>()
            .AddSingleton<IVariableTreeFolderBuilder, VariableTreeFolderBuilder>()

            //Intellisense.Constructors
            .AddSingleton<IChildConstructorFinder, ChildConstructorFinder>()
            .AddSingleton<IConstructorManager, ConstructorManager>()
            .AddSingleton<IConstructorXmlParser, ConstructorXmlParser>()
            .AddSingleton<IExistingConstructorFinder, ExistingConstructorFinder>()

            //Intellisense.Functions
            .AddSingleton<IFunctionManager, FunctionManager>()
            .AddSingleton<IFunctionNodeInfoManager, FunctionNodeInfoManager>()
            .AddSingleton<IFunctionXmlParser, FunctionXmlParser>()
            .AddSingleton<IReturnTypeManager, ReturnTypeManager>()
            .AddSingleton<IReturnTypeManager, ReturnTypeManager>()
            .AddSingleton<IReturnTypeXmlParser, ReturnTypeXmlParser>()

            //Intellisense.Parameters
            .AddSingleton<IMultipleChoiceParameterValidator, MultipleChoiceParameterValidator>()
            .AddSingleton<IParametersManager, ParametersManager>()
            .AddSingleton<IParametersMatcher, ParametersMatcher>()
            .AddSingleton<IParametersXmlParser, ParametersXmlParser>()

            //Intellisense.Variables
            .AddSingleton<IVariableHelper, VariableHelper>()
            .AddSingleton<IVariablesManager, VariablesManager>()
            .AddSingleton<IVariablesNodeInfoManager, VariablesNodeInfoManager>()
            .AddSingleton<IVariablesXmlParser, VariablesXmlParser>()
            .AddSingleton<IVariableValidationHelper, VariableValidationHelper>()

            //Reflection
            .AddSingleton<IApplicationTypeInfoManager, ApplicationTypeInfoManager>()
            .AddSingleton<IAssemblyHelper, AssemblyHelper>()
            .AddSingleton<IAssemblyLoader, AssemblyLoader>()
            .AddSingleton<ILoadContextSponsor, LoadContextSponsor>()

            //XmlValidation
            .AddSingleton<IXmlValidator, XmlValidator>()

            //XmlValidation.Configuration
            .AddSingleton<IConstructorsXmlValidator, ConstructorsXmlValidator>()
            .AddSingleton<IFunctionsXmlValidator, FunctionsXmlValidator>()
            .AddSingleton<IVariablesXmlValidator, VariablesXmlValidator>();

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