using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Components;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Commands
{
    internal class DeleteRichInputBoxTextCommand : ClickCommandBase
    {
        private readonly RichInputBox richInputBox;

        public DeleteRichInputBoxTextCommand(
            IRichInputBoxValueControl richInputBoxValueControl)
        {
            richInputBox = richInputBoxValueControl.RichInputBox;
        }

        public override void Execute()
        {
            if (!richInputBox.IsSelectionEligibleForLink())
                return;

            richInputBox.SelectionProtected = false;
            richInputBox.InsertText(string.Empty);
        }
    }
}
