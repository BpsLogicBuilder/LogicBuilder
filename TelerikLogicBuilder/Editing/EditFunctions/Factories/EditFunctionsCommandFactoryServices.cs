using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditFunctionsCommandFactoryServices
    {
        internal static IServiceCollection AddEditFunctionsCommandFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IEditFunctionsCommandFactory, EditFunctionsCommandFactory>();
        }
    }
}
