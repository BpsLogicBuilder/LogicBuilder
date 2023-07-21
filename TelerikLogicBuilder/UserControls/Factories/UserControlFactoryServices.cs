using ABIS.LogicBuilder.FlowBuilder.UserControls.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class UserControlFactoryServices
    {
        internal static IServiceCollection AddUserControlFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IUserControlFactory, UserControlFactory>();
        }
    }
}
