﻿using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.HelperStatusListBuilders;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.HelperStatusListBuilders.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DialogFormMessageControlHelpers.Factories;
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

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables
{
    internal partial class ConfigureVariablesForm : RadForm, IConfigureVariablesForm
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IConfigureVariablesChildNodesRenamer _configureVariablesChildNodesRenamer;
        private readonly IConfigureVariablesCommandFactory _configureVariablesCommandFactory;
        private readonly IConfigureVariablesControlFactory _configureVariablesControlFactory;
        private readonly IConfigureVariablesTreeViewBuilder _configureVariablesTreeViewBuilder;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly IImageListService _imageListService;
        private readonly ILoadVariables _loadVariables;
        private readonly ITreeViewService _treeViewService;
        private readonly ITreeViewXmlDocumentHelper _treeViewXmlDocumentHelper;
        private readonly IUpdateVariables _updateVariables;
        private readonly IVariableHelperStatusBuilder _variableHelperStatusBuilder;
        private readonly IVariablesXmlParser _variablesXmlParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private static readonly string VARIABLENAMES_NODEXPATH = $"(//{XmlDataConstants.LITERALVARIABLEELEMENT}|//{XmlDataConstants.OBJECTVARIABLEELEMENT}|//{XmlDataConstants.LITERALLISTVARIABLEELEMENT}|//{XmlDataConstants.OBJECTLISTVARIABLEELEMENT})/@{XmlDataConstants.NAMEATTRIBUTE}";

        private readonly bool openedAsReadOnly;
        private ApplicationTypeInfo _application;
        private EventHandler btnHelperClickHandler;
        private EventHandler btnImportClickHandler;
        private EventHandler mnuItemAddLiteralVariableClickHandler;
        private EventHandler mnuItemAddObjectVariableClickHandler;
        private EventHandler mnuItemAddListOfLiteralsVariableClickHandler;
        private EventHandler mnuItemAddListOfObjectsVariableClickHandler;
        private EventHandler mnuItemAddMembersClickHandler;
        private EventHandler mnuItemAddFolderClickHandler;
        private EventHandler mnuItemDeleteClickHandler;
        private EventHandler mnuItemCutClickHandler;
        private EventHandler mnuItemPasteClickHandler;
        private EventHandler mnuItemCopyXmlClickHandler;
        private readonly ConfigureVariablesTreeView radTreeView1;
        private readonly RadMenuItem mnuItemAdd = new(Strings.mnuItemAddTextWithEllipses);
        private readonly RadMenuItem mnuItemAddLiteralVariable = new(Strings.mnuItemAddImplementedLiteralVariableText);
        private readonly RadMenuItem mnuItemAddObjectVariable = new(Strings.mnuItemAddImplementedObjectVariableText);
        private readonly RadMenuItem mnuItemAddListOfLiteralsVariable = new(Strings.mnuItemAddImplementedListOfLiteralsVariableText);
        private readonly RadMenuItem mnuItemAddListOfObjectsVariable = new(Strings.mnuItemAddImplementedListOfObjectsVariableText);
        private readonly RadMenuItem mnuItemAddMembers = new(Strings.mnuItemAddMembersText);
        private readonly RadMenuItem mnuItemAddFolder = new(Strings.mnuItemAddConfigurationFolderText);
        private readonly RadMenuItem mnuItemDelete = new(Strings.mnuItemDeleteText) { ImageIndex = ImageIndexes.DELETEIMAGEINDEX };
        private readonly RadMenuItem mnuItemCut = new(Strings.mnuItemCutText) { ImageIndex = ImageIndexes.CUTIMAGEINDEX };
        private readonly RadMenuItem mnuItemPaste = new(Strings.mnuItemPasteText);
        private readonly RadMenuItem mnuItemCopyXml = new(Strings.mnuItemCopyXml);

        public ConfigureVariablesForm(
            IConfigurationFormChildNodesRenamerFactory configurationFormChildNodesRenamerFactory,
            IConfigureVariablesCommandFactory configureVariablesCommandFactory,
            IConfigureVariablesControlFactory configureVariablesControlFactory,
            IConfigureVariablesFactory configureVariablesFactory,
            IDialogFormMessageControlFactory dialogFormMessageControlFactory,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IHelperStatusBuilderFactory helperStatusBuilderFactory,
            IImageListService imageListService,
            ILoadVariables loadVariables,
            IServiceFactory serviceFactory,
            ITreeViewBuilderFactory treeViewBuilderFactory,
            ITreeViewService treeViewService,
            IUpdateVariables updateVariables,
            IVariablesXmlParser variablesXmlParser,
            IXmlDocumentHelpers xmlDocumentHelpers,
            bool openedAsReadOnly)
        {
            InitializeComponent();
            _configureVariablesCommandFactory = configureVariablesCommandFactory;
            _configureVariablesControlFactory = configureVariablesControlFactory;
            _dialogFormMessageControl = dialogFormMessageControlFactory.GetDialogFormMessageControl();//_applicationDropDownList may try to set messages so do this first
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _configureVariablesChildNodesRenamer = configurationFormChildNodesRenamerFactory.GetConfigureVariablesChildNodesRenamer(this);
            _configureVariablesTreeViewBuilder = treeViewBuilderFactory.GetConfigureVariablesTreeViewBuilder(this);
            radTreeView1 = configureVariablesFactory.GetConfigureVariablesTreeView(this);
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            _imageListService = imageListService;
            _loadVariables = loadVariables;
            _treeViewService = treeViewService;
            _treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper
            (
                SchemaName.VariablesSchema
            );
            _updateVariables = updateVariables;
            _variableHelperStatusBuilder = helperStatusBuilderFactory.GetVariableHelperStatusBuilder(this);
            _variablesXmlParser = variablesXmlParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.openedAsReadOnly = openedAsReadOnly;
            Initialize();
        }

        #region Properties

        public IConfigureVariablesTreeNodeControl CurrentTreeNodeControl
        {
            get
            {
                if (radPanelFields.Controls.Count != 1)
                    throw _exceptionHelper.CriticalException("{77B05C78-8A80-4160-BAE2-B91E8C5DC780}");

                return (IConfigureVariablesTreeNodeControl)radPanelFields.Controls[0];
            }
        }

        public ApplicationTypeInfo Application => _application ?? throw _exceptionHelper.CriticalException("{A5890E08-525D-404D-9A29-CA2633424503}");

        public IList<RadTreeNode> CutTreeNodes { get; } = [];

        public IDictionary<string, string> ExpandedNodes { get; } = new Dictionary<string, string>();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HelperStatus? HelperStatus { get; set; }

        public RadTreeView TreeView => radTreeView1;

        public IDictionary<string, VariableBase> VariablesDictionary => _xmlDocumentHelpers.SelectElements
        (
            XmlDocument,
            $"//{XmlDataConstants.LITERALVARIABLEELEMENT}|//{XmlDataConstants.OBJECTVARIABLEELEMENT}|//{XmlDataConstants.LITERALLISTVARIABLEELEMENT}|//{XmlDataConstants.OBJECTLISTVARIABLEELEMENT}"
        )
        .Select(_variablesXmlParser.Parse)
        .ToDictionary(e => e.Name);

        public XmlDocument XmlDocument => _treeViewXmlDocumentHelper.XmlTreeDocument;

        public HashSet<string> VariableNames => XmlDocument.SelectNodes(VARIABLENAMES_NODEXPATH)?.OfType<XmlAttribute>()
                                                    .Select(a => a.Value)
                                                    .ToHashSet() ?? [];

        public bool CanExecuteImport => TreeView.Nodes.Count > 0 && TreeView.Nodes[0].Nodes.Count == 0;
        #endregion Properties

        public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        public void CheckEnableImportButton() => btnImport.Enabled = CanExecuteImport;

        public void ClearMessage() => _dialogFormMessageControl.ClearMessage();

        public void RebuildTreeView() => BuildTreeView();

        public void ReloadXmlDocument(string xmlString) => _treeViewXmlDocumentHelper.LoadXmlDocument(xmlString);

        public void RenameChildNodes(RadTreeNode treeNode)
            => _configureVariablesChildNodesRenamer.RenameChildNodes(treeNode);

        public void SetErrorMessage(string message)
           => _dialogFormMessageControl.SetErrorMessage(message);

        public void SetMessage(string message, string title = "")
            => _dialogFormMessageControl.SetMessage(message, title);

        public void SelectTreeNode(RadTreeNode treeNode) => TreeView.SelectedNode = treeNode;

        public void ValidateXmlDocument()
            => _treeViewXmlDocumentHelper.ValidateXmlDocument();

        private void AddClickCommands()
        {
            RemoveClickCommands();
            btnHelper.Click += btnHelperClickHandler;
            btnImport.Click += btnImportClickHandler;
            mnuItemAddLiteralVariable.Click += mnuItemAddLiteralVariableClickHandler;
            mnuItemAddObjectVariable.Click += mnuItemAddObjectVariableClickHandler;
            mnuItemAddListOfLiteralsVariable.Click += mnuItemAddListOfLiteralsVariableClickHandler;
            mnuItemAddListOfObjectsVariable.Click += mnuItemAddListOfObjectsVariableClickHandler;
            mnuItemAddMembers.Click += mnuItemAddMembersClickHandler;
            mnuItemAddFolder.Click += mnuItemAddFolderClickHandler;
            mnuItemDelete.Click += mnuItemDeleteClickHandler;
            mnuItemCut.Click += mnuItemCutClickHandler;
            mnuItemPaste.Click += mnuItemPasteClickHandler;
            mnuItemCopyXml.Click += mnuItemCopyXmlClickHandler;
        }

        private static EventHandler AddContextMenuClickCommand(IClickCommand command)
        {
            return (sender, args) => command.Execute();
        }

        private static EventHandler AddButtonClickCommand(IClickCommand command)
        {
            return (sender, args) => command.Execute();
        }

        private void BuildTreeView()
        {
            if (XmlDocument.DocumentElement == null)
                throw _exceptionHelper.CriticalException("{E8AF482B-11CC-4B0C-963B-389053CCB33C}");

            try
            {
                _treeViewXmlDocumentHelper.ValidateXmlDocument();
            }
            catch (LogicBuilderException ex)
            {
                throw new CriticalLogicBuilderException(ex.Message, ex);
            }

            _configureVariablesTreeViewBuilder.Build(TreeView, XmlDocument);
            CheckEnableImportButton();
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [MemberNotNull(nameof(mnuItemAddLiteralVariableClickHandler),
        nameof(mnuItemAddObjectVariableClickHandler),
        nameof(mnuItemAddListOfLiteralsVariableClickHandler),
        nameof(mnuItemAddListOfObjectsVariableClickHandler),
        nameof(mnuItemAddMembersClickHandler),
        nameof(mnuItemAddFolderClickHandler),
        nameof(mnuItemDeleteClickHandler),
        nameof(mnuItemCutClickHandler),
        nameof(mnuItemPasteClickHandler),
        nameof(mnuItemCopyXmlClickHandler))]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        private void CreateContextMenus()
        {
            mnuItemAddLiteralVariableClickHandler = AddContextMenuClickCommand(_configureVariablesCommandFactory.GetAddLiteralVariableCommand(this));
            mnuItemAddObjectVariableClickHandler = AddContextMenuClickCommand(_configureVariablesCommandFactory.GetAddObjectVariableCommand(this));
            mnuItemAddListOfLiteralsVariableClickHandler = AddContextMenuClickCommand(_configureVariablesCommandFactory.GetAddLiteralListVariableCommand(this));
            mnuItemAddListOfObjectsVariableClickHandler = AddContextMenuClickCommand(_configureVariablesCommandFactory.GetAddObjectListVariableCommand(this));
            mnuItemAddMembersClickHandler = AddContextMenuClickCommand(_configureVariablesCommandFactory.GetConfigureVariablesAddMembersCommand(this));
            mnuItemAddFolderClickHandler = AddContextMenuClickCommand(_configureVariablesCommandFactory.GetConfigureVariablesAddFolderCommand(this));
            mnuItemDeleteClickHandler = AddContextMenuClickCommand(_configureVariablesCommandFactory.GetConfigureVariablesDeleteCommand(this));
            mnuItemCutClickHandler = AddContextMenuClickCommand(_configureVariablesCommandFactory.GetConfigureVariablesCutCommand(this));
            mnuItemPasteClickHandler = AddContextMenuClickCommand(_configureVariablesCommandFactory.GetConfigureVariablesPasteCommand(this));
            mnuItemCopyXmlClickHandler = AddContextMenuClickCommand(_configureVariablesCommandFactory.GetConfigureVariablesCopyXmlCommand(this));

            mnuItemAdd.Items.AddRange
            (
                new RadItem[]
                {
                    mnuItemAddLiteralVariable,
                    mnuItemAddObjectVariable,
                    mnuItemAddListOfLiteralsVariable,
                    mnuItemAddListOfObjectsVariable,
                    new RadMenuSeparatorItem(),
                    mnuItemAddMembers,
                    new RadMenuSeparatorItem(),
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
        [MemberNotNull(nameof(btnHelperClickHandler),
        nameof(btnImportClickHandler),
        nameof(mnuItemAddLiteralVariableClickHandler),
        nameof(mnuItemAddObjectVariableClickHandler),
        nameof(mnuItemAddListOfLiteralsVariableClickHandler),
        nameof(mnuItemAddListOfObjectsVariableClickHandler),
        nameof(mnuItemAddMembersClickHandler),
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
            InitializeApplicationDropDownList();

            _applicationDropDownList.ApplicationChanged += ApplicationDropDownList_ApplicationChanged;

            TreeView.AllowDragDrop = true;
            TreeView.MultiSelect = true;

            TreeView.CreateNodeElement += RadTreeView1_CreateNodeElement;
            TreeView.MouseDown += RadTreeView1_MouseDown;
            TreeView.NodeFormatting += RadTreeView1_NodeFormatting;
            TreeView.NodeExpandedChanged += RadTreeView1_NodeExpandedChanged;
            TreeView.NodeMouseClick += RadTreeView1_NodeMouseClick;
            TreeView.SelectedNodeChanged += RadTreeView1_SelectedNodeChanged;
            TreeView.SelectedNodeChanging += RadTreeView1_SelectedNodeChanging;
            Disposed += ConfigureVariablesForm_Disposed;
            FormClosing += ConfigureVariablesForm_FormClosing;

            _formInitializer.SetFormDefaults(this, 719); 
            _formInitializer.SetToConfigSize(this);
            btnCancel.CausesValidation = false;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnOk.Enabled = !openedAsReadOnly;

            LoadXmlDocument();
            BuildTreeView();
            TreeView.SelectedNode ??= TreeView.Nodes[0];
            SetControlValues(TreeView.SelectedNode);

            CreateContextMenus();

            CollapsePanelBorder(radPanelApplication);
            CollapsePanelBorder(radPanelBottom);
            CollapsePanelBorder(radPanelButtons);
            CollapsePanelBorder(radPanelFields);
            CollapsePanelBorder(radPanelMessages);

            btnHelperClickHandler = AddButtonClickCommand(_configureVariablesCommandFactory.GetConfigureVariablesHelperCommand(this));
            btnImportClickHandler = AddButtonClickCommand(_configureVariablesCommandFactory.GetConfigureVariablesImportCommand(this));
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

        private void InitializeApplicationDropDownList()
        {
            ControlsLayoutUtility.LayoutApplicationGroupBox(this, radPanelApplication, radGroupBoxApplication, _applicationDropDownList);
        }

        private void InitializeDialogFormMessageControl()
        {
            ControlsLayoutUtility.LayoutBottomPanel(radPanelBottom, radPanelMessages, radPanelButtons, tableLayoutPanelButtons, _dialogFormMessageControl);
        }

        private void LoadXmlDocument()
        {
            _treeViewXmlDocumentHelper.LoadXmlDocument(_loadVariables.Load().OuterXml);
        }

        private void Navigate(Control newEditingControl)
        {
            NavigationUtility.Navigate(this.Handle, radPanelFields, newEditingControl);
        }

        private void Navigate(RadTreeNode treeNode)
        {
            bool isFolderNode = _treeViewService.IsFolderNode(treeNode);
            btnHelper.Enabled = !isFolderNode;

            if (_treeViewService.IsRootNode(treeNode))
                Navigate((Control)_configureVariablesControlFactory.GetConfigureVariablesRootNodeControl());
            else if (isFolderNode)
                Navigate((Control)_configureVariablesControlFactory.GetConfigureVariablesFolderControl(this));
            else if (_treeViewService.IsLiteralTypeNode(treeNode))
                Navigate((Control)_configureVariablesControlFactory.GetConfigureLiteralVariableControl(this));
            else if (_treeViewService.IsObjectTypeNode(treeNode))
                Navigate((Control)_configureVariablesControlFactory.GetConfigureObjectVariableControl(this));
            else if (_treeViewService.IsListOfLiteralsTypeNode(treeNode))
                Navigate((Control)_configureVariablesControlFactory.GetConfigureLiteralListVariableControl(this));
            else if (_treeViewService.IsListOfObjectsTypeNode(treeNode))
                Navigate((Control)_configureVariablesControlFactory.GetConfigureObjectListVariableControl(this));
            else
                throw _exceptionHelper.CriticalException("{090DC892-5A03-4164-B707-95AEC4FED792}");
        }

        private void RemoveClickCommands()
        {
            btnHelper.Click -= btnHelperClickHandler;
            btnImport.Click -= btnImportClickHandler;
            mnuItemAddLiteralVariable.Click -= mnuItemAddLiteralVariableClickHandler;
            mnuItemAddObjectVariable.Click -= mnuItemAddObjectVariableClickHandler;
            mnuItemAddListOfLiteralsVariable.Click -= mnuItemAddListOfLiteralsVariableClickHandler;
            mnuItemAddListOfObjectsVariable.Click -= mnuItemAddListOfObjectsVariableClickHandler;
            mnuItemAddMembers.Click -= mnuItemAddMembersClickHandler;
            mnuItemAddFolder.Click -= mnuItemAddFolderClickHandler;
            mnuItemDelete.Click -= mnuItemDeleteClickHandler;
            mnuItemCut.Click -= mnuItemCutClickHandler;
            mnuItemPaste.Click -= mnuItemPasteClickHandler;
            mnuItemCopyXml.Click -= mnuItemCopyXmlClickHandler;
        }

        private void RemoveEventHandlers()
        {
            TreeView.CreateNodeElement -= RadTreeView1_CreateNodeElement;
            TreeView.MouseDown -= RadTreeView1_MouseDown;
            TreeView.NodeFormatting -= RadTreeView1_NodeFormatting;
            TreeView.NodeExpandedChanged -= RadTreeView1_NodeExpandedChanged;
            TreeView.NodeMouseClick -= RadTreeView1_NodeMouseClick;
            TreeView.SelectedNodeChanged -= RadTreeView1_SelectedNodeChanged;
            TreeView.SelectedNodeChanging -= RadTreeView1_SelectedNodeChanging;
            FormClosing -= ConfigureVariablesForm_FormClosing;
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
            if (CurrentTreeNodeControl is IConfigureVariableControl)
            {
                var status = _variableHelperStatusBuilder.Build();
                if ((status?.Path.Count ?? 0) > 0)
                    this.HelperStatus = status;
            }
        }

        private void UpdateXmlDocument(RadTreeNode treeNode)
        {
            if (radPanelFields.Controls.Count != 1)
                throw _exceptionHelper.CriticalException("{1041E885-A931-4859-9B52-F60AF35DF852}");

            if (radPanelFields.Controls[0] is not IConfigureVariablesTreeNodeControl xmlElementControl)
                throw _exceptionHelper.CriticalException("{93462F0F-B751-4B16-A133-D919182AAC69}");

            xmlElementControl.UpdateXmlDocument(treeNode);
        }

        #region Event Handlers
        private void ApplicationDropDownList_ApplicationChanged(object? sender, ApplicationChangedEventArgs e)
        {
            _application = e.Application;
            ApplicationChanged?.Invoke(this, e);
        }

        private void ConfigureVariablesForm_Disposed(object? sender, EventArgs e)
        {
            RemoveEventHandlers();
            RemoveClickCommands();
            _treeViewService.ClearImageLists(TreeView);
        }

        private void ConfigureVariablesForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            try
            {
                if (!this.openedAsReadOnly && this.DialogResult == DialogResult.OK)
                {
                    if (TreeView.SelectedNode != null)
                        UpdateXmlDocument(TreeView.SelectedNode);

                    _updateVariables.Update(XmlDocument);
                }
            }
            catch (LogicBuilderException ex)
            {
                e.Cancel = true;
                _dialogFormMessageControl.SetErrorMessage(ex.Message);
            }
        }

        private void RadTreeView1_CreateNodeElement(object sender, CreateTreeNodeElementEventArgs e)
        {
            e.NodeElement = new StateImageTreeNodeElement();
        }

        private void RadTreeView1_MouseDown(object? sender, MouseEventArgs e)
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

        private void RadTreeView1_NodeFormatting(object sender, TreeNodeFormattingEventArgs e)
        {
            if (e.Node is not StateImageRadTreeNode treeNode)
                throw _exceptionHelper.CriticalException("{F224B663-FD30-4DD2-AD3E-908A418C3AE7}");

            if (e.NodeElement is not StateImageTreeNodeElement treeNodeElement)
                throw _exceptionHelper.CriticalException("{E90E6EDC-72FD-457F-AAD5-A9A89360841D}");

            treeNodeElement.StateImage = treeNode.StateImage;
        }

        private void RadTreeView1_NodeExpandedChanged(object sender, RadTreeViewEventArgs e)
        {
            if (_treeViewService.IsVariableNode(e.Node))/*NodeExpandedChanged runs for non-folder nodes on double click*/
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

        private void RadTreeView1_NodeMouseClick(object sender, RadTreeViewEventArgs e)
        {
            SetContextMenuState(_treeViewService.GetSelectedNodes(TreeView));
        }

        private void RadTreeView1_SelectedNodeChanged(object sender, RadTreeViewEventArgs e)
        {
            if (e.Node?.Selected != true)
                return;

            SetControlValues(e.Node);
        }

        private void RadTreeView1_SelectedNodeChanging(object sender, RadTreeViewCancelEventArgs e)
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
