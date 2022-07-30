using ABIS.LogicBuilder.FlowBuilder.Commands;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DialogFormMessageControlHelpers
{
    internal class CopyToClipboardCommand : ClickCommandBase
    {
        private readonly RadLabel _radLabelMessages;

        public CopyToClipboardCommand(RadLabel radLabelMessages)
        {
            _radLabelMessages = radLabelMessages;
        }

        public override void Execute()
        {
            if (string.IsNullOrEmpty(_radLabelMessages.Text))
                return;

            Clipboard.SetDataObject(_radLabelMessages.Text);
        }
    }
}
