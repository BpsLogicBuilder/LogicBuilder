using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.LiteralListItemEditor.Helpers;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.LiteralListItemEditor.Factories
{
    internal interface ILiteralListItemControlHelperFactory
    {
        ILiteralListItemRichInputBoxEventsHelper GetLiteralListItemRichInputBoxEventsHelper(IRichInputBoxValueControl richInputBoxValueControl);
    }
}
