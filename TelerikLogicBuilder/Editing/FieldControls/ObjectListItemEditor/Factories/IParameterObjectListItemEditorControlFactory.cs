using ABIS.LogicBuilder.FlowBuilder.Data;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ObjectListItemEditor.Factories
{
    internal interface IParameterObjectListItemEditorControlFactory
    {
        IListOfObjectsItemRichTextBoxControl GetListOfObjectsItemRichTextBoxControl(
            IEditingControl editingControl,
            ObjectListParameterElementInfo listInfo);
    }
}
