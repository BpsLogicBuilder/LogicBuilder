using ABIS.LogicBuilder.FlowBuilder.Editing.EditSetValueFunction.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditSetValueFunctionCommandFactoryServices
    {
        internal static IServiceCollection AddEditSetValueFunctionCommandFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IEditSetValueFunctionCommandFactory, EditSetValueFunctionCommandFactory>();
        }
    }
}
