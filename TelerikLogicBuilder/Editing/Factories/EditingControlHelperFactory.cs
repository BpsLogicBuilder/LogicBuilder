using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class EditingControlHelperFactory : IEditingControlHelperFactory
    {
        private readonly Func<IRichInputBoxValueControl, ICreateRichInputBoxContextMenu> _getCreateRichInputBoxContextMenu;
        private readonly Func<IEditFunctionControl, IEditFunctionControlHelper> _getEditFunctionControlHelper;
        private readonly Func<IDataGraphEditingControl, IDataGraphEditingHost, ILoadParameterControlsDictionary> _getLoadParameterControlsDictionary;

        public EditingControlHelperFactory(
            Func<IRichInputBoxValueControl, ICreateRichInputBoxContextMenu> getCreateRichInputBoxContextMenu,
            Func<IEditFunctionControl, IEditFunctionControlHelper> getEditFunctionControlHelper,
            Func<IDataGraphEditingControl, IDataGraphEditingHost, ILoadParameterControlsDictionary> getLoadParameterControlsDictionary)
        {
            _getCreateRichInputBoxContextMenu = getCreateRichInputBoxContextMenu;
            _getEditFunctionControlHelper = getEditFunctionControlHelper;
            _getLoadParameterControlsDictionary = getLoadParameterControlsDictionary;
        }

        public ICreateRichInputBoxContextMenu GetCreateRichInputBoxContextMenu(IRichInputBoxValueControl richInputBoxValueControl)
            => _getCreateRichInputBoxContextMenu(richInputBoxValueControl);

        public IEditFunctionControlHelper GetEditFunctionControlHelper(IEditFunctionControl editFunctionControl)
            => _getEditFunctionControlHelper(editFunctionControl);

        public ILoadParameterControlsDictionary GetLoadParameterControlsDictionary(IDataGraphEditingControl editingControl, IDataGraphEditingHost dataGraphEditingHost)
            => _getLoadParameterControlsDictionary(editingControl, dataGraphEditingHost);
    }
}
