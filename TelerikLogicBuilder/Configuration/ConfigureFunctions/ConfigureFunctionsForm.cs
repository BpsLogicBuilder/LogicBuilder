using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunction;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.HelperStatusListBuilders;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.HelperStatusListBuilders.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions
{
    internal partial class ConfigureFunctionsForm : Telerik.WinControls.UI.RadForm, IConfigureFunctionsForm
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IConfigureFunctionsChildNodesRenamer _configureFunctionsChildNodesRenamer;
        private readonly IConfigureFunctionsCommandFactory _configureFunctionsCommandFactory;
        private readonly IConfigureFunctionsControlFactory _configureFunctionsControlFactory;
        private readonly IConfigureFunctionsTreeViewBuilder _configureFunctionsTreeViewBuilder;
        private readonly IConstructorXmlParser _constructorXmlParser;
        private readonly IXmlValidator _constructorXmlValidator;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly IFunctionHelperStatusBuilder _functionHelperStatusBuilder;
        private readonly IImageListService _imageListService;
        private readonly ILoadConstructors _loadConstructors;
        private readonly ILoadFunctions _loadFunctions;
        private readonly ILoadVariables _loadVariables;
        private readonly IParametersControlFactory _parametersControlFactory;
        private readonly ITreeViewService _treeViewService;
        private readonly ITreeViewXmlDocumentHelper _treeViewXmlDocumentHelper;
        private readonly IUpdateConstructors _updateConstructors;
        private readonly IUpdateFunctions _updateFunctions;
        private readonly IVariablesXmlParser _variablesXmlParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private static readonly string FUNCTIONNAMES_NODEXPATH = $"//{XmlDataConstants.FUNCTIONELEMENT}/@{XmlDataConstants.NAMEATTRIBUTE}";

        private readonly bool openedAsReadOnly;
        private ApplicationTypeInfo _application;
        private readonly ConfigureFunctionsTreeView radTreeView1;
        private bool _constructorsDocUpdated;

        private XmlDocument? _constructorsDoc;
        private XmlDocument? _variablesDoc;
        private EventHandler btnHelperClickHandler;
        private EventHandler btnImportClickHandler;
        private EventHandler mnuItemAddStandardFunctionClickHandler;
        private EventHandler mnuItemAddDialogFunctionClickHandler;
        private EventHandler mnuItemAddBinaryOperatorClickHandler;
        private EventHandler mnuItemAddMembersClickHandler;
        private EventHandler mnuItemAddLiteralParameterClickHandler;
        private EventHandler mnuItemAddObjectParameterClickHandler;
        private EventHandler mnuItemAddGenericParameterClickHandler;
        private EventHandler mnuItemAddListOfLiteralsParameterClickHandler;
        private EventHandler mnuItemAddListOfObjectsParameterClickHandler;
        private EventHandler mnuItemAddListOfGenericsParameterClickHandler;
        private EventHandler mnuItemAddFolderClickHandler;
        private EventHandler mnuItemDeleteClickHandler;
        private EventHandler mnuItemCutClickHandler;
        private EventHandler mnuItemPasteClickHandler;
        private EventHandler mnuItemCopyXmlClickHandler;

        private XmlDocument VariablesDoc => _variablesDoc ??= _loadVariables.Load();

        private readonly RadMenuItem mnuItemAdd = new(Strings.mnuItemAddTextWithEllipses);
        private readonly RadMenuItem mnuItemAddStandardFunction = new(Strings.mnuItemAddStandardFunctionText);
        private readonly RadMenuItem mnuItemAddDialogFunction = new(Strings.mnuItemAddDialogFunctionText);
        private readonly RadMenuItem mnuItemAddBinaryOperator = new(Strings.mnuItemAddBinaryOperatorText);
        private readonly RadMenuItem mnuItemAddMembers = new(Strings.mnuItemAddMembersText);
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

        public ConfigureFunctionsForm(
            IConfigurationFormChildNodesRenamerFactory configurationFormChildNodesRenamerFactory,
            IConfigureFunctionsCommandFactory configureFunctionsCommandFactory,
            IConfigureFunctionsControlFactory configureFunctionsControlFactory,
            IConfigureFunctionsFactory configureFunctionsFactory,
            IConstructorXmlParser constructorXmlParser,
            IDialogFormMessageControl dialogFormMessageControl,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IHelperStatusBuilderFactory helperStatusBuilderFactory,
            IImageListService imageListService,
            ILoadConstructors loadConstructors,
            ILoadFunctions loadFunctions,
            ILoadVariables loadVariables,
            IParametersControlFactory parametersControlFactory,
            IServiceFactory serviceFactory,
            ITreeViewBuilderFactory treeViewBuilderFactory,
            ITreeViewService treeViewService,
            IUpdateConstructors updateConstructors,
            IUpdateFunctions updateFunctions,
            IVariablesXmlParser variablesXmlParser,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlValidatorFactory xmlValidatorFactory,
            bool openedAsReadOnly)
        {
            InitializeComponent();
            _configureFunctionsChildNodesRenamer = configurationFormChildNodesRenamerFactory.GetConfigureFunctionsChildNodesRenamer(this);
            _configureFunctionsCommandFactory = configureFunctionsCommandFactory;
            _configureFunctionsControlFactory = configureFunctionsControlFactory;
            _configureFunctionsTreeViewBuilder = treeViewBuilderFactory.GetConfigureFunctionsTreeViewBuilder(this);
            _constructorXmlParser = constructorXmlParser;
            _constructorXmlValidator = xmlValidatorFactory.GetXmlValidator(SchemaName.ConstructorSchema);
            _dialogFormMessageControl = dialogFormMessageControl;//_applicationDropDownList may try to set messages so do this first
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            _functionHelperStatusBuilder = helperStatusBuilderFactory.GetFunctionHelperStatusBuilder(this);
            _imageListService = imageListService;
            _loadConstructors = loadConstructors;
            _loadFunctions = loadFunctions;
            _loadVariables = loadVariables;
            _parametersControlFactory = parametersControlFactory;
            _treeViewService = treeViewService;
            _treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper
            (
                SchemaName.FunctionsSchema
            );
            _updateConstructors = updateConstructors;
            _updateFunctions = updateFunctions;
            radTreeView1 = configureFunctionsFactory.GetConfigureFunctionsTreeView(this);
            _variablesXmlParser = variablesXmlParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.openedAsReadOnly = openedAsReadOnly;
            Initialize();
        }

        public IConfigureFunctionsTreeNodeControl CurrentTreeNodeControl
        {
            get
            {
                if (radPanelFields.Controls.Count != 1)
                    throw _exceptionHelper.CriticalException("{2A184F34-C4EA-4A2A-971D-5BE1F8DC4B90}");

                return (IConfigureFunctionsTreeNodeControl)radPanelFields.Controls[0];
            }
        }

        public HelperStatus? HelperStatus { get; set; }

        public bool CanExecuteImport => TreeView.Nodes.Count > 0 && TreeView.Nodes[0].Nodes.Count == 0;

        public XmlDocument ConstructorsDoc => _constructorsDoc ??= _loadConstructors.Load();

        public IList<RadTreeNode> CutTreeNodes { get; } = new List<RadTreeNode>();

        public IDictionary<string, string> ExpandedNodes { get; } = new Dictionary<string, string>();

        public RadTreeView TreeView => radTreeView1;

        public XmlDocument XmlDocument => _treeViewXmlDocumentHelper.XmlTreeDocument;

        public ApplicationTypeInfo Application => _application ?? throw _exceptionHelper.CriticalException("{F530C477-CDE3-48C8-BB26-C79A5D17487F}");

        public HashSet<string> FunctionNames => XmlDocument.SelectNodes(FUNCTIONNAMES_NODEXPATH)?.OfType<XmlAttribute>()
                                                    .Select(a => a.Value)
                                                    .ToHashSet() ?? new HashSet<string>();

        public IDictionary<string, Constructor> ConstructorsDictionary => _xmlDocumentHelpers.SelectElements
        (
            ConstructorsDoc,
            $"//{XmlDataConstants.CONSTRUCTORELEMENT}"
        )
        .Select(_constructorXmlParser.Parse)
        .ToDictionary(e => e.Name);

        public IDictionary<string, VariableBase> VariablesDictionary => _xmlDocumentHelpers.SelectElements
        (
            VariablesDoc,
            $"//{XmlDataConstants.LITERALVARIABLEELEMENT}|//{XmlDataConstants.OBJECTVARIABLEELEMENT}|//{XmlDataConstants.LITERALLISTVARIABLEELEMENT}|//{XmlDataConstants.OBJECTLISTVARIABLEELEMENT}"
        )
        .Select(_variablesXmlParser.Parse)
        .ToDictionary(e => e.Name);

        public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        public void CheckEnableImportButton() => btnImport.Enabled = CanExecuteImport;

        public void ClearMessage() => _dialogFormMessageControl.ClearMessage();

        public void RebuildTreeView() => BuildTreeView();

        public void ReloadXmlDocument(string xmlString) => _treeViewXmlDocumentHelper.LoadXmlDocument(xmlString);

        public void RenameChildNodes(RadTreeNode treeNode)
            => _configureFunctionsChildNodesRenamer.RenameChildNodes(treeNode);

        public void SelectTreeNode(RadTreeNode treeNode) => TreeView.SelectedNode = treeNode;

        public void SetErrorMessage(string message)
           => _dialogFormMessageControl.SetErrorMessage(message);

        public void SetMessage(string message, string title = "")
            => _dialogFormMessageControl.SetMessage(message, title);

        public void UpdateConstructorsConfiguration(ICollection<Constructor> constructors)
        {
            if (constructors.Count == 0)
                return;

            _xmlDocumentHelpers
                .SelectSingleElement(ConstructorsDoc, $"/{XmlDataConstants.FORMELEMENT}")
                .AppendChild(GetFolderElement());

            XmlValidationResponse response = _constructorXmlValidator.Validate(ConstructorsDoc.OuterXml);
            if (!response.Success)
                throw new LogicBuilderException(string.Join(Environment.NewLine, response.Errors));

            this._constructorsDocUpdated = true;

            XmlElement GetFolderElement()
                => constructors.Aggregate
                (
                    _xmlDocumentHelpers.MakeElement
                    (
                        ConstructorsDoc,
                        XmlDataConstants.FOLDERELEMENT,
                        null,
                        new Dictionary<string, string> { [XmlDataConstants.NAMEATTRIBUTE] = DateTime.Now.ToString() }
                    ),
                    (element, next) =>
                    {
                        element.AppendChild
                        (
                            _xmlDocumentHelpers.AddElementToDoc
                            (
                                ConstructorsDoc,
                                _xmlDocumentHelpers.ToXmlElement(next.ToXml)
                            )
                        );

                        return element;
                    }
                );
        }

        public void ValidateXmlDocument()
            => _treeViewXmlDocumentHelper.ValidateXmlDocument();

        private static EventHandler AddButtonClickCommand(IClickCommand command)
        {
            return (sender, args) => command.Execute();
        }

        private void AddClickCommands()
        {
            RemoveClickCommands();
            btnHelper.Click += btnHelperClickHandler;
            btnImport.Click += btnImportClickHandler;
            mnuItemAddStandardFunction.Click += mnuItemAddStandardFunctionClickHandler;
            mnuItemAddDialogFunction.Click += mnuItemAddDialogFunctionClickHandler;
            mnuItemAddBinaryOperator.Click += mnuItemAddBinaryOperatorClickHandler;
            mnuItemAddMembers.Click += mnuItemAddMembersClickHandler;
            mnuItemAddLiteralParameter.Click += mnuItemAddLiteralParameterClickHandler;
            mnuItemAddObjectParameter.Click += mnuItemAddObjectParameterClickHandler;
            mnuItemAddGenericParameter.Click += mnuItemAddGenericParameterClickHandler;
            mnuItemAddListOfLiteralsParameter.Click += mnuItemAddListOfLiteralsParameterClickHandler;
            mnuItemAddListOfObjectsParameter.Click += mnuItemAddListOfObjectsParameterClickHandler;
            mnuItemAddListOfGenericsParameter.Click += mnuItemAddListOfGenericsParameterClickHandler;
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

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private void BuildTreeView()
        {
            if (XmlDocument.DocumentElement == null)
                throw _exceptionHelper.CriticalException("{D8705B99-0F39-435A-97BC-1155008D5D78}");

            try
            {
                _treeViewXmlDocumentHelper.ValidateXmlDocument();
            }
            catch (LogicBuilderException ex)
            {
                throw new CriticalLogicBuilderException(ex.Message, ex);
            }

            _configureFunctionsTreeViewBuilder.Build(TreeView, XmlDocument);
            CheckEnableImportButton();
        }

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [MemberNotNull(nameof(mnuItemAddStandardFunctionClickHandler),
        nameof(mnuItemAddDialogFunctionClickHandler),
        nameof(mnuItemAddBinaryOperatorClickHandler),
        nameof(mnuItemAddMembersClickHandler),
        nameof(mnuItemAddLiteralParameterClickHandler),
        nameof(mnuItemAddObjectParameterClickHandler),
        nameof(mnuItemAddGenericParameterClickHandler),
        nameof(mnuItemAddListOfLiteralsParameterClickHandler),
        nameof(mnuItemAddListOfObjectsParameterClickHandler),
        nameof(mnuItemAddListOfGenericsParameterClickHandler),
        nameof(mnuItemAddFolderClickHandler),
        nameof(mnuItemDeleteClickHandler),
        nameof(mnuItemCutClickHandler),
        nameof(mnuItemPasteClickHandler),
        nameof(mnuItemCopyXmlClickHandler))]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        private void CreateContextMenus()
        {
            mnuItemAddStandardFunctionClickHandler = AddContextMenuClickCommand(_configureFunctionsCommandFactory.GetConfigureFunctionsAddStandardFunctionCommand(this));
            mnuItemAddDialogFunctionClickHandler = AddContextMenuClickCommand(_configureFunctionsCommandFactory.GetConfigureFunctionsAddDialogFunctionCommand(this));
            mnuItemAddBinaryOperatorClickHandler = AddContextMenuClickCommand(_configureFunctionsCommandFactory.GetConfigureFunctionsAddBinaryOperatorCommand(this));
            mnuItemAddMembersClickHandler = AddContextMenuClickCommand(_configureFunctionsCommandFactory.GetConfigureFunctionsAddClassMembersCommand(this));
            mnuItemAddLiteralParameterClickHandler = AddContextMenuClickCommand(_configureFunctionsCommandFactory.GetConfigureFunctionsAddLiteralParameterCommand(this));
            mnuItemAddObjectParameterClickHandler = AddContextMenuClickCommand(_configureFunctionsCommandFactory.GetConfigureFunctionsAddObjectParameterCommand(this));
            mnuItemAddGenericParameterClickHandler = AddContextMenuClickCommand(_configureFunctionsCommandFactory.GetConfigureFunctionsAddGenericParameterCommand(this));
            mnuItemAddListOfLiteralsParameterClickHandler = AddContextMenuClickCommand(_configureFunctionsCommandFactory.GetConfigureFunctionsAddListOfLiteralsParameterCommand(this));
            mnuItemAddListOfObjectsParameterClickHandler = AddContextMenuClickCommand(_configureFunctionsCommandFactory.GetConfigureFunctionsAddListOfObjectsParameterCommand(this));
            mnuItemAddListOfGenericsParameterClickHandler = AddContextMenuClickCommand(_configureFunctionsCommandFactory.GetConfigureFunctionsAddListOfGenericsParameterCommand(this));
            mnuItemAddFolderClickHandler = AddContextMenuClickCommand(_configureFunctionsCommandFactory.GetConfigureFunctionsAddFolderCommand(this));
            mnuItemDeleteClickHandler = AddContextMenuClickCommand(_configureFunctionsCommandFactory.GetConfigureFunctionsDeleteCommand(this));
            mnuItemCutClickHandler = AddContextMenuClickCommand(_configureFunctionsCommandFactory.GetConfigureFunctionsCutCommand(this));
            mnuItemPasteClickHandler = AddContextMenuClickCommand(_configureFunctionsCommandFactory.GetConfigureFunctionsPasteCommand(this));
            mnuItemCopyXmlClickHandler = AddContextMenuClickCommand(_configureFunctionsCommandFactory.GetConfigureFunctionsCopyXmlCommand(this));

            mnuItemAdd.Items.AddRange
            (
                new RadItem[]
                {
                    mnuItemAddStandardFunction,
                    mnuItemAddDialogFunction,
                    mnuItemAddBinaryOperator,
                    mnuItemAddMembers,
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

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [MemberNotNull(nameof(btnHelperClickHandler),
        nameof(btnImportClickHandler),
        nameof(mnuItemAddStandardFunctionClickHandler),
        nameof(mnuItemAddDialogFunctionClickHandler),
        nameof(mnuItemAddBinaryOperatorClickHandler),
        nameof(mnuItemAddMembersClickHandler),
        nameof(mnuItemAddLiteralParameterClickHandler),
        nameof(mnuItemAddObjectParameterClickHandler),
        nameof(mnuItemAddGenericParameterClickHandler),
        nameof(mnuItemAddListOfLiteralsParameterClickHandler),
        nameof(mnuItemAddListOfObjectsParameterClickHandler),
        nameof(mnuItemAddListOfGenericsParameterClickHandler),
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

            TreeView.CreateNodeElement += TreeView_CreateNodeElement;
            TreeView.MouseDown += TreeView_MouseDown;
            TreeView.NodeFormatting += TreeView_NodeFormatting;
            TreeView.NodeExpandedChanged += TreeView_NodeExpandedChanged;
            TreeView.NodeMouseClick += TreeView_NodeMouseClick;
            TreeView.SelectedNodeChanged += TreeView_SelectedNodeChanged;
            TreeView.SelectedNodeChanging += TreeView_SelectedNodeChanging;
            Disposed += ConfigureFunctionsForm_Disposed;
            FormClosing += ConfigureFunctionsForm_FormClosing;

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

            btnHelperClickHandler = AddButtonClickCommand(_configureFunctionsCommandFactory.GetConfigureFunctionsHelperCommand(this));
            btnImportClickHandler = AddButtonClickCommand(_configureFunctionsCommandFactory.GetConfigureFunctionsImportCommand(this));
            AddClickCommands();
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
            _treeViewXmlDocumentHelper.LoadXmlDocument(_loadFunctions.Load().OuterXml);
        }

        private void Navigate(Control newEditingControl)
        {
            NavigationUtility.Navigate(this.Handle, radPanelFields, newEditingControl);
        }

        private void Navigate(RadTreeNode treeNode)
        {
            bool isFunctionNode = _treeViewService.IsMethodNode(treeNode);
            btnHelper.Enabled = isFunctionNode;

            if (_treeViewService.IsRootNode(treeNode))
                Navigate((Control)_configureFunctionsControlFactory.GetConfigureFunctionsRootNodeControl());
            else if (_treeViewService.IsFolderNode(treeNode))
                Navigate((Control)_configureFunctionsControlFactory.GetConfigureFunctionsFolderControl(this));
            else if (isFunctionNode)
                Navigate((Control)_configureFunctionsControlFactory.GetConfigureFunctionControl(this));
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
                throw _exceptionHelper.CriticalException("{C5ECCBC0-6828-4179-87AD-030A08969C10}");
        }

        private void RemoveClickCommands()
        {
            btnHelper.Click -= btnHelperClickHandler;
            btnImport.Click -= btnImportClickHandler;
            mnuItemAddStandardFunction.Click -= mnuItemAddStandardFunctionClickHandler;
            mnuItemAddDialogFunction.Click -= mnuItemAddDialogFunctionClickHandler;
            mnuItemAddBinaryOperator.Click -= mnuItemAddBinaryOperatorClickHandler;
            mnuItemAddMembers.Click -= mnuItemAddMembersClickHandler;
            mnuItemAddLiteralParameter.Click -= mnuItemAddLiteralParameterClickHandler;
            mnuItemAddObjectParameter.Click -= mnuItemAddObjectParameterClickHandler;
            mnuItemAddGenericParameter.Click -= mnuItemAddGenericParameterClickHandler;
            mnuItemAddListOfLiteralsParameter.Click -= mnuItemAddListOfLiteralsParameterClickHandler;
            mnuItemAddListOfObjectsParameter.Click -= mnuItemAddListOfObjectsParameterClickHandler;
            mnuItemAddListOfGenericsParameter.Click -= mnuItemAddListOfGenericsParameterClickHandler;
            mnuItemAddFolder.Click -= mnuItemAddFolderClickHandler;
            mnuItemDelete.Click -= mnuItemDeleteClickHandler;
            mnuItemCut.Click -= mnuItemCutClickHandler;
            mnuItemPaste.Click -= mnuItemPasteClickHandler;
            mnuItemCopyXml.Click -= mnuItemCopyXmlClickHandler;
        }

        private void RemoveEventHandlers()
        {
            TreeView.CreateNodeElement -= TreeView_CreateNodeElement;
            TreeView.MouseDown -= TreeView_MouseDown;
            TreeView.NodeFormatting -= TreeView_NodeFormatting;
            TreeView.NodeExpandedChanged -= TreeView_NodeExpandedChanged;
            TreeView.NodeMouseClick -= TreeView_NodeMouseClick;
            TreeView.SelectedNodeChanged -= TreeView_SelectedNodeChanged;
            TreeView.SelectedNodeChanging -= TreeView_SelectedNodeChanging;
            FormClosing -= ConfigureFunctionsForm_FormClosing;
        }

        private void SetContextMenuState(IList<RadTreeNode> selectedNodes)
        {
            mnuItemPaste.Enabled = CutTreeNodes.Count > 0 && selectedNodes.Count == 1;
            mnuItemCut.Enabled = selectedNodes.Count > 0 && TreeView.Nodes[0].Selected == false;
            mnuItemDelete.Enabled = selectedNodes.Count > 0 && TreeView.Nodes[0].Selected == false;
            mnuItemCopyXml.Enabled = selectedNodes.Count == 1;

            if (selectedNodes.Count != 1)
            {
                mnuItemAddStandardFunction.Enabled = false;
                mnuItemAddDialogFunction.Enabled = false;
                mnuItemAddBinaryOperator.Enabled = false;
                mnuItemAddMembers.Enabled = false;
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
                bool isFunctionNode = _treeViewService.IsMethodNode(selectedNode);
                bool isParameterNode = _treeViewService.IsParameterNode(selectedNode);
                mnuItemAddStandardFunction.Enabled = !isParameterNode;
                mnuItemAddDialogFunction.Enabled = !isParameterNode;
                mnuItemAddBinaryOperator.Enabled = !isParameterNode;
                mnuItemAddMembers.Enabled = !isParameterNode;
                mnuItemAddFolder.Enabled = !isParameterNode;
                mnuItemAddListOfGenericsParameter.Enabled = isFunctionNode || isParameterNode;
                mnuItemAddListOfObjectsParameter.Enabled = isFunctionNode || isParameterNode;
                mnuItemAddListOfLiteralsParameter.Enabled = isFunctionNode || isParameterNode;
                mnuItemAddGenericParameter.Enabled = isFunctionNode || isParameterNode;
                mnuItemAddObjectParameter.Enabled = isFunctionNode || isParameterNode;
                mnuItemAddLiteralParameter.Enabled = isFunctionNode || isParameterNode;

                if (isFunctionNode || isParameterNode)
                {
                    string genericArgumentsXPath = isFunctionNode
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
            if (CurrentTreeNodeControl is IConfigureFunctionControl)
            {
                var status = _functionHelperStatusBuilder.Build();
                if (status != null)
                    HelperStatus = status;
            }
        }

        private void UpdateXmlDocument(RadTreeNode treeNode)
        {
            if (radPanelFields.Controls.Count != 1)
                throw _exceptionHelper.CriticalException("{A64AAD2D-9A29-4B95-A975-8A611DFA89B1}");

            if (radPanelFields.Controls[0] is not IConfigureFunctionsTreeNodeControl xmlElementControl)
                throw _exceptionHelper.CriticalException("{44E812B9-8DD4-407C-9094-6FFE167BA435}");

            xmlElementControl.UpdateXmlDocument(treeNode);
        }

        #region Event Handlers
        private void ApplicationDropDownList_ApplicationChanged(object? sender, ApplicationChangedEventArgs e)
        {
            _application = e.Application;
            ApplicationChanged?.Invoke(this, e);
        }

        private void ConfigureFunctionsForm_Disposed(object? sender, EventArgs e)
        {
            RemoveEventHandlers();
            RemoveClickCommands();
        }

        private void ConfigureFunctionsForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            try
            {
                if (!this.openedAsReadOnly && this.DialogResult == DialogResult.OK)
                {
                    if (TreeView.SelectedNode != null)
                        UpdateXmlDocument(TreeView.SelectedNode);

                    if (_constructorsDocUpdated)
                        _updateConstructors.Update(ConstructorsDoc);

                    _updateFunctions.Update(XmlDocument);
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
                throw _exceptionHelper.CriticalException("{D3A3C77B-A413-4B32-B2C5-385172BCF78A}");

            if (e.NodeElement is not StateImageTreeNodeElement treeNodeElement)
                throw _exceptionHelper.CriticalException("{BB856F18-06FD-41BB-A222-52FFB10EEDC8}");

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
