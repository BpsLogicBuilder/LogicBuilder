using ABIS.LogicBuilder.FlowBuilder.Editing.EditValueFunction.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditValueFunctionCommandFactoryServices
    {
        internal static IServiceCollection AddEditValueFunctionCommandFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IEditValueFunctionCommandFactory, EditValueFunctionCommandFactory>();
        }
    }
}
