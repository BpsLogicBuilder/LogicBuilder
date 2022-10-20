using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class RulesGeneratorServices
    {
        internal static IServiceCollection AddRulesGenerator(this IServiceCollection services)
            => services
                .AddSingleton<IApiFileListDeleter, ApiFileListDeleter>()
                .AddSingleton<IApiFileListDeployer, ApiFileListDeployer>()
                .AddSingleton<IBuildSaveAssembleRulesForSelectedDocuments, BuildSaveAssembleRulesForSelectedDocuments>()
                .AddSingleton<ICellHelper, CellHelper>()
                .AddSingleton<IDeleteSelectedFilesFromApi, DeleteSelectedFilesFromApi>()
                .AddSingleton<IDeleteSelectedFilesFromFileSystem, DeleteSelectedFilesFromFileSystem>()
                .AddSingleton<IDeploySelectedFilesToApi, DeploySelectedFilesToApi>()
                .AddSingleton<IDeploySelectedFilesToFileSystem, DeploySelectedFilesToFileSystem>()
                .AddSingleton<IFileSystemFileDeleter, FileSystemFileDeleter>()
                .AddSingleton<IFileSystemFileDeployer, FileSystemFileDeployer>()
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
                .AddSingleton<IValidateSelectedDocuments, ValidateSelectedDocuments>()
                .AddSingleton<IValidateSelectedRules, ValidateSelectedRules>()
                .AddRulesGeneratorFactories()
                .AddRulesGeneratorForms()
                .AddRulesGeneratorRuleBuilders();
    }
}
