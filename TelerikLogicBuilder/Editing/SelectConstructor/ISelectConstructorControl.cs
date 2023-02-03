using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor
{
    internal interface ISelectConstructorControl
    {
        event EventHandler? Changed;
        DockStyle Dock { set; }
        IDictionary<string, string> ExpandedNodes { get; }
        bool IsValid { get; }
        bool ItemSelected { get; }
        Point Location { set; }
        string? ConstructorName { get; }
        void ClearMessage();
        void RequestDocumentUpdate();
        void SetErrorMessage(string message);
        void SetMessage(string message, string title = "");
    }
}
