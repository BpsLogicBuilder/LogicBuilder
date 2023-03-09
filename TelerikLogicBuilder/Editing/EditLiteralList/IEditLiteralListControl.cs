using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.ItemEditorControls;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList
{
    internal interface IEditLiteralListControl : IDataGraphEditingControl
    {
        IApplicationForm ApplicationForm { get; }
        ListParameterInputStyle ListControl { get; }
        Type LiteralType { get; }
        IRadListBoxManager<ILiteralListBoxItem> RadListBoxManager { get; }
        ILiteralListItemValueControl ValueControl { get; }
    }
}
