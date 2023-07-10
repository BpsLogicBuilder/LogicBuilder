using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction.Factories
{
    internal class SelectFunctionViewControlFactory : ISelectFunctionViewControlFactory
    {
        public ISelectFunctionDropDownViewControl GetSelectFunctionDropdownViewControl(ISelectFunctionControl selectFunctionControl)
            => new SelectFunctionDropDownViewControl
            (
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                selectFunctionControl
            );

        public ISelectFunctionListViewControl GetSelectFunctionListViewControl(ISelectFunctionControl selectFunctionControl)
            => new SelectFunctionListViewControl
            (
                selectFunctionControl
            );

        public ISelectFunctionTreeViewControl GetSelectFunctionTreeViewControl(ISelectFunctionControl selectFunctionControl)
            => new SelectFunctionTreeViewControl
            (
                Program.ServiceProvider.GetRequiredService<ITreeViewBuilderFactory>(),
                Program.ServiceProvider.GetRequiredService<ITreeViewService>(),
                selectFunctionControl
            );
    }
}
