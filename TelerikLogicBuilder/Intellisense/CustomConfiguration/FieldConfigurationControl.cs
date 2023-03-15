using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.CustomConfiguration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.CustomConfiguration
{
    internal partial class FieldConfigurationControl : UserControl, IFieldConfigurationControl
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IIntellisenseVariableControlsValidator _intellisenseVariableControlsValidator;
        private readonly ITypeAutoCompleteManager _cmbCastVariableAsTypeAutoCompleteManager;

        private readonly IConfiguredItemHelperForm configuredItemHelperForm;

        public FieldConfigurationControl(
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
            if (treeNode is not FieldTreeNode fieldTreeNode)
                throw _exceptionHelper.CriticalException("{6DC2BD32-93C4-4BE4-8EA9-A2E4EA93CC4A}");

            RemoveEventHandlers();
            cmbCastVariableAs.Text = fieldTreeNode.CastVariableDefinition;
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

            if (treeNode is not FieldTreeNode fieldTreeNode)
                throw _exceptionHelper.CriticalException("{BE794351-4FF2-4FF3-A0A4-25C1683A1C89}");

            ClearMessage();
            ValidateInputBoxes(fieldTreeNode);
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
            CollapsePanelBorder(radPanelField);
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
            ControlsLayoutUtility.LayoutControls
            (
                groupBoxField,
                radPanelField,
                radPanelTableParent,
                tableLayoutPanel,
                1
            );
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

        private void ValidateCastAs(FieldTreeNode treeNode)
        {
            _intellisenseVariableControlsValidator.ValidateCastAs(treeNode);
        }

        private void ValidateInputBoxes(FieldTreeNode treeNode)
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
