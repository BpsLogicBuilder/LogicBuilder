using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditConstructorCommandFactoryServices
    {
        internal static IServiceCollection AddEditConstructorCommandFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IEditConstructorCommandFactory, EditConstructorCommandFactory>();
        }
    }
}
