using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList
{
    internal interface IEditParameterLiteralListControl : IDataGraphEditingControl
    {
        IApplicationControl ApplicationControl { get; }
        ListParameterInputStyle ListControl { get; }
        Type LiteralType { get; }
        IRadListBoxManager<ILiteralListBoxItem> RadListBoxManager { get; }
        ILiteralListItemValueControl ValueControl { get; }
    }
}
