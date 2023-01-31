using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.CustomConfiguration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.ComponentModel;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.CustomConfiguration
{
    internal partial class PropertyConfigurationControl : UserControl, IPropertyConfigurationControl
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IIntellisenseVariableControlsValidator _intellisenseVariableControlsValidator;
        private readonly ITypeAutoCompleteManager _cmbCastVariableAsTypeAutoCompleteManager;

        private readonly IConfiguredItemHelperForm configuredItemHelperForm;

        public PropertyConfigurationControl(
            IExceptionHelper exceptionHelper,
            IIntellisenseCustomConfigurationValidatorFactory intellisenseCustomConfigurationValidatorFactory,
            IServiceFactory serviceFactory,
            IConfiguredItemHelperForm configuredItemHelperForm)
        {
            InitializeComponent();
            _exceptionHelper = exceptionHelper;
            _intellisenseVariableControlsValidator = intellisenseCustomConfigurationValidatorFactory.GetIntellisenseVariableControlsValidator(this);
            _cmbCastVariableAsTypeAutoCompleteManager = serviceFactory.GetTypeAutoCompleteManager
            (
                configuredItemHelperForm,
                cmbCastVariableAs
            );
            this.configuredItemHelperForm = configuredItemHelperForm;
            Initialize();
        }

        private readonly HelpProvider helpProvider = new();
        private readonly RadToolTip toolTip = new();

        public ApplicationTypeInfo Application => configuredItemHelperForm.Application;

        public AutoCompleteRadDropDownList CmbCastVariableAs => cmbCastVariableAs;

        public void ClearMessage()
            => this.configuredItemHelperForm.ClearMessage();

        public void SetControlValues(RadTreeNode treeNode)
        {
            if (treeNode is not PropertyTreeNode propertyTreeNode)
                throw _exceptionHelper.CriticalException("{9A785717-7D06-4853-BEF2-AAFD1FD88D5B}");

            RemoveEventHandlers();
            cmbCastVariableAs.Text = propertyTreeNode.CastVariableDefinition;
            AddEventHandlers();
        }

        public void SetErrorMessage(string message)
            => this.configuredItemHelperForm.SetErrorMessage(message);

        public void SetMessage(string message, string title = "")
            => this.configuredItemHelperForm.SetMessage(message, title);

        public void ValidateFields()
        {
            RadTreeNode? treeNode = configuredItemHelperForm.TreeView.SelectedNode;
            if (treeNode == null)
                return;

            if (treeNode is not PropertyTreeNode propertyTreeNode)
                throw _exceptionHelper.CriticalException("{828747F1-DC27-4BFF-9E6B-6B47600294B1}");

            ClearMessage();
            ValidateInputBoxes(propertyTreeNode);
        }

        private void AddEventHandlers()
        {
            cmbCastVariableAs.Validating += CmbCastVariableAs_Validating;
            tableLayoutPanel.Validating += TableLayoutPanel_Validating;
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private static void CollapsePanelBorder(RadScrollablePanel radPanel)
            => radPanel.PanelElement.Border.Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            InitializeTableLayoutPanel();
            configuredItemHelperForm.ApplicationChanged += ConfiguredItemHelperForm_ApplicationChanged;
            CollapsePanelBorder(radPanelProperty);
            CollapsePanelBorder(radPanelTableParent);
            InitializeVariableControls();
            _cmbCastVariableAsTypeAutoCompleteManager.Setup();

            if (!configuredItemHelperForm.Application.AssemblyAvailable)
            {
                SetErrorMessage(configuredItemHelperForm.Application.UnavailableMessage);
            }
        }

        private void InitializeTableLayoutPanel()
        {
            float size_20 = 20F / 76 * 100;
            float size_30 = 30F / 76 * 100;
            float size_6 = 6F / 76 * 100;

            ((ISupportInitialize)this.radPanelTableParent).BeginInit();
            this.radPanelTableParent.SuspendLayout();

            tableLayoutPanel.RowStyles[0] = new RowStyle(SizeType.Percent, size_20);
            tableLayoutPanel.RowStyles[1] = new RowStyle(SizeType.Percent, size_30);
            tableLayoutPanel.RowStyles[2] = new RowStyle(SizeType.Percent, size_6);
            tableLayoutPanel.RowStyles[3] = new RowStyle(SizeType.Percent, size_20);

            ((ISupportInitialize)this.radPanelTableParent).EndInit();
            this.radPanelTableParent.ResumeLayout(true);
        }

        private void InitializeVariableControls()
        {
            helpProvider.SetHelpString(this.cmbCastVariableAs, Strings.varConfigCastVariableAsHelp);
            toolTip.SetToolTip(this.lblCastVariableAs, Strings.varConfigCastVariableAsHelp);
        }

        private void RemoveEventHandlers()
        {
            cmbCastVariableAs.Validating -= CmbCastVariableAs_Validating;
            tableLayoutPanel.Validating -= TableLayoutPanel_Validating;
        }

        private void UpdateTreeNodeOnChange()
        {
            try
            {
                ValidateFields();
            }
            catch (LogicBuilderException ex)
            {
                SetErrorMessage(ex.Message);
                configuredItemHelperForm.BtnOk.Enabled = false;
                return;
            }

            this.configuredItemHelperForm.UpdateSelectedVariableConfiguration
            (
                new CustomVariableConfiguration
                (
                    null,
                    cmbCastVariableAs.Text.Trim().Length == 0 ? MiscellaneousConstants.TILDE : cmbCastVariableAs.Text.Trim(),
                    MiscellaneousConstants.TILDE
                )
            );
        }

        private void ValidateCastAs(PropertyTreeNode treeNode)
        {
            _intellisenseVariableControlsValidator.ValidateCastAs(treeNode);
        }

        private void ValidateInputBoxes(PropertyTreeNode treeNode)
        {
            ValidateCastAs(treeNode);
        }

        #region Event Handlers
        private void CmbCastVariableAs_Validating(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            UpdateTreeNodeOnChange();
        }

        private void ConfiguredItemHelperForm_ApplicationChanged(object? sender, Structures.ApplicationChangedEventArgs e)
        {
            bool assenblyAvailable = configuredItemHelperForm.Application.AssemblyAvailable;
            if (!assenblyAvailable)
                SetErrorMessage(configuredItemHelperForm.Application.UnavailableMessage);
        }

        private void TableLayoutPanel_Validating(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                ValidateFields();
            }
            catch (LogicBuilderException ex)
            {
                SetErrorMessage(ex.Message);
                configuredItemHelperForm.BtnOk.Enabled = false;
                e.Cancel = true;
            }
        }
        #endregion Event Handlers
    }
}
