using ABIS.LogicBuilder.FlowBuilder.UserControls.RichTextBoxPanelHelpers.Commands;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.RichTextBoxPanelHelpers.Factories
{
    internal class RichTextBoxPanelCommandFactory : IRichTextBoxPanelCommandFactory
    {
        private readonly Func<RichTextBox, RichTextBoxCopySelectedTextCommand> _getRichTextBoxCopySelectedTextCommand;
        private readonly Func<RichTextBox, RichTextBoxCutSelectedTextCommand> _getRichTextBoxCutSelectedTextCommand;
        private readonly Func<RichTextBox, RichTextBoxPasteTextCommand> _getRichTextBoxPasteTextCommand;
        private readonly Func<RichTextBox, RichTextBoxSelectAllCommand> _getRichTextBoxSelectAllCommand;
        private readonly Func<RichTextBox, SelectFragmentCommand> _getSelectFragmentCommand;

        public RichTextBoxPanelCommandFactory(
            Func<RichTextBox, RichTextBoxCopySelectedTextCommand> getRichTextBoxCopySelectedTextCommand,
            Func<RichTextBox, RichTextBoxCutSelectedTextCommand> getRichTextBoxCutSelectedTextCommand,
            Func<RichTextBox, RichTextBoxPasteTextCommand> getRichTextBoxPasteTextCommand,
            Func<RichTextBox, RichTextBoxSelectAllCommand> getRichTextBoxSelectAllCommand,
            Func<RichTextBox, SelectFragmentCommand> getSelectFragmentCommand)
        {
            _getRichTextBoxCopySelectedTextCommand = getRichTextBoxCopySelectedTextCommand;
            _getRichTextBoxCutSelectedTextCommand = getRichTextBoxCutSelectedTextCommand;
            _getRichTextBoxPasteTextCommand = getRichTextBoxPasteTextCommand;
            _getRichTextBoxSelectAllCommand = getRichTextBoxSelectAllCommand;
            _getSelectFragmentCommand = getSelectFragmentCommand;
        }

        public RichTextBoxCopySelectedTextCommand GetRichTextBoxCopySelectedTextCommand(RichTextBox richTextBox)
            => _getRichTextBoxCopySelectedTextCommand(richTextBox);

        public RichTextBoxCutSelectedTextCommand GetRichTextBoxCutSelectedTextCommand(RichTextBox richTextBox)
            => _getRichTextBoxCutSelectedTextCommand(richTextBox);

        public RichTextBoxPasteTextCommand GetRichTextBoxPasteTextCommand(RichTextBox richTextBox)
            => _getRichTextBoxPasteTextCommand(richTextBox);

        public RichTextBoxSelectAllCommand GetRichTextBoxSelectAllCommand(RichTextBox richTextBox)
            => _getRichTextBoxSelectAllCommand(richTextBox);

        public SelectFragmentCommand GetSelectFragmentCommand(RichTextBox richTextBox)
            => _getSelectFragmentCommand(richTextBox);
    }
}
