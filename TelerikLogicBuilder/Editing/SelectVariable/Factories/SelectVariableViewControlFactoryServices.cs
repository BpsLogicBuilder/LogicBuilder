using ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class SelectVariableViewControlFactoryServices
    {
        internal static IServiceCollection AddSelectVariableControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<ISelectVariableViewControlFactory, SelectVariableViewControlFactory>()
                .AddTransient<Func<ISelectVariableControl, ISelectVariableDropdownViewControl>>
                (
                    provider =>
                    selectVariableControl => new SelectVariableDropdownViewControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        selectVariableControl
                    )
                )
                .AddTransient<Func<ISelectVariableControl, ISelectVariableListViewControl>>
                (
                    provider =>
                    selectVariableControl => new SelectVariableListViewControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        selectVariableControl
                    )
                )
                .AddTransient<Func<ISelectVariableControl, ISelectVariableTreeViewControl>>
                (
                    provider =>
                    selectVariableControl => new SelectVariableTreeViewControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<ITreeViewBuilderFactory>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        selectVariableControl
                    )
                );
        }
    }
}
