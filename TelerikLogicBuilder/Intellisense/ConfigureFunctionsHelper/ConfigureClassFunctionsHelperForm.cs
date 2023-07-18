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
using ABIS.LogicBuilder.FlowBuilder.UserControls.DialogFormMessageControlHelpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureFunctionsHelper
{
    internal partial class ConfigureClassFunctionsHelperForm : Telerik.WinControls.UI.RadForm, IConfigureClassFunctionsHelperForm
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IChildConstructorFinderFactory _childConstructorFinderFactory;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly IFunctionManager _functionManager;
        private readonly IIntellisenseCustomConfigurationControlFactory _intellisenseCustomConfigurationControlFactory;
        private readonly IIntellisenseFunctionsFormManager _intellisenseFunctionsFormManager;
        private readonly IMemberAttributeReader _memberAttributeReader;
        private readonly IMultipleChoiceParameterValidator _multipleChoiceParameterValidator;
        private readonly ITypeHelper _typeHelper;

        private ApplicationTypeInfo _application;
        private readonly IDictionary<string, Constructor> originalConstructors;

        private IList<Function>? selectedFunctions;

        public ConfigureClassFunctionsHelperForm(
            IChildConstructorFinderFactory childConstructorFinderFactory,
            IDialogFormMessageControlFactory dialogFormMessageControlFactory,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IFunctionManager functionManager,
            IIntellisenseCustomConfigurationControlFactory intellisenseCustomConfigurationControlFactory,
            IIntellisenseFactory intellisenseFactory,
            IMemberAttributeReader memberAttributeReader,
            IMultipleChoiceParameterValidator multipleChoiceParameterValidator,
            IServiceFactory serviceFactory,
            ITypeHelper typeHelper,
            IDictionary<string, Constructor> existingConstructors,
            IDictionary<string, VariableBase> existingVariables,
            HelperStatus? helperStatus)
        {
            InitializeComponent();
            this.originalConstructors = existingConstructors;

            _dialogFormMessageControl = dialogFormMessageControlFactory.GetDialogFormMessageControl();//_applicationDropDownList may try to set messages so do this first
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _childConstructorFinderFactory = childConstructorFinderFactory;
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            _functionManager = functionManager;
            _intellisenseCustomConfigurationControlFactory = intellisenseCustomConfigurationControlFactory;
            _intellisenseFunctionsFormManager = intellisenseFactory.GetIntellisenseFunctionsFormManager(this);
            _memberAttributeReader = memberAttributeReader;
            _multipleChoiceParameterValidator = multipleChoiceParameterValidator;
            _typeHelper = typeHelper;
            ExistingVariables = existingVariables;
            Initialize();

            TreeView.NodeExpandedChanging -= TreeView_NodeExpandedChanging;
            _intellisenseFunctionsFormManager.UpdateSelection(helperStatus);
            TreeView.NodeExpandedChanging += TreeView_NodeExpandedChanging;
        }

        private IIntellisenseConfigurationControl CurrentTreeNodeControl
        {
            get
            {
                if (radPanelFields.Controls.Count != 1)
                    throw _exceptionHelper.CriticalException("{91C49DE8-6E3C-400D-9921-7B8A51F4C563}");

                return (IIntellisenseConfigurationControl)radPanelFields.Controls[0];
            }
        }

        public IList<Function> Functions => selectedFunctions ?? throw _exceptionHelper.CriticalException("{6AD3DBD0-5E48-4070-A551-4171FFC49803}");

        public ICollection<Constructor> NewConstructors => listNewConstructors.Items.Select(i => (Constructor)i.Value).ToArray();

        public RadButton BtnOk => btnOk;

        public ApplicationDropDownList CmbApplication => (ApplicationDropDownList)_applicationDropDownList;

        public AutoCompleteRadDropDownList CmbClass => cmbClass;

        public RadDropDownList CmbReferenceCategory => cmbReferenceCategory;

        public IDictionary<string, VariableBase> ExistingVariables { get; }

        public HelperStatus HelperStatus => _intellisenseFunctionsFormManager.HelperStatus;

        public ReferenceCategories ReferenceCategory => (ReferenceCategories)cmbReferenceCategory.SelectedValue;

        public VariableTreeNode ReferenceTreeNode => (VariableTreeNode)TreeView.SelectedNode;/*used only for instance and static references*/

        public RadTreeView TreeView => radTreeView1;

        public ApplicationTypeInfo Application => _application ?? throw _exceptionHelper.CriticalException("{27589C08-2D4A-4DA5-AB2E-ABEA7EEF46D2}");

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
            if (this.Disposing || this.IsDisposed) return;
            //ValidateOk() can be called on changed for components deing disposed.

            ClearMessage();
            listNewConstructors.Items.Clear();
            if (TreeView.SelectedNode == null
                || (MethodIsChildOfReferenceNode() && TreeView.SelectedNode is FunctionTreeNode))
            {
                btnOk.Enabled = false;
                return;
            }

            IDictionary<string, Constructor> constructors = new Dictionary<string, Constructor>(originalConstructors);
            IChildConstructorFinder childConstructorFinder = _childConstructorFinderFactory.GetChildConstructorFinder
            (
                constructors
            );

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

            try
            {
                this.selectedFunctions = GetFunctions();
            }
            catch (LogicBuilderException ex)
            {
                SetErrorMessage(ex.Message);
                btnOk.Enabled = false;
                return;
            }

            if (this.selectedFunctions.Count == 0)
            {
                btnOk.Enabled = false;
                return;
            }

            SortedDictionary<string, Constructor> sortedDictionary = new
            (
                constructors
                    .Where(c => !this.originalConstructors.ContainsKey(c.Key))
                    .ToDictionary(k => k.Key, v => v.Value)
            );
            listNewConstructors.Items.AddRange(sortedDictionary.Values.Select(i => new RadListDataItem(i.ToString(), i)));

            btnOk.Enabled = true;

            IList<FunctionTreeNode> GetChildNodes()
                => MethodIsChildOfReferenceNode()
                    ? TreeView.SelectedNode.Nodes.OfType<FunctionTreeNode>().ToList()
                    : TreeView.Nodes.OfType<FunctionTreeNode>().ToList();
            bool MethodIsChildOfReferenceNode()
                => ReferenceCategory == ReferenceCategories.InstanceReference || ReferenceCategory == ReferenceCategories.StaticReference;
            List<Function> GetFunctions()
            {
                List<Function> list = new();
                foreach (FunctionTreeNode nextChildNode in GetChildNodes())
                {
                    if (!_memberAttributeReader.IsFunctionConfigurableFromClassHelper(nextChildNode.MInfo))
                        continue;

                    if (!_multipleChoiceParameterValidator.ValidateMultipleChoiceParameter(nextChildNode.MInfo))
                        continue;

                    childConstructorFinder.AddChildConstructors
                    (
                        nextChildNode.MInfo
                            .GetParameters()
                            .Where(p => !_typeHelper.IsValidConnectorList(p.ParameterType))
                            .ToArray()
                    );

                    Function? function = _functionManager.GetFunction
                    (
                        _intellisenseFunctionsFormManager.TypeName,
                        _intellisenseFunctionsFormManager.ReferenceName,
                        _intellisenseFunctionsFormManager.ReferenceDefinition,
                        _intellisenseFunctionsFormManager.CastReferenceAs,
                        _intellisenseFunctionsFormManager.ReferenceCategory,
                        ParametersLayout.Sequential,
                        nextChildNode.MInfo
                    );

                    if (function != null)
                        list.Add(function);
                }

                return list;
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
            InitializeFieldsPanel();
            InitializeDialogFormMessageControl();
            InitializeApplicationDropDownList();
            ControlsLayoutUtility.LayoutGroupBox(radPanelNewConstructors, radGroupBoxNewConstructors);

            Disposed += ConfigureClassFunctionsHelperForm_Disposed;
            _applicationDropDownList.ApplicationChanged += ApplicationDropDownList_ApplicationChanged;
            CmbClass.TextChanged += CmbClass_TextChanged;
            CmbReferenceCategory.SelectedIndexChanged += CmbReferenceCategory_SelectedIndexChanged;
            TreeView.NodeExpandedChanging += TreeView_NodeExpandedChanging;
            TreeView.SelectedNodeChanged += TreeView_SelectedNodeChanged;

            _intellisenseFunctionsFormManager.Initialize();

            _formInitializer.SetFormDefaults(this, 870);
            _formInitializer.SetToConfigSize(this);

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
            ControlsLayoutUtility.LayoutApplicationPanel(radPanelApplication, _applicationDropDownList);
        }

        private void InitializeDialogFormMessageControl()
        {
            ControlsLayoutUtility.LayoutBottomPanel(radPanelBottom, radPanelMessages, radPanelButtons, tableLayoutPanelButtons, _dialogFormMessageControl);
        }

        private void InitializeFieldsPanel()
        {
            ((ISupportInitialize)splitPanelRight).BeginInit();
            splitPanelRight.SuspendLayout();
            radPanelFields.Size = new Size(radPanelFields.Width, PerFontSizeConstants.ThreeRowGroupBoxHeight);
            ((ISupportInitialize)splitPanelRight).EndInit();
            splitPanelRight.ResumeLayout(true);
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
            else if (treeNode is FunctionTreeNode)
                Navigate((Control)_intellisenseCustomConfigurationControlFactory.GetFunctionConfigurationControl());
            else if (treeNode is IndexerTreeNode)
                Navigate((Control)_intellisenseCustomConfigurationControlFactory.GetIndexerConfigurationControl(this));
            else if (treeNode is PropertyTreeNode)
                Navigate((Control)_intellisenseCustomConfigurationControlFactory.GetPropertyConfigurationControl(this));
            else
                throw _exceptionHelper.CriticalException("{ADAAFF7B-89AA-4C7F-9AA4-8A60E96B3018}");
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

        private void ConfigureClassFunctionsHelperForm_Disposed(object? sender, EventArgs e)
        {
            RemoveEventHandlers();
            ClearTreeViewImageLists();
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
