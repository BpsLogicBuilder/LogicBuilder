using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunction.Commands
{
    internal class ConfigureFunctionReturnTypeCommand : ClickCommandBase
    {
        private readonly IConfigureFunctionControl configureFunctionControl;

        public ConfigureFunctionReturnTypeCommand(
            IConfigureFunctionControl configureFunctionControl)
        {
            this.configureFunctionControl = configureFunctionControl;
        }

        public override void Execute()
        {
            DisplayMessage.Show("ConfigureFunctionReturnTypeCommand");
        }
    }
}
