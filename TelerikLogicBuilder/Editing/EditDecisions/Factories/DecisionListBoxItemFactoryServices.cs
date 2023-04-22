using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class DecisionListBoxItemFactoryServices
    {
        internal static IServiceCollection AddDecisionListBoxItemFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<string, string, IApplicationControl, IDecisionListBoxItem>>
                (
                    provider =>
                    (visibleText, hiddenText, applicationControl) => new DecisionListBoxItem
                    (
                        provider.GetRequiredService<IDecisionElementValidator>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        visibleText,
                        hiddenText,
                        applicationControl
                    )
                )
                .AddTransient<IDecisionListBoxItemFactory,  DecisionListBoxItemFactory>();
        }
    }
}
