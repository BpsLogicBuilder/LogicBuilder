using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
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
        IDictionary<string, Function> FunctionDictionary { get; }
        IList<TreeFolder> TreeFolders { get; }
        void ClearMessage();
        void SetErrorMessage(string message);
        void SetFunction(string functionName);
        void SetMessage(string message, string title = "");
    }
}
