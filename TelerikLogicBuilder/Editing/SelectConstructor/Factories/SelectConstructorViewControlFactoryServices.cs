using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class SelectConstructorViewControlFactoryServices
    {
        internal static IServiceCollection AddSelectConstructorViewControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<ISelectConstructorViewControlFactory, SelectConstructorViewControlFactory>()
                .AddTransient<Func<ISelectConstructorControl, ISelectConstructorDropDownViewControl>>
                (
                    provider =>
                    selectConstructorControl => new SelectConstructorDropDownViewControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        selectConstructorControl
                    )
                )
                .AddTransient<Func<ISelectConstructorControl, ISelectConstructorListViewControl>>
                (
                    provider =>
                    selectConstructorControl => new SelectConstructorListViewControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        selectConstructorControl
                    )
                )
                .AddTransient<Func<ISelectConstructorControl, ISelectConstructorTreeViewControl>>
                (
                    provider =>
                    selectConstructorControl => new SelectConstructorTreeViewControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<ITreeViewBuilderFactory>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        selectConstructorControl
                    )
                );
        }
    }
}
