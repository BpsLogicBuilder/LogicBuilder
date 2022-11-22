using ABIS.LogicBuilder.FlowBuilder.Components.Helpers;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Commands.TypeAutoComplete
{
    internal class PasteTextCommand : ClickCommandBase
    {
        private readonly ITypeAutoCompleteTextControl textControl;

        public PasteTextCommand(
            ITypeAutoCompleteTextControl textControl)
        {
            this.textControl = textControl;
        }

        public override void Execute()
        {
            if (!textControl.Enabled)
                return;

            string text = Clipboard.GetText();
            if (text != null)
                textControl.SelectedText = text;
        }
    }
}
