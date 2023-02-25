using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable.Factories
{
    internal class EditVariableViewControlFactory : IEditVariableViewControlFactory
    {
        private readonly Func<IEditVariableControl, IEditVariableDropdownViewControl> _getEditVariableDropdownViewControl;
        private readonly Func<IEditVariableControl, IEditVariableListViewControl> _getEditVariableListViewControl;
        private readonly Func<IEditVariableControl, IEditVariableTreeViewControl> _getEditVariableTreeViewControl;

        public EditVariableViewControlFactory(
            Func<IEditVariableControl, IEditVariableDropdownViewControl> getEditVariableDropdownViewControl,
            Func<IEditVariableControl, IEditVariableListViewControl> getEditVariableListViewControl,
            Func<IEditVariableControl, IEditVariableTreeViewControl> getEditVariableTreeViewControl)
        {
            _getEditVariableDropdownViewControl = getEditVariableDropdownViewControl;
            _getEditVariableListViewControl = getEditVariableListViewControl;
            _getEditVariableTreeViewControl = getEditVariableTreeViewControl;
        }

        public IEditVariableDropdownViewControl GetEditVariableDropdownViewControl(IEditVariableControl editVariableControl)
            => _getEditVariableDropdownViewControl(editVariableControl);

        public IEditVariableListViewControl GetEditVariableListViewControl(IEditVariableControl editVariableControl)
            => _getEditVariableListViewControl(editVariableControl);

        public IEditVariableTreeViewControl GetEditVariableTreeViewControl(IEditVariableControl editVariableControl)
            => _getEditVariableTreeViewControl(editVariableControl);
    }
}
