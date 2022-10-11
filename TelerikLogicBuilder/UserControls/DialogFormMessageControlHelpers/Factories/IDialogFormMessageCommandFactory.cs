using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DialogFormMessageControlHelpers.Factories
{
    internal interface IDialogFormMessageCommandFactory
    {
        CopyToClipboardCommand GetCopyToClipboardCommand(RadLabel messagesLabel);
        OpenInTextViewerCommand GetOpenInTextViewerCommand(RadLabel messagesLabel);
    }
}
