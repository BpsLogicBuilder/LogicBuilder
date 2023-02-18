using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories
{
    internal interface IFieldControlHelperFactory
    {
        ICreateRichInputBoxContextMenu GetCreateRichInputBoxContextMenu(IRichInputBoxValueControl richInputBoxValueControl);
        IEditObjectVariableHelper GetEditObjectVariableHelper(IObjectRichTextBoxValueControl objectRichTextBoxValueControl);
        IEditVariableHelper GetEditVariableHelper(IRichInputBoxValueControl richInputBoxValueControl);
        IObjectRichTextBoxEventsHelper GetObjectRichTextBoxEventsHelper(IObjectRichTextBoxValueControl objectRichTextBoxValueControl);
        IRichInputBoxEventsHelper GetRichInputBoxEventsHelper(IRichInputBoxValueControl richInputBoxValueControl);
        IUpdateObjectRichTextBoxXml GetUpdateObjectRichTextBoxXml(IObjectRichTextBoxValueControl objectRichTextBoxValueControl);
    }
}
