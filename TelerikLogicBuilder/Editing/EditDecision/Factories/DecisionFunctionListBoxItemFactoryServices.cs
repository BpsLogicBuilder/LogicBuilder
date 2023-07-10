using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecision.Factories
{
    internal static class DecisionFunctionListBoxItemFactoryServices
    {
        internal static IServiceCollection AddDecisionFunctionListBoxItemFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IDecisionFunctionListBoxItemFactory, DecisionFunctionListBoxItemFactory>();
        }
    }
}
