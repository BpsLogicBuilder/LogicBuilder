using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Components;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Commands
{
    internal class CutRichInputBoxTextCommand : ClickCommandBase
    {
        private readonly IRichInputBox richInputBox;

        public CutRichInputBoxTextCommand(
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

            richInputBox.SelectionProtected = false;
            richInputBox.InsertText(string.Empty);
        }
    }
}
