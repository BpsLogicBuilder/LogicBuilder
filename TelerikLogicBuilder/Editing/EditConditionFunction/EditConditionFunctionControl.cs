using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunction.Factories;
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

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunction
{
    internal partial class EditConditionFunctionControl : UserControl, IEditConditionFunctionControl
    {
        private readonly IConfigurationService _configurationService;
        private readonly IDataGraphEditingHostEventsHelper _dataGraphEditingHostEventsHelper;
        private readonly IEditConditionFunctionCommandFactory _editConditionFunctionCommandFactory;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IParametersDataTreeBuilder _parametersDataTreeBuilder;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly IRefreshVisibleTextHelper _refreshVisibleTextHelper;
        private readonly ITreeViewXmlDocumentHelper _treeViewXmlDocumentHelper;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IApplicationForm parentForm;

        public EditConditionFunctionControl(
            IConfigurationService configurationService,
            IEditConditionFunctionCommandFactory editConditionFunctionCommandFactory,
            IEditingFormHelperFactory editingFormHelperFactory,
            IFunctionDataParser functionDataParser,
            IRadDropDownListHelper radDropDownListHelper,
            IRefreshVisibleTextHelper refreshVisibleTextHelper,
            IServiceFactory serviceFactory,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IApplicationForm parentForm)
        {
            InitializeComponent();
            _configurationService = configurationService;
            _editConditionFunctionCommandFactory = editConditionFunctionCommandFactory;
            _functionDataParser = functionDataParser;
            _radDropDownListHelper = radDropDownListHelper;
            _refreshVisibleTextHelper = refreshVisibleTextHelper;
            _treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper(SchemaName.FunctionDataSchema);
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;

            _dataGraphEditingHostEventsHelper = editingFormHelperFactory.GetDataGraphEditingHostEventsHelper(this);
            _parametersDataTreeBuilder = editingFormHelperFactory.GetParametersDataTreeBuilder(this);

            this.parentForm = parentForm;

            Initialize();
        }

        public HelperButtonDropDownList CmbSelectFunction => cmbSelectFunction;

        public IEditingControl? CurrentEditingControl
        {
            get
            {
                if (RadPanelFields.Controls.Count != 1)
                    return null;

                return (IEditingControl)RadPanelFields.Controls[0];
            }
        }

        public bool DenySpecialCharacters => false;

        public bool DisplayNotCheckBox => true;

        public RadPanel RadPanelFields => radPanelFields;

        public RadTreeView TreeView => radTreeView1;

        public XmlDocument XmlDocument => _treeViewXmlDocumentHelper.XmlTreeDocument;

        public XmlElement XmlResult
            => _refreshVisibleTextHelper.RefreshFunctionVisibleTexts
            (
                _xmlDocumentHelpers.GetDocumentElement(XmlDocument),
                Application
            );

        public Type AssignedTo => typeof(bool);

        public ApplicationTypeInfo Application => parentForm.Application;

        public IDictionary<string, string> ExpandedNodes { get; } = new Dictionary<string, string>();

        public string VisibleText
        {
            get
            {
                FunctionData functionData = _functionDataParser.Parse(XmlResult);
                if (!functionData.IsNotFunction)
                    return functionData.VisibleText;

                return string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.notFromDecisionStringFormat,
                    Strings.notString,
                    Strings.notFromDecisionSeparator,
                    functionData.VisibleText
                );
            }
        }

        public event EventHandler? Changed;

        public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        public void ClearInputControls()
        {
            TreeView.BeginUpdate();
            TreeView.Nodes.Clear();
            TreeView.EndUpdate();
            ClearFieldControls();
            SetFunctionName(string.Empty);
            void ClearFieldControls()
            {
                foreach (Control control in radPanelFields.Controls)
                {
                    control.Visible = false;
                    if (!control.IsDisposed)
                        control.Dispose();
                }

                radPanelFields.Controls.Clear();
            }
        }

        public void ClearMessage() => parentForm.ClearMessage();

        public void DisableControlsDuringEdit(bool disable)
        {
        }

        public void RebuildTreeView() => LoadTreeview();

        public void ReloadXmlDocument(string xmlString) => _treeViewXmlDocumentHelper.LoadXmlDocument(xmlString);

        public void RequestDocumentUpdate(IEditingControl editingControl)
        {
            _dataGraphEditingHostEventsHelper.RequestDocumentUpdate(editingControl);
            if (editingControl?.IsValid == true)
                Changed?.Invoke(this, EventArgs.Empty);
        }

        public void SetErrorMessage(string message) => parentForm.SetErrorMessage(message);

        public void SetFunctionName(string functionName)
        {
            cmbSelectFunction.Changed -= CmbSelectFunction_Changed;
            cmbSelectFunction.Text = functionName;
            cmbSelectFunction.Changed += CmbSelectFunction_Changed;
        }

        public void SetMessage(string message, string title = "") => parentForm.SetMessage(message, title);

        public void UpdateInputControls(string xmlString)
        {
            _treeViewXmlDocumentHelper.LoadXmlDocument(xmlString);
            LoadTreeview();

            SetFunctionName
            (
                _functionDataParser.Parse
                (
                    _xmlDocumentHelpers.ToXmlElement(xmlString)
                ).Name
            );
        }

        public void ValidateXmlDocument()
        {
            //BooleanFunction specfic error checks (i.e. not configured, must be bool) here should not be necessary
            //New function selections are always checked against _configurationService.FunctionList.BooleanFunctions
            _treeViewXmlDocumentHelper.ValidateXmlDocument();
        }

        private static void AddButtonClickCommand(HelperButtonDropDownList helperButtonDropDownList, IClickCommand command)
        {
            helperButtonDropDownList.ButtonClick += (sender, args) => command.Execute();
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
            InitializeSelectFunctionDropDownList();

            parentForm.ApplicationChanged += EditConditionFunctionsForm_ApplicationChanged;
            cmbSelectFunction.Changed += CmbSelectFunction_Changed;

            _dataGraphEditingHostEventsHelper.Setup();
            SetUpSelectFunctionDropDownList();
            AddButtonClickCommand(cmbSelectFunction, _editConditionFunctionCommandFactory.GetSelectConditionFunctionCommand(this));
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

            SetFunctionName
            (
                _functionDataParser.Parse
                (
                    _xmlDocumentHelpers.GetDocumentElement(XmlDocument)
                ).Name
            );
        }

        #region Event Handlers
        private void EditConditionFunctionsForm_ApplicationChanged(object? sender, ApplicationChangedEventArgs e)
        {
            ApplicationChanged?.Invoke(this, e);
        }

        private void CmbSelectFunction_Changed(object? sender, EventArgs e) => CmbSelectFunctionChanged();
        #endregion Event Handlers
    }
}
