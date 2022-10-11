using System;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DialogFormMessageControlHelpers.Factories
{
    internal class DialogFormMessageCommandFactory : IDialogFormMessageCommandFactory
    {
        private readonly Func<RadLabel, CopyToClipboardCommand> _getCopyToClipboardCommand;
        private readonly Func<RadLabel, OpenInTextViewerCommand> _getOpenInTextViewerCommand;

        public DialogFormMessageCommandFactory(
            Func<RadLabel, CopyToClipboardCommand> getCopyToClipboardCommand,
            Func<RadLabel, OpenInTextViewerCommand> getOpenInTextViewerCommand)
        {
            _getCopyToClipboardCommand = getCopyToClipboardCommand;
            _getOpenInTextViewerCommand = getOpenInTextViewerCommand;
        }

        public CopyToClipboardCommand GetCopyToClipboardCommand(RadLabel messagesLabel)
            => _getCopyToClipboardCommand(messagesLabel);

        public OpenInTextViewerCommand GetOpenInTextViewerCommand(RadLabel messagesLabel)
            => _getOpenInTextViewerCommand(messagesLabel);
    }
}
