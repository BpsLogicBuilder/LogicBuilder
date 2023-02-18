using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Commands
{
    internal class EditObjectRichTextBoxObjectListCommand : ClickCommandBase
    {
        private readonly IObjectRichTextBoxValueControl objectRichTextBoxValueControl;

        public EditObjectRichTextBoxObjectListCommand(
            IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
        {
            this.objectRichTextBoxValueControl = objectRichTextBoxValueControl;
        }

        public override void Execute()
        {
            DisplayMessage.Show("EditObjectRichTextBoxObjectListCommand");
        }
    }
}
