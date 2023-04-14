using ABIS.LogicBuilder.FlowBuilder.Data;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls.ObjectListItemControls.Factories
{
    internal class VariableObjectListItemEditorControlFactory : IVariableObjectListItemEditorControlFactory
    {
        private readonly Func<IEditingControl, ObjectListVariableElementInfo, IListOfObjectsVariableItemRichTextBoxControl> _getListOfObjectsVariableItemRichTextBoxControl;

        public VariableObjectListItemEditorControlFactory(
            Func<IEditingControl, ObjectListVariableElementInfo, IListOfObjectsVariableItemRichTextBoxControl> getListOfObjectsVariableItemRichTextBoxControl)
        {
            _getListOfObjectsVariableItemRichTextBoxControl = getListOfObjectsVariableItemRichTextBoxControl;
        }

        public IListOfObjectsVariableItemRichTextBoxControl GetListOfObjectsVariableItemRichTextBoxControl(IEditingControl editingControl, ObjectListVariableElementInfo listInfo)
            => _getListOfObjectsVariableItemRichTextBoxControl(editingControl, listInfo);
    }
}
