using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls
{
    internal interface ILiteralListItemValueControl : IValueControl
    {
        void DisableControls();
        void EnableControls();
        void SetAssignedToType(Type type);
    }
}
