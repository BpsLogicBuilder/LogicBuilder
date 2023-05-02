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
    internal partial class ArrayIndexerConfigurationControl : UserControl, IArrayIndexerConfigurationControl
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IIntellisenseVariableControlsValidator _intellisenseVariableControlsValidator;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly ITypeAutoCompleteManager _cmbCastVariableAsTypeAutoCompleteManager;
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;

        private readonly IConfiguredItemHelperForm configuredItemHelperForm;

        public ArrayIndexerConfigurationControl(
            IExceptionHelper exceptionHelper,
            IIntellisenseCustomConfigurationValidatorFactory intellisenseCustomConfigurationValidatorFactory,
            IRadDropDownListHelper radDropDownListHelper,
            IServiceFactory serviceFactory,
            ITypeHelper typeHelper,
            ITypeLoadHelper typeLoadHelper,
            IConfiguredItemHelperForm configuredItemHelperForm)
        {
            InitializeComponent();
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
            if (treeNode is not ArrayIndexerTreeNode arrayIndexerTreeNode)
                throw _exceptionHelper.CriticalException("{0FCBEE2F-6FFA-46BF-88A3-ACE6492C7601}");

            RemoveEventHandlers();
            txtMemberName.Text = arrayIndexerTreeNode.MemberText;
            cmbCastVariableAs.Text = arrayIndexerTreeNode.CastVariableDefinition;
            cmbVariableCategory.SelectedValue = arrayIndexerTreeNode.VariableCategory;
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

            if (treeNode is not ArrayIndexerTreeNode arrayIndexerTreeNode)
                throw _exceptionHelper.CriticalException("{D6582314-B0CD-402C-9E56-C3263EFC7198}");

            ClearMessage();
            ValidateInputBoxes(arrayIndexerTreeNode);
        }

        private void AddEventHandlers()
        {
            txtMemberName.Validating += TxtMemberName_Validating;
            cmbVariableCategory.SelectedIndexChanged += CmbVariableCategory_SelectedIndexChanged;
            cmbCastVariableAs.TextChanged += CmbCastVariableAs_TextChanged;
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
            CollapsePanelBorder(radPanelArrayItem);
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
                groupBoxArrayItem,
                radPanelArrayItem,
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

            static bool IsInvalidCategory(VariableCategory category)
            {
                return !(
                    category == VariableCategory.VariableArrayIndexer
                    || category == VariableCategory.ArrayIndexer
                );
            }
        }

        private void RemoveEventHandlers()
        {
            txtMemberName.Validating -= TxtMemberName_Validating;
            cmbVariableCategory.SelectedIndexChanged -= CmbVariableCategory_SelectedIndexChanged;
            cmbCastVariableAs.TextChanged -= CmbCastVariableAs_TextChanged;
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

        private void ValidateCastAs(ArrayIndexerTreeNode treeNode)
        {
            _intellisenseVariableControlsValidator.ValidateCastAs(treeNode);
        }

        private void ValidateMemberName()
        {
            ValidateMemberName
            (
                (VariableCategory)cmbVariableCategory.SelectedValue,
                txtMemberName.Text.Trim()
            );

            void ValidateMemberName(VariableCategory variableCategory, string memberName)
            {
                if (variableCategory == VariableCategory.VariableArrayIndexer)
                {
                    ValidateForVariableIndexer(memberName);
                    return;
                }

                string[] variableNames = memberName.Trim().Split(new char[] { MiscellaneousConstants.COMMA }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string index in variableNames)
                {
                    if (!int.TryParse(index, out int arrayIndex) || arrayIndex < 0)
                        throw new LogicBuilderException(Strings.arrayKeyIndexIsInvalid);
                }
            }

            void ValidateForVariableIndexer(string memberName)
            {
                string[] indexes = memberName.Trim().Split(new char[] { MiscellaneousConstants.COMMA }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string index in indexes)
                {
                    if (configuredItemHelperForm.ExistingVariables.TryGetValue(memberName, out VariableBase? variable))
                    {
                        if (!_typeLoadHelper.TryGetSystemType(variable, configuredItemHelperForm.Application, out Type? variableType))
                            throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeForVariableFormat, variable.ObjectTypeString, variable.Name));

                        if (!_typeHelper.AssignableFrom(typeof(int), variableType))
                            throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.variableNotAssignableFormat, variable.Name, typeof(int).ToString()));
                    }
                    else if (int.TryParse(index, out int arrayIndex) && arrayIndex > -1)
                    {
                    }
                    else
                    {
                        throw new LogicBuilderException(Strings.variableArrayIndexIsInvalid);
                    }
                }
            }
        }

        private void ValidateInputBoxes(ArrayIndexerTreeNode treeNode)
        {
            ValidateMemberName();
            ValidateCastAs(treeNode);
        }

        #region Event Handlers
        private void CmbCastVariableAs_TextChanged(object? sender, EventArgs e)
        {
            if (!_typeLoadHelper.TryGetSystemType(cmbCastVariableAs.Text, Application, out Type? _))
                return;

            UpdateTreeNodeOnChange();
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
