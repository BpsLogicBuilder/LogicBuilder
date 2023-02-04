using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class SelectFunctionViewControlFactoryServices
    {
        internal static IServiceCollection AddSelectFunctionViewControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<ISelectFunctionViewControlFactory, SelectFunctionViewControlFactory>()
                .AddTransient<Func<ISelectFunctionControl, ISelectFunctionDropDownViewControl>>
                (
                    provider =>
                    selectFunctionControl => new SelectFunctionDropDownViewControl
                    (
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        selectFunctionControl
                    )
                )
                .AddTransient<Func<ISelectFunctionControl, ISelectFunctionListViewControl>>
                (
                    provider =>
                    selectFunctionControl => new SelectFunctionListViewControl
                    (
                        selectFunctionControl
                    )
                )
                .AddTransient<Func<ISelectFunctionControl, ISelectFunctionTreeViewControl>>
                (
                    provider =>
                    selectFunctionControl => new SelectFunctionTreeViewControl
                    (
                        provider.GetRequiredService<ITreeViewBuilderFactory>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        selectFunctionControl
                    )
                );
        }
    }
}
