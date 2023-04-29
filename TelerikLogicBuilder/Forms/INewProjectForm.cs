using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Forms
{
    internal interface INewProjectForm : IDisposable
    {
        DialogResult DialogResult { get; }
        string ProjectName { get; }
        string ProjectPath { get; }
        HelperButtonTextBox TxtProjectPath { get; }
        DialogResult ShowDialog(IWin32Window owner);
    }
}
