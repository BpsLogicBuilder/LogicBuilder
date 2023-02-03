using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable.Factories
{
    internal class SelectVariableViewControlFactory : ISelectVariableViewControlFactory
    {
        private readonly Func<ISelectVariableControl, ISelectVariableDropDownViewControl> _getSelectVariableDropdownViewControl;
        private readonly Func<ISelectVariableControl, ISelectVariableListViewControl> _getSelectVariableListViewControl;
        private readonly Func<ISelectVariableControl, ISelectVariableTreeViewControl> _getSelectVariableTreeViewControl;

        public SelectVariableViewControlFactory(
            Func<ISelectVariableControl, ISelectVariableDropDownViewControl> getSelectVariableDropdownViewControl,
            Func<ISelectVariableControl, ISelectVariableListViewControl> getSelectVariableListViewControl,
            Func<ISelectVariableControl, ISelectVariableTreeViewControl> getSelectVariableTreeViewControl)
        {
            _getSelectVariableDropdownViewControl = getSelectVariableDropdownViewControl;
            _getSelectVariableListViewControl = getSelectVariableListViewControl;
            _getSelectVariableTreeViewControl = getSelectVariableTreeViewControl;
        }

        public ISelectVariableDropDownViewControl GetSelectVariableDropdownViewControl(ISelectVariableControl selectVariableControl)
            => _getSelectVariableDropdownViewControl(selectVariableControl);

        public ISelectVariableListViewControl GetSelectVariableListViewControl(ISelectVariableControl selectVariableControl)
            => _getSelectVariableListViewControl(selectVariableControl);

        public ISelectVariableTreeViewControl GetSelectVariableTreeViewControl(ISelectVariableControl selectVariableControl)
            => _getSelectVariableTreeViewControl(selectVariableControl);
    }
}
