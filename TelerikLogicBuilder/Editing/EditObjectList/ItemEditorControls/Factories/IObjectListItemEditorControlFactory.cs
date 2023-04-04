using ABIS.LogicBuilder.FlowBuilder.Data;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.ItemEditorControls.Factories
{
    internal interface IObjectListItemEditorControlFactory
    {
        IListOfObjectsItemRichTextBoxControl GetListOfObjectsItemRichTextBoxControl(
            IEditingControl editingControl,
            ObjectListParameterElementInfo listInfo);
    }
}
