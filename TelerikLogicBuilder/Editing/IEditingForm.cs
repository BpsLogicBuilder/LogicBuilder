using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal interface IEditingForm : IApplicationForm
    {
        Type AssignedTo { get; }
        IDictionary<string, string> ExpandedNodes { get; }
        void RequestDocumentUpdate(IEditingControl editingControl);
    }
}
