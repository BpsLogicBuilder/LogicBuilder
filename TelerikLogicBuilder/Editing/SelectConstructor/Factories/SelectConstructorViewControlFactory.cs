using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor.Factories
{
    internal class SelectConstructorViewControlFactory : ISelectConstructorViewControlFactory
    {
        private readonly Func<ISelectConstructorControl, ISelectConstructorDropDownViewControl> _getSelectConstructorDropdownViewControl;
        private readonly Func<ISelectConstructorControl, ISelectConstructorListViewControl> _getSelectConstructorListViewControl;
        private readonly Func<ISelectConstructorControl, ISelectConstructorTreeViewControl> _getSelectConstructorTreeViewControl;

        public SelectConstructorViewControlFactory(
            Func<ISelectConstructorControl, ISelectConstructorDropDownViewControl> getSelectConstructorDropdownViewControl,
            Func<ISelectConstructorControl, ISelectConstructorListViewControl> getSelectConstructorListViewControl,
            Func<ISelectConstructorControl, ISelectConstructorTreeViewControl> getSelectConstructorTreeViewControl)
        {
            _getSelectConstructorDropdownViewControl = getSelectConstructorDropdownViewControl;
            _getSelectConstructorListViewControl = getSelectConstructorListViewControl;
            _getSelectConstructorTreeViewControl = getSelectConstructorTreeViewControl;
        }

        public ISelectConstructorDropDownViewControl GetSelectConstructorDropdownViewControl(ISelectConstructorControl selectConstructorControl)
            => _getSelectConstructorDropdownViewControl(selectConstructorControl);

        public ISelectConstructorListViewControl GetSelectConstructorListViewControl(ISelectConstructorControl selectConstructorControl)
            => _getSelectConstructorListViewControl(selectConstructorControl);

        public ISelectConstructorTreeViewControl GetSelectConstructorTreeViewControl(ISelectConstructorControl selectConstructorControl)
            => _getSelectConstructorTreeViewControl(selectConstructorControl);
    }
}
