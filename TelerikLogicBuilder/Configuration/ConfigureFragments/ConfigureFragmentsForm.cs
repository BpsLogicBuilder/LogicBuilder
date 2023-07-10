using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments
{
    internal partial class ConfigureFragmentsForm : Telerik.WinControls.UI.RadForm, IConfigureFragmentsForm
    {
        private readonly IConfigureFragmentsChildNodesRenamer _configureFragmentsChildNodesRenamer;
        private readonly IConfigureFragmentsCommandFactory _configureFragmentsCommandFactory;
        private readonly IConfigureFragmentsControlFactory _configureFragmentsControlFactory;
        private readonly IConfigureFragmentsTreeViewBuilder _configureFragmentsTreeViewBuilder;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly IImageListService _imageListService;
        private readonly ILoadFragments _loadFragments;
        private readonly ITreeViewService _treeViewService;
        private readonly ITreeViewXmlDocumentHelper _treeViewXmlDocumentHelper;
        private readonly IUpdateFragments _updateFragments;

        private static readonly string FRAGMENTNAMES_NODEXPATH = $"//{XmlDataConstants.FRAGMENTELEMENT}/@{XmlDataConstants.NAMEATTRIBUTE}";

        private readonly bool openedAsReadOnly;
        private readonly ConfigureFragmentsTreeView radTreeView1;
        private readonly RadMenuItem mnuItemAdd = new(Strings.mnuItemAddTextWithEllipses);
        private readonly RadMenuItem mnuItemAddFragment = new(Strings.mnuItemAddFragmentText);
        private readonly RadMenuItem mnuItemAddFolder = new(Strings.mnuItemAddConfigurationFolderText);
        private readonly RadMenuItem mnuItemDelete = new(Strings.mnuItemDeleteText) { ImageIndex = ImageIndexes.DELETEIMAGEINDEX };
        private readonly RadMenuItem mnuItemCut = new(Strings.mnuItemCutText) { ImageIndex = ImageIndexes.CUTIMAGEINDEX };
        private readonly RadMenuItem mnuItemPaste = new(Strings.mnuItemPasteText);
        private readonly RadMenuItem mnuItemCopyXml = new(Strings.mnuItemCopyXml);
        private EventHandler btnImportClickHandler;
        private EventHandler mnuItemAddFragmentClickHandler;
        private EventHandler mnuItemAddFolderClickHandler;
        private EventHandler mnuItemDeleteClickHandler;
        private EventHandler mnuItemCutClickHandler;
        private EventHandler mnuItemPasteClickHandler;
        private EventHandler mnuItemCopyXmlClickHandler;

        public ConfigureFragmentsForm(
            IConfigurationFormChildNodesRenamerFactory configurationFormChildNodesRenamerFactory,
            IConfigureFragmentsCommandFactory configureFragmentsCommandFactory,
            IConfigureFragmentsControlFactory configureFragmentsControlFactory,
            IConfigureFragmentsFactory configureFragmentsFactory,
            IDialogFormMessageControl dialogFormMessageControl,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IImageListService imageListService,
            ILoadFragments loadFragments,
            IServiceFactory serviceFactory,
            ITreeViewBuilderFactory treeViewBuilderFactory,
            ITreeViewService treeViewService,
            IUpdateFragments updateFragments,
            bool openedAsReadOnly)
        {
            InitializeComponent();
            _configureFragmentsCommandFactory = configureFragmentsCommandFactory;
            _configureFragmentsControlFactory = configureFragmentsControlFactory;

            _configureFragmentsChildNodesRenamer = configurationFormChildNodesRenamerFactory.GetConfigureFragmentsChildNodesRenamer(this);
            _configureFragmentsTreeViewBuilder = treeViewBuilderFactory.GetConfigureFragmentsTreeViewBuilder(this);
            _dialogFormMessageControl = dialogFormMessageControl;
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            _imageListService = imageListService;
            _loadFragments = loadFragments;
            _treeViewService = treeViewService;
            _treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper
            (
                SchemaName.FragmentsSchema
            );
            _updateFragments = updateFragments;
            radTreeView1 = configureFragmentsFactory.GetConfigureFragmentsTreeView(this);
            this.openedAsReadOnly = openedAsReadOnly;
            Initialize();
        }

        private IConfigureFragmentsTreeNodeControl CurrentTreeNodeControl
        {
            get
            {
                if (radPanelFields.Controls.Count != 1)
                    throw _exceptionHelper.CriticalException("{A07FDB85-5484-4325-8437-0F897BF2A094}");

                return (IConfigureFragmentsTreeNodeControl)radPanelFields.Controls[0];
            }
        }

        public HashSet<string> FragmentNames => XmlDocument.SelectNodes(FRAGMENTNAMES_NODEXPATH)?.OfType<XmlAttribute>()
                                                    .Select(a => a.Value)
                                                    .ToHashSet() ?? new HashSet<string>();

        public bool CanExecuteImport => TreeView.Nodes.Count > 0 && TreeView.Nodes[0].Nodes.Count == 0;

        public IList<RadTreeNode> CutTreeNodes { get; } = new List<RadTreeNode>();

        public IDictionary<string, string> ExpandedNodes { get; } = new Dictionary<string, string>();

        public RadTreeView TreeView => radTreeView1;

        public XmlDocument XmlDocument => _treeViewXmlDocumentHelper.XmlTreeDocument;

        public ApplicationTypeInfo Application => throw new NotImplementedException();

        event EventHandler<ApplicationChangedEventArgs>? IApplicationHostControl.ApplicationChanged
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        public void CheckEnableImportButton() => btnImport.Enabled = CanExecuteImport;

        public void ClearMessage() => _dialogFormMessageControl.ClearMessage();

        public void RebuildTreeView() => BuildTreeView();

        public void ReloadXmlDocument(string xmlString) => _treeViewXmlDocumentHelper.LoadXmlDocument(xmlString);
        public void RenameChildNodes(RadTreeNode treeNode)
            => _configureFragmentsChildNodesRenamer.RenameChildNodes(treeNode);

        public void SelectTreeNode(RadTreeNode treeNode) => TreeView.SelectedNode = treeNode;

        public void SetErrorMessage(string message)
           => _dialogFormMessageControl.SetErrorMessage(message);

        public void SetMessage(string message, string title = "")
            => _dialogFormMessageControl.SetMessage(message, title);

        public void ValidateXmlDocument()
            => _treeViewXmlDocumentHelper.ValidateXmlDocument();

        private static EventHandler AddContextMenuClickCommand(IClickCommand command)
        {
            return (sender, args) => command.Execute();
        }

        private static EventHandler AddButtonClickCommand(IClickCommand command)
        {
            return (sender, args) => command.Execute();
        }

        private void AddClickCommands()
        {
            RemoveClickCommands();
            btnImport.Click += btnImportClickHandler;
            mnuItemAddFragment.Click += mnuItemAddFragmentClickHandler;
            mnuItemAddFolder.Click += mnuItemAddFolderClickHandler;
            mnuItemDelete.Click += mnuItemDeleteClickHandler;
            mnuItemCut.Click += mnuItemCutClickHandler;
            mnuItemPaste.Click += mnuItemPasteClickHandler;
            mnuItemCopyXml.Click += mnuItemCopyXmlClickHandler;
        }

        private void BuildTreeView()
        {
            if (XmlDocument.DocumentElement == null)
                throw _exceptionHelper.CriticalException("{5DEB9A19-D716-4573-83C3-7148DBA5E03C}");

            try
            {
                _treeViewXmlDocumentHelper.ValidateXmlDocument();
            }
            catch (LogicBuilderException ex)
            {
                throw new CriticalLogicBuilderException(ex.Message, ex);
            }

            _configureFragmentsTreeViewBuilder.Build(TreeView, XmlDocument);
            CheckEnableImportButton();
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
                => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [MemberNotNull(nameof(mnuItemAddFragmentClickHandler),
        nameof(mnuItemAddFolderClickHandler),
        nameof(mnuItemDeleteClickHandler),
        nameof(mnuItemCutClickHandler),
        nameof(mnuItemPasteClickHandler),
        nameof(mnuItemCopyXmlClickHandler))]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        private void CreateContextMenus()
        {
            mnuItemAddFragmentClickHandler = AddContextMenuClickCommand(_configureFragmentsCommandFactory.GetConfigureFragmentsAddFragmentCommand(this));
            mnuItemAddFolderClickHandler = AddContextMenuClickCommand(_configureFragmentsCommandFactory.GetConfigureFragmentsAddFolderCommand(this));
            mnuItemDeleteClickHandler = AddContextMenuClickCommand(_configureFragmentsCommandFactory.GetConfigureFragmentsDeleteCommand(this));
            mnuItemCutClickHandler = AddContextMenuClickCommand(_configureFragmentsCommandFactory.GetConfigureFragmentsCutCommand(this));
            mnuItemPasteClickHandler = AddContextMenuClickCommand(_configureFragmentsCommandFactory.GetConfigureFragmentsPasteCommand(this));
            mnuItemCopyXmlClickHandler = AddContextMenuClickCommand(_configureFragmentsCommandFactory.GetConfigureFragmentsCopyXmlCommand(this));

            mnuItemAdd.Items.AddRange
            (
                new RadItem[]
                {
                    mnuItemAddFragment,
                    mnuItemAddFolder
                }
            );

            TreeView.RadContextMenu = new()
            {
                ImageList = _imageListService.ImageList,
                Items =
                {
                    mnuItemAdd,
                    mnuItemDelete,
                    new RadMenuSeparatorItem(),
                    mnuItemCut,
                    mnuItemPaste,
                    new RadMenuSeparatorItem(),
                    mnuItemCopyXml
                }
            };
        }

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [MemberNotNull(nameof(btnImportClickHandler),
        nameof(mnuItemAddFragmentClickHandler),
        nameof(mnuItemAddFolderClickHandler),
        nameof(mnuItemDeleteClickHandler),
        nameof(mnuItemCutClickHandler),
        nameof(mnuItemPasteClickHandler),
        nameof(mnuItemCopyXmlClickHandler))]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        private void Initialize()
        {
            InitializeTreeView();
            InitializeDialogFormMessageControl();

            TreeView.AllowDragDrop = true;
            TreeView.MultiSelect = true;

            this.Disposed += ConfigureFragmentsForm_Disposed;
            TreeView.MouseDown += TreeView_MouseDown;
            TreeView.NodeExpandedChanged += TreeView_NodeExpandedChanged;
            TreeView.NodeMouseClick += TreeView_NodeMouseClick;
            TreeView.SelectedNodeChanged += TreeView_SelectedNodeChanged;
            TreeView.SelectedNodeChanging += TreeView_SelectedNodeChanging;
            FormClosing += ConfigureFragmentsForm_FormClosing;

            _formInitializer.SetFormDefaults(this, 719);
            btnCancel.CausesValidation = false;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnOk.Enabled = !openedAsReadOnly;
            _formInitializer.SetToConfigFragmentSize(this);

            LoadXmlDocument();
            BuildTreeView();
            TreeView.SelectedNode ??= TreeView.Nodes[0];
            SetControlValues(TreeView.SelectedNode);

            CreateContextMenus();

            CollapsePanelBorder(radPanelBottom);
            CollapsePanelBorder(radPanelButtons);
            CollapsePanelBorder(radPanelFields);
            CollapsePanelBorder(radPanelMessages);

            btnImportClickHandler = AddButtonClickCommand(_configureFragmentsCommandFactory.GetConfigureFragmentsImportCommand(this));
            AddClickCommands();
        }

        private void InitializeTreeView()
        {
            ((ISupportInitialize)this.splitPanelLeft).BeginInit();
            this.splitPanelLeft.SuspendLayout();

            ((ISupportInitialize)this.TreeView).BeginInit();
            this.TreeView.Dock = DockStyle.Fill;
            this.TreeView.Location = new System.Drawing.Point(0, 0);
            this.TreeView.Name = "radTreeView1";
            this.TreeView.Size = new System.Drawing.Size(450, 635);
            this.TreeView.SpacingBetweenNodes = -1;
            this.TreeView.TabIndex = 0;
            this.splitPanelLeft.Controls.Add(this.TreeView);
            ((ISupportInitialize)this.TreeView).EndInit();

            ((ISupportInitialize)this.splitPanelLeft).EndInit();
            this.splitPanelLeft.ResumeLayout(true);
        }

        private void InitializeDialogFormMessageControl()
        {
            ControlsLayoutUtility.LayoutBottomPanel(radPanelBottom, radPanelMessages, radPanelButtons, tableLayoutPanelButtons, _dialogFormMessageControl);
        }

        private void LoadXmlDocument()
        {
            _treeViewXmlDocumentHelper.LoadXmlDocument(_loadFragments.Load().OuterXml);
        }

        private void Navigate(Control newEditingControl)
        {
            NavigationUtility.Navigate(this.Handle, radPanelFields, newEditingControl);
        }

        private void Navigate(RadTreeNode treeNode)
        {
            if (_treeViewService.IsRootNode(treeNode))
                Navigate((Control)_configureFragmentsControlFactory.GetConfigureFragmentsRootNodeControl());
            else if (_treeViewService.IsFolderNode(treeNode))
                Navigate((Control)_configureFragmentsControlFactory.GetConfigureFragmentsFolderControl(this));
            else if (_treeViewService.IsFileNode(treeNode))
                Navigate((Control)_configureFragmentsControlFactory.GetConfigureFragmentControl(this));
            else
                throw _exceptionHelper.CriticalException("{DDC093E2-F80D-40E6-B776-44D1261E5F65}");
        }

        private void RemoveClickCommands()
        {
            btnImport.Click -= btnImportClickHandler;
            mnuItemAddFragment.Click -= mnuItemAddFragmentClickHandler;
            mnuItemAddFolder.Click -= mnuItemAddFolderClickHandler;
            mnuItemDelete.Click -= mnuItemDeleteClickHandler;
            mnuItemCut.Click -= mnuItemCutClickHandler;
            mnuItemPaste.Click -= mnuItemPasteClickHandler;
            mnuItemCopyXml.Click -= mnuItemCopyXmlClickHandler;
        }

        private void RemoveEventHandlers()
        {
            TreeView.MouseDown -= TreeView_MouseDown;
            TreeView.NodeExpandedChanged -= TreeView_NodeExpandedChanged;
            TreeView.NodeMouseClick -= TreeView_NodeMouseClick;
            TreeView.SelectedNodeChanged -= TreeView_SelectedNodeChanged;
            TreeView.SelectedNodeChanging -= TreeView_SelectedNodeChanging;
            FormClosing -= ConfigureFragmentsForm_FormClosing;
        }

        private void SetContextMenuState(IList<RadTreeNode> selectedNodes)
        {
            mnuItemPaste.Enabled = CutTreeNodes.Count > 0 && selectedNodes.Count == 1;
            mnuItemCut.Enabled = selectedNodes.Count > 0 && this.TreeView.Nodes[0].Selected == false;
            mnuItemDelete.Enabled = selectedNodes.Count > 0 && this.TreeView.Nodes[0].Selected == false;
        }

        private void SetControlValues(RadTreeNode treeNode)
        {
            Navigate(treeNode);
            CurrentTreeNodeControl.SetControlValues(treeNode);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.WaitForFullGCApproach();
            GC.WaitForFullGCComplete();
            GC.Collect();

            this.Text = $"Total Memory: {GC.GetTotalMemory(true)}";
        }

        private void UpdateXmlDocument(RadTreeNode treeNode)
        {
            if (radPanelFields.Controls.Count != 1)
                throw _exceptionHelper.CriticalException("{AF3AA656-ED38-44C8-ACBA-B735BE6536E7}");

            if (radPanelFields.Controls[0] is not IConfigureFragmentsTreeNodeControl xmlElementControl)
                throw _exceptionHelper.CriticalException("{A5260BE9-5133-4F70-888C-37EBC17EDD72}");

            xmlElementControl.UpdateXmlDocument(treeNode);
        }

        #region Event Handlers
        private void ConfigureFragmentsForm_Disposed(object? sender, EventArgs e)
        {
            RemoveEventHandlers();
            RemoveClickCommands();
        }

        private void ConfigureFragmentsForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            try
            {
                if (!this.openedAsReadOnly && this.DialogResult == DialogResult.OK)
                {
                    UpdateXmlDocument(TreeView.SelectedNode);
                    _updateFragments.Update(XmlDocument);
                }
            }
            catch (LogicBuilderException ex)
            {
                e.Cancel = true;
                _dialogFormMessageControl.SetErrorMessage(ex.Message);
            }
        }

        private void TreeView_MouseDown(object? sender, MouseEventArgs e)
        {//handles case in which clicked area doesn't have a node
            RadTreeNode treeNode = this.TreeView.GetNodeAt(e.Location);
            if (treeNode == null && this.TreeView.Nodes.Count > 0)
            {
                //SelectTreeNode(this.TreeView.Nodes[0]);
                //SetContextMenuState(_treeViewService.GetSelectedNodes(TreeView));
            }
            else if (treeNode != null && TreeView.SelectedNode != treeNode)
            {
            }
        }

        private void TreeView_NodeExpandedChanged(object sender, RadTreeViewEventArgs e)
        {
            if (_treeViewService.IsFileNode(e.Node))/*NodeExpandedChanged runs for non-folder nodes on double click*/
                return;

            if (e.Node.Expanded)
            {
                if (!ExpandedNodes.ContainsKey(e.Node.Name))
                    ExpandedNodes.Add(e.Node.Name, e.Node.Text);
            }
            else
            {
                if (ExpandedNodes.ContainsKey(e.Node.Name))
                    ExpandedNodes.Remove(e.Node.Name);
            }

            if (CutTreeNodes.ToHashSet().Contains(e.Node))
            {
                e.Node.ImageIndex = e.Node.Expanded
                    ? ImageIndexes.CUTOPENEDFOLDERIMAGEINDEX
                    : ImageIndexes.CUTCLOSEDFOLDERIMAGEINDEX;
            }
            else
            {
                e.Node.ImageIndex = e.Node.Expanded
                    ? ImageIndexes.OPENEDFOLDERIMAGEINDEX
                    : ImageIndexes.CLOSEDFOLDERIMAGEINDEX;
            }
        }

        private void TreeView_NodeMouseClick(object sender, RadTreeViewEventArgs e)
        {
            SetContextMenuState(_treeViewService.GetSelectedNodes(TreeView));
        }

        private void TreeView_SelectedNodeChanged(object sender, RadTreeViewEventArgs e)
        {
            if (e.Node?.Selected != true)
                return;

            SetControlValues(e.Node);
        }

        private void TreeView_SelectedNodeChanging(object sender, RadTreeViewCancelEventArgs e)
        {
            try
            {
                if (e.Node?.Selected == true)
                    return;

                if (TreeView.SelectedNode == null
                    || e.Node == null)//Don't update if e.Node is null because
                    return;             //1) The selected node may have been deleted
                                        //2) There is no navigation (i.e. e.Node == null)

                _dialogFormMessageControl.ClearMessage();
                UpdateXmlDocument(TreeView.SelectedNode);
            }
            catch (XmlException ex)
            {
                e.Cancel = true;
                _dialogFormMessageControl.SetErrorMessage(ex.Message);
            }
            catch (LogicBuilderException ex)
            {
                e.Cancel = true;
                _dialogFormMessageControl.SetErrorMessage(ex.Message);
            }
        }
        #endregion Event Handlers
    }
}
