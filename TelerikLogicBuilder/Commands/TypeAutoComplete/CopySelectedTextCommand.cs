using ABIS.LogicBuilder.FlowBuilder.Components.Helpers;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Commands.TypeAutoComplete
{
    internal class CopySelectedTextCommand : ClickCommandBase
    {
        private readonly ITypeAutoCompleteTextControl textControl;

        public CopySelectedTextCommand(
            ITypeAutoCompleteTextControl textControl)
        {
            this.textControl = textControl;
        }

        public override void Execute()
        {
            if (!textControl.Enabled)
                return;

            if (!string.IsNullOrEmpty(textControl.SelectedText))
                Clipboard.SetText(textControl.SelectedText);
        }
    }
}
