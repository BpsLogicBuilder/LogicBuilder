using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.ItemEditorControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.ItemEditorControls.Factories
{
    internal interface ILiteralListItemControlHelperFactory
    {
        ILiteralListItemRichInputBoxEventsHelper GetLiteralListItemRichInputBoxEventsHelper(IRichInputBoxValueControl richInputBoxValueControl);
    }
}
