using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Commands
{
    internal class EditObjectRichTextBoxFunctionCommand : ClickCommandBase
    {
        private readonly IObjectRichTextBoxValueControl objectRichTextBoxValueControl;

        public EditObjectRichTextBoxFunctionCommand(
            IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
        {
            this.objectRichTextBoxValueControl = objectRichTextBoxValueControl;
        }

        public override void Execute()
        {
            DisplayMessage.Show("EditObjectRichTextBoxFunctionCommand");
        }
    }
}
