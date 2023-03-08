using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.ItemEditorControls
{
    internal interface IObjectListItemValueControl : IValueControl
    {
        void DisableControls();
        void EnableControls();
        void SetAssignedToType(Type type);
    }
}
