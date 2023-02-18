using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Commands
{
    internal class EditObjectRichTextBoxConstructorCommand : ClickCommandBase
    {
        private readonly IObjectRichTextBoxValueControl objectRichTextBoxValueControl;

        public EditObjectRichTextBoxConstructorCommand(
            IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
        {
            this.objectRichTextBoxValueControl = objectRichTextBoxValueControl;
        }

        public override void Execute()
        {
            DisplayMessage.Show("EditObjectRichTextBoxConstructorCommand");
        }
    }
}
