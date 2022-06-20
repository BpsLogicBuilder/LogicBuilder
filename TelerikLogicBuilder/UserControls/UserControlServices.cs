using ABIS.LogicBuilder.FlowBuilder.UserControls;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class UserControlServices
    {
        internal static IServiceCollection AddUserControls(this IServiceCollection services)
        {
            return services
                .AddTransient<ProjectExplorer>();
        }
    }
}
