using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class SelectFragmentViewControlFactoryServices
    {
        internal static IServiceCollection AddSelectFragmentViewControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<ISelectFragmentControl, ISelectFragmentDropDownViewControl>>
                (
                    provider =>
                    selectFragmentControl => new SelectFragmentDropDownViewControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        selectFragmentControl
                    )
                )
                .AddTransient<Func<ISelectFragmentControl, ISelectFragmentListViewControl>>
                (
                    provider =>
                    selectFragmentControl => new SelectFragmentListViewControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        selectFragmentControl
                    )
                )
                .AddTransient<Func<ISelectFragmentControl, ISelectFragmentTreeViewControl>>
                (
                    provider =>
                    selectFragmentControl => new SelectFragmentTreeViewControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<ITreeViewBuilderFactory>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        selectFragmentControl
                    )
                )
                .AddTransient<ISelectFragmentViewControlFactory, SelectFragmentViewControlFactory>();
        }
    }
}
