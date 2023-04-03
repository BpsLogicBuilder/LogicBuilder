using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal interface IRichInputBoxEventsHelper
    {
        void RichInputBox_KeyUp(object? sender, KeyEventArgs e);

        void RichInputBox_MouseClick(object? sender, MouseEventArgs e);

        void RichInputBox_MouseUp(object? sender, MouseEventArgs e);

        void RichInputBox_TextChanged(object? sender, EventArgs e);
    }
}
