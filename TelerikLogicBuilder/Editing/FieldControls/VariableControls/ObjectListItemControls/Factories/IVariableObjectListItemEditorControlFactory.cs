using ABIS.LogicBuilder.FlowBuilder.Data;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls.ObjectListItemControls.Factories
{
    internal interface IVariableObjectListItemEditorControlFactory
    {
        IListOfObjectsVariableItemRichTextBoxControl GetListOfObjectsVariableItemRichTextBoxControl(
            IEditingControl editingControl,
            ObjectListVariableElementInfo listInfo);
    }
}
