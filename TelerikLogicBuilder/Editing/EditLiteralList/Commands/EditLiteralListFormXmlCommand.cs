using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Commands
{
    internal class EditLiteralListFormXmlCommand : ClickCommandBase
    {
        private readonly IEditLiteralListForm editLiteralListForm;

        public EditLiteralListFormXmlCommand(IEditLiteralListForm editLiteralListForm)
        {
            this.editLiteralListForm = editLiteralListForm;
        }

        public override void Execute()
        {
            DisplayMessage.Show("EditLiteralListFormXmlCommand");
        }
    }
}
