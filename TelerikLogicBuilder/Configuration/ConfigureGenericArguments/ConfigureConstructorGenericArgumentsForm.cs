﻿using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericArgumentsRootNode;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DialogFormMessageControlHelpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments
{
    internal partial class ConfigureConstructorGenericArgumentsForm : RadForm, IConfigureGenericArgumentsForm
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IConfigureConstructorGenericArgumentsTreeViewBuilder _configureConstructorGenericArgumentsTreeViewBuilder;
        private readonly IConfigureGenericArgumentsCommandFactory _configureGenericArgumentsCommandFactory;
        private readonly IConfigureGenericArgumentsControlFactory _configureGenericArgumentsControlFactory;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly IGenericConfigXmlParser _genericConfigXmlParser;
        private readonly IGenericsConfigrationValidator _genericsConfigrationValidator;
        private readonly ITreeViewService _treeViewService;
        private readonly ITreeViewXmlDocumentHelper _treeViewXmlDocumentHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private const string rootNodeXPath = $"/{XmlDataConstants.CONSTRUCTORELEMENT}/{XmlDataConstants.GENERICARGUMENTSELEMENT}";

        private ApplicationTypeInfo _application;
        private EventHandler mnuItemReplaceWithLiteralParameterClickHandler;
        private EventHandler mnuItemReplaceWithObjectParameterClickHandler;
        private EventHandler mnuItemReplaceWithListOfLiteralsParameterClickHandler;
        private EventHandler mnuItemReplaceWithListOfObjectsParameterClickHandler;
        private readonly Type genericTypeDefinition;
        private readonly XmlDocument _xmlDocument;
        private readonly RadMenuItem mnuItemReplaceWithLiteralParameter = new(Strings.mnuItemReplaceWithLiteralParameterText);
        private readonly RadMenuItem mnuItemReplaceWithObjectParameter = new(Strings.mnuItemReplaceWithObjectParameterText);
        private readonly RadMenuItem mnuItemReplaceWithListOfLiteralsParameter = new(Strings.mnuItemReplaceWithListOfLiteralsParameterText);
        private readonly RadMenuItem mnuItemReplaceWithListOfObjectsParameter = new(Strings.mnuItemReplaceWithListOfObjectsParameterText);

        public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        public ConfigureConstructorGenericArgumentsForm(
            IConfigureConstructorGenericArgumentsTreeViewBuilder configureConstructorGenericArgumentsTreeViewBuilder,
            IConfigureGenericArgumentsCommandFactory configureGenericArgumentsCommandFactory,
            IConfigureGenericArgumentsControlFactory configureGenericArgumentsControlFactory,
            IDialogFormMessageControlFactory dialogFormMessageControlFactory,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IGenericConfigXmlParser genericConfigXmlParser,
            IGenericsConfigrationValidator genericsConfigrationValidator,
            IServiceFactory serviceFactory,
            ITreeViewService treeViewService,
            ITypeLoadHelper typeLoadHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            XmlDocument xmlDocument,
            IList<string> configuredGenericArgumentNames,
            IList<ParameterBase> memberParameters,
            Type genericTypeDefinition)
        {
            InitializeComponent();
            _configureConstructorGenericArgumentsTreeViewBuilder = configureConstructorGenericArgumentsTreeViewBuilder;
            _configureGenericArgumentsCommandFactory = configureGenericArgumentsCommandFactory;
            _configureGenericArgumentsControlFactory = configureGenericArgumentsControlFactory;
            _dialogFormMessageControl = dialogFormMessageControlFactory.GetDialogFormMessageControl();//_applicationDropDownList may try to set messages so do this first
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            _genericConfigXmlParser = genericConfigXmlParser;
            _genericsConfigrationValidator = genericsConfigrationValidator;
            _treeViewService = treeViewService;
            _treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper
            (
                SchemaName.ParametersDataSchema
            );
            _typeLoadHelper = typeLoadHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _xmlDocument = xmlDocument;
            ConfiguredGenericArgumentNames = configuredGenericArgumentNames;
            MemberParameters = memberParameters;
            this.genericTypeDefinition = genericTypeDefinition;
            Initialize();
        }

        #region Properties
        public ApplicationTypeInfo Application => _application ?? throw _exceptionHelper.CriticalException("{BDA938CF-4388-428E-BE26-B585326150D3}");

        public IList<GenericConfigBase> GenericArguments => _xmlDocumentHelpers.GetChildElements
        (
            _xmlDocumentHelpers.SelectSingleElement(XmlDocument, rootNodeXPath)
        )
        .Select(_genericConfigXmlParser.Parse)
        .ToArray();

        public RadTreeView TreeView => radTreeView1;

        public XmlDocument XmlDocument => _treeViewXmlDocumentHelper.XmlTreeDocument;

        public IList<string> ConfiguredGenericArgumentNames { get; }

        public IList<ParameterBase> MemberParameters { get; }
        #endregion Properties

        #region Methods
        public void ClearMessage() => _dialogFormMessageControl.ClearMessage();

        public void SetErrorMessage(string message)
            => _dialogFormMessageControl.SetErrorMessage(message);

        public void SetMessage(string message, string title = "")
            => _dialogFormMessageControl.SetMessage(message, title);

        public void ValidateXmlDocument()
            => _treeViewXmlDocumentHelper.ValidateXmlDocument();

        private void AddClickCommands()
        {
            RemoveClickCommands();
            mnuItemReplaceWithLiteralParameter.Click += mnuItemReplaceWithLiteralParameterClickHandler;
            mnuItemReplaceWithObjectParameter.Click += mnuItemReplaceWithObjectParameterClickHandler;
            mnuItemReplaceWithListOfLiteralsParameter.Click += mnuItemReplaceWithListOfLiteralsParameterClickHandler;
            mnuItemReplaceWithListOfObjectsParameter.Click += mnuItemReplaceWithListOfObjectsParameterClickHandler;
        }

        private void BuildTreeView()
        {
            if (XmlDocument.DocumentElement == null)
                throw _exceptionHelper.CriticalException("{9C3C500E-442E-4CA8-A128-862A34D4A29A}");

            try
            {
                _treeViewXmlDocumentHelper.ValidateXmlDocument();
            }
            catch (LogicBuilderException ex)
            {
                throw new CriticalLogicBuilderException(ex.Message, ex);
            }

            _configureConstructorGenericArgumentsTreeViewBuilder.Build(radTreeView1, XmlDocument);
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [MemberNotNull(nameof(mnuItemReplaceWithLiteralParameterClickHandler),
        nameof(mnuItemReplaceWithObjectParameterClickHandler),
        nameof(mnuItemReplaceWithListOfLiteralsParameterClickHandler),
        nameof(mnuItemReplaceWithListOfObjectsParameterClickHandler))]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        private void CreateContextMenus()
        {
            mnuItemReplaceWithLiteralParameterClickHandler = InitializeContextMenuClickCommand
            (
                _configureGenericArgumentsCommandFactory.GetReplaceWithLiteralParameterCommand(this)
            );
            mnuItemReplaceWithObjectParameterClickHandler = InitializeContextMenuClickCommand
            (
                _configureGenericArgumentsCommandFactory.GetReplaceWithObjectParameterCommand(this)
            );
            mnuItemReplaceWithListOfLiteralsParameterClickHandler = InitializeContextMenuClickCommand
            (
                _configureGenericArgumentsCommandFactory.GetReplaceWithListOfLiteralsParameterCommand(this)
            );
            mnuItemReplaceWithListOfObjectsParameterClickHandler = InitializeContextMenuClickCommand
            (
                _configureGenericArgumentsCommandFactory.GetReplaceWithListOfObjectsParameterCommand(this)
            );

            radTreeView1.RadContextMenu = new()
            {
                Items =
                {
                    new RadMenuSeparatorItem(),
                    mnuItemReplaceWithLiteralParameter,
                    mnuItemReplaceWithObjectParameter,
                    mnuItemReplaceWithListOfLiteralsParameter,
                    mnuItemReplaceWithListOfObjectsParameter,
                    new RadMenuSeparatorItem()
                }
            };
        }

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [MemberNotNull(nameof(mnuItemReplaceWithLiteralParameterClickHandler),
        nameof(mnuItemReplaceWithObjectParameterClickHandler),
        nameof(mnuItemReplaceWithListOfLiteralsParameterClickHandler),
        nameof(mnuItemReplaceWithListOfObjectsParameterClickHandler))]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        private void Initialize()
        {
            InitializeDialogFormMessageControl();
            InitializeApplicationDropDownList();

            _applicationDropDownList.ApplicationChanged += ApplicationDropDownList_ApplicationChanged;
            radTreeView1.SelectedNodeChanged += RadTreeView1_SelectedNodeChanged;
            radTreeView1.SelectedNodeChanging += RadTreeView1_SelectedNodeChanging;
            radTreeView1.NodeMouseClick += RadTreeView1_NodeMouseClick;
            radTreeView1.MouseDown += RadTreeView1_MouseDown;
            Disposed += ConfigureConstructorGenericArgumentsForm_Disposed;
            FormClosing += ConfigureConstructorGenericArgumentsForm_FormClosing;

            _formInitializer.SetFormDefaults(this, 685);
            _formInitializer.SetToConfigSize(this);
            btnCancel.CausesValidation = false;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            LoadXmlDocument();
            BuildTreeView();
            CreateContextMenus();

            radTreeView1.SelectedNode ??= radTreeView1.Nodes[0];
            CollapsePanelBorder(radPanelApplication);
            CollapsePanelBorder(radPanelBottom);
            CollapsePanelBorder(radPanelButtons);
            CollapsePanelBorder(radPanelFields);
            CollapsePanelBorder(radPanelMessages);
            AddClickCommands();
        }

        private void InitializeApplicationDropDownList()
        {
            ControlsLayoutUtility.LayoutApplicationGroupBox(this, radPanelApplication, radGroupBoxApplication, _applicationDropDownList);
        }

        private static EventHandler InitializeContextMenuClickCommand(IClickCommand command)
        {
            return (sender, args) => command.Execute();
        }

        private void InitializeDialogFormMessageControl()
        {
            ControlsLayoutUtility.LayoutBottomPanel(radPanelBottom, radPanelMessages, radPanelButtons, tableLayoutPanelButtons, _dialogFormMessageControl);
        }

        private void LoadXmlDocument()
        {
            _treeViewXmlDocumentHelper.LoadXmlDocument(_xmlDocument.OuterXml);
        }

        private void Navigate(Control newEditingControl)
        {
            NavigationUtility.Navigate(this.Handle, radPanelFields, newEditingControl);
        }

        private void Navigate(RadTreeNode treeNode)
        {
            if (_treeViewService.IsRootNode(treeNode))
                Navigate(new ConfigureGenericArgumentsRootNodeControl());
            else if (_treeViewService.IsLiteralTypeNode(treeNode))
                Navigate(_configureGenericArgumentsControlFactory.GetConfigureGenericLiteralArgumentControl(this));
            else if (_treeViewService.IsObjectTypeNode(treeNode))
                Navigate(_configureGenericArgumentsControlFactory.GetConfigureGenericObjectArgumentControl(this));
            else if (_treeViewService.IsListOfLiteralsTypeNode(treeNode))
                Navigate(_configureGenericArgumentsControlFactory.GetConfigureGenericLiteralListArgumentControl(this));
            else if (_treeViewService.IsListOfObjectsTypeNode(treeNode))
                Navigate(_configureGenericArgumentsControlFactory.GetConfigureGenericObjectListArgumentControl(this));
            else
                throw _exceptionHelper.CriticalException("{633CEB23-4758-4DFC-9EBF-5753E2D0ED4D}");
        }

        private void SetContextMenuState(RadTreeNode treeNode)
        {
            bool isRootNode = _treeViewService.IsRootNode(treeNode);
            mnuItemReplaceWithLiteralParameter.Enabled = !isRootNode;
            mnuItemReplaceWithObjectParameter.Enabled = !isRootNode;
            mnuItemReplaceWithListOfLiteralsParameter.Enabled = !isRootNode;
            mnuItemReplaceWithListOfObjectsParameter.Enabled = !isRootNode;
        }

        private void RemoveClickCommands()
        {
            mnuItemReplaceWithLiteralParameter.Click -= mnuItemReplaceWithLiteralParameterClickHandler;
            mnuItemReplaceWithObjectParameter.Click -= mnuItemReplaceWithObjectParameterClickHandler;
            mnuItemReplaceWithListOfLiteralsParameter.Click -= mnuItemReplaceWithListOfLiteralsParameterClickHandler;
            mnuItemReplaceWithListOfObjectsParameter.Click -= mnuItemReplaceWithListOfObjectsParameterClickHandler;
        }

        private void RemoveEventHandlers()
        {
            _applicationDropDownList.ApplicationChanged -= ApplicationDropDownList_ApplicationChanged;
            radTreeView1.SelectedNodeChanged -= RadTreeView1_SelectedNodeChanged;
            radTreeView1.SelectedNodeChanging -= RadTreeView1_SelectedNodeChanging;
            radTreeView1.NodeMouseClick -= RadTreeView1_NodeMouseClick;
            radTreeView1.MouseDown -= RadTreeView1_MouseDown;
            FormClosing -= ConfigureConstructorGenericArgumentsForm_FormClosing;
        }

        private void SetControlValues(RadTreeNode treeNode)
        {
            if (treeNode == null)
                return;

            Navigate(treeNode);
            IConfigurationXmlElementControl xmlElementControl = (IConfigurationXmlElementControl)radPanelFields.Controls[0];
            xmlElementControl.SetControlValues(treeNode);
        }

        private void UpdateXmlDocument(RadTreeNode treeNode)
        {
            if (radPanelFields.Controls.Count != 1)
                throw _exceptionHelper.CriticalException("{7881BC94-A8BD-4B11-874B-44A1673ACD60}");

            if (radPanelFields.Controls[0] is not IConfigurationXmlElementControl xmlElementControl)
                throw _exceptionHelper.CriticalException("{08969996-18B6-4397-80F0-36617E69510B}");

            xmlElementControl.UpdateXmlDocument(treeNode);
        }

        private void ValidateGenericTypes()
        {
            if (!_genericsConfigrationValidator.GenericArgumentCountMatchesType(genericTypeDefinition, GenericArguments))
            {
                throw new LogicBuilderException
                (
                    string.Format
                    (
                        CultureInfo.CurrentCulture,
                        Strings.genericArgumentsCountMismatch,
                        GenericArguments.Count,
                        genericTypeDefinition.Name
                    )
                );
            }

            try
            {
                genericTypeDefinition.MakeGenericType
                (
                    GenericArguments.Select
                    (
                        ga =>
                        {
                            _typeLoadHelper.TryGetSystemType(ga, this.Application, out Type? type);

                            return type ?? throw new ArgumentException
                            (
                                string.Format
                                (
                                    CultureInfo.CurrentCulture,
                                    Strings.cannotLoadTypeForGenericArgument,
                                    ga.GenericArgumentName
                                )
                            );
                        }
                    ).ToArray()
                );
            }
            catch (Exception ex)
            {
                throw new LogicBuilderException(ex.Message);
            }
        }
        #endregion Methods

        #region Event Handlers
        private void ConfigureConstructorGenericArgumentsForm_Disposed(object? sender, EventArgs e)
        {
            RemoveEventHandlers();
            RemoveClickCommands();
            _treeViewService.ClearImageLists(TreeView);
        }

        private void RadTreeView1_MouseDown(object? sender, MouseEventArgs e)
        {//handles case in which clicked area doesn't have a node
            RadTreeNode treeNode = this.radTreeView1.GetNodeAt(e.Location);
            if (treeNode == null && this.radTreeView1.Nodes.Count > 0)
            {
                //this.radTreeView1.SelectedNode = this.radTreeView1.Nodes[0];
                //SetContextMenuState(this.radTreeView1.SelectedNode);
            }
        }

        private void RadTreeView1_NodeMouseClick(object sender, RadTreeViewEventArgs e)
        {
            this.radTreeView1.SelectedNode = e.Node;
            SetContextMenuState(this.radTreeView1.SelectedNode);
        }

        private void RadTreeView1_SelectedNodeChanging(object sender, RadTreeViewCancelEventArgs e)
        {
            try
            {
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

        private void RadTreeView1_SelectedNodeChanged(object sender, RadTreeViewEventArgs e)
        {
            SetControlValues(e.Node);
        }

        private void ApplicationDropDownList_ApplicationChanged(object? sender, ApplicationChangedEventArgs e)
        {
            _application = e.Application;
            ApplicationChanged?.Invoke(this, e);
        }

        private void ConfigureConstructorGenericArgumentsForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.DialogResult == DialogResult.OK)
                {
                    _dialogFormMessageControl.ClearMessage();
                    if (radTreeView1.SelectedNode != null)
                    {
                        UpdateXmlDocument(radTreeView1.SelectedNode);
                    }

                    ValidateXmlDocument();
                    ValidateGenericTypes();
                }
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
