using ABIS.LogicBuilder.FlowBuilder.Data;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ObjectListItemEditor.Factories
{
    internal class ParameterObjectListItemEditorControlFactory : IParameterObjectListItemEditorControlFactory
    {
        private readonly Func<IEditingControl, ObjectListParameterElementInfo, IListOfObjectsItemRichTextBoxControl> _getListOfObjectsItemRichTextBoxControl;

        public ParameterObjectListItemEditorControlFactory(
            Func<IEditingControl, ObjectListParameterElementInfo, IListOfObjectsItemRichTextBoxControl> getListOfObjectsItemRichTextBoxControl)
        {
            _getListOfObjectsItemRichTextBoxControl = getListOfObjectsItemRichTextBoxControl;
        }

        public IListOfObjectsItemRichTextBoxControl GetListOfObjectsItemRichTextBoxControl(IEditingControl editingControl, ObjectListParameterElementInfo listInfo)
            => _getListOfObjectsItemRichTextBoxControl(editingControl, listInfo);
    }
}
