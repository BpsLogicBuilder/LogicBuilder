using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal interface IEditingControlHelperFactory
    {
        ICreateRichInputBoxContextMenu GetCreateRichInputBoxContextMenu(IRichInputBoxValueControl richInputBoxValueControl);
        IEditFunctionControlHelper GetEditFunctionControlHelper(IEditFunctionControl editFunctionControl);
        ILoadParameterControlsDictionary GetLoadParameterControlsDictionary(IDataGraphEditingControl editingControl, IDataGraphEditingHost dataGraphEditingHost);
    }
}
