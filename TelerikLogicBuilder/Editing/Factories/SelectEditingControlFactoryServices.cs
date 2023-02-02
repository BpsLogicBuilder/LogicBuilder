using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class SelectEditingControlFactoryServices
    {
        internal static IServiceCollection AddSelectEditingControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<ISelectEditingControlFactory, SelectEditingControlFactory>()
                .AddTransient<Func<IEditingForm, Type, ISelectVariableControl>>
                (
                    provider =>
                    (editingForm, assignedToType) => new SelectVariableControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ISelectVariableViewControlFactory>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        editingForm, 
                        assignedToType
                    )
                ); 
        }
    }
}
