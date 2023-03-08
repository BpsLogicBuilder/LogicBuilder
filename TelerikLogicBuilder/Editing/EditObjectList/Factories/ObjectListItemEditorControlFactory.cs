using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.ItemEditorControls;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories
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
