using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList
{
    internal partial class EditParameterLiteralListForm : Telerik.WinControls.UI.RadForm, IEditParameterLiteralListForm
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IDataGraphEditingFormEventsHelper _dataGraphEditingFormEventsHelper;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IEditLiteralListCommandFactory _editLiteralListCommandFactory;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly IParametersDataTreeBuilder _parametersDataTreeBuilder;
        private readonly IRefreshVisibleTextHelper _refreshVisibleTextHelper;
        private readonly ITreeViewXmlDocumentHelper _treeViewXmlDocumentHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private ApplicationTypeInfo _application;
        private readonly Type assignedTo;
        private readonly LiteralListParameterElementInfo literalListInfo;

        public EditParameterLiteralListForm(
            IDialogFormMessageControl dialogFormMessageControl,
            IEditingFormHelperFactory editingFormHelperFactory,
            IEditLiteralListCommandFactory editLiteralListCommandFactory,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IRefreshVisibleTextHelper refreshVisibleTextHelper,
            IServiceFactory serviceFactory,
            IXmlDocumentHelpers xmlDocumentHelpers,
            Type assignedTo,
            LiteralListParameterElementInfo literalListInfo,
            XmlDocument literalListXmlDocument)
        {
            InitializeComponent();
            _dialogFormMessageControl = dialogFormMessageControl;//_applicationDropDownList may try to set messages so do this first
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _editLiteralListCommandFactory = editLiteralListCommandFactory;
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            _refreshVisibleTextHelper = refreshVisibleTextHelper;
            _treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper(SchemaName.ParametersDataSchema);
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.assignedTo = assignedTo;this.Text = string.Format(CultureInfo.CurrentCulture, Strings.objectFormsAssignToFormat, this.Text, this.assignedTo.ToString());
            
            this.literalListInfo = literalListInfo;

            _treeViewXmlDocumentHelper.LoadXmlDocument(literalListXmlDocument.OuterXml);
            _dataGraphEditingFormEventsHelper = editingFormHelperFactory.GetDataGraphEditingFormEventsHelper(this);
            _parametersDataTreeBuilder = editingFormHelperFactory.GetParametersDataTreeBuilder(this);
            Initialize();
        }

        public bool DenySpecialCharacters => false;

        public bool DisplayNotCheckBox => false;

        public RadPanel RadPanelFields => radPanelFields;

        public RadTreeView TreeView => radTreeView1;

        public string VisibleText => XmlResult.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value;

        public XmlDocument XmlDocument => _treeViewXmlDocumentHelper.XmlTreeDocument;

        public XmlElement XmlResult 
            => _refreshVisibleTextHelper.RefreshLiteralListVisibleTexts
            (
                _xmlDocumentHelpers.GetDocumentElement(XmlDocument),
                Application
            );

        public Type AssignedTo => assignedTo;

        public IDictionary<string, string> ExpandedNodes { get; } = new Dictionary<string, string>();

        public ApplicationTypeInfo Application => _application ?? throw _exceptionHelper.CriticalException("{78430B81-8E77-44CE-834C-D5EFC7AC92FF}");

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

        public void SetMessage(string message, string title = "") => _dialogFormMessageControl.SetMessage(message, title);

        public void ValidateXmlDocument() => _treeViewXmlDocumentHelper.ValidateXmlDocument();

        private static void AddButtonClickCommand(RadButton radButton, IClickCommand command)
        {
            radButton.Click += (sender, args) => command.Execute();
        }

        private void Initialize()
        {
            InitializeDialogFormMessageControl();
            InitializeApplicationDropDownList();

            _applicationDropDownList.ApplicationChanged += ApplicationDropDownList_ApplicationChanged;

            _formInitializer.SetFormDefaults(this, 719);
            btnCancel.CausesValidation = false;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            _formInitializer.SetToEditSize(this);

            _dataGraphEditingFormEventsHelper.Setup();
            AddButtonClickCommand(btnPasteXml, _editLiteralListCommandFactory.GetEditParameterLiteralListFormXmlCommand(this));
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

        private void LoadTreeview()
        {
            _parametersDataTreeBuilder.CreateLiteralListTreeProfile(TreeView, XmlDocument, assignedTo, literalListInfo);
            if (TreeView.SelectedNode == null)
                TreeView.SelectedNode = TreeView.Nodes[0];
        }

        #region Event Handlers
        private void ApplicationDropDownList_ApplicationChanged(object? sender, ApplicationChangedEventArgs e)
        {
            _application = e.Application;
            ApplicationChanged?.Invoke(this, e);
        }
        #endregion Event Handlers
    }
}
