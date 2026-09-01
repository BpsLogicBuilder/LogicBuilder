using LogicBuilder.App.Bsl.Flow;
using LogicBuilder.App.Bsl.Flow.Interfaces;
using LogicBuilder.App.Utils.Rules;
using LogicBuilder.RulesDirector;
using Shop.Bsl.Flow;
using Shop.Bsl.Flow.Interfaces;

#pragma warning disable IDE0130 //Microsoft recommended namespace for service registrations
namespace Microsoft.Extensions.DependencyInjection
#pragma warning restore IDE0130
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class FlowServiceRegistrations
    {
        public static IServiceCollection AddContosoBslFlowServices(this IServiceCollection services)
        {
            return services
                .AddAppUtilsServices()
                .AddHttpClient()
                .AddFlowFactories()
                .AddBslUtilsServices()
                .AddRulesCacheService
                (
                    new RulesLoaderRequest
                    (
                        "Contoso.Bsl.Flow.Rulesets",
                        typeof(FlowActivity),
                        [
                            typeof(LogicBuilder.App.Utils.Interfaces.ITypeHelper).Assembly,
                            typeof(LogicBuilder.Forms.Parameters.Expansions.SelectExpandDefinitionParameters).Assembly,
                            typeof(DirectorBase).Assembly,
                            typeof(string).Assembly
                        ]
                    )
                )
                .AddTransient<ICustomActions, CustomActions>()
                .AddTransient<IFlowManager, FlowManager>()
                .AddScoped<IFlowDataCache, FlowDataCache>()
                .AddScoped<Progress>();
        }
    }
}
