using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Commands
{
    internal class EditObjectListFormXmlCommand : ClickCommandBase
    {
        private readonly IEditObjectListForm editObjectListForm;

        public EditObjectListFormXmlCommand(IEditObjectListForm editObjectListForm)
        {
            this.editObjectListForm = editObjectListForm;
        }

        public override void Execute()
        {
            DisplayMessage.Show("EditObjectListFormXmlCommand");
        }
    }
}
