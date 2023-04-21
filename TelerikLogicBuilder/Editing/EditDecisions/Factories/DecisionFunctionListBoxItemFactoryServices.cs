using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class DecisionFunctionListBoxItemFactoryServices
    {
        internal static IServiceCollection AddDecisionFunctionListBoxItemFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<string, string, IApplicationControl, IDecisionFunctionListBoxItem>>
                (
                    provider =>
                    (visibleText, hiddenText, applicationControl) => new DecisionFunctionListBoxItem
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFunctionDataParser>(),
                        provider.GetRequiredService<IFunctionElementValidator>(),
                        provider.GetRequiredService<IFunctionHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        visibleText,
                        hiddenText,
                        applicationControl
                    )
                )
                .AddTransient<IDecisionFunctionListBoxItemFactory, DecisionFunctionListBoxItemFactory>();
        }
    }
}
