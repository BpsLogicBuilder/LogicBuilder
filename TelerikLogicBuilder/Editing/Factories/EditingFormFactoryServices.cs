using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditingFormFactoryServices
    {
        internal static IServiceCollection AddEditingFormFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IEditingFormFactory, EditingFormFactory>()
                .AddTransient<Func<Type, ISelectVariableForm>>
                (
                    provider =>
                    assignedTo => new SelectVariableForm
                    (
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<ISelectEditingControlFactory>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        assignedTo
                    )
                );
        }
    }
}
