using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Services;
using ABIS.LogicBuilder.FlowBuilder.Services.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Services.Configuration.Initialization;
using ABIS.LogicBuilder.FlowBuilder.Services.Data;
using ABIS.LogicBuilder.FlowBuilder.Services.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Services.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.Services.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.CompilerServices;
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

        public static IServiceProvider ServiceProvider { get; set; }
        public static IServiceCollection ServiceCollection => new ServiceCollection()
            .AddTransient<MDIParent, MDIParent>()

            //Services
            .AddSingleton<IAssemblyLoadContextManager, AssemblyLoadContextManager>()
            .AddSingleton<ICellXmlHelper, CellXmlHelper>()
            .AddSingleton<ICheckSelectedApplication, CheckSelectedApplication>()
            .AddSingleton<IConstructorTypeHelper, ConstructorTypeHelper>()
            .AddSingleton<IContextProvider, ContextProvider>()
            .AddSingleton<IEncryption, Encryption>()
            .AddSingleton<IEnumHelper, EnumHelper>()
            .AddSingleton<IExceptionHelper, ExceptionHelper>()
            .AddSingleton<IFileIOHelper, FileIOHelper>()
            .AddSingleton<IFormInitializer, FormInitializer>()
            .AddSingleton<IMemberAttributeReader, MemberAttributeReader>()
            .AddSingleton<IMessageBoxOptionsHelper, MessageBoxOptionsHelper>()
            .AddSingleton<IModuleNamesReader, ModuleNamesReader>()
            .AddSingleton<IParameterAttributeReader, ParameterAttributeReader>()
            .AddSingleton<IPathHelper, PathHelper>()
            .AddSingleton<IReflectionHelper, ReflectionHelper>()
            .AddSingleton<IResultMessageBuilder, ResultMessageBuilder>()
            .AddSingleton<IShapeDataCellManager, ShapeDataCellManager>()
            .AddSingleton<IShapeXmlHelper, ShapeXmlHelper>()
            .AddSingleton<IStringHelper, StringHelper>()
            .AddSingleton<IThemeManager, ThemeManager>()
            .AddSingleton<ITreeViewService, TreeViewService>()
            .AddSingleton<ITypeHelper, TypeHelper>()
            .AddSingleton<ITypeLoadHelper, TypeLoadHelper>()
            .AddSingleton<IXmlDocumentHelpers, XmlDocumentHelpers>()
            .AddTransient(typeof(IScopedDisposableManager<>), typeof(ScopedDisposableManager<>))

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

            //Data
            .AddSingleton<IAnyParametersHelper, AnyParametersHelper>()
            .AddSingleton<IGenericConstructorHelper, GenericConstructorHelper>()
            .AddSingleton<IGenericFunctionHelper, GenericFunctionHelper>()
            .AddSingleton<IGenericParametersHelper, GenericParametersHelper>()
            .AddSingleton<IGenericReturnTypeHelper, GenericReturnTypeHelper>()
            .AddSingleton<IGetValidConfigurationFromData, GetValidConfigurationFromData>()

            //DataParsers
            .AddSingleton<IAssertFunctionDataParser, AssertFunctionDataParser>()
            .AddSingleton<IConditionsDataParser, ConditionsDataParser>()
            .AddSingleton<IConnectorDataParser, ConnectorDataParser>()
            .AddSingleton<IConstructorDataParser, ConstructorDataParser>()
            .AddSingleton<IDecisionDataParser, DecisionDataParser>()
            .AddSingleton<IDecisionsDataParser, DecisionsDataParser>()
            .AddSingleton<IDiagramErrorSourceDataParser, DiagramErrorSourceDataParser>()
            .AddSingleton<IFunctionDataParser, FunctionDataParser>()
            .AddSingleton<IFunctionsDataParser, FunctionsDataParser>()
            .AddSingleton<IJumpDataParser, JumpDataParser>()
            .AddSingleton<ILiteralListDataParser, LiteralListDataParser>()
            .AddSingleton<ILiteralListParameterDataParser, LiteralListParameterDataParser>()
            .AddSingleton<ILiteralListVariableDataParser, LiteralListVariableDataParser>()
            .AddSingleton<IMetaObjectDataParser, MetaObjectDataParser>()
            .AddSingleton<IModuleDataParser, ModuleDataParser>()
            .AddSingleton<IObjectDataParser, ObjectDataParser>()
            .AddSingleton<IObjectListDataParser, ObjectListDataParser>()
            .AddSingleton<IObjectListParameterDataParser, ObjectListParameterDataParser>()
            .AddSingleton<IObjectListVariableDataParser, ObjectListVariableDataParser>()
            .AddSingleton<IObjectParameterDataParser, ObjectParameterDataParser>()
            .AddSingleton<IObjectVariableDataParser, ObjectVariableDataParser>()
            .AddSingleton<IPriorityDataParser, PriorityDataParser>()
            .AddSingleton<IRetractFunctionDataParser, RetractFunctionDataParser>()
            .AddSingleton<ITableErrorSourceDataParser, TableErrorSourceDataParser>()
            .AddSingleton<IVariableDataParser, VariableDataParser>()
            .AddSingleton<IVariableValueDataParser, VariableValueDataParser>()

            //Intellisense.Constructors
            .AddSingleton<IChildConstructorFinder, ChildConstructorFinder>()
            .AddSingleton<IConstructorManager, ConstructorManager>()
            .AddSingleton<IConstructorXmlParser, ConstructorXmlParser>()
            .AddSingleton<IExistingConstructorFinder, ExistingConstructorFinder>()

            //Intellisense.Functions
            .AddSingleton<IFunctionHelper, FunctionHelper>()
            .AddSingleton<IFunctionManager, FunctionManager>()
            .AddSingleton<IFunctionNodeInfoManager, FunctionNodeInfoManager>()
            .AddSingleton<IFunctionValidationHelper, FunctionValidationHelper>()
            .AddSingleton<IFunctionXmlParser, FunctionXmlParser>()
            .AddSingleton<IReturnTypeManager, ReturnTypeManager>()
            .AddSingleton<IReturnTypeManager, ReturnTypeManager>()
            .AddSingleton<IReturnTypeXmlParser, ReturnTypeXmlParser>()

            //Intellisense.GenericArguments
            .AddSingleton<IGenericConfigXmlParser, GenericConfigXmlParser>()

            //Intellisense.Parameters
            .AddSingleton<IMultipleChoiceParameterValidator, MultipleChoiceParameterValidator>()
            .AddSingleton<IParameterHelper, ParameterHelper>()
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

            //RulesGenerator
            .AddSingleton<IApiFileListDeleter, ApiFileListDeleter>()
            .AddSingleton<IApiFileListDeployer, ApiFileListDeployer>()
            .AddSingleton<IBuildSaveAssembleRulesForSelectedDocuments, BuildSaveAssembleRulesForSelectedDocuments>()
            .AddSingleton<ICellHelper, CellHelper>()
            .AddSingleton<IDeleteSelectedFilesFromApi, DeleteSelectedFilesFromApi>()
            .AddSingleton<IDeleteSelectedFilesFromFileSystem, DeleteSelectedFilesFromFileSystem>()
            .AddSingleton<IDeploySelectedFilesToApi, DeploySelectedFilesToApi>()
            .AddSingleton<IDeploySelectedFilesToFileSystem, DeploySelectedFilesToFileSystem>()
            .AddSingleton<IDiagramRulesBuilder, DiagramRulesBuilder>()
            .AddSingleton<IDiagramValidator, DiagramValidator>()
            .AddSingleton<IFileSystemFileDeleter, FileSystemFileDeleter>()
            .AddSingleton<IFileSystemFileDeployer, FileSystemFileDeployer>()
            .AddSingleton<IGetRuleShapes, GetRuleShapes>()
            .AddSingleton<IRulesAssembler, RulesAssembler>()
            .AddSingleton<IRuleSetLoader, RuleSetLoader>()
            .AddSingleton<IRulesValidator, RulesValidator>()
            .AddSingleton<ISaveDiagramResources, SaveDiagramResources>()
            .AddSingleton<ISaveDiagramRules, SaveDiagramRules>()
            .AddSingleton<ISaveResources, SaveResources>()
            .AddSingleton<ISaveRules, SaveRules>()
            .AddSingleton<ISaveTableResources, SaveTableResources>()
            .AddSingleton<ISaveTableRules, SaveTableRules>()
            .AddSingleton<IShapeHelper, ShapeHelper>()
            .AddSingleton<ITableRulesBuilder, TableRulesBuilder>()
            .AddSingleton<ITableValidator, TableValidator>()
            .AddSingleton<IValidateSelectedDocuments, ValidateSelectedDocuments>()
            .AddSingleton<IValidateSelectedRules, ValidateSelectedRules>()

            //RulesGenerator.Forms
            .AddTransient<ISelectRulesResourcesPairFormFactory, SelectRulesResourcesPairFormFactory>()
            .AddTransient<ISelectRulesFormFactory, SelectRulesFormFactory>()
            .AddTransient<SelectDocumentsForm, SelectDocumentsForm>()
            .AddTransient<Func<string, SelectRulesForm>>
            (
                provider =>
                applicationName => ActivatorUtilities.CreateInstance<SelectRulesForm>
                (
                    provider,
                    provider.GetRequiredService<IFormInitializer>(),
                    provider.GetRequiredService<IGetAllCheckedNodes>(),
                    provider.GetRequiredService<ISelectRulesTreeViewBuilder>(),
                    applicationName
                )
            )
            .AddTransient<Func<string, SelectRulesResourcesPairForm>>
            (
                provider =>
                applicationName => ActivatorUtilities.CreateInstance<SelectRulesResourcesPairForm>
                (
                    provider,
                    provider.GetRequiredService<IFormInitializer>(),
                    provider.GetRequiredService<IGetAllCheckedNodes>(),
                    provider.GetRequiredService<ISelectModulesForDeploymentTreeViewBuilder>(),
                    applicationName
                )
             )
            .AddTransient<SelectRulesForm, SelectRulesForm>()
            .AddTransient<SelectRulesResourcesPairForm, SelectRulesResourcesPairForm>()

            //RulesGenerator.ShapeValidators
            .AddSingleton<IActionShapeValidator, ActionShapeValidator>()
            .AddSingleton<IApplicationConnectorValidator, ApplicationConnectorValidator>()
            .AddSingleton<IApplicationSpecificFlowShapeValidator, ApplicationSpecificFlowShapeValidator>()
            .AddSingleton<IBeginShapeValidator, BeginShapeValidator>()
            .AddSingleton<ICommentShapeValidator, CommentShapeValidator>()
            .AddSingleton<IConditionShapeValidator, ConditionShapeValidator>()
            .AddSingleton<IDecisionShapeValidator, DecisionShapeValidator>()
            .AddSingleton<IDialogShapeValidator, DialogShapeValidator>()
            .AddSingleton<IEndShapeValidator, EndShapeValidator>()
            .AddSingleton<IJumpShapeValidator, JumpShapeValidator>()
            .AddSingleton<IMergeShapeValidator, MergeShapeValidator>()
            .AddSingleton<IModuleShapeValidator, ModuleShapeValidator>()
            .AddSingleton<IRegularConnectorValidator, RegularConnectorValidator>()
            .AddSingleton<IWaitConditionShapeValidator, WaitConditionShapeValidator>()
            .AddSingleton<IWaitDecisionShapeValidator, WaitDecisionShapeValidator>()
            .AddSingleton<IShapeValidator, ShapeValidator>()

            //RulesGenerator.RuleBuilders
            .AddSingleton<IBeginFlowRuleBuilder, BeginFlowRuleBuilder>()
            .AddSingleton<IConditionsRuleBuilder, ConditionsRuleBuilder>()
            .AddSingleton<IDecisionsRuleBuilder, DecisionsRuleBuilder>()
            .AddSingleton<IDialogWithExitsRuleBuilder, DialogWithExitsRuleBuilder>()
            .AddSingleton<IDialogWithoutExitsRuleBuilder, DialogWithoutExitsRuleBuilder>()
            .AddSingleton<IMergeRuleBuilder, MergeRuleBuilder>()
            .AddSingleton<IModuleBeginRuleBuilder, ModuleBeginRuleBuilder>()
            .AddSingleton<IModuleRuleBuilder, ModuleRuleBuilder>()
            .AddSingleton<IShapeSetRuleBuilder, ShapeSetRuleBuilder>()
            .AddSingleton<ITableRowRuleBuilder, TableRowRuleBuilder>()
            .AddSingleton<IWaitConditionsRuleBuilder, WaitConditionsRuleBuilder>()
            .AddSingleton<IWaitDecisionsRuleBuilder, WaitDecisionsRuleBuilder>()
            .AddSingleton<IDiagramResourcesManager, DiagramResourcesManager>()
            .AddSingleton<ILongStringManager, LongStringManager>()
            .AddSingleton<ITableResourcesManager, TableResourcesManager>()

            //TreeViewBuiilders
            .AddSingleton<IEmptyFolderRemover, EmptyFolderRemover>()
            .AddSingleton<IGetAllCheckedNodes, GetAllCheckedNodes>()
            .AddSingleton<ISelectDocunentsTreeViewBuilder, SelectDocunentsTreeViewBuilder>()
            .AddSingleton<ISelectModulesForDeploymentTreeViewBuilder, SelectModulesForDeploymentTreeViewBuilder>()
            .AddSingleton<ISelectRulesTreeViewBuilder, SelectRulesTreeViewBuilder>()

            //XmlValidation
            .AddSingleton<IXmlValidator, XmlValidator>()

            //XmlValidation.Configuration
            .AddSingleton<IConnectorDataXmlValidator, ConnectorDataXmlValidator>()
            .AddSingleton<IConstructorsXmlValidator, ConstructorsXmlValidator>()
            .AddSingleton<IFunctionsXmlValidator, FunctionsXmlValidator>()
            .AddSingleton<IVariablesXmlValidator, VariablesXmlValidator>()

            //XmlValidation.DataValidation
            .AddSingleton<IAssertFunctionElementValidator, AssertFunctionElementValidator>()
            .AddSingleton<IBinaryOperatorFunctionElementValidator, BinaryOperatorFunctionElementValidator>()
            .AddSingleton<ICallElementValidator, CallElementValidator>()
            .AddSingleton<IConditionsElementValidator, ConditionsElementValidator>()
            .AddSingleton<IConnectorElementValidator, ConnectorElementValidator>()
            .AddSingleton<IConstructorElementValidator, ConstructorElementValidator>()
            .AddSingleton<IConstructorGenericsConfigrationValidator, ConstructorGenericsConfigrationValidator>()
            .AddSingleton<IDecisionElementValidator, DecisionElementValidator>()
            .AddSingleton<IDecisionsElementValidator, DecisionsElementValidator>()
            .AddSingleton<IFunctionElementValidator, FunctionElementValidator>()
            .AddSingleton<IFunctionGenericsConfigrationValidator, FunctionGenericsConfigrationValidator>()
            .AddSingleton<IFunctionsElementValidator, FunctionsElementValidator>()
            .AddSingleton<IGenericsConfigrationValidator, GenericsConfigrationValidator>()
            .AddSingleton<ILiteralElementValidator, LiteralElementValidator>()
            .AddSingleton<ILiteralListElementValidator, LiteralListElementValidator>()
            .AddSingleton<ILiteralListParameterElementValidator, LiteralListParameterElementValidator>()
            .AddSingleton<ILiteralListVariableElementValidator, LiteralListVariableElementValidator>()
            .AddSingleton<ILiteralParameterElementValidator, LiteralParameterElementValidator>()
            .AddSingleton<ILiteralVariableElementValidator, LiteralVariableElementValidator>()
            .AddSingleton<IMetaObjectElementValidator, MetaObjectElementValidator>()
            .AddSingleton<IObjectElementValidator, ObjectElementValidator>()
            .AddSingleton<IObjectListElementValidator, ObjectListElementValidator>()
            .AddSingleton<IObjectListParameterElementValidator, ObjectListParameterElementValidator>()
            .AddSingleton<IObjectListVariableElementValidator, ObjectListVariableElementValidator>()
            .AddSingleton<IObjectParameterElementValidator, ObjectParameterElementValidator>()
            .AddSingleton<IObjectVariableElementValidator, ObjectVariableElementValidator>()
            .AddSingleton<IParameterElementValidator, ParameterElementValidator>()
            .AddSingleton<IParametersElementValidator, ParametersElementValidator>()
            .AddSingleton<IRetractFunctionElementValidator, RetractFunctionElementValidator>()
            .AddSingleton<IRuleChainingUpdateFunctionElementValidator, RuleChainingUpdateFunctionElementValidator>()
            .AddSingleton<IVariableElementValidator, VariableElementValidator>()
            .AddSingleton<IXmlElementValidator, XmlElementValidator>();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Telerik.WinControls.ThemeResolutionService.ApplicationThemeName = Properties.Settings.Default.themeName;

            var form = ServiceProvider.GetRequiredService<MDIParent>();

            ServiceProvider.GetRequiredService<IFormInitializer>().SetCenterScreen(form);

            Application.Run(form);
        }
    }
}