using System;
using System.Drawing;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal interface IRichTextBoxPanel
    {
        DockStyle Dock { set; }
        Point Location { set; }

        string[] Lines { get; set; }
        RichTextBox RichTextBox { get; }
        string Text { get; set; }

        event EventHandler? TextChanged;
    }
}
