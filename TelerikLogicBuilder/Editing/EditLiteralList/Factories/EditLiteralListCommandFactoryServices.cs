using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditLiteralListCommandFactoryServices
    {
        internal static IServiceCollection AddEditLiteralListCommandFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IEditLiteralListCommandFactory, EditLiteralListCommandFactory>();
        }
    }
}
