using Contoso.Test.Flow;
using Contoso.Test.Flow.Cache;
using LogicBuilder.RulesDirector;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection AddServiceRegistrations(this IServiceCollection services)
        {
            return services
                .AddTransient<IFlowManager, FlowManager>()
                .AddTransient<IFilterHelper, FilterHelper>()
                .AddTransient<ISelectorHelper, SelectorHelper>()
                .AddTransient<FlowActivityFactory, FlowActivityFactory>()
                .AddTransient<DirectorFactory, DirectorFactory>()
                .AddTransient<ICustomActions, CustomActions>()
                .AddTransient<ICustomDialogs, CustomDialogs>()
                .AddScoped<FlowDataCache, FlowDataCache>()
                .AddScoped<Progress, Progress>();
        }
    }
}
