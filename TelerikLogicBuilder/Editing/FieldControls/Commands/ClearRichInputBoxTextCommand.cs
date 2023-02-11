using ABIS.LogicBuilder.FlowBuilder.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Commands
{
    internal class ClearRichInputBoxTextCommand : ClickCommandBase
    {
        private readonly IRichInputBoxValueControl richInputBoxValueControl;

        public ClearRichInputBoxTextCommand(
            IRichInputBoxValueControl richInputBoxValueControl)
        {
            this.richInputBoxValueControl = richInputBoxValueControl;
        }

        public override void Execute()
        {
            this.richInputBoxValueControl.RichInputBox.Clear();
        }
    }
}
