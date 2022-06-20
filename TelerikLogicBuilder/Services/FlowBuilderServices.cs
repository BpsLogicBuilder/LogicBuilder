using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class FlowBuilderServices
    {
        internal static IServiceCollection AddFlowBuilder(this IServiceCollection services)
            => services
                .AddTransient<MDIParent>()
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
                .AddSingleton<IResultMessageBuilder, ResultMessageBuilder>()
                .AddSingleton<IShapeDataCellManager, ShapeDataCellManager>()
                .AddSingleton<IShapeXmlHelper, ShapeXmlHelper>()
                .AddSingleton<IStringHelper, StringHelper>()
                .AddSingleton<IThemeManager, ThemeManager>()
                .AddSingleton<ITreeViewService, TreeViewService>()
                .AddSingleton<ITypeHelper, TypeHelper>()
                .AddSingleton<ITypeLoadHelper, TypeLoadHelper>()
                .AddSingleton<IXmlDocumentHelpers, XmlDocumentHelpers>()
                .AddTransient(typeof(IScopedDisposableManager<>), typeof(ScopedDisposableManager<>));
    }
}
