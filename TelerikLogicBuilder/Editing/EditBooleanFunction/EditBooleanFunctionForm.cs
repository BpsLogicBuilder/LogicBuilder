using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditBooleanFunction.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditBooleanFunction
{
    internal partial class EditBooleanFunctionForm : Telerik.WinControls.UI.RadForm, IEditBooleanFunctionForm
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IConfigurationService _configurationService;
        private readonly IDataGraphEditingFormEventsHelper _dataGraphEditingFormEventsHelper;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IEditBooleanFunctionCommandFactory _editFunctionCommandFactory;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IFunctionHelper _functionHelper;
        private readonly IParametersDataTreeBuilder _parametersDataTreeBuilder;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly IRefreshVisibleTextHelper _refreshVisibleTextHelper;
        private readonly ITreeViewXmlDocumentHelper _treeViewXmlDocumentHelper;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private ApplicationTypeInfo _application;

        public EditBooleanFunctionForm(
            IConfigurationService configurationService,
            IDialogFormMessageControl dialogFormMessageControl,
            IEditBooleanFunctionCommandFactory editFunctionCommandFactory,
            IEditingFormHelperFactory editingFormHelperFactory,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IFunctionDataParser functionDataParser,
            IFunctionHelper functionHelper,
            IRadDropDownListHelper radDropDownListHelper,
            IRefreshVisibleTextHelper refreshVisibleTextHelper,
            IServiceFactory serviceFactory,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            XmlDocument? functionXmlDocument)
        {
            InitializeComponent();
            _configurationService = configurationService;
            _dialogFormMessageControl = dialogFormMessageControl;//_applicationDropDownList may try to set messages so do this first
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _editFunctionCommandFactory = editFunctionCommandFactory;
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            _functionDataParser = functionDataParser;
            _functionHelper = functionHelper;
            _radDropDownListHelper = radDropDownListHelper;
            _refreshVisibleTextHelper = refreshVisibleTextHelper;
            _treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper(SchemaName.ParametersDataSchema);
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;

            if (functionXmlDocument != null)
                LoadXmlDocument(functionXmlDocument);
            _dataGraphEditingFormEventsHelper = editingFormHelperFactory.GetDataGraphEditingFormEventsHelper(this);
            _parametersDataTreeBuilder = editingFormHelperFactory.GetParametersDataTreeBuilder(this);
            Initialize();
        }

        public HelperButtonDropDownList CmbSelectFunction => cmbSelectFunction;

        public bool DenySpecialCharacters => true;

        public bool DisplayNotCheckBox => false;

        public RadPanel RadPanelFields => radPanelFields;

        public RadTreeView TreeView => radTreeView1;

        public string VisibleText => _functionDataParser.Parse(XmlResult).VisibleText;

        public XmlDocument XmlDocument => _treeViewXmlDocumentHelper.XmlTreeDocument;

        public XmlElement XmlResult
            => _refreshVisibleTextHelper.RefreshFunctionVisibleTexts
            (
                _xmlDocumentHelpers.GetDocumentElement(XmlDocument),
                Application
            );

        public Type AssignedTo => typeof(bool);

        public IDictionary<string, string> ExpandedNodes { get; } = new Dictionary<string, string>();

        public ApplicationTypeInfo Application => _application ?? throw _exceptionHelper.CriticalException("{626F8319-3399-4587-B38E-6C29CEB0674D}");

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

        public void SetErrorMessage(string message) => _dialogFormMessageControl.SetErrorMessage(message);

        public void SetFunctionName(string functionName)
        {
            cmbSelectFunction.Changed -= CmbSelectFunction_Changed;
            cmbSelectFunction.Text = functionName;
            cmbSelectFunction.Changed += CmbSelectFunction_Changed;
        }

        public void SetMessage(string message, string title = "") => _dialogFormMessageControl.SetMessage(message, title);

        public void ValidateXmlDocument()
        {
            //BooleanFunction specfic error checks (i.e. not configured, must be boolean can't be dialog) here should not be necessary
            //New function selections are always checked against _configurationService.FunctionList.BooleanFunctions
            _treeViewXmlDocumentHelper.ValidateXmlDocument();
        }

        private static void AddButtonClickCommand(HelperButtonDropDownList helperButtonDropDownList, IClickCommand command)
        {
            helperButtonDropDownList.ButtonClick += (sender, args) => command.Execute();
        }

        private static void AddButtonClickCommand(RadButton radButton, IClickCommand command)
        {
            radButton.Click += (sender, args) => command.Execute();
        }

        private void CmbSelectFunctionChanged()
        {
            if (!_configurationService.FunctionList.BooleanFunctions.TryGetValue(cmbSelectFunction.Text, out Function? function))
                return;

            _treeViewXmlDocumentHelper.LoadXmlDocument(_xmlDataHelper.BuildEmptyFunctionXml(function.Name, function.Name));
            LoadTreeview();
        }

        private void Initialize()
        {
            InitializeDialogFormMessageControl();
            InitializeApplicationDropDownList();
            InitializeSelectFunctionDropDownList();

            _applicationDropDownList.ApplicationChanged += ApplicationDropDownList_ApplicationChanged;
            cmbSelectFunction.Changed += CmbSelectFunction_Changed;

            _formInitializer.SetFormDefaults(this, 719);
            btnCancel.CausesValidation = false;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            _formInitializer.SetToEditSize(this);

            _dataGraphEditingFormEventsHelper.Setup();
            SetUpSelectFunctionDropDownList();
            AddButtonClickCommand(btnPasteXml, _editFunctionCommandFactory.GetEditBooleanFunctionFormXmlCommand(this));
            AddButtonClickCommand(cmbSelectFunction, _editFunctionCommandFactory.GetSelectBooleanFunctionCommand(this));
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

        private void InitializeSelectFunctionDropDownList()
        {
            ControlsLayoutUtility.LayoutSelectConfiguredItemGroupBox(this, radPanelSelectFunction, radGroupBoxSelectFunction, cmbSelectFunction);
        }

        private void LoadTreeview()
        {
            if (XmlDocument.DocumentElement == null)
                return;

            _parametersDataTreeBuilder.CreateFunctionTreeProfile(TreeView, XmlDocument, AssignedTo);
            if (TreeView.SelectedNode == null)
                TreeView.SelectedNode = TreeView.Nodes[0];
        }

        private void LoadXmlDocument(XmlDocument functionXmlDocument)
        {
            FunctionData functionData = _functionDataParser.Parse
            (
                _xmlDocumentHelpers.GetDocumentElement(functionXmlDocument)
            );
            if (!_configurationService.FunctionList.BooleanFunctions.TryGetValue(functionData.Name, out Function? function))
            {
                SetErrorMessage(string.Format(CultureInfo.CurrentCulture, Strings.functionNotConfiguredFormat, functionData.Name));
                return;
            }

            if (!_functionHelper.IsBoolean(function))
            {
                SetErrorMessage(string.Format(CultureInfo.CurrentCulture, Strings.returnTypeMustBeBooleanFormat, functionData.Name));
                return;
            }

            if (_functionHelper.IsDialog(function))
            {
                SetErrorMessage(string.Format(CultureInfo.CurrentCulture, Strings.dialogFunctionsInvalidForBooleanFunctionFormat, functionData.Name));
                return;
            }

            _treeViewXmlDocumentHelper.LoadXmlDocument(functionXmlDocument.OuterXml);
        }

        private void SetUpSelectFunctionDropDownList()
        {
            _radDropDownListHelper.LoadTextItems
            (
                cmbSelectFunction.DropDownList,
                _configurationService.FunctionList.BooleanFunctions.Select(f => f.Key).Order(),
                Telerik.WinControls.RadDropDownStyle.DropDown
            );

            if (XmlDocument.DocumentElement == null)
                return;

            FunctionData functionData = _functionDataParser.Parse
            (
                _xmlDocumentHelpers.GetDocumentElement(XmlDocument)
            );

            SetFunctionName(functionData.Name);
        }

        #region Event Handlers
        private void ApplicationDropDownList_ApplicationChanged(object? sender, ApplicationChangedEventArgs e)
        {
            _application = e.Application;
            ApplicationChanged?.Invoke(this, e);
        }

        private void CmbSelectFunction_Changed(object? sender, EventArgs e) => CmbSelectFunctionChanged();
        #endregion Event Handlers
    }
}
