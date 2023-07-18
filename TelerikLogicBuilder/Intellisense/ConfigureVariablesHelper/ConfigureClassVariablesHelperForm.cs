using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.CustomConfiguration;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.CustomConfiguration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DialogFormMessageControlHelpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureVariablesHelper
{
    internal partial class ConfigureClassVariablesHelperForm : Telerik.WinControls.UI.RadForm, IConfigureClassVariablesHelperForm
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly IIntellisenseCustomConfigurationControlFactory _intellisenseCustomConfigurationControlFactory;
        private readonly IIntellisenseVariablesFormManager _intellisenseVariablesFormManager;
        private readonly IVariablesManager _variablesManager;

        private ApplicationTypeInfo _application;
        private IList<VariableBase>? selectedVariables;

        public ConfigureClassVariablesHelperForm(
            IDialogFormMessageControlFactory dialogFormMessageControlFactory,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IIntellisenseCustomConfigurationControlFactory intellisenseCustomConfigurationControlFactory,
            IIntellisenseFactory intellisenseFactory,
            IServiceFactory serviceFactory,
            IVariablesManager variablesManager,
            IDictionary<string, VariableBase> existingVariables,
            HelperStatus? helperStatus)
        {
            InitializeComponent();
            _dialogFormMessageControl = dialogFormMessageControlFactory.GetDialogFormMessageControl();//_applicationDropDownList may try to set messages so do this first
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            _intellisenseCustomConfigurationControlFactory = intellisenseCustomConfigurationControlFactory;
            _intellisenseVariablesFormManager = intellisenseFactory.GetIntellisenseVariablesFormManager(this);
            _variablesManager = variablesManager;
            ExistingVariables = existingVariables;

            Initialize();

            TreeView.NodeExpandedChanging -= TreeView_NodeExpandedChanging;
            _intellisenseVariablesFormManager.UpdateSelection(helperStatus);
            TreeView.NodeExpandedChanging += TreeView_NodeExpandedChanging;
        }

        private IIntellisenseConfigurationControl CurrentTreeNodeControl
        {
            get
            {
                if (radPanelFields.Controls.Count != 1)
                    throw _exceptionHelper.CriticalException("{B19D1412-2C57-428C-A5CB-EA8920BE6EFC}");

                return (IIntellisenseConfigurationControl)radPanelFields.Controls[0];
            }
        }

        public IList<VariableBase> Variables => selectedVariables ?? throw _exceptionHelper.CriticalException("{7CC4F380-C1BD-4DEE-9AE5-67EFCEFE1F68}");

        public RadButton BtnOk => btnOk;

        public ApplicationDropDownList CmbApplication => (ApplicationDropDownList)_applicationDropDownList;

        public AutoCompleteRadDropDownList CmbClass => cmbClass;

        public RadDropDownList CmbReferenceCategory => cmbReferenceCategory;

        public IDictionary<string, VariableBase> ExistingVariables { get; }

        public HelperStatus HelperStatus => _intellisenseVariablesFormManager.HelperStatus;

        public ReferenceCategories ReferenceCategory => (ReferenceCategories)cmbReferenceCategory.SelectedValue;

        public VariableTreeNode ReferenceTreeNode => (VariableTreeNode)TreeView.SelectedNode;/*used only for instance and static references*/

        public RadTreeView TreeView => radTreeView1;

        public ApplicationTypeInfo Application => _application ?? throw _exceptionHelper.CriticalException("{453D7548-FA92-4768-9DCC-1F1D40F84AA5}");

        public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        public void ClearMessage() => _dialogFormMessageControl.ClearMessage();

        public void ClearTreeView()
        {
            TreeView.Nodes.Clear();
            ClearFieldControls();
            ValidateOk();
        }

        public void SetErrorMessage(string message)
           => _dialogFormMessageControl.SetErrorMessage(message);

        public void SetMessage(string message, string title = "")
            => _dialogFormMessageControl.SetMessage(message, title);

        public void UpdateSelectedVariableConfiguration(CustomVariableConfiguration customVariableConfiguration)
        {
            _intellisenseVariablesFormManager.UpdateSelectedVariableConfiguration(customVariableConfiguration);
        }

        public void ValidateOk()
        {
            if (this.Disposing || this.IsDisposed) return;
            //ValidateOk() can be called on changed for components deing disposed.

            ClearMessage();
            if (TreeView.SelectedNode == null)
            {
                btnOk.Enabled = false;
                return;
            }

            try
            {
                CurrentTreeNodeControl.ValidateFields();
            }
            catch (LogicBuilderException ex)
            {
                SetErrorMessage(ex.Message);
                btnOk.Enabled = false;
                return;
            }

            IList<VariableTreeNode> childNodes = GetChildNodes();
            if (childNodes.Count == 0)
            {
                btnOk.Enabled = false;
                return;
            }

            selectedVariables = childNodes.Select
            (
                treeNode => _variablesManager.GetVariable
                (
                    treeNode.MemberText,
                    treeNode.VariableCategory,
                    treeNode.CastVariableDefinition,
                    _intellisenseVariablesFormManager.TypeName,
                    _intellisenseVariablesFormManager.ReferenceName,
                    _intellisenseVariablesFormManager.ReferenceDefinition,
                    _intellisenseVariablesFormManager.CastReferenceAs,
                    _intellisenseVariablesFormManager.ReferenceCategory,
                    treeNode.MemberInfo,
                    treeNode.MemberType
                )
            ).ToArray();

            btnOk.Enabled = true;

            IList<VariableTreeNode> GetChildNodes()
            {
                VariableTreeNode selectedNode = (VariableTreeNode)TreeView.SelectedNode;
                ReferenceCategories referenceCategory = ReferenceCategory;
                return (referenceCategory == ReferenceCategories.InstanceReference || referenceCategory == ReferenceCategories.StaticReference)
                    ? selectedNode.Nodes.Cast<VariableTreeNode>().ToArray()
                    : TreeView.Nodes.Cast<VariableTreeNode>().ToArray();
            }
        }

        private void ClearFieldControls()
        {
            foreach (Control control in radPanelFields.Controls)
            {
                control.Visible = false;
                if (!control.IsDisposed)
                    control.Dispose();
            }

            radPanelFields.Controls.Clear();
        }

        private void ClearTreeViewImageLists()
        {
            TreeView.ImageList = null;
            if (TreeView.RadContextMenu != null)
                TreeView.RadContextMenu.ImageList = null;
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private static void CollapsePanelBorder(RadScrollablePanel radPanel)
            => radPanel.PanelElement.Border.Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            InitializeTableLayoutPanel();
            InitializeDialogFormMessageControl();
            InitializeApplicationDropDownList();

            Disposed += ConfigureClassVariablesHelperForm_Disposed;
            _applicationDropDownList.ApplicationChanged += ApplicationDropDownList_ApplicationChanged;
            CmbClass.TextChanged += CmbClass_TextChanged;
            CmbReferenceCategory.SelectedIndexChanged += CmbReferenceCategory_SelectedIndexChanged;
            TreeView.NodeExpandedChanging += TreeView_NodeExpandedChanging;
            TreeView.SelectedNodeChanged += TreeView_SelectedNodeChanged;

            _intellisenseVariablesFormManager.Initialize();

            _formInitializer.SetFormDefaults(this, 717);
            _formInitializer.SetToConfigSize(this);

            btnCancel.CausesValidation = false;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            CollapsePanelBorder(radPanelApplication);
            CollapsePanelBorder(radPanelBottom);
            CollapsePanelBorder(radPanelButtons);
            CollapsePanelBorder(radPanelFields);
            CollapsePanelBorder(radPanelMessages);
            CollapsePanelBorder(radPanelSource);
            CollapsePanelBorder(radPanelTableParent);
        }

        private void InitializeApplicationDropDownList()
        {
            ControlsLayoutUtility.LayoutApplicationPanel(radPanelApplication, _applicationDropDownList);
        }

        private void InitializeDialogFormMessageControl()
        {
            ControlsLayoutUtility.LayoutBottomPanel(radPanelBottom, radPanelMessages, radPanelButtons, tableLayoutPanelButtons, _dialogFormMessageControl);
        }

        private void InitializeTableLayoutPanel()
        {
            this.SuspendLayout();
            ControlsLayoutUtility.LayoutControls
            (
                radGroupBoxSource,
                radPanelSource,
                radPanelTableParent,
                tableLayoutPanel,
                3,
                false
            );

            //must adjust height because radGroupBoxSource.Dock is not Fill.
            ControlsLayoutUtility.LayoutThreeRowGroupBox(this, radGroupBoxSource, false);
            this.ResumeLayout(true);
        }

        private void Navigate(Control newEditingControl)
        {
            NavigationUtility.Navigate(this.Handle, radPanelFields, newEditingControl);
        }

        private void Navigate(RadTreeNode treeNode)
        {
            if (treeNode is ArrayIndexerTreeNode)
                Navigate((Control)_intellisenseCustomConfigurationControlFactory.GetArrayIndexerConfigurationControl(this));
            else if (treeNode is FieldTreeNode)
                Navigate((Control)_intellisenseCustomConfigurationControlFactory.GetFieldConfigurationControl(this));
            else if (treeNode is IndexerTreeNode)
                Navigate((Control)_intellisenseCustomConfigurationControlFactory.GetIndexerConfigurationControl(this));
            else if (treeNode is PropertyTreeNode)
                Navigate((Control)_intellisenseCustomConfigurationControlFactory.GetPropertyConfigurationControl(this));
            else
                throw _exceptionHelper.CriticalException("{A5FE2D19-5A01-4B80-8839-D86A5EFE6735}");
        }

        private void RemoveEventHandlers()
        {
            _applicationDropDownList.ApplicationChanged -= ApplicationDropDownList_ApplicationChanged;
            CmbClass.TextChanged -= CmbClass_TextChanged;
            CmbReferenceCategory.SelectedIndexChanged -= CmbReferenceCategory_SelectedIndexChanged;
            TreeView.NodeExpandedChanging -= TreeView_NodeExpandedChanging;
            TreeView.SelectedNodeChanged -= TreeView_SelectedNodeChanged;
        }

        private void SetControlValues(RadTreeNode treeNode)
        {
            if (treeNode == null)
                return;

            Navigate(treeNode);
            CurrentTreeNodeControl.SetControlValues(treeNode);
        }

        #region Event Handlers
        private void ApplicationDropDownList_ApplicationChanged(object? sender, ApplicationChangedEventArgs e)
        {
            _application = e.Application;
            ApplicationChanged?.Invoke(this, e);

            _intellisenseVariablesFormManager.ApplicationChanged();
        }

        private void CmbClass_TextChanged(object? sender, EventArgs e)
        {
            _intellisenseVariablesFormManager.CmbClassTextChanged();
        }

        private void CmbReferenceCategory_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            _intellisenseVariablesFormManager.CmbReferenceCategorySelectedIndexChanged();
        }

        private void ConfigureClassVariablesHelperForm_Disposed(object? sender, EventArgs e)
        {
            RemoveEventHandlers();
            ClearTreeViewImageLists();
        }

        private void TreeView_NodeExpandedChanging(object sender, RadTreeViewCancelEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (e.Node.Expanded)
            {
                _intellisenseVariablesFormManager.BeforeCollapse((BaseTreeNode)e.Node);
            }
            else
            {
                _intellisenseVariablesFormManager.BeforeExpand((BaseTreeNode)e.Node);
            }
            this.Cursor = Cursors.Default;
        }

        private void TreeView_SelectedNodeChanged(object sender, RadTreeViewEventArgs e)
        {
            SetControlValues(e.Node);
            ValidateOk();
        }
        #endregion Event Handlers
    }
}
