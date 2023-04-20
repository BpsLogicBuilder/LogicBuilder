using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions.Factories;
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
