using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.ItemEditorControls
{
    internal interface ILiteralListItemValueControl : IValueControl
    {
        void DisableControls();
        void EnableControls();
        void SetAssignedToType(Type type);
    }
}
