using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.CustomConfiguration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.CustomConfiguration
{
    internal partial class IndexerConfigurationControl : UserControl, IIndexerConfigurationControl
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IIntellisenseVariableControlsValidator _intellisenseVariableControlsValidator;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly ITypeAutoCompleteManager _cmbCastVariableAsTypeAutoCompleteManager;
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;

        private readonly IConfiguredItemHelperForm configuredItemHelperForm;

        public IndexerConfigurationControl(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IIntellisenseCustomConfigurationValidatorFactory intellisenseCustomConfigurationValidatorFactory,
            IRadDropDownListHelper radDropDownListHelper,
            IServiceFactory serviceFactory,
            ITypeHelper typeHelper,
            ITypeLoadHelper typeLoadHelper,
            IConfiguredItemHelperForm configuredItemHelperForm)
        {
            InitializeComponent();
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _intellisenseVariableControlsValidator = intellisenseCustomConfigurationValidatorFactory.GetIntellisenseVariableControlsValidator(this);
            _radDropDownListHelper = radDropDownListHelper;
            _typeHelper = typeHelper;
            _typeLoadHelper = typeLoadHelper;
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
            if (treeNode is not IndexerTreeNode indexerTreeNode)
                throw _exceptionHelper.CriticalException("{B6A4A71B-3C41-48AA-A758-B2961200B795}");

            RemoveEventHandlers();
            txtMemberName.Text = indexerTreeNode.MemberText;
            cmbCastVariableAs.Text = indexerTreeNode.CastVariableDefinition;
            cmbVariableCategory.SelectedValue = indexerTreeNode.VariableCategory;
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

            if (treeNode is not IndexerTreeNode indexerTreeNode)
                throw _exceptionHelper.CriticalException("{53516912-F00F-4505-9FFE-CC4BCE08F8BC}");

            ClearMessage();
            ValidateInputBoxes(indexerTreeNode);
        }

        private void AddEventHandlers()
        {
            txtMemberName.Validating += TxtMemberName_Validating;
            cmbVariableCategory.SelectedIndexChanged += CmbVariableCategory_SelectedIndexChanged;
            cmbCastVariableAs.TextChanged += CmbCastVariableAs_TextChanged;
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
            CollapsePanelBorder(radPanelIndexer);
            CollapsePanelBorder(radPanelTableParent);
            InitializeVariableControls();
            LoadVariableDropDownLists();
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
                groupBoxIndexer,
                radPanelIndexer,
                radPanelTableParent,
                tableLayoutPanel,
                3
            );
        }

        private void InitializeVariableControls()
        {
            helpProvider.SetHelpString(this.txtMemberName, Strings.varConfigVariableNameHelp);
            helpProvider.SetHelpString(this.cmbVariableCategory, Strings.varConfigVariableCategoryHelp);
            helpProvider.SetHelpString(this.cmbCastVariableAs, Strings.varConfigCastVariableAsHelp);
            toolTip.SetToolTip(this.lblMemberName, Strings.varConfigVariableNameHelp);
            toolTip.SetToolTip(this.lblVariableCategory, Strings.varConfigVariableCategoryHelp);
            toolTip.SetToolTip(this.lblCastVariableAs, Strings.varConfigCastVariableAsHelp);
        }

        private void LoadVariableDropDownLists()
        {
            _radDropDownListHelper.LoadComboItems
            (
                cmbVariableCategory,
                RadDropDownStyle.DropDownList,
                Enum.GetValues<VariableCategory>()
                    .Where(IsInvalidCategory)
                    .ToArray()
            );

            bool IsInvalidCategory(VariableCategory category)
            {
                return !(
                    category == VariableCategory.VariableKeyIndexer
                    || category == _enumHelper.GetIndexVariableCategory(((IndexerTreeNode)configuredItemHelperForm.TreeView.SelectedNode).IndexType)
                );
            }
        }

        private void RemoveEventHandlers()
        {
            txtMemberName.Validating -= TxtMemberName_Validating;
            cmbVariableCategory.SelectedIndexChanged -= CmbVariableCategory_SelectedIndexChanged;
            cmbCastVariableAs.TextChanged -= CmbCastVariableAs_TextChanged;
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
                    (VariableCategory)cmbVariableCategory.SelectedValue,
                    cmbCastVariableAs.Text.Trim().Length == 0 ? MiscellaneousConstants.TILDE : cmbCastVariableAs.Text.Trim(),
                    txtMemberName.Text.Trim().Length == 0 ? MiscellaneousConstants.TILDE : txtMemberName.Text.Trim()
                )
            );
        }

        private void ValidateCastAs(IndexerTreeNode treeNode)
        {
            _intellisenseVariableControlsValidator.ValidateCastAs(treeNode);
        }

        private void ValidateMemberName(IndexerTreeNode treeNode)
        {
            ValidateMemberName
            (
                (VariableCategory)cmbVariableCategory.SelectedValue,
                txtMemberName.Text.Trim()
            );

            void ValidateMemberName(VariableCategory variableCategory, string memberName)
            {
                if (variableCategory == VariableCategory.VariableKeyIndexer)
                {
                    ValidateForVariableKeyIndexer(memberName);
                    return;
                }

                if (string.IsNullOrEmpty(memberName))
                    return;

                if (!_typeHelper.TryParse(memberName, treeNode.IndexType, out object? _))
                {
                    throw new LogicBuilderException
                    (
                        string.Format
                        (
                            CultureInfo.CurrentCulture,
                            Strings.memberNameIsInvalidForVariableCategoryFormat,
                            memberName,
                            _enumHelper.GetEnumResourceString(Enum.GetName(variableCategory))
                        )
                    );
                }
            }

            void ValidateForVariableKeyIndexer(string memberName)
            {
                if (!configuredItemHelperForm.ExistingVariables.TryGetValue(memberName, out VariableBase? variable))
                    throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.cannotEvaluateVariableFormat, memberName));

                if (!_typeLoadHelper.TryGetSystemType(variable, configuredItemHelperForm.Application, out Type? variableType))
                    throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeForVariableFormat, variable.ObjectTypeString, variable.Name));

                if (!_typeHelper.AssignableFrom(treeNode.IndexType, variableType))
                    throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.variableNotAssignableFormat, variable.Name, treeNode.IndexType.ToString()));
            }
        }

        private void ValidateInputBoxes(IndexerTreeNode treeNode)
        {
            ValidateMemberName(treeNode);
            ValidateCastAs(treeNode);
        }

        #region Event Handlers
        private void CmbCastVariableAs_Validating(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            UpdateTreeNodeOnChange();
        }

        private void CmbCastVariableAs_TextChanged(object? sender, EventArgs e)
        {
            if (!_typeLoadHelper.TryGetSystemType(cmbCastVariableAs.Text, Application, out Type? type))
                return;

            if (configuredItemHelperForm.TreeView.SelectedNode is IndexerTreeNode indexerTreeNode
                && indexerTreeNode.IndexType == typeof(string)
                && indexerTreeNode.CastVariableDefinition != cmbCastVariableAs.Text)
            {
                txtMemberName.Text = cmbCastVariableAs.Text;
            }
        }

        private void CmbVariableCategory_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
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

        private void TxtMemberName_Validating(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            UpdateTreeNodeOnChange();
        }
        #endregion Event Handlers
    }
}
