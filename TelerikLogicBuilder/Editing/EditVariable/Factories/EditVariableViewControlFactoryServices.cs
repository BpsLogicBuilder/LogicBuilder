using ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditVariableViewControlFactoryServices
    {
        internal static IServiceCollection AddSelectVariableViewControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IEditVariableViewControlFactory, EditVariableViewControlFactory>()
                .AddTransient<Func<IEditVariableControl, IEditVariableDropdownViewControl>>
                (
                    provider =>
                    editVariableControl => new EditVariableDropdownViewControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        editVariableControl
                    )
                )
                .AddTransient<Func<IEditVariableControl, IEditVariableListViewControl>>
                (
                    provider =>
                    editVariableControl => new EditVariableListViewControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        editVariableControl
                    )
                )
                .AddTransient<Func<IEditVariableControl, IEditVariableTreeViewControl>>
                (
                    provider =>
                    editVariableControl => new EditVariableTreeViewControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<ITreeViewBuilderFactory>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        editVariableControl
                    )
                );
        }
    }
}
