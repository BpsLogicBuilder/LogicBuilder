using ABIS.LogicBuilder.FlowBuilder.UserControls.DialogFormMessageControlHelpers.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class DialogFormMessageControlFactoryServices
    {
        internal static IServiceCollection AddDialogFormMessageControlFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IDialogFormMessageControlFactory, DialogFormMessageControlFactory>();
        }
    }
}
