using ABIS.LogicBuilder.FlowBuilder.Data;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ObjectListItemEditor.Factories
{
    internal class ParameterObjectListItemEditorControlFactory : IParameterObjectListItemEditorControlFactory
    {
        private readonly Func<IEditingControl, ObjectListParameterElementInfo, IListOfObjectsParameterItemRichTextBoxControl> _getListOfObjectsParameterItemRichTextBoxControl;

        public ParameterObjectListItemEditorControlFactory(
            Func<IEditingControl, ObjectListParameterElementInfo, IListOfObjectsParameterItemRichTextBoxControl> getListOfObjectsParameterItemRichTextBoxControl)
        {
            _getListOfObjectsParameterItemRichTextBoxControl = getListOfObjectsParameterItemRichTextBoxControl;
        }

        public IListOfObjectsParameterItemRichTextBoxControl GetListOfObjectsParameterItemRichTextBoxControl(IEditingControl editingControl, ObjectListParameterElementInfo listInfo)
            => _getListOfObjectsParameterItemRichTextBoxControl(editingControl, listInfo);
    }
}
