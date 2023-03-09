using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class EditingControlHelperFactory : IEditingControlHelperFactory
    {
        private readonly Func<IEditFunctionControl, IEditFunctionControlHelper> _getEditFunctionControlHelper;
        private readonly Func<IEditingControl, IEditingForm, ILoadParameterControlsDictionary> _getLoadParameterControlsDictionary;

        public EditingControlHelperFactory(
            Func<IEditFunctionControl, IEditFunctionControlHelper> getEditFunctionControlHelper,
            Func<IEditingControl, IEditingForm, ILoadParameterControlsDictionary> getLoadParameterControlsDictionary)
        {
            _getEditFunctionControlHelper = getEditFunctionControlHelper;
            _getLoadParameterControlsDictionary = getLoadParameterControlsDictionary;
        }

        public IEditFunctionControlHelper GetEditFunctionControlHelper(IEditFunctionControl editFunctionControl)
            => _getEditFunctionControlHelper(editFunctionControl);

        public ILoadParameterControlsDictionary GetLoadParameterControlsDictionary(IEditingControl editingControl, IEditingForm editingForm)
            => _getLoadParameterControlsDictionary(editingControl, editingForm);
    }
}
