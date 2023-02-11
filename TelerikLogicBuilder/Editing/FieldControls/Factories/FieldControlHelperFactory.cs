using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories
{
    internal class FieldControlHelperFactory : IFieldControlHelperFactory
    {
        private readonly Func<IRichInputBoxValueControl, ICreateRichInputBoxContextMenu> _getCreateRichInputBoxContextMenu;
        private readonly Func<IRichInputBoxValueControl, IEditVariableHelper> _getEditVariableHelper;
        private readonly Func<IRichInputBoxValueControl, IRichInputBoxEventsHelper> _getRichInputBoxEventsHelper;

        public FieldControlHelperFactory(
            Func<IRichInputBoxValueControl, ICreateRichInputBoxContextMenu> getCreateRichInputBoxContextMenu,
            Func<IRichInputBoxValueControl, IEditVariableHelper> getEditVariableHelper,
            Func<IRichInputBoxValueControl, IRichInputBoxEventsHelper> getRichInputBoxEventsHelper)
        {
            _getCreateRichInputBoxContextMenu = getCreateRichInputBoxContextMenu;
            _getEditVariableHelper = getEditVariableHelper;
            _getRichInputBoxEventsHelper = getRichInputBoxEventsHelper;
        }

        public ICreateRichInputBoxContextMenu GetCreateRichInputBoxContextMenu(IRichInputBoxValueControl richInputBoxValueControl)
            => _getCreateRichInputBoxContextMenu(richInputBoxValueControl);

        public IEditVariableHelper GetEditVariableHelper(IRichInputBoxValueControl richInputBoxValueControl)
            => _getEditVariableHelper(richInputBoxValueControl);

        public IRichInputBoxEventsHelper GetRichInputBoxEventsHelper(IRichInputBoxValueControl richInputBoxValueControl)
            => _getRichInputBoxEventsHelper(richInputBoxValueControl);
    }
}
