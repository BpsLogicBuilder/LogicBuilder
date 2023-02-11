using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Components;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Commands
{
    internal class CopyRichInputBoxTextCommand : ClickCommandBase
    {
        private readonly RichInputBox richInputBox;

        public CopyRichInputBoxTextCommand(
            IRichInputBoxValueControl richInputBoxValueControl)
        {
            richInputBox = richInputBoxValueControl.RichInputBox;
        }

        public override void Execute()
        {
            if (richInputBox.LinkInSelection())
                return;
            if (!richInputBox.IsSelectionEligibleForLink())
                return;

            if (!string.IsNullOrEmpty(richInputBox.SelectedText))
                Clipboard.SetText(richInputBox.SelectedText);
        }
    }
}
