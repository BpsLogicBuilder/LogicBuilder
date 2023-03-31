using ABIS.LogicBuilder.FlowBuilder.Commands;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.RichTextBoxPanelHelpers.Commands
{
    internal class RichTextBoxCutSelectedTextCommand : ClickCommandBase
    {
        private readonly RichTextBox richTextBox;

        public RichTextBoxCutSelectedTextCommand(RichTextBox richTextBox)
        {
            this.richTextBox = richTextBox;
        }

        public override void Execute()
        {
            if (!string.IsNullOrEmpty(richTextBox.SelectedText))
            {
                Clipboard.SetText(richTextBox.SelectedText);
                richTextBox.SelectedText = string.Empty;
            }
        }
    }
}
