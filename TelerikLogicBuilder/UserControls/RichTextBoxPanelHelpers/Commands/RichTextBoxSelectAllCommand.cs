using ABIS.LogicBuilder.FlowBuilder.Commands;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.RichTextBoxPanelHelpers.Commands
{
    internal class RichTextBoxSelectAllCommand : ClickCommandBase
    {
        private readonly RichTextBox richTextBox;

        public RichTextBoxSelectAllCommand(RichTextBox richTextBox)
        {
            this.richTextBox = richTextBox;
        }

        public override void Execute()
        {
            if (!richTextBox.Enabled)
                return;

            richTextBox.SelectAll();
        }
    }
}
