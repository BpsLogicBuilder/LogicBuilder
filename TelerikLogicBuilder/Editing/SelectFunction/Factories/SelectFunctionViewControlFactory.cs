using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction.Factories
{
    internal class SelectFunctionViewControlFactory : ISelectFunctionViewControlFactory
    {
        private readonly Func<ISelectFunctionControl, ISelectFunctionDropDownViewControl> _getSelectFunctionDropdownViewControl;
        private readonly Func<ISelectFunctionControl, ISelectFunctionListViewControl> _getSelectFunctionListViewControl;
        private readonly Func<ISelectFunctionControl, ISelectFunctionTreeViewControl> _getSelectFunctionTreeViewControl;

        public SelectFunctionViewControlFactory(
            Func<ISelectFunctionControl, ISelectFunctionDropDownViewControl> getSelectFunctionDropdownViewControl,
            Func<ISelectFunctionControl, ISelectFunctionListViewControl> getSelectFunctionListViewControl,
            Func<ISelectFunctionControl, ISelectFunctionTreeViewControl> getSelectFunctionTreeViewControl)
        {
            _getSelectFunctionDropdownViewControl = getSelectFunctionDropdownViewControl;
            _getSelectFunctionListViewControl = getSelectFunctionListViewControl;
            _getSelectFunctionTreeViewControl = getSelectFunctionTreeViewControl;
        }

        public ISelectFunctionDropDownViewControl GetSelectFunctionDropdownViewControl(ISelectFunctionControl selectFunctionControl)
            => _getSelectFunctionDropdownViewControl(selectFunctionControl);

        public ISelectFunctionListViewControl GetSelectFunctionListViewControl(ISelectFunctionControl selectFunctionControl)
            => _getSelectFunctionListViewControl(selectFunctionControl);

        public ISelectFunctionTreeViewControl GetSelectFunctionTreeViewControl(ISelectFunctionControl selectFunctionControl)
            => _getSelectFunctionTreeViewControl(selectFunctionControl);
    }
}
