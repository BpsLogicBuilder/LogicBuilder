using System;
using System.Drawing;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Components
{
    internal interface IObjectRichTextBox : IWin32Window
    {
        DockStyle Dock { set; }
        Point Location { set; }

        string Text { get; set; }
        BorderStyle BorderStyle { get; set; }
        Padding Margin { get; set; }
        string Name { get; set; }

        bool DetectUrls { get; set; }
        bool HideSelection { get; set; }
        bool Multiline { get; set; }
        bool ReadOnly { get; set; }
        bool Visible { get; set; }

        void Select();
        void SetDefaultFormat();
        void SetLinkFormat();

        event EventHandler? Disposed;
        event MouseEventHandler? MouseClick;
    }
}
