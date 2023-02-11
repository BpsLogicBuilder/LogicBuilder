using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditingControlFactoryServices
    {
        internal static IServiceCollection AddEditingControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IEditingForm, Constructor, Type, IEditConstructorControl>>
                (
                    provider =>
                    (editingForm, constructor, assignedTo) => new EditConstructorControl
                    (
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
                        provider.GetRequiredService<ITableLayoutPanelHelper>(),
                        editingForm,
                        constructor,
                        assignedTo
                    )
                )
                .AddTransient<IEditingControlFactory, EditingControlFactory>();
        }
    }
}
