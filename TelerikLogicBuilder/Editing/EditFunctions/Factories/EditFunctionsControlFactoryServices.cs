using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditFunctionsControlFactoryServices
    {
        internal static IServiceCollection AddEditFunctionsControlFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IEditFunctionsControlFactory, EditFunctionsControlFactory>();
        }
    }
}
