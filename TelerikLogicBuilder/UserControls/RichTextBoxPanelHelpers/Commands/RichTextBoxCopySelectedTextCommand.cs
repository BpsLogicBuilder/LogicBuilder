using ABIS.LogicBuilder.FlowBuilder.Commands;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.RichTextBoxPanelHelpers.Commands
{
    internal class RichTextBoxCopySelectedTextCommand : ClickCommandBase
    {
        private readonly RichTextBox richTextBox;

        public RichTextBoxCopySelectedTextCommand(RichTextBox richTextBox)
        {
            this.richTextBox = richTextBox;
        }

        public override void Execute()
        {
            if (!string.IsNullOrEmpty(richTextBox.SelectedText))
                Clipboard.SetText(richTextBox.SelectedText);
        }
    }
}
