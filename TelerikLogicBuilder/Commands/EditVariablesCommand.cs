using ABIS.LogicBuilder.FlowBuilder.Prompts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class EditVariablesCommand : ClickCommandBase
    {
        public override void Execute()
        {
            DisplayMessage.Show("EditVariablesCommand");
        }
    }
}
