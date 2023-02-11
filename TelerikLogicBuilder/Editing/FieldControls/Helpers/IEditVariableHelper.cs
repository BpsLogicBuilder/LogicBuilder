using ABIS.LogicBuilder.FlowBuilder.Data;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal interface IEditVariableHelper
    {
        void Edit(Type assignedTo, VariableData? variableData = null);
    }
}
