using ABIS.LogicBuilder.FlowBuilder.UserControls.RichTextBoxPanelHelpers.Commands;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.RichTextBoxPanelHelpers.Factories
{
    internal interface IRichTextBoxPanelCommandFactory
    {
        RichTextBoxCopySelectedTextCommand GetRichTextBoxCopySelectedTextCommand(RichTextBox richTextBox);
        RichTextBoxCutSelectedTextCommand GetRichTextBoxCutSelectedTextCommand(RichTextBox richTextBox);
        RichTextBoxPasteTextCommand GetRichTextBoxPasteTextCommand(RichTextBox richTextBox);
        RichTextBoxSelectAllCommand GetRichTextBoxSelectAllCommand(RichTextBox richTextBox);
        SelectFragmentCommand GetSelectFragmentCommand(RichTextBox richTextBox);
    }
}
