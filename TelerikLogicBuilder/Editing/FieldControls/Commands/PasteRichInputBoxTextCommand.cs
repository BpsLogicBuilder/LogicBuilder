using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Components;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Commands
{
    internal class PasteRichInputBoxTextCommand : ClickCommandBase
    {
        private readonly RichInputBox richInputBox;

        public PasteRichInputBoxTextCommand(
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

            richInputBox.SelectionProtected = false;
            string text = Clipboard.GetText();
            if (text != null)
                richInputBox.InsertText(text);
        }
    }
}
