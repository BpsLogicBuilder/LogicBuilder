using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Forms;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class FlowBuilderServices
    {
        internal static IServiceCollection AddFlowBuilder(this IServiceCollection services)
            => services
                .AddTransient<IMDIParent, MDIParent>()
                .AddSingleton<IAssemblyLoadContextManager, AssemblyLoadContextManager>()
                .AddSingleton<ICellXmlHelper, CellXmlHelper>()
                .AddSingleton<ICheckSelectedApplication, CheckSelectedApplication>()
                .AddSingleton<ICheckVisioConfiguration, CheckVisioConfiguration>()
                .AddSingleton<ICompareImages, CompareImages>()
                .AddSingleton<IConstructorTypeHelper, ConstructorTypeHelper>()
                .AddSingleton<IDisplayResultMessages, DisplayResultMessages>()
                .AddSingleton<IEncryption, Encryption>()
                .AddSingleton<IEnumHelper, EnumHelper>()
                .AddSingleton<IExceptionHelper, ExceptionHelper>()
                .AddSingleton<IFileIOHelper, FileIOHelper>()
                .AddSingleton<IFormInitializer, FormInitializer>()
                .AddSingleton<IGetPromptForLiteralDomainUpdate, GetPromptForLiteralDomainUpdate>()
                .AddSingleton<IImageListService, ImageListService>()
                .AddSingleton<IMemberAttributeReader, MemberAttributeReader>()
                .AddSingleton<IMainWindow, MainWindow>()
                .AddSingleton<IModuleNamesReader, ModuleNamesReader>()
                .AddTransient<INewProjectForm, NewProjectForm>()
                .AddSingleton<IParameterAttributeReader, ParameterAttributeReader>()
                .AddSingleton<IPathHelper, PathHelper>()
                .AddSingleton<IResultMessageBuilder, ResultMessageBuilder>()
                .AddSingleton<IShapeDataCellManager, ShapeDataCellManager>()
                .AddSingleton<IShapeXmlHelper, ShapeXmlHelper>()
                .AddTransient<ISplashScreen, SplashScreen>()
                .AddSingleton<IStringHelper, StringHelper>()
                .AddSingleton<IThemeManager, ThemeManager>()
                .AddSingleton<ITreeViewService, TreeViewService>()
                .AddSingleton<ITypeHelper, TypeHelper>()
                .AddSingleton<ITypeLoadHelper, TypeLoadHelper>()
                .AddSingleton<IUiNotificationService, UiNotificationService>()
                .AddSingleton<IXmlDocumentHelpers, XmlDocumentHelpers>()
                .AddTransient(typeof(IScopedDisposableManager<>), typeof(ScopedDisposableManager<>))
                .AddAttributeReaderFactories()
                .AddFactories()
                .AddNewProjectFormCommandFactories()
                .AddStructuresFactories();
    }
}
