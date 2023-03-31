using ABIS.LogicBuilder.FlowBuilder.Commands;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.RichTextBoxPanelHelpers.Commands
{
    internal class RichTextBoxPasteTextCommand : ClickCommandBase
    {
        private readonly RichTextBox richTextBox;

        public RichTextBoxPasteTextCommand(RichTextBox richTextBox)
        {
            this.richTextBox = richTextBox;
        }

        public override void Execute()
        {
            if (!richTextBox.Enabled)
                return;

            string text = Clipboard.GetText();
            if (text != null)
                richTextBox.SelectedText = text;
        }
    }
}
