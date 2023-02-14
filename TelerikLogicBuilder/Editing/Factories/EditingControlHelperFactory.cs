using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class EditingControlHelperFactory : IEditingControlHelperFactory
    {
        private readonly Func<IEditingControl, IEditingForm, ILoadParameterControlsDictionary> _getLoadParameterControlsDictionary;

        public EditingControlHelperFactory(
            Func<IEditingControl, IEditingForm, ILoadParameterControlsDictionary> getLoadParameterControlsDictionary)
        {
            _getLoadParameterControlsDictionary = getLoadParameterControlsDictionary;
        }

        public ILoadParameterControlsDictionary GetLoadParameterControlsDictionary(IEditingControl editingControl, IEditingForm editingForm)
            => _getLoadParameterControlsDictionary(editingControl, editingForm);
    }
}
