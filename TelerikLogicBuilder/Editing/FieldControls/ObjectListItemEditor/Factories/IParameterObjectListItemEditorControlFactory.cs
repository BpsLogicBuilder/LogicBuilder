using ABIS.LogicBuilder.FlowBuilder.Data;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ObjectListItemEditor.Factories
{
    internal interface IParameterObjectListItemEditorControlFactory
    {
        IListOfObjectsParameterItemRichTextBoxControl GetListOfObjectsParameterItemRichTextBoxControl(
            IEditingControl editingControl,
            ObjectListParameterElementInfo listInfo);
    }
}
