using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditVoidFunctionCommandFactoryServices
    {
        internal static IServiceCollection AddEditVoidFunctionCommandFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IEditVoidFunctionCommandFactory, EditVoidFunctionCommandFactory>();
        }
    }
}
