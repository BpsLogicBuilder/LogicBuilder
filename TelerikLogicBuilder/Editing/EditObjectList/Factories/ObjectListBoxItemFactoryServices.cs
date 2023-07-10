using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ObjectListBoxItemFactoryServices
    {
        internal static IServiceCollection AddObjectListBoxItemFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IObjectListBoxItemFactory, ObjectListBoxItemFactory>();
        }
    }
}
