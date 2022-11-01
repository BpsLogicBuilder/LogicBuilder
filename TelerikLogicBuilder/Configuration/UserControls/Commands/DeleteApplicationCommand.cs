using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls.Commands
{
    internal class DeleteApplicationCommand : ClickCommandBase
    {
        private readonly IProjectPropertiesXmlTreeViewSynchronizer _projectPropertiesXmlTreeViewSynchronizer;
        private readonly ITreeViewService _treeViewService;

        private readonly IConfigureProjectProperties configureProjectProperties;

        public DeleteApplicationCommand(
            ITreeViewService treeViewService,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureProjectProperties configureProjectProperties)
        {
            _treeViewService = treeViewService;

            this.configureProjectProperties = configureProjectProperties;

            _projectPropertiesXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetProjectPropertiesXmlTreeViewSynchronizer
            (
                this.configureProjectProperties
            );
        }

        public override void Execute()
        {
            try
            {
                RadTreeNode selecteNode = this.configureProjectProperties.TreeView.SelectedNode;
                if (selecteNode == null)
                    return;

                if (_treeViewService.IsApplicationNode(selecteNode) == false)
                    return;

                _projectPropertiesXmlTreeViewSynchronizer.DeleteNode(selecteNode);
            }
            catch (LogicBuilderException ex)
            {
                this.configureProjectProperties.SetErrorMessage(ex.Message);
            }
        }
    }
}
