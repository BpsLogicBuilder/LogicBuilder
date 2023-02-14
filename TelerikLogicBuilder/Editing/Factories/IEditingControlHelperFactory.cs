using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal interface IEditingControlHelperFactory
    {
        ILoadParameterControlsDictionary GetLoadParameterControlsDictionary(IEditingControl editingControl, IEditingForm editingForm);
    }
}
