using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class EditingControlHelperFactory : IEditingControlHelperFactory
    {
        private readonly Func<IEditFunctionControl, IEditFunctionControlHelper> _getEditFunctionControlHelper;
        private readonly Func<IDataGraphEditingControl, IEditingForm, ILoadParameterControlsDictionary> _getLoadParameterControlsDictionary;
        private readonly Func<IRichInputBoxValueControl, IRichInputBoxEventsHelper> _getRichInputBoxEventsHelper;

        public EditingControlHelperFactory(
            Func<IEditFunctionControl, IEditFunctionControlHelper> getEditFunctionControlHelper,
            Func<IDataGraphEditingControl, IEditingForm, ILoadParameterControlsDictionary> getLoadParameterControlsDictionary,
            Func<IRichInputBoxValueControl, IRichInputBoxEventsHelper> getRichInputBoxEventsHelper)
        {
            _getEditFunctionControlHelper = getEditFunctionControlHelper;
            _getLoadParameterControlsDictionary = getLoadParameterControlsDictionary;
            _getRichInputBoxEventsHelper = getRichInputBoxEventsHelper;
        }

        public IEditFunctionControlHelper GetEditFunctionControlHelper(IEditFunctionControl editFunctionControl)
            => _getEditFunctionControlHelper(editFunctionControl);

        public ILoadParameterControlsDictionary GetLoadParameterControlsDictionary(IDataGraphEditingControl editingControl, IEditingForm editingForm)
            => _getLoadParameterControlsDictionary(editingControl, editingForm);

        public IRichInputBoxEventsHelper GetRichInputBoxEventsHelper(IRichInputBoxValueControl richInputBoxValueControl)
            => _getRichInputBoxEventsHelper(richInputBoxValueControl);
    }
}
