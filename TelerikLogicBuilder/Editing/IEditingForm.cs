using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal interface IEditingForm : IApplicationForm
    {
        bool DenySpecialCharacters { get; }
        bool DisplayNotCheckBox { get; }
        IDictionary<string, string> ExpandedNodes { get; }
        void RequestDocumentUpdate();
    }
}
