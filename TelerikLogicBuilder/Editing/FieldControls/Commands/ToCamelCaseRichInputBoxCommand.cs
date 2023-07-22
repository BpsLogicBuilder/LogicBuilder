using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Commands
{
    internal class ToCamelCaseRichInputBoxCommand : ClickCommandBase
    {
        private readonly IStringHelper _stringHelper;
        private readonly IRichInputBox richInputBox;

        public ToCamelCaseRichInputBoxCommand(
            IStringHelper stringHelper,
            IRichInputBoxValueControl richInputBoxValueControl)
        {
            _stringHelper = stringHelper;
            richInputBox = richInputBoxValueControl.RichInputBox;
        }

        public override void Execute()
        {
            richInputBox.SelectAll();

            if (richInputBox.LinkInSelection() || !richInputBox.IsSelectionEligibleForLink())
            {
                richInputBox.Select(0, 0);
                return;
            }

            richInputBox.Text = _stringHelper.ToCamelCase(richInputBox.Text);

            richInputBox.Select(0, 0);
        }
    }
}
