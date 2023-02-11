using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Commands
{
    internal class EditRichInputBoxFunctionCommand : ClickCommandBase
    {
        private readonly IRichInputBoxValueControl richInputBoxValueControl;

        public EditRichInputBoxFunctionCommand(
            IRichInputBoxValueControl richInputBoxValueControl)
        {
            this.richInputBoxValueControl = richInputBoxValueControl;
        }

        public override void Execute()
        {
            DisplayMessage.Show("EditRichInputBoxFunctionCommand");
        }
    }
}
