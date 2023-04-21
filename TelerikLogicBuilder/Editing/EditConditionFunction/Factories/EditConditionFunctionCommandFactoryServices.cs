using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunction.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunction.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditConditionFunctionCommandFactoryServices
    {
        internal static IServiceCollection AddEditConditionFunctionCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IEditConditionFunctionCommandFactory, EditConditionFunctionCommandFactory>()
                .AddTransient<Func<IEditConditionFunctionControl, SelectConditionFunctionCommand>>
                (
                    provider =>
                    editConditionFunctionControl => new SelectConditionFunctionCommand
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        editConditionFunctionControl
                    )
                );
        }
    }
}
