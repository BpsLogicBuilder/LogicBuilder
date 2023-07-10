using ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditVariableViewControlFactoryServices
    {
        internal static IServiceCollection AddSelectVariableViewControlFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IEditVariableViewControlFactory, EditVariableViewControlFactory>();
        }
    }
}
