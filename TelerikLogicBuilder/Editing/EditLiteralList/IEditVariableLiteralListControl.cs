using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList
{
    internal interface IEditVariableLiteralListControl : IDataGraphEditingControl
    {
        IApplicationControl ApplicationControl { get; }
        ListVariableInputStyle ListControl { get; }
        Type LiteralType { get; }
        IRadListBoxManager<ILiteralListBoxItem> RadListBoxManager { get; }
        ILiteralListItemValueControl ValueControl { get; }
    }
}
