using ABIS.LogicBuilder.FlowBuilder.Data;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal interface IEditObjectVariableHelper
    {
        void Edit(Type assignedTo, VariableData? variableData = null);
    }
}
