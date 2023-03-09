﻿using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.ItemEditorControls;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList
{
    internal interface IEditObjectListControl : IEditingControl
    {
        IApplicationForm ApplicationForm { get; }
        ListParameterInputStyle ListControl { get; }
        Type ObjectType { get; }
        IRadListBoxManager<IObjectListBoxItem> RadListBoxManager { get; }
        IObjectListItemValueControl ValueControl { get; }
    }
}