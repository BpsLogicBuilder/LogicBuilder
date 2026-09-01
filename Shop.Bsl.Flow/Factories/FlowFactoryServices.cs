using LogicBuilder.RulesDirector;
using Shop.Bsl.Flow;
using Shop.Bsl.Flow.Factories;
using Shop.Bsl.Flow.Interfaces;
using System;

#pragma warning disable IDE0130 //Microsoft recommended namespace for service registrations
namespace Microsoft.Extensions.DependencyInjection
#pragma warning restore IDE0130
{
    public static class FlowFactoryServices
    {
        public static IServiceCollection AddFlowFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IFlowManager, DirectorBase>>
                (
                    provider =>
                    flowManager => new Director(flowManager)
                )
                .AddTransient<Func<IFlowManager, IFlowActivity>>
                (
                    provider =>
                    flowManager => new FlowActivity(flowManager)
                )
                .AddTransient<IFlowFactory, FlowFactory>();
        }
    }
}
