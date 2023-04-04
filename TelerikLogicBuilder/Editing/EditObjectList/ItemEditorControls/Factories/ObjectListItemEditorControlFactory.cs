using ABIS.LogicBuilder.FlowBuilder.Data;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.ItemEditorControls.Factories
{
    internal class ObjectListItemEditorControlFactory : IObjectListItemEditorControlFactory
    {
        private readonly Func<IEditingControl, ObjectListParameterElementInfo, IListOfObjectsItemRichTextBoxControl> _getListOfObjectsItemRichTextBoxControl;

        public ObjectListItemEditorControlFactory(
            Func<IEditingControl, ObjectListParameterElementInfo, IListOfObjectsItemRichTextBoxControl> getListOfObjectsItemRichTextBoxControl)
        {
            _getListOfObjectsItemRichTextBoxControl = getListOfObjectsItemRichTextBoxControl;
        }

        public IListOfObjectsItemRichTextBoxControl GetListOfObjectsItemRichTextBoxControl(IEditingControl editingControl, ObjectListParameterElementInfo listInfo)
            => _getListOfObjectsItemRichTextBoxControl(editingControl, listInfo);
    }
}
