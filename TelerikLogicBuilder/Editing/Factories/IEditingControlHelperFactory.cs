using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal interface IEditingControlHelperFactory
    {
        IEditFunctionControlHelper GetEditFunctionControlHelper(IEditFunctionControl editFunctionControl);
        ILoadParameterControlsDictionary GetLoadParameterControlsDictionary(IDataGraphEditingControl editingControl, IEditingForm editingForm);
        IRichInputBoxEventsHelper GetRichInputBoxEventsHelper(IRichInputBoxValueControl richInputBoxValueControl);
    }
}
