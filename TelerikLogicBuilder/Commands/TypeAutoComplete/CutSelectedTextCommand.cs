using ABIS.LogicBuilder.FlowBuilder.Components.Helpers;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Commands.TypeAutoComplete
{
    internal class CutSelectedTextCommand : ClickCommandBase
    {
        private readonly ITypeAutoCompleteTextControl textControl;

        public CutSelectedTextCommand(
            ITypeAutoCompleteTextControl textControl)
        {
            this.textControl = textControl;
        }

        public override void Execute()
        {
            if (!textControl.Enabled)
                return;

            if (!string.IsNullOrEmpty(textControl.SelectedText))
            {
                Clipboard.SetText(textControl.SelectedText);
                textControl.SelectedText = string.Empty;
            }
        }
    }
}
