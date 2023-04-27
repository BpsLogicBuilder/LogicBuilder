using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions
{
    internal partial class EditVoidFunctionControl : UserControl, IEditVoidFunctionControl
    {
        private readonly IConfigurationService _configurationService;
        private readonly IDataGraphEditingHostEventsHelper _dataGraphEditingHostEventsHelper;
        private readonly IEditVoidFunctionCommandFactory _editVoidFunctionCommandFactory;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IParametersDataTreeBuilder _parametersDataTreeBuilder;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly IRefreshVisibleTextHelper _refreshVisibleTextHelper;
        private readonly ITreeViewXmlDocumentHelper _treeViewXmlDocumentHelper;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IEditFunctionsForm editFunctionsForm;

        public EditVoidFunctionControl(
            IConfigurationService configurationService,
            IEditVoidFunctionCommandFactory editVoidFunctionCommandFactory,
            IEditingFormHelperFactory editingFormHelperFactory,
            IExceptionHelper exceptionHelper,
            IRadDropDownListHelper radDropDownListHelper,
            IRefreshVisibleTextHelper refreshVisibleTextHelper,
            IServiceFactory serviceFactory,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditFunctionsForm editFunctionsForm)
        {
            InitializeComponent();
            _configurationService = configurationService;
            _editVoidFunctionCommandFactory = editVoidFunctionCommandFactory;
            _exceptionHelper = exceptionHelper;
            _radDropDownListHelper = radDropDownListHelper;
            _refreshVisibleTextHelper = refreshVisibleTextHelper;
            _treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper(SchemaName.FunctionDataSchema);
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;

            _dataGraphEditingHostEventsHelper = editingFormHelperFactory.GetDataGraphEditingHostEventsHelper(this);
            _parametersDataTreeBuilder = editingFormHelperFactory.GetParametersDataTreeBuilder(this);

            this.editFunctionsForm = editFunctionsForm;

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

        public bool DisplayNotCheckBox => false;

        public RadPanel RadPanelFields => radPanelFields;

        public RadTreeView TreeView => radTreeView1;

        public XmlDocument XmlDocument => _treeViewXmlDocumentHelper.XmlTreeDocument;

        public XmlElement XmlResult
            => _refreshVisibleTextHelper.RefreshAllVisibleTexts
            (/*Need <assertFunction />, <retractFunction /> and <function /> hance RefreshAllVisibleTexts.*/
                _xmlDocumentHelpers.GetDocumentElement(XmlDocument),
                Application
            );

        public Type AssignedTo => typeof(object);

        public ApplicationTypeInfo Application => editFunctionsForm.Application;

        public IDictionary<string, string> ExpandedNodes { get; } = new Dictionary<string, string>();

        public string VisibleText => XmlResult.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value;

        public IDictionary<string, Function> FunctionDictionary => editFunctionsForm.FunctionDictionary;

        public IList<TreeFolder> TreeFolders => editFunctionsForm.TreeFolders;

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

        public void ClearMessage() => editFunctionsForm.ClearMessage();

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

        public void SetErrorMessage(string message) => editFunctionsForm.SetErrorMessage(message);

        public void SetFunctionName(string functionName)
        {
            cmbSelectFunction.Changed -= CmbSelectFunction_Changed;
            cmbSelectFunction.Text = functionName;
            cmbSelectFunction.Changed += CmbSelectFunction_Changed;
        }

        public void SetMessage(string message, string title = "") => editFunctionsForm.SetMessage(message, title);

        public void UpdateInputControls(string xmlString)
        {
            _treeViewXmlDocumentHelper.LoadXmlDocument(xmlString);
            LoadTreeview();
            SetFunctionName
            (
                _xmlDocumentHelpers
                    .GetDocumentElement(XmlDocument)
                    .Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value
            );
        }

        public void ValidateXmlDocument()
        {
            //VoidFunction specfic error checks (i.e. not configured, must be void) here should not be necessary
            //New function selections are always checked against _configurationService.FunctionList.VoidFunctions
            _treeViewXmlDocumentHelper.ValidateXmlDocument();
        }

        private static void AddButtonClickCommand(HelperButtonDropDownList helperButtonDropDownList, IClickCommand command)
        {
            helperButtonDropDownList.ButtonClick += (sender, args) => command.Execute();
        }

        private void CmbSelectFunctionChanged()
        {
            if (!FunctionDictionary.TryGetValue(cmbSelectFunction.Text, out Function? function))
                return;

            _treeViewXmlDocumentHelper.LoadXmlDocument(GetEmptyFunctioXml());
            LoadTreeview();

            string GetEmptyFunctioXml()
            {
                return function.FunctionCategory switch
                {
                    FunctionCategories.Assert => _xmlDataHelper.BuildEmptyAssertFunctionXml(function.Name),
                    FunctionCategories.Standard or FunctionCategories.RuleChainingUpdate => _xmlDataHelper.BuildEmptyFunctionXml(function.Name, function.Name),
                    FunctionCategories.Retract => _xmlDataHelper.BuildEmptyRetractFunctionXml(function.Name),
                    _ => throw _exceptionHelper.CriticalException("{B213177A-C628-4979-8627-336B5F488163}"),
                };
            }
        }

        private void Initialize()
        {
            InitializeSelectFunctionDropDownList();

            editFunctionsForm.ApplicationChanged += EditFunctionsForm_ApplicationChanged;
            cmbSelectFunction.Changed += CmbSelectFunction_Changed;

            _dataGraphEditingHostEventsHelper.Setup();
            SetUpSelectFunctionDropDownList();
            AddButtonClickCommand(cmbSelectFunction, _editVoidFunctionCommandFactory.GetSelectVoidFunctionCommand(this));
        }

        private void InitializeSelectFunctionDropDownList()
        {
            ControlsLayoutUtility.LayoutSelectConfiguredItemGroupBox(this, radPanelSelectFunction, radGroupBoxSelectFunction, cmbSelectFunction);
        }

        private void LoadTreeview()
        {
            if (XmlDocument.DocumentElement == null)
                return;

            switch(XmlDocument.DocumentElement.Name)
            {
                case XmlDataConstants.ASSERTFUNCTIONELEMENT:
                    _parametersDataTreeBuilder.CreateAssertFunctionTreeProfile(TreeView, XmlDocument);
                    break;
                case XmlDataConstants.FUNCTIONELEMENT:
                    _parametersDataTreeBuilder.CreateFunctionTreeProfile(TreeView, XmlDocument, AssignedTo);
                    break;
                case XmlDataConstants.RETRACTFUNCTIONELEMENT:
                    _parametersDataTreeBuilder.CreateRetractFunctionTreeProfile(TreeView, XmlDocument);
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{059A1C84-0535-426F-81FF-6829AEB49922}");
            }
            
            if (TreeView.SelectedNode == null)
                TreeView.SelectedNode = TreeView.Nodes[0];
        }

        private void SetUpSelectFunctionDropDownList()
        {
            _radDropDownListHelper.LoadTextItems
            (
                cmbSelectFunction.DropDownList,
                FunctionDictionary.Select(f => f.Key).Order(),
                Telerik.WinControls.RadDropDownStyle.DropDown
            );

            if (XmlDocument.DocumentElement == null)
                return;

            SetFunctionName
            (
                _xmlDocumentHelpers
                    .GetDocumentElement(XmlDocument)
                    .Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value
            );
        }

        #region Event Handlers
        private void EditFunctionsForm_ApplicationChanged(object? sender, ApplicationChangedEventArgs e)
        {
            ApplicationChanged?.Invoke(this, e);
        }

        private void CmbSelectFunction_Changed(object? sender, EventArgs e) => CmbSelectFunctionChanged();
        #endregion Event Handlers
    }
}
