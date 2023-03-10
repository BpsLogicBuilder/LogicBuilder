using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.ItemEditorControls.Helpers;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.ItemEditorControls.Factories
{
    internal interface ILiteralListItemControlHelperFactory
    {
        ILiteralListItemRichInputBoxEventsHelper GetLiteralListItemRichInputBoxEventsHelper(IRichInputBoxValueControl richInputBoxValueControl);
    }
}
