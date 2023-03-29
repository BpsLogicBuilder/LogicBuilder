﻿using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor
{
    internal partial class EditConstructorForm : Telerik.WinControls.UI.RadForm, IEditConstructorForm
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IConfigurationService _configurationService;
        private readonly IDataGraphEditingFormEventsHelper _dataGraphEditingFormEventsHelper;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IEditConstructorCommandFactory _editConstructorCommandFactory;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly IParametersDataTreeBuilder _parametersDataTreeBuilder;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly ITreeViewXmlDocumentHelper _treeViewXmlDocumentHelper;
        private readonly IXmlDataHelper _xmlDataHelper;

        private ApplicationTypeInfo _application;
        private readonly Type assignedTo;
        private readonly HashSet<string> constructorNames;
        private string selectedConstructor;

        public EditConstructorForm(
            IConfigurationService configurationService,
            IDialogFormMessageControl dialogFormMessageControl,
            IEditConstructorCommandFactory editConstructorCommandFactory,
            IEditingFormHelperFactory editingFormHelperFactory,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IRadDropDownListHelper radDropDownListHelper,
            IServiceFactory serviceFactory,
            IXmlDataHelper xmlDataHelper,
            Type assignedTo,
            XmlDocument constructorXmlDocument,
            HashSet<string> constructorNames,
            string selectedConstructor)
        {
            InitializeComponent();
            _configurationService = configurationService;
            _dialogFormMessageControl = dialogFormMessageControl;//_applicationDropDownList may try to set messages so do this first
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _editConstructorCommandFactory = editConstructorCommandFactory;
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            _radDropDownListHelper = radDropDownListHelper;
            _treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper(SchemaName.ParametersDataSchema);
            _xmlDataHelper = xmlDataHelper;
            this.assignedTo = assignedTo;
            this.constructorNames = constructorNames;
            this.selectedConstructor = selectedConstructor;

            _treeViewXmlDocumentHelper.LoadXmlDocument(constructorXmlDocument.OuterXml);
            _dataGraphEditingFormEventsHelper = editingFormHelperFactory.GetDataGraphEditingFormEventsHelper(this);
            _parametersDataTreeBuilder = editingFormHelperFactory.GetParametersDataTreeBuilder(this);
            Initialize();
        }

        public ApplicationTypeInfo Application => _application ?? throw _exceptionHelper.CriticalException("{0C223B16-511C-4019-A272-7AB8CEC6E297}");

        public Type AssignedTo => assignedTo;

        public HelperButtonDropDownList CmbSelectConstructor => cmbSelectConstructor;

        public bool DenySpecialCharacters => false;

        public bool DisplayNotCheckBox => false;

        public IDictionary<string, string> ExpandedNodes { get; } = new Dictionary<string, string>();

        public RadPanel RadPanelFields => radPanelFields;

        public RadTreeView TreeView => radTreeView1;

        public XmlDocument XmlDocument => _treeViewXmlDocumentHelper.XmlTreeDocument;

        public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        public void ClearMessage() => _dialogFormMessageControl.ClearMessage();

        public void DisableControlsDuringEdit(bool disable)
        {
        }

        public void RebuildTreeView() => LoadTreeview();

        public void ReloadXmlDocument(string xmlString) => _treeViewXmlDocumentHelper.LoadXmlDocument(xmlString);

        public void RequestDocumentUpdate(IEditingControl editingControl) => _dataGraphEditingFormEventsHelper.RequestDocumentUpdate(editingControl);

        public void SetConstructorName(string constructorName)
        {
            cmbSelectConstructor.Changed -= CmbSelectConstructor_Changed;
            cmbSelectConstructor.Text = constructorName;
            cmbSelectConstructor.Changed += CmbSelectConstructor_Changed;
        }

        public void SetErrorMessage(string message) => _dialogFormMessageControl.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => _dialogFormMessageControl.SetMessage(message, title);

        public void ValidateXmlDocument() => _treeViewXmlDocumentHelper.ValidateXmlDocument();

        private static void AddButtonClickCommand(HelperButtonDropDownList helperButtonDropDownList, IClickCommand command)
        {
            helperButtonDropDownList.ButtonClick += (sender, args) => command.Execute();
        }

        private static void AddButtonClickCommand(RadButton radButton, IClickCommand command)
        {
            radButton.Click += (sender, args) => command.Execute();
        }

        private void CmbSelectConstructorChanged()
        {
            if (!_configurationService.ConstructorList.Constructors.TryGetValue(cmbSelectConstructor.Text, out Constructor? constructor))
                return;

            this.selectedConstructor = constructor.Name;
            _treeViewXmlDocumentHelper.LoadXmlDocument(_xmlDataHelper.BuildEmptyConstructorXml(constructor.Name, constructor.Name));
            LoadTreeview();
        }

        private void Initialize()
        {
            InitializeDialogFormMessageControl();
            InitializeApplicationDropDownList();
            InitializeSelectConstructorDropDownList();

            _applicationDropDownList.ApplicationChanged += ApplicationDropDownList_ApplicationChanged;

            _formInitializer.SetFormDefaults(this, 719);
            btnCancel.CausesValidation = false;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            _formInitializer.SetToEditSize(this);

            _dataGraphEditingFormEventsHelper.Setup();
            SetUpSelectConstructorDropDownList();
            AddButtonClickCommand(btnPasteXml, _editConstructorCommandFactory.GetEditFormXmlCommand(this));
            AddButtonClickCommand(cmbSelectConstructor, _editConstructorCommandFactory.GetSelectConstructorCommand(this));
            LoadTreeview();
        }

        private void InitializeApplicationDropDownList()
        {
            ControlsLayoutUtility.LayoutApplicationGroupBox(this, radPanelApplication, radGroupBoxApplication, _applicationDropDownList);
        }

        private void InitializeDialogFormMessageControl()
        {
            ControlsLayoutUtility.LayoutBottomPanel(radPanelBottom, radPanelMessages, radPanelButtons, tableLayoutPanelButtons, _dialogFormMessageControl);
        }

        private void InitializeSelectConstructorDropDownList()
        {
            ControlsLayoutUtility.LayoutSelectConfiguredItemGroupBox(this, radPanelSelectConstructor, radGroupBoxSelectConstructor, cmbSelectConstructor);
        }

        private void LoadTreeview()
        {
            _parametersDataTreeBuilder.CreateConstructorTreeProfile(TreeView, XmlDocument, assignedTo);
            if (TreeView.SelectedNode == null)
                TreeView.SelectedNode = TreeView.Nodes[0];
        }

        private void SetUpSelectConstructorDropDownList()
        {
            _radDropDownListHelper.LoadTextItems(cmbSelectConstructor.DropDownList, this.constructorNames, RadDropDownStyle.DropDown);
            SetConstructorName(this.selectedConstructor);
        }

        #region Event Handlers
        private void ApplicationDropDownList_ApplicationChanged(object? sender, ApplicationChangedEventArgs e)
        {
            _application = e.Application;
            ApplicationChanged?.Invoke(this, e);
        }

        private void CmbSelectConstructor_Changed(object? sender, EventArgs e) => CmbSelectConstructorChanged();
        #endregion Event Handlers
    }
}
