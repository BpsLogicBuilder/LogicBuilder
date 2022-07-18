using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal partial class RulesExplorer : UserControl
    {
        private readonly IMainWindow _mainWindow;
        private readonly IRulesExplorerTreeViewBuilder _rulesExplorerTreeViewBuilder;
        private readonly ITreeViewService _treeViewService;
        private readonly UiNotificationService _uiNotificationService;
        private readonly Dictionary<string, string> expandedNodes = new();

        private readonly IDisposable refreshTreeViewSubscription;

        public RulesExplorer(
            IMainWindow mainWindow,
            IRulesExplorerTreeViewBuilder rulesExplorerTreeViewBuilder,
            ITreeViewService treeViewService,
            UiNotificationService uiNotificationService)
        {
            _mainWindow = mainWindow;
            _rulesExplorerTreeViewBuilder = rulesExplorerTreeViewBuilder;
            _treeViewService = treeViewService;
            _uiNotificationService = uiNotificationService;

            refreshTreeViewSubscription = _uiNotificationService.RulesExplorerRefreshSubject.Subscribe(RefreshTreeView);

            InitializeComponent();
            Initialize();
        }

        public void ClearProfile()
        {
            radTreeView1.Nodes.Clear();
            expandedNodes.Clear();
        }

        public void CreateProfile()
        {
            BuildTreeView();
        }

        public void RefreshTreeView()
            => BuildTreeView();

        private void BuildTreeView()
            => _rulesExplorerTreeViewBuilder.Build
            (
                radTreeView1,
                expandedNodes
            );

        private static void Dispose(IDisposable disposable)
        {
            if (disposable != null)
                disposable.Dispose();
        }

        private void Initialize()
        {
            this.radTreeView1.NodeExpandedChanged += RadTreeView1_NodeExpandedChanged;
            this.Disposed += RulesExplorer_Disposed;
            this.Load += RulesExplorer_Load;
        }

        private void RefreshTreeView(bool refresh) 
            => BuildTreeView();

        #region Event Handlers
        private void RadTreeView1_NodeExpandedChanged(object sender, RadTreeViewEventArgs e)
        {
            if (_treeViewService.IsRootNode(e.Node))
                return;

            if (e.Node.Expanded)
            {
                if (!expandedNodes.ContainsKey(e.Node.Name))
                {
                    expandedNodes.Add(e.Node.Name, e.Node.Text);
                }
            }
            else
            {
                if (expandedNodes.ContainsKey(e.Node.Name))
                {
                    expandedNodes.Remove(e.Node.Name);
                }
            }
        }

        private void RulesExplorer_Disposed(object? sender, EventArgs e)
        {
            Dispose(refreshTreeViewSubscription);
        }

        private void RulesExplorer_Load(object? sender, EventArgs e)
        {
            _mainWindow.Instance.Activated += MainWind_Activated;
        }

        private void MainWind_Activated(object? sender, EventArgs e)
        {
            RefreshTreeView();
        }
        #endregion Event Handlers
    }
}
