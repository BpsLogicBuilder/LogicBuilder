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
using System.Linq;

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

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private static void CollapsePanelBorder(RadScrollablePanel radPanel)
            => radPanel.PanelElement.Border.Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            InitializeTableLayoutPanel();
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

        private void InitializeTableLayoutPanel()
        {
            float size_20 = 20F / 148 * 100;
            float size_30 = 30F / 148 * 100;
            float size_6 = 6F / 148 * 100;

            ((ISupportInitialize)this.radPanelTableParent).BeginInit();
            this.radPanelTableParent.SuspendLayout();

            this.tableLayoutPanel.RowStyles[0] = new RowStyle(SizeType.Percent, size_20);
            this.tableLayoutPanel.RowStyles[1] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[2] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[3] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[4] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[5] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[6] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[7] = new RowStyle(SizeType.Percent, size_20);

            ((ISupportInitialize)this.radPanelTableParent).EndInit();
            this.radPanelTableParent.ResumeLayout(true);
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
