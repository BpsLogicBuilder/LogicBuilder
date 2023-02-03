using ABIS.LogicBuilder.FlowBuilder.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction
{
    internal interface ISelectFunctionControl
    {
        event EventHandler? Changed;
        DockStyle Dock { set; }
        IDictionary<string, string> ExpandedNodes { get; }
        string? FunctionName { get; }
        bool IsValid { get; }
        bool ItemSelected { get; }
        Point Location { set; }
        IList<TreeFolder> TreeFolders { get; }
        void ClearMessage();
        void RequestDocumentUpdate();
        void SetErrorMessage(string message);
        void SetMessage(string message, string title = "");
    }
}
