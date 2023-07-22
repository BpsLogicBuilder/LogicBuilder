using System;
using System.Drawing;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Components
{
    internal interface IObjectRichTextBox : IWin32Window
    {
        BorderStyle BorderStyle { get; set; }
        bool DetectUrls { get; set; }
        DockStyle Dock { set; }
        bool HideSelection { get; set; }
        Point Location { set; }
        Padding Margin { get; set; }
        bool Multiline { get; set; }
        string Name { get; set; }
        bool ReadOnly { get; set; }
        string Text { get; set; }
        bool Visible { get; set; }

        void Select();
        void SetDefaultFormat();
        void SetLinkFormat();

        event EventHandler? Disposed;
        event MouseEventHandler? MouseClick;
    }
}
