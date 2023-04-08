using ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Helpers
{
    internal class DataGraphEditingHostEventsHelper : IDataGraphEditingHostEventsHelper
    {
        private readonly IDataGraphEditingManager _dataGraphEditingManager;
        private readonly IDataGraphEditingHost dataGraphEditingHost;

        public DataGraphEditingHostEventsHelper(
            IEditingFormHelperFactory editingFormHelperFactory,
            IDataGraphEditingHost dataGraphEditingHost)
        {
            _dataGraphEditingManager = editingFormHelperFactory.GetDataGraphEditingManager(dataGraphEditingHost);
            this.dataGraphEditingHost = dataGraphEditingHost;
        }

        private RadTreeView TreeView => dataGraphEditingHost.TreeView;

        public void RequestDocumentUpdate(IEditingControl editingControl)
        {
            TreeView.SelectedNodeChanged -= TreeView_SelectedNodeChanged;
            _dataGraphEditingManager.RequestDocumentUpdate(editingControl);
            TreeView.SelectedNodeChanged += TreeView_SelectedNodeChanged;
        }

        public void Setup()
        {
            TreeView.MouseDown += TreeView_MouseDown;
            TreeView.NodeExpandedChanged += TreeView_NodeExpandedChanged;
            TreeView.NodeExpandedChanging += TreeView_NodeExpandedChanging;
            TreeView.NodeMouseClick += TreeView_NodeMouseClick;
            TreeView.NodeMouseDoubleClick += TreeView_NodeMouseDoubleClick;
            TreeView.SelectedNodeChanged += TreeView_SelectedNodeChanged;
            TreeView.SelectedNodeChanging += TreeView_SelectedNodeChanging;
            _dataGraphEditingManager.CreateContextMenus();
        }

        #region Event Handlers
        private void TreeView_MouseDown(object? sender, System.Windows.Forms.MouseEventArgs e)
        {
            RadTreeNode treeNode = this.TreeView.GetNodeAt(e.Location);
            if (treeNode == null && this.TreeView.Nodes.Count > 0)
            {
                //TreeView.SelectedNode = TreeView.Nodes[0];
                //SetContextMenuState
            }
        }

        private void TreeView_NodeExpandedChanged(object sender, RadTreeViewEventArgs e)
        {
            if (e.Node.Expanded)
            {
                if (!dataGraphEditingHost.ExpandedNodes.ContainsKey(e.Node.Name))
                    dataGraphEditingHost.ExpandedNodes.Add(e.Node.Name, e.Node.Text);
            }
            else
            {
                if (dataGraphEditingHost.ExpandedNodes.ContainsKey(e.Node.Name))
                    dataGraphEditingHost.ExpandedNodes.Remove(e.Node.Name);
            }
        }

        private void TreeView_NodeExpandedChanging(object sender, RadTreeViewCancelEventArgs e)
        {
            if (!e.Node.Expanded)
            {
                if (TreeView.SelectedNode == null && e.Action == RadTreeViewAction.ByMouse)//Select the expanding node if it is null
                    TreeView.SelectedNode = e.Node;//Prevents treeview1.Nodes[0] from being selected instead
            }
        }

        private void TreeView_NodeMouseClick(object sender, RadTreeViewEventArgs e)
        {
            //TreeView.SelectedNode = e.Node;
            //Setting the selected node does not seem to make a difference for the RadTreeView
            //Expanding without selection and right-click both show the same behavior irrespective of this statement (TreeView.SelectedNode = e.Node;)
            _dataGraphEditingManager.SetContextMenuState((ParametersDataTreeNode)e.Node);
        }

        private void TreeView_NodeMouseDoubleClick(object sender, RadTreeViewEventArgs e)
        {
            //treeView1.SelectedNode = null;//takes focus off the treeview and allows a text box 
            TreeView.SelectedNode = e.Node;//in the right panel to be selected when AfterSelect runs.
        }

        private void TreeView_SelectedNodeChanged(object sender, RadTreeViewEventArgs e)
        {
            if (TreeView.SelectedNode == null)
            {
                //this.UnLockWindowUpdate();
                return;
            }

            _dataGraphEditingManager.SetControlValues((ParametersDataTreeNode)e.Node);
        }

        private void TreeView_SelectedNodeChanging(object sender, RadTreeViewCancelEventArgs e)
        {
            try
            {
                if (TreeView.SelectedNode == null
                    || e.Node == null)//Don't update if e.Node is null because
                    return;             //1) The selected node may have been deleted
                                        //2) There is no navigation (i.e. e.Node == null)

                dataGraphEditingHost.ClearMessage();
                _dataGraphEditingManager.UpdateXmlDocument(TreeView.SelectedNode);
            }
            catch (XmlException ex)
            {
                e.Cancel = true;
                dataGraphEditingHost.SetErrorMessage(ex.Message);
            }
            catch (LogicBuilderException ex)
            {
                e.Cancel = true;
                dataGraphEditingHost.SetErrorMessage(ex.Message);
            }
        }
        #endregion Event Handlers
    }
}
