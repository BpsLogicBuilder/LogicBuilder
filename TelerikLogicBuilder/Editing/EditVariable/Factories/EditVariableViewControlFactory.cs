using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable.Factories
{
    internal class EditVariableViewControlFactory : IEditVariableViewControlFactory
    {
        public IEditVariableDropdownViewControl GetEditVariableDropdownViewControl(IEditVariableControl editVariableControl)
            => new EditVariableDropdownViewControl
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                editVariableControl
            );

        public IEditVariableListViewControl GetEditVariableListViewControl(IEditVariableControl editVariableControl)
            => new EditVariableListViewControl
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                editVariableControl
            );

        public IEditVariableTreeViewControl GetEditVariableTreeViewControl(IEditVariableControl editVariableControl)
            => new EditVariableTreeViewControl
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<ITreeViewBuilderFactory>(),
                Program.ServiceProvider.GetRequiredService<ITreeViewService>(),
                editVariableControl
            );
    }
}
