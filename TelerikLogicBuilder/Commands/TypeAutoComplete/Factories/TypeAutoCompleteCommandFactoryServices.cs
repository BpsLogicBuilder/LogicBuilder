using ABIS.LogicBuilder.FlowBuilder.Commands.TypeAutoComplete.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class TypeAutoCompleteCommandFactoryServices
    {
        internal static IServiceCollection AddTypeAutoCompleteCommandFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<ITypeAutoCompleteCommandFactory, TypeAutoCompleteCommandFactory>();
        }
    }
}
