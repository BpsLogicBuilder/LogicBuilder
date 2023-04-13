using ABIS.LogicBuilder.FlowBuilder.Data;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ParameterControls.ObjectListItemControls.Factories
{
    internal interface IParameterObjectListItemEditorControlFactory
    {
        IListOfObjectsParameterItemRichTextBoxControl GetListOfObjectsParameterItemRichTextBoxControl(
            IEditingControl editingControl,
            ObjectListParameterElementInfo listInfo);
    }
}
