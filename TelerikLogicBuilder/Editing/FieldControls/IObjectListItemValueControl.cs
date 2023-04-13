using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls
{
    internal interface IObjectListItemValueControl : IValueControl
    {
        void DisableControls();
        void EnableControls();
        void SetAssignedToType(Type type);
    }
}
