using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Components
{
    internal interface IRichInputBox
    {
        BorderStyle BorderStyle { get; set; }
        bool DenySpecialCharacters { set; }
        bool DetectUrls { get; set; }
        DockStyle Dock { set; }
        bool HideSelection { get; set; }
        string[] Lines { get; set; }
        Point Location { set; }
        Padding Margin { get; set; }
        bool Modified { get; set; }
        bool Multiline { get; set; }
        string Name { get; set; }
        int TextLength { get; }
        bool ReadOnly { get; set; }
        string SelectedText { get; set; }
        int SelectionLength { get; set; }
        bool SelectionProtected { get; set; }
        int SelectionStart { get; set; }
        string Text { get; set; }
        bool Visible { get; set; }
        bool WordWrap { get; set; }

        void Clear();
        bool Focus();
        LinkBoundaries? GetBoundary(int position);
        int GetCharIndexFromPosition(Point pt);
        string GetHiddenLinkText(int position);
        string GetMixedXml();
        string GetVisibleText();
        void InsertLink(string text, string hyperlink, LinkType linkType);
        void InsertLink(string text, string hyperlink, int position, LinkType linkType);
        void InsertText(string text);
        void InsertText(string text, int position);
        bool IsSelectionEligibleForLink();
        bool LinkInSelection();
        void Select();
        void Select(int start, int length);
        void SelectAll();

        event EventHandler? Disposed;
        event KeyEventHandler? KeyUp;
        event MouseEventHandler? MouseClick;
        event MouseEventHandler? MouseUp;
        event EventHandler? TextChanged;
        event EventHandler? Validated;
    }
}
