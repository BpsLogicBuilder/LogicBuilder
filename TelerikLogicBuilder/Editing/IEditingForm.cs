using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal interface IEditingForm : IApplicationForm
    {
        IDictionary<string, string> ExpandedNodes { get; }
        void RequestDocumentUpdate();
    }
}
