using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.CustomConfiguration;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.CustomConfiguration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureFunctionsHelper
{
    internal partial class ConfigureFunctionsHelperForm : Telerik.WinControls.UI.RadForm, IConfigureFunctionsHelperForm
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IChildConstructorFinderFactory _childConstructorFinderFactory;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly IFunctionManager _functionManager;
        private readonly IIntellisenseCustomConfigurationControlFactory _intellisenseCustomConfigurationControlFactory;
        private readonly IIntellisenseFunctionsFormManager _intellisenseFunctionsFormManager;
        private readonly IMultipleChoiceParameterValidator _multipleChoiceParameterValidator;
        private readonly ITypeHelper _typeHelper;

        private ApplicationTypeInfo _application;
        private readonly IDictionary<string, Constructor> originalConstructors;
        
        private Function? selectedFunction;

        public ConfigureFunctionsHelperForm(
            IChildConstructorFinderFactory childConstructorFinderFactory,
            IDialogFormMessageControl dialogFormMessageControl,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IFunctionManager functionManager,
            IIntellisenseCustomConfigurationControlFactory intellisenseCustomConfigurationControlFactory,
            IIntellisenseFactory intellisenseFactory,
            IMultipleChoiceParameterValidator multipleChoiceParameterValidator,
            IServiceFactory serviceFactory,
            ITypeHelper typeHelper,
            IDictionary<string, Constructor> existingConstructors,
            IDictionary<string, VariableBase> existingVariables,
            HelperStatus? helperStatus)
        {
            InitializeComponent();
            this.originalConstructors = existingConstructors;
            
            _dialogFormMessageControl = dialogFormMessageControl;//_applicationDropDownList may try to set messages so do this first
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _childConstructorFinderFactory = childConstructorFinderFactory;
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            _functionManager = functionManager;
            _intellisenseCustomConfigurationControlFactory = intellisenseCustomConfigurationControlFactory;
            _intellisenseFunctionsFormManager = intellisenseFactory.GetIntellisenseFunctionsFormManager(this);
            _multipleChoiceParameterValidator = multipleChoiceParameterValidator;
            _typeHelper = typeHelper;
            ExistingVariables = existingVariables;
            Initialize();

            TreeView.NodeExpandedChanging -= TreeView_NodeExpandedChanging;
            _intellisenseFunctionsFormManager.UpdateSelection(helperStatus);
            TreeView.NodeExpandedChanging += TreeView_NodeExpandedChanging;
        }

        private IIntellisenseConfigurationControl CurrentTreeNodeControl => (IIntellisenseConfigurationControl)radPanelFields.Controls[0];

        public RadButton BtnOk => btnOk;

        public ApplicationDropDownList CmbApplication => (ApplicationDropDownList)_applicationDropDownList;

        public AutoCompleteRadDropDownList CmbClass => cmbClass;

        public RadDropDownList CmbReferenceCategory => cmbReferenceCategory;

        public IDictionary<string, VariableBase> ExistingVariables { get; }

        public ReferenceCategories ReferenceCategory => (ReferenceCategories)cmbReferenceCategory.SelectedValue;

        public VariableTreeNode ReferenceTreeNode => (VariableTreeNode)((BaseTreeNode)TreeView.SelectedNode).ParentNode!;/*used only for instance and static references*/

        public RadTreeView TreeView => radTreeView1;

        public ApplicationTypeInfo Application => _application ?? throw _exceptionHelper.CriticalException("{532AB4D1-25D4-4C15-8F8B-DDA12D21FA05}");

        public Function Function => selectedFunction ?? throw _exceptionHelper.CriticalException("{619171E0-4371-4E81-96A3-3DBE8481A69E}");

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
            _intellisenseFunctionsFormManager.UpdateSelectedVariableConfiguration(customVariableConfiguration);
        }

        public void ValidateOk()
        {
            ClearMessage();
            listNewConstructors.Items.Clear();
            if (TreeView.SelectedNode == null 
                || TreeView.SelectedNode is not FunctionTreeNode)
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

            FunctionTreeNode treeNode = (FunctionTreeNode)TreeView.SelectedNode;

            if (_multipleChoiceParameterValidator.ValidateMultipleChoiceParameter(treeNode.MInfo))
            {
                SetErrorMessage(string.Format(CultureInfo.CurrentCulture, Strings.multipleChoiceParamNotLastFormat, treeNode.MInfo.Name));
                btnOk.Enabled = false;
                return;
            }

            IDictionary<string, Constructor> constructors = new Dictionary<string, Constructor>(originalConstructors);
            IChildConstructorFinder childConstructorFinder = _childConstructorFinderFactory.GetChildConstructorFinder
            (
                constructors
            );

            childConstructorFinder.AddChildConstructors
            (
               treeNode.MInfo
                    .GetParameters()
                    .Where(p => !_typeHelper.IsValidConnectorList(p.ParameterType))
                    .ToArray()
            );

            SortedDictionary<string, Constructor> sortedDictionary = new
            (
                constructors
                    .Where(c => !this.originalConstructors.ContainsKey(c.Key))
                    .ToDictionary(k => k.Key, v => v.Value)
            );

            listNewConstructors.Items.AddRange(sortedDictionary.Values.Select(i => new RadListDataItem(i.ToString(), i)));

            this.selectedFunction = _functionManager.GetFunction
            (
                _intellisenseFunctionsFormManager.TypeName,
                _intellisenseFunctionsFormManager.ReferenceName,
                _intellisenseFunctionsFormManager.ReferenceDefinition,
                _intellisenseFunctionsFormManager.CastReferenceAs,
                _intellisenseFunctionsFormManager.ReferenceCategory,
                ParametersLayout.Sequential,
                treeNode.MInfo
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

            _intellisenseFunctionsFormManager.Initialize();

            _formInitializer.SetFormDefaults(this, 717);

            btnCancel.CausesValidation = false;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            CollapsePanelBorder(radPanelApplication);
            CollapsePanelBorder(radPanelBottom);
            CollapsePanelBorder(radPanelButtons);
            CollapsePanelBorder(radPanelFields);
            CollapsePanelBorder(radPanelMessages);
            CollapsePanelBorder(radPanelNewConstructors);
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
            else if (treeNode is FunctionTreeNode)
                Navigate((Control)_intellisenseCustomConfigurationControlFactory.GetFunctionConfigurationControl());
            else if (treeNode is IndexerTreeNode)
                Navigate((Control)_intellisenseCustomConfigurationControlFactory.GetIndexerConfigurationControl(this));
            else if (treeNode is PropertyTreeNode)
                Navigate((Control)_intellisenseCustomConfigurationControlFactory.GetPropertyConfigurationControl(this));
            else
                throw _exceptionHelper.CriticalException("{ADAAFF7B-89AA-4C7F-9AA4-8A60E96B3018}");
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

            _intellisenseFunctionsFormManager.ApplicationChanged();
        }

        private void CmbClass_TextChanged(object? sender, EventArgs e)
        {
            _intellisenseFunctionsFormManager.CmbClassTextChanged();
        }

        private void CmbReferenceCategory_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            _intellisenseFunctionsFormManager.CmbReferenceCategorySelectedIndexChanged();
        }

        private void TreeView_NodeExpandedChanging(object sender, RadTreeViewCancelEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (e.Node.Expanded)
            {
                _intellisenseFunctionsFormManager.BeforeCollapse((BaseTreeNode)e.Node);
            }
            else
            {
                _intellisenseFunctionsFormManager.BeforeExpand((BaseTreeNode)e.Node);
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
