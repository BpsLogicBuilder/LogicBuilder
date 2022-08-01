using ABIS.LogicBuilder.FlowBuilder.Enums;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal interface IMessages
    {
        MessageTab SelectedMessageTab { set; }
        DockStyle Dock { set; }
        bool Visible { set; }
        void Clear(MessageTab messageTab);
        void GoToNextEmptyLine(MessageTab messageTab);
        void InsertLink(string text, string hyperlink, LinkType linkType, MessageTab messageTab);
        void InsertLink(string text, string hyperlink, int position, LinkType linkType, MessageTab messageTab);
        void InsertText(string text, MessageTab messageTab);
        void InsertText(string text, int position, MessageTab messageTab);
        void Select(int start, int length, MessageTab messageTab);
    }
}
