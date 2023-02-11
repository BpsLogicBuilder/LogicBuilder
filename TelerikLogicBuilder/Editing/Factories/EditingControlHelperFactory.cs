using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class EditingControlHelperFactory : IEditingControlHelperFactory
    {
        private readonly Func<IEditingControl, ILoadParameterControlsDictionary> _getLoadParameterControlsDictionary;

        public EditingControlHelperFactory(
            Func<IEditingControl, ILoadParameterControlsDictionary> getLoadParameterControlsDictionary)
        {
            _getLoadParameterControlsDictionary = getLoadParameterControlsDictionary;
        }

        public ILoadParameterControlsDictionary GetLoadParameterControlsDictionary(IEditingControl editingControl)
            => _getLoadParameterControlsDictionary(editingControl);
    }
}
