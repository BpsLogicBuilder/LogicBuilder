using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConditionFunctionListBoxItemFactoryServices
    {
        internal static IServiceCollection AddConditionFunctionListBoxItemFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<string, string, IApplicationControl, IConditionFunctionListBoxItem>>
                (
                    provider =>
                    (visibleText, hiddenText, applicationControl) => new ConditionFunctionListBoxItem
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
                .AddTransient<IConditionFunctionListBoxItemFactory, ConditionFunctionListBoxItemFactory>();
        }
    }
}
