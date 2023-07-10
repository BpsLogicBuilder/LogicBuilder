using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
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
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DialogFormMessageControlHelpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
        private readonly IRefreshVisibleTextHelper _refreshVisibleTextHelper;
        private readonly ITreeViewXmlDocumentHelper _treeViewXmlDocumentHelper;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private ApplicationTypeInfo _application;
        private readonly Type assignedTo;
        private readonly HashSet<string> constructorNames;
        private string selectedConstructor;
        private EventHandler btnPasteXmlClickHandler;
        private EventHandler<EventArgs> cmbSelectConstructorButtonClickHandler;
        private readonly bool denySpecialCharacters;

        public EditConstructorForm(
            IConfigurationService configurationService,
            IDialogFormMessageControlFactory dialogFormMessageControlFactory,
            IEditConstructorCommandFactory editConstructorCommandFactory,
            IEditingFormHelperFactory editingFormHelperFactory,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IRadDropDownListHelper radDropDownListHelper,
            IRefreshVisibleTextHelper refreshVisibleTextHelper,
            IServiceFactory serviceFactory,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            Type assignedTo,
            XmlDocument constructorXmlDocument,
            HashSet<string> constructorNames,
            string selectedConstructor,
            bool denySpecialCharacters)
        {
            InitializeComponent();
            _configurationService = configurationService;
            _dialogFormMessageControl = dialogFormMessageControlFactory.GetDialogFormMessageControl();//_applicationDropDownList may try to set messages so do this first
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _editConstructorCommandFactory = editConstructorCommandFactory;
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            _radDropDownListHelper = radDropDownListHelper;
            _refreshVisibleTextHelper = refreshVisibleTextHelper;
            _treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper(SchemaName.ParametersDataSchema);
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.assignedTo = assignedTo;
            this.Text = string.Format(CultureInfo.CurrentCulture, Strings.objectFormsAssignToFormat, this.Text, this.assignedTo.ToString());
            this.constructorNames = constructorNames;
            this.selectedConstructor = selectedConstructor;
            this.denySpecialCharacters = denySpecialCharacters;

            _treeViewXmlDocumentHelper.LoadXmlDocument(constructorXmlDocument.OuterXml);
            _dataGraphEditingFormEventsHelper = editingFormHelperFactory.GetDataGraphEditingFormEventsHelper(this);
            _parametersDataTreeBuilder = editingFormHelperFactory.GetParametersDataTreeBuilder(this);
            Initialize();
        }

        public ApplicationTypeInfo Application => _application ?? throw _exceptionHelper.CriticalException("{0C223B16-511C-4019-A272-7AB8CEC6E297}");

        public Type AssignedTo => assignedTo;

        public HelperButtonDropDownList CmbSelectConstructor => cmbSelectConstructor;

        public bool DenySpecialCharacters => denySpecialCharacters;

        public bool DisplayNotCheckBox => false;

        public IDictionary<string, string> ExpandedNodes { get; } = new Dictionary<string, string>();

        public RadPanel RadPanelFields => radPanelFields;

        public RadTreeView TreeView => radTreeView1;

        public string VisibleText => XmlResult.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value;

        public XmlDocument XmlDocument => _treeViewXmlDocumentHelper.XmlTreeDocument;

        public XmlElement XmlResult
            => _refreshVisibleTextHelper.RefreshConstructorVisibleTexts
            (
                _xmlDocumentHelpers.GetDocumentElement(XmlDocument),
                Application
            );

        public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        public void ClearMessage() => _dialogFormMessageControl.ClearMessage();

        public void DisableControlsDuringEdit(bool disable)
        {
            btnOk.Enabled = !disable;
            btnPasteXml.Enabled = !disable;
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

        private void AddClickCommands()
        {
            RemoveClickCommands();
            btnPasteXml.Click += btnPasteXmlClickHandler;
            cmbSelectConstructor.ButtonClick += cmbSelectConstructorButtonClickHandler;
        }

        private static EventHandler<EventArgs> AddHelperButtonDropDownListClickCommand(IClickCommand command)
        {
            return (sender, args) => command.Execute();
        }

        private static EventHandler AddButtonClickCommand(IClickCommand command)
        {
            return (sender, args) => command.Execute();
        }

        private void CmbSelectConstructorChanged()
        {
            if (!_configurationService.ConstructorList.Constructors.TryGetValue(cmbSelectConstructor.Text, out Constructor? constructor))
                return;

            this.selectedConstructor = constructor.Name;
            _treeViewXmlDocumentHelper.LoadXmlDocument(_xmlDataHelper.BuildEmptyConstructorXml(constructor.Name, constructor.Name));
            LoadTreeview();
        }

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [MemberNotNull(nameof(btnPasteXmlClickHandler),
        nameof(cmbSelectConstructorButtonClickHandler))]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        private void Initialize()
        {
            InitializeDialogFormMessageControl();
            InitializeApplicationDropDownList();
            InitializeSelectConstructorDropDownList();

            _applicationDropDownList.ApplicationChanged += ApplicationDropDownList_ApplicationChanged;
            Disposed += EditConstructorForm_Disposed;

            _formInitializer.SetFormDefaults(this, 719);
            btnCancel.CausesValidation = false;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            _formInitializer.SetToEditSize(this);

            _dataGraphEditingFormEventsHelper.Setup();
            SetUpSelectConstructorDropDownList();
            btnPasteXmlClickHandler = AddButtonClickCommand(_editConstructorCommandFactory.GetEditFormXmlCommand(this));
            cmbSelectConstructorButtonClickHandler = AddHelperButtonDropDownListClickCommand(_editConstructorCommandFactory.GetSelectConstructorCommand(this));
            LoadTreeview();
            AddClickCommands();
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

        private void RemoveClickCommands()
        {
            btnPasteXml.Click -= btnPasteXmlClickHandler;
            cmbSelectConstructor.ButtonClick -= cmbSelectConstructorButtonClickHandler;
        }

        private void RemoveEventHandlers()
        {
            _applicationDropDownList.ApplicationChanged -= ApplicationDropDownList_ApplicationChanged;
            cmbSelectConstructor.Changed -= CmbSelectConstructor_Changed;
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

        private void EditConstructorForm_Disposed(object? sender, EventArgs e)
        {
            RemoveClickCommands();
            RemoveEventHandlers();
        }
        #endregion Event Handlers
    }
}
