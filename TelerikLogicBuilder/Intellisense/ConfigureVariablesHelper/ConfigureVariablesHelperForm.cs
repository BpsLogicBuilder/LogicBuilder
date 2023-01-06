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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureVariablesHelper
{
    internal partial class ConfigureVariablesHelperForm : Telerik.WinControls.UI.RadForm, IConfigureVariablesHelperForm
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly IIntellisenseCustomConfigurationControlFactory _intellisenseCustomConfigurationControlFactory;
        private readonly IIntellisenseVariablesFormManager _intellisenseVariablesFormManager;
        private readonly IVariablesManager _variablesManager;

        private ApplicationTypeInfo _application;
        private VariableBase? selectedVariable;

        public ConfigureVariablesHelperForm(
            IDialogFormMessageControl dialogFormMessageControl,
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
            _dialogFormMessageControl = dialogFormMessageControl;//_applicationDropDownList may try to set messages so do this first
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

        private IIntellisenseConfigurationControl CurrentTreeNodeControl => (IIntellisenseConfigurationControl)radPanelFields.Controls[0];

        public VariableBase Variable => selectedVariable ?? throw _exceptionHelper.CriticalException("{9EFAAA1D-ADFD-4F67-8886-0089732C4108}");

        public RadButton BtnOk => btnOk;

        public ApplicationDropDownList CmbApplication => (ApplicationDropDownList)_applicationDropDownList;

        public AutoCompleteRadDropDownList CmbClass => cmbClass;

        public RadDropDownList CmbReferenceCategory => cmbReferenceCategory;

        public IDictionary<string, VariableBase> ExistingVariables { get; }

        public ReferenceCategories ReferenceCategory => (ReferenceCategories)cmbReferenceCategory.SelectedValue;

        public VariableTreeNode ReferenceTreeNode => (VariableTreeNode)((BaseTreeNode)TreeView.SelectedNode).ParentNode!;/*used only for instance and static references*/

        public RadTreeView TreeView => radTreeView1;

        public ApplicationTypeInfo Application => _application ?? throw _exceptionHelper.CriticalException("{B78581C5-1DA1-4D0F-B89B-7FB681EC30B7}");

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
            ClearMessage();
            if (TreeView.SelectedNode == null)
            {
                btnOk.Enabled = false;
                return;
            }

            VariableTreeNode treeNode = (VariableTreeNode)TreeView.SelectedNode;
            ReferenceCategories referenceCategory = (ReferenceCategories)cmbReferenceCategory.SelectedValue;
            if (treeNode.ParentNode == null && (referenceCategory == ReferenceCategories.InstanceReference || referenceCategory == ReferenceCategories.StaticReference))
            {//Instance and static references must be references of somnething.
             //Only This and Type references can be root nodes
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

            this.selectedVariable = _variablesManager.GetVariable
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
            );

            btnOk.Enabled = true;
        }

        private void ClearFieldControls()
        {
            foreach (Control control in radPanelFields.Controls)
                control.Visible = false;

            radPanelFields.Controls.Clear();
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            InitializeDialogFormMessageControl();
            InitializeApplicationDropDownList();

            _applicationDropDownList.ApplicationChanged += ApplicationDropDownList_ApplicationChanged;
            CmbClass.TextChanged += CmbClass_TextChanged;
            CmbReferenceCategory.SelectedIndexChanged += CmbReferenceCategory_SelectedIndexChanged;
            TreeView.NodeExpandedChanging += TreeView_NodeExpandedChanging;
            TreeView.SelectedNodeChanged += TreeView_SelectedNodeChanged;

            _intellisenseVariablesFormManager.Initialize();

            _formInitializer.SetFormDefaults(this, 717);

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
            ((ISupportInitialize)radPanelApplication).BeginInit();
            radPanelApplication.SuspendLayout();

            _applicationDropDownList.Dock = DockStyle.Fill;
            _applicationDropDownList.Location = new Point(0, 0);
            radPanelApplication.Controls.Add((Control)_applicationDropDownList);

            ((ISupportInitialize)radPanelApplication).EndInit();
            radPanelApplication.ResumeLayout(true);
        }

        private void InitializeDialogFormMessageControl()
        {
            ((ISupportInitialize)this.radPanelMessages).BeginInit();
            this.radPanelMessages.SuspendLayout();

            _dialogFormMessageControl.Dock = DockStyle.Fill;
            _dialogFormMessageControl.Location = new Point(0, 0);
            this.radPanelMessages.Controls.Add((Control)_dialogFormMessageControl);

            ((ISupportInitialize)this.radPanelMessages).EndInit();
            this.radPanelMessages.ResumeLayout(true);
        }

        private void Navigate(Control newEditingControl)
        {
            Native.NativeMethods.LockWindowUpdate(this.Handle);
            ((ISupportInitialize)radPanelFields).BeginInit();
            radPanelFields.SuspendLayout();

            ClearFieldControls();
            newEditingControl.Dock = DockStyle.Fill;
            newEditingControl.Location = new Point(0, 0);
            radPanelFields.Controls.Add(newEditingControl);

            ((ISupportInitialize)radPanelFields).EndInit();
            radPanelFields.ResumeLayout(true);

            Native.NativeMethods.LockWindowUpdate(IntPtr.Zero);
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
