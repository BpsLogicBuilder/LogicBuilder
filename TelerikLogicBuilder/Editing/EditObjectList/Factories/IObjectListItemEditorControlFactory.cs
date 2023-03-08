using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.ItemEditorControls;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories
{
    internal interface IObjectListItemEditorControlFactory
    {
        IListOfObjectsItemRichTextBoxControl GetListOfObjectsItemRichTextBoxControl(
            IEditingControl editingControl,
            ObjectListParameterElementInfo listInfo);
    }
}
