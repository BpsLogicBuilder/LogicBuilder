using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
{
    internal partial class SelectDocumentsForm : RadForm, ISelectDocumentsForm
    {
        private readonly IFormInitializer _formInitializer;
        private readonly IGetAllCheckedNodes _getAllCheckedNodeNames;
        private readonly ISelectDocunentsTreeViewBuilder _selectDocunentsTreeViewBuilder;
        private readonly ITreeViewService _treeViewService;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;

        public SelectDocumentsForm(IFormInitializer formInitializer,
                                   IGetAllCheckedNodes getAllCheckedNodeNames,
                                   ISelectDocunentsTreeViewBuilder selectDocunentsTreeViewBuilder,
                                   ITreeViewService treeViewService,
                                   IDialogFormMessageControl dialogFormMessageControl)
        {
            _formInitializer = formInitializer;
            _getAllCheckedNodeNames = getAllCheckedNodeNames;
            _selectDocunentsTreeViewBuilder = selectDocunentsTreeViewBuilder;
            _treeViewService = treeViewService;
            _dialogFormMessageControl = dialogFormMessageControl;
            InitializeComponent();
            Initialize();
        }

        public IList<string> SourceFiles => _getAllCheckedNodeNames.GetNames(radTreeView.Nodes[0]);

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            InitializeDialogFormMessageControl();
            _formInitializer.SetFormDefaults(this, 648);
            _selectDocunentsTreeViewBuilder.Build(radTreeView);

            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
            CollapsePanelBorder(radPanelBottom);
            CollapsePanelBorder(radPanelButtons);
            CollapsePanelBorder(radPanelMessages);
            CollapsePanelBorder(radPanelTop);
        }

        private void InitializeDialogFormMessageControl()
        {
            ((ISupportInitialize)(this.radPanelMessages)).BeginInit();
            this.radPanelMessages.SuspendLayout();

            _dialogFormMessageControl.Dock = DockStyle.Fill;
            _dialogFormMessageControl.Location = new Point(0, 0);
            this.radPanelMessages.Controls.Add((Control)_dialogFormMessageControl);

            ((ISupportInitialize)(this.radPanelMessages)).EndInit();
            this.radPanelMessages.ResumeLayout(true);
        }

        private void RadTreeView_NodeExpandedChanged(object sender, Telerik.WinControls.UI.RadTreeViewEventArgs e)
        {
            if (_treeViewService.IsRootNode(e.Node)
                || _treeViewService.IsFileNode(e.Node))/*NodeExpandedChanged runs for file nodes on double click*/
                return;

            e.Node.ImageIndex = e.Node.Expanded
                ? ImageIndexes.OPENEDFOLDERIMAGEINDEX
                : ImageIndexes.CLOSEDFOLDERIMAGEINDEX;
        }
    }
}
