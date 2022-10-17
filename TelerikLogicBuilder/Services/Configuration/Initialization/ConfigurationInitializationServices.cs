using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using ABIS.LogicBuilder.FlowBuilder.Services.Configuration.Initialization;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigurationInitializationServices
    {
        internal static IServiceCollection AddConfigurationInitialization(this IServiceCollection services)
            => services
                .AddSingleton<IConstructorDictionaryBuilder, ConstructorDictionaryBuilder>()
                .AddSingleton<IConstructorListInitializer, ConstructorListInitializer>()
                .AddSingleton<IConstructorTreeFolderBuilder, ConstructorTreeFolderBuilder>()
                .AddSingleton<IEmptyTreeFolderRemover, EmptyTreeFolderRemover>()
                .AddSingleton<IFragmentDictionaryBuilder, FragmentDictionaryBuilder>()
                .AddSingleton<IFragmentListInitializer, FragmentListInitializer>()
                .AddSingleton<IFragmentTreeFolderBuilder, FragmentTreeFolderBuilder>()
                .AddSingleton<IFunctionDictionaryBuilder, FunctionDictionaryBuilder>()
                .AddSingleton<IFunctionListInitializer, FunctionListInitializer>()
                .AddSingleton<IFunctionTreeFolderBuilder, FunctionTreeFolderBuilder>()
                .AddSingleton<IVariableDictionaryBuilder, VariableDictionaryBuilder>()
                .AddSingleton<IVariableListInitializer, VariableListInitializer>()
                .AddSingleton<IVariableTreeFolderBuilder, VariableTreeFolderBuilder>();
    }
}
