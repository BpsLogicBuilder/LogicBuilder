using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Forms.Commands
{
    internal class UpdateExcludedModulesCommand : ClickCommandBase
    {
        private readonly IGetAllCheckedNodes _getAllCheckedNodes;
        private readonly IConfigureExcludedModules configureExcludedModules;


        public UpdateExcludedModulesCommand(
            IGetAllCheckedNodes getAllCheckedNodes,
            IConfigureExcludedModules configureExcludedModules)
        {
            _getAllCheckedNodes = getAllCheckedNodes;
            this.configureExcludedModules = configureExcludedModules;
        }

        public override void Execute()
        {
            configureExcludedModules.ListControl.Items.Clear();
            configureExcludedModules.ListControl.Items.AddRange
            (
                _getAllCheckedNodes
                    .GetNodes(configureExcludedModules.TreeView.Nodes[0])
                    .Select(n => n.Text)
                    .OrderBy(n => n)
                    .ToArray()
            );
            configureExcludedModules.OkButton.Enabled = true;
        }
    }
}
