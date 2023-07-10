using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment.Factories
{
    internal class SelectFragmentViewControlFactory : ISelectFragmentViewControlFactory
    {
        public ISelectFragmentDropDownViewControl GetSelectFragmentDropdownViewControl(ISelectFragmentControl selectFragmentControl)
            => new SelectFragmentDropDownViewControl
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                selectFragmentControl
            );

        public ISelectFragmentListViewControl GetSelectFragmentListViewControl(ISelectFragmentControl selectFragmentControl)
            => new SelectFragmentListViewControl
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                selectFragmentControl
            );

        public ISelectFragmentTreeViewControl GetSelectFragmentTreeViewControl(ISelectFragmentControl selectFragmentControl)
            => new SelectFragmentTreeViewControl
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<ITreeViewBuilderFactory>(),
                Program.ServiceProvider.GetRequiredService<ITreeViewService>(),
                selectFragmentControl
            );
    }
}
