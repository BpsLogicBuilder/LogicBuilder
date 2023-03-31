using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment.Factories
{
    internal class SelectFragmentViewControlFactory : ISelectFragmentViewControlFactory
    {
        private readonly Func<ISelectFragmentControl, ISelectFragmentDropDownViewControl> _getSelectFragmentDropdownViewControl;
        private readonly Func<ISelectFragmentControl, ISelectFragmentListViewControl> _getSelectFragmentListViewControl;
        private readonly Func<ISelectFragmentControl, ISelectFragmentTreeViewControl> _getSelectFragmentTreeViewControl;

        public SelectFragmentViewControlFactory(
            Func<ISelectFragmentControl, ISelectFragmentDropDownViewControl> getSelectFragmentDropdownViewControl,
            Func<ISelectFragmentControl, ISelectFragmentListViewControl> getSelectFragmentListViewControl,
            Func<ISelectFragmentControl, ISelectFragmentTreeViewControl> getSelectFragmentTreeViewControl)
        {
            _getSelectFragmentDropdownViewControl = getSelectFragmentDropdownViewControl;
            _getSelectFragmentListViewControl = getSelectFragmentListViewControl;
            _getSelectFragmentTreeViewControl = getSelectFragmentTreeViewControl;
        }

        public ISelectFragmentDropDownViewControl GetSelectFragmentDropdownViewControl(ISelectFragmentControl selectFragmentControl)
            => _getSelectFragmentDropdownViewControl(selectFragmentControl);

        public ISelectFragmentListViewControl GetSelectFragmentListViewControl(ISelectFragmentControl selectFragmentControl)
            => _getSelectFragmentListViewControl(selectFragmentControl);

        public ISelectFragmentTreeViewControl GetSelectFragmentTreeViewControl(ISelectFragmentControl selectFragmentControl)
            => _getSelectFragmentTreeViewControl(selectFragmentControl);
    }
}
