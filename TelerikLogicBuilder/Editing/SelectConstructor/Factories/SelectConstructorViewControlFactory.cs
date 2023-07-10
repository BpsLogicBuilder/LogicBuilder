using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor.Factories
{
    internal class SelectConstructorViewControlFactory : ISelectConstructorViewControlFactory
    {
        public ISelectConstructorDropDownViewControl GetSelectConstructorDropdownViewControl(ISelectConstructorControl selectConstructorControl)
            => new SelectConstructorDropDownViewControl
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                selectConstructorControl
            );

        public ISelectConstructorListViewControl GetSelectConstructorListViewControl(ISelectConstructorControl selectConstructorControl)
            => new SelectConstructorListViewControl
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                selectConstructorControl
            );

        public ISelectConstructorTreeViewControl GetSelectConstructorTreeViewControl(ISelectConstructorControl selectConstructorControl)
            => new SelectConstructorTreeViewControl
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<ITreeViewBuilderFactory>(),
                Program.ServiceProvider.GetRequiredService<ITreeViewService>(),
                selectConstructorControl
            );
    }
}
