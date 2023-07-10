using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class FunctionListBoxItemFactoryServices
    {
        internal static IServiceCollection AddFunctionListBoxItemFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IFunctionListBoxItemFactory, FunctionListBoxItemFactory>();
        }
    }
}
