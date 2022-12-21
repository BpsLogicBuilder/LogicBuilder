using ABIS.LogicBuilder.FlowBuilder.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Commands
{
    internal class ConfigureVariablesHelperCommand : ClickCommandBase
    {
        private readonly IConfigureVariablesForm configureVariablesForm;

        public ConfigureVariablesHelperCommand(IConfigureVariablesForm configureVariablesForm)
        {
            this.configureVariablesForm = configureVariablesForm;
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
