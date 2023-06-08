using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructor;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.HelperStatusListBuilders;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.HelperStatusListBuilders.Factories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors
{
    internal partial class ConfigureConstructorsForm : Telerik.WinControls.UI.RadForm, IConfigureConstructorsForm
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IConfigureConstructorsChildNodesRenamer _configureConstructorsChildNodesRenamer;
        private readonly IConfigureConstructorsCommandFactory _configureConstructorsCommandFactory;
        private readonly IConfigureConstructorsControlFactory _configureConstructorsControlFactory;
        private readonly IConfigureConstructorsTreeViewBuilder _configureConstructorsTreeViewBuilder;
        private readonly IConstructorXmlParser _constructorXmlParser;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly IImageListService _imageListService;
        private readonly ILoadConstructors _loadConstructors;
        private readonly IParametersControlFactory _parametersControlFactory;
        private readonly ITreeViewService _treeViewService;
        private readonly ITreeViewXmlDocumentHelper _treeViewXmlDocumentHelper;
        private readonly IUpdateConstructors _updateConstructors;
        private readonly IConstructorHelperStatusBuilder _constructorHelperStatusBuilder;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly bool openedAsReadOnly;
        private ApplicationTypeInfo _application;
        private readonly ConfigureConstructorsTreeView radTreeView1;
        private ConstructorHelperStatus? helperStatus;

        private readonly RadMenuItem mnuItemAdd = new(Strings.mnuItemAddTextWithEllipses);
        private readonly RadMenuItem mnuItemAddConstructor = new(Strings.mnuItemAddConstructorText);
        private readonly RadMenuItem mnuItemAddLiteralParameter = new(Strings.mnuItemAddLiteralParameterText);
        private readonly RadMenuItem mnuItemAddObjectParameter = new(Strings.mnuItemAddObjectParameterText);
        private readonly RadMenuItem mnuItemAddGenericParameter = new(Strings.mnuItemAddGenericParameterText);
        private readonly RadMenuItem mnuItemAddListOfLiteralsParameter = new(Strings.mnuItemAddListOfLiteralsParameterText);
        private readonly RadMenuItem mnuItemAddListOfObjectsParameter = new(Strings.mnuItemAddListOfObjectsParameterText);
        private readonly RadMenuItem mnuItemAddListOfGenericsParameter = new(Strings.mnuItemAddListOfGenericsParameterText);
        private readonly RadMenuItem mnuItemAddFolder = new(Strings.mnuItemAddConfigurationFolderText);
        private readonly RadMenuItem mnuItemDelete = new(Strings.mnuItemDeleteText) { ImageIndex = ImageIndexes.DELETEIMAGEINDEX };
        private readonly RadMenuItem mnuItemCut = new(Strings.mnuItemCutText) { ImageIndex = ImageIndexes.CUTIMAGEINDEX };
        private readonly RadMenuItem mnuItemPaste = new(Strings.mnuItemPasteText);
        private readonly RadMenuItem mnuItemCopyXml = new(Strings.mnuItemCopyXml);

        public ConfigureConstructorsForm(
            IConfigurationFormChildNodesRenamerFactory configurationFormChildNodesRenamerFactory,
            IConfigureConstructorsCommandFactory configureConstructorsCommandFactory,
            IConfigureConstructorsControlFactory configureConstructorsControlFactory,
            IConfigureConstructorsFactory configureConstructorsFactory,
            IConstructorXmlParser constructorXmlParser,
            IDialogFormMessageControl dialogFormMessageControl,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IHelperStatusBuilderFactory helperStatusBuilderFactory,
            IImageListService imageListService,
            ILoadConstructors loadConstructors,
            IParametersControlFactory parametersControlFactory,
            IServiceFactory serviceFactory,
            ITreeViewBuilderFactory treeViewBuilderFactory,
            ITreeViewService treeViewService,
            IUpdateConstructors updateConstructors,
            IXmlDocumentHelpers xmlDocumentHelpers,
            bool openedAsReadOnly)
        {
            InitializeComponent();
            _configureConstructorsCommandFactory = configureConstructorsCommandFactory;
            _configureConstructorsControlFactory = configureConstructorsControlFactory;
            _constructorHelperStatusBuilder = helperStatusBuilderFactory.GetConstructorHelperStatusBuilder(this);
            _constructorXmlParser = constructorXmlParser;
            _dialogFormMessageControl = dialogFormMessageControl;//_applicationDropDownList may try to set messages so do this first
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _configureConstructorsChildNodesRenamer = configurationFormChildNodesRenamerFactory.GetConfigureConstructorsChildNodesRenamer(this);
            _configureConstructorsTreeViewBuilder = treeViewBuilderFactory.GetConfigureConstructorsTreeViewBuilder(this);
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            _imageListService = imageListService;
            _loadConstructors = loadConstructors;
            _parametersControlFactory = parametersControlFactory;
            _treeViewService = treeViewService;
            _treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper
            (
                SchemaName.ConstructorSchema
            );
            _updateConstructors = updateConstructors;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            radTreeView1 = configureConstructorsFactory.GetConfigureConstructorsTreeView(this);
            this.openedAsReadOnly = openedAsReadOnly;
            Initialize();
        }

        public IDictionary<string, Constructor> ConstructorsDictionary => _xmlDocumentHelpers.SelectElements
        (
            XmlDocument,
            $"//{XmlDataConstants.CONSTRUCTORELEMENT}"
        )
        .Select(_constructorXmlParser.Parse)
        .ToDictionary(e => e.Name);

        public IConfigureConstructorsTreeNodeControl CurrentTreeNodeControl
        {
            get
            {
                if (radPanelFields.Controls.Count != 1)
                    throw _exceptionHelper.CriticalException("{22F0DD35-503C-4D3B-9098-9E8E5A755B21}");

                return (IConfigureConstructorsTreeNodeControl)radPanelFields.Controls[0];
            }
        }

        public ConstructorHelperStatus? HelperStatus => helperStatus;

        public bool CanExecuteImport => TreeView.Nodes.Count > 0 && TreeView.Nodes[0].Nodes.Count == 0;

        public IList<RadTreeNode> CutTreeNodes { get; } = new List<RadTreeNode>();

        public IDictionary<string, string> ExpandedNodes { get; } = new Dictionary<string, string>();

        public RadTreeView TreeView => radTreeView1;

        public XmlDocument XmlDocument => _treeViewXmlDocumentHelper.XmlTreeDocument;

        public ApplicationTypeInfo Application => _application ?? throw _exceptionHelper.CriticalException("{73FC707D-83C0-4F55-9BDF-A90A8C94D96C}");

        public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        public void CheckEnableImportButton() => btnImport.Enabled = CanExecuteImport;

        public void ClearMessage() => _dialogFormMessageControl.ClearMessage();

        public void RebuildTreeView() => BuildTreeView();

        public void ReloadXmlDocument(string xmlString) => _treeViewXmlDocumentHelper.LoadXmlDocument(xmlString);

        public void RenameChildNodes(RadTreeNode treeNode)
            => _configureConstructorsChildNodesRenamer.RenameChildNodes(treeNode);

        public void SelectTreeNode(RadTreeNode treeNode) => TreeView.SelectedNode = treeNode;

        public void SetErrorMessage(string message)
           => _dialogFormMessageControl.SetErrorMessage(message);

        public void SetMessage(string message, string title = "")
            => _dialogFormMessageControl.SetMessage(message, title);

        public void ValidateXmlDocument()
            => _treeViewXmlDocumentHelper.ValidateXmlDocument();

        private static void AddContextMenuClickCommand(RadMenuItem radMenuItem, IClickCommand command)
        {
            radMenuItem.Click += (sender, args) => command.Execute();
        }

        private static void AddButtonClickCommand(RadButton radButton, IClickCommand command)
        {
            radButton.Click += (sender, args) => command.Execute();
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private void BuildTreeView()
        {
            if (XmlDocument.DocumentElement == null)
                throw _exceptionHelper.CriticalException("{EBED0C98-0AAB-4636-9BDD-EB45FB608DC4}");

            try
            {
                _treeViewXmlDocumentHelper.ValidateXmlDocument();
            }
            catch (LogicBuilderException ex)
            {
                throw new CriticalLogicBuilderException(ex.Message, ex);
            }

            _configureConstructorsTreeViewBuilder.Build(TreeView, XmlDocument);
            CheckEnableImportButton();
        }

        private void CreateContextMenus()
        {
            AddContextMenuClickCommand(mnuItemAddConstructor, _configureConstructorsCommandFactory.GetConfigureConstructorsAddConstructorCommand(this));
            AddContextMenuClickCommand(mnuItemAddLiteralParameter, _configureConstructorsCommandFactory.GetConfigureConstructorsAddLiteralParameterCommand(this));
            AddContextMenuClickCommand(mnuItemAddObjectParameter, _configureConstructorsCommandFactory.GetConfigureConstructorsAddObjectParameterCommand(this));
            AddContextMenuClickCommand(mnuItemAddGenericParameter, _configureConstructorsCommandFactory.GetConfigureConstructorsAddGenericParameterCommand(this));
            AddContextMenuClickCommand(mnuItemAddListOfLiteralsParameter, _configureConstructorsCommandFactory.GetConfigureConstructorsAddListOfLiteralsParameterCommand(this));
            AddContextMenuClickCommand(mnuItemAddListOfObjectsParameter, _configureConstructorsCommandFactory.GetConfigureConstructorsAddListOfObjectsParameterCommand(this));
            AddContextMenuClickCommand(mnuItemAddListOfGenericsParameter, _configureConstructorsCommandFactory.GetConfigureConstructorsAddListOfGenericsParameterCommand(this));
            AddContextMenuClickCommand(mnuItemAddFolder, _configureConstructorsCommandFactory.GetConfigureConstructorsAddFolderCommand(this));
            AddContextMenuClickCommand(mnuItemDelete, _configureConstructorsCommandFactory.GetConfigureConstructorsDeleteCommand(this));
            AddContextMenuClickCommand(mnuItemCut, _configureConstructorsCommandFactory.GetConfigureConstructorsCutCommand(this));
            AddContextMenuClickCommand(mnuItemPaste, _configureConstructorsCommandFactory.GetConfigureConstructorsPasteCommand(this));
            AddContextMenuClickCommand(mnuItemCopyXml, _configureConstructorsCommandFactory.GetConfigureConstructorsCopyXmlCommand(this));

            mnuItemAdd.Items.AddRange
            (
                new RadItem[]
                {
                    mnuItemAddConstructor,
                    new RadMenuSeparatorItem(),
                    mnuItemAddLiteralParameter,
                    mnuItemAddObjectParameter,
                    mnuItemAddGenericParameter,
                    mnuItemAddListOfLiteralsParameter,
                    mnuItemAddListOfObjectsParameter,
                    mnuItemAddListOfGenericsParameter,
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

        private void Initialize()
        {
            InitializeTreeView();
            InitializeDialogFormMessageControl();
            InitializeApplicationDropDownList();

            _applicationDropDownList.ApplicationChanged += ApplicationDropDownList_ApplicationChanged;

            TreeView.AllowDragDrop = true;
            TreeView.MultiSelect = true;

            TreeView.CreateNodeElement += TreeView_CreateNodeElement;
            TreeView.MouseDown += TreeView_MouseDown;
            TreeView.NodeFormatting += TreeView_NodeFormatting;
            TreeView.NodeExpandedChanged += TreeView_NodeExpandedChanged;
            TreeView.NodeMouseClick += TreeView_NodeMouseClick;
            TreeView.SelectedNodeChanged += TreeView_SelectedNodeChanged;
            TreeView.SelectedNodeChanging += TreeView_SelectedNodeChanging;
            FormClosing += ConfigureConstructorsForm_FormClosing;

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

            AddButtonClickCommand(btnHelper, _configureConstructorsCommandFactory.GetConfigureConstructorsHelperCommand(this));
            AddButtonClickCommand(btnImport, _configureConstructorsCommandFactory.GetConfigureConstructorsImportCommand(this));
        }

        private void InitializeTreeView()
        {
            ((ISupportInitialize)this.splitPanelLeft).BeginInit();
            this.splitPanelLeft.SuspendLayout();

            ((ISupportInitialize)this.TreeView).BeginInit();
            this.TreeView.Dock = DockStyle.Fill;
            this.TreeView.Location = new Point(0, 0);
            this.TreeView.Name = "radTreeView1";
            this.TreeView.Size = new Size(450, 635);
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
            _treeViewXmlDocumentHelper.LoadXmlDocument(_loadConstructors.Load().OuterXml);
        }

        private void Navigate(Control newEditingControl)
        {
            NavigationUtility.Navigate(this.Handle, radPanelFields, newEditingControl);
        }

        private void Navigate(RadTreeNode treeNode)
        {
            bool isConstructorNode = _treeViewService.IsConstructorNode(treeNode);
            btnHelper.Enabled = isConstructorNode;

            if (_treeViewService.IsRootNode(treeNode))
                Navigate((Control)_configureConstructorsControlFactory.GetConfigureConstructorsRootNodeControl());
            else if (_treeViewService.IsFolderNode(treeNode))
                Navigate((Control)_configureConstructorsControlFactory.GetConfigureConstructorsFolderControl(this));
            else if (isConstructorNode)
                Navigate((Control)_configureConstructorsControlFactory.GetConfigureConstructorControl(this));
            else if (_treeViewService.IsLiteralTypeNode(treeNode))
                Navigate((Control)_parametersControlFactory.GetConfigureLiteralParameterControl(this));
            else if (_treeViewService.IsObjectTypeNode(treeNode))
                Navigate((Control)_parametersControlFactory.GetConfigureObjectParameterControl(this));
            else if (_treeViewService.IsGenericTypeNode(treeNode))
                Navigate((Control)_parametersControlFactory.GetConfigureGenericParameterControl(this));
            else if (_treeViewService.IsListOfLiteralsTypeNode(treeNode))
                Navigate((Control)_parametersControlFactory.GetConfigureLiteralListParameterControl(this));
            else if (_treeViewService.IsListOfObjectsTypeNode(treeNode))
                Navigate((Control)_parametersControlFactory.GetConfigureObjectListParameterControl(this));
            else if (_treeViewService.IsListOfGenericsTypeNode(treeNode))
                Navigate((Control)_parametersControlFactory.GetConfigureGenericListParameterControl(this));
            else
                throw _exceptionHelper.CriticalException("{17122141-C4CA-419A-8497-A85B3C69A4E1}");
        }

        private void SetContextMenuState(IList<RadTreeNode> selectedNodes)
        {
            mnuItemPaste.Enabled = CutTreeNodes.Count > 0 && selectedNodes.Count == 1;
            mnuItemCut.Enabled = selectedNodes.Count > 0 && TreeView.Nodes[0].Selected == false;
            mnuItemDelete.Enabled = selectedNodes.Count > 0 && TreeView.Nodes[0].Selected == false;
            mnuItemCopyXml.Enabled = selectedNodes.Count == 1;

            if (selectedNodes.Count != 1)
            {
                mnuItemAddConstructor.Enabled = false;
                mnuItemAddListOfGenericsParameter.Enabled = false;
                mnuItemAddListOfObjectsParameter.Enabled = false;
                mnuItemAddListOfLiteralsParameter.Enabled = false;
                mnuItemAddGenericParameter.Enabled = false;
                mnuItemAddObjectParameter.Enabled = false;
                mnuItemAddLiteralParameter.Enabled = false;
                mnuItemAddFolder.Enabled = false;
            }
            else
            {
                RadTreeNode selectedNode = selectedNodes[0];
                bool isConstructorNode = _treeViewService.IsConstructorNode(selectedNode);
                bool isParameterNode = _treeViewService.IsParameterNode(selectedNode);
                mnuItemAddConstructor.Enabled = !isParameterNode;
                mnuItemAddFolder.Enabled = !isParameterNode;
                mnuItemAddListOfGenericsParameter.Enabled = isConstructorNode || isParameterNode;
                mnuItemAddListOfObjectsParameter.Enabled = isConstructorNode || isParameterNode;
                mnuItemAddListOfLiteralsParameter.Enabled = isConstructorNode || isParameterNode;
                mnuItemAddGenericParameter.Enabled = isConstructorNode || isParameterNode;
                mnuItemAddObjectParameter.Enabled = isConstructorNode || isParameterNode;
                mnuItemAddLiteralParameter.Enabled = isConstructorNode || isParameterNode;

                if (isConstructorNode || isParameterNode)
                {
                    string genericArgumentsXPath = isConstructorNode
                        ? $"{selectedNode.Name}/{XmlDataConstants.GENERICARGUMENTSELEMENT}"
                        : $"{selectedNode.Parent.Name}/{XmlDataConstants.GENERICARGUMENTSELEMENT}";

                    SetEnableForGenericMenuItems
                    (
                        _xmlDocumentHelpers.GetGenericArguments(XmlDocument, genericArgumentsXPath).Length
                    );
                }

                void SetEnableForGenericMenuItems(int genericArgumentsCount)
                {
                    mnuItemAddListOfGenericsParameter.Enabled = genericArgumentsCount > 0;
                    mnuItemAddGenericParameter.Enabled = genericArgumentsCount > 0;
                }
            }
        }

        private void SetControlValues(RadTreeNode treeNode)
        {
            Navigate(treeNode);
            CurrentTreeNodeControl.SetControlValues(treeNode);
            if (CurrentTreeNodeControl is IConfigureConstructorControl)
            {
                var status = _constructorHelperStatusBuilder.Build();
                //if (status != null)
                this.helperStatus = status;/*more confusing for constructors if a previous value is used.*/
            }
        }

        private void UpdateXmlDocument(RadTreeNode treeNode)
        {
            if (radPanelFields.Controls.Count != 1)
                throw _exceptionHelper.CriticalException("{A7BD5594-E3FC-4D67-BD2C-5723030446B2}");

            if (radPanelFields.Controls[0] is not IConfigureConstructorsTreeNodeControl xmlElementControl)
                throw _exceptionHelper.CriticalException("{CB825014-9763-4FA7-B4FD-E4217BF2BF50}");

            xmlElementControl.UpdateXmlDocument(treeNode);
        }

        #region Event Handlers
        private void ApplicationDropDownList_ApplicationChanged(object? sender, ApplicationChangedEventArgs e)
        {
            _application = e.Application;
            ApplicationChanged?.Invoke(this, e);
        }

        private void ConfigureConstructorsForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            try
            {
                if (!this.openedAsReadOnly && this.DialogResult == DialogResult.OK)
                {
                    if (TreeView.SelectedNode != null)
                        UpdateXmlDocument(TreeView.SelectedNode);

                    _updateConstructors.Update(XmlDocument);
                }
            }
            catch (LogicBuilderException ex)
            {
                e.Cancel = true;
                _dialogFormMessageControl.SetErrorMessage(ex.Message);
            }
        }

        private void TreeView_CreateNodeElement(object sender, CreateTreeNodeElementEventArgs e)
        {
            e.NodeElement = new StateImageTreeNodeElement();
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

        private void TreeView_NodeFormatting(object sender, TreeNodeFormattingEventArgs e)
        {
            if (e.Node is not StateImageRadTreeNode treeNode)
                throw _exceptionHelper.CriticalException("{A1585A19-77E4-49E7-A385-82B0ACBC1F64}");

            if (e.NodeElement is not StateImageTreeNodeElement treeNodeElement)
                throw _exceptionHelper.CriticalException("{E881BAD3-8004-49F4-BB82-B0DE1E0D056D}");

            treeNodeElement.StateImage = treeNode.StateImage;
        }

        private void TreeView_NodeExpandedChanged(object sender, RadTreeViewEventArgs e)
        {
            if (!_treeViewService.IsFolderNode(e.Node))/*NodeExpandedChanged runs for non-folder nodes on double click*/
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
