using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditObjectListCommandFactoryServices
    {
        internal static IServiceCollection AddEditObjectListCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IEditObjectListCommandFactory, EditObjectListCommandFactory>();
        }
    }
}
