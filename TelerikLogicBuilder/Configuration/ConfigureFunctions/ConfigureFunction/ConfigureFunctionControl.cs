using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunction.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Helpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.StateImageSetters;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunction
{
    internal partial class ConfigureFunctionControl : UserControl, IConfigureFunctionControl
    {
        private readonly IConfigureFunctionControlCommandFactory _configureFunctionControlCommandFactory;
        private readonly IConfigureFunctionsStateImageSetter _configureFunctionsStateImageSetter;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFunctionControlsValidator _functionControlsValidator;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly IStringHelper _stringHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly ITypeAutoCompleteManager _txtTypeNameTypeAutoCompleteManager;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureFunctionsForm configureFunctionsForm;

        public ConfigureFunctionControl(
            IConfigureFunctionControlCommandFactory configureFunctionControlCommandFactory,
            IConfigureFunctionsStateImageSetter configureFunctionsStateImageSetter,
            IFunctionControlValidatorFactory functionControlValidatorFactory,
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IRadDropDownListHelper radDropDownListHelper,
            IServiceFactory serviceFactory,
            IStringHelper stringHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureFunctionsForm configureFunctionsForm)
        {
            InitializeComponent();
            
            _configureFunctionControlCommandFactory = configureFunctionControlCommandFactory;
            _configureFunctionsStateImageSetter = configureFunctionsStateImageSetter;
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _functionControlsValidator = functionControlValidatorFactory.GetFunctionControlsValidator(this);
            _radDropDownListHelper = radDropDownListHelper;
            _stringHelper = stringHelper;
            _treeViewService = treeViewService;
            _txtTypeNameTypeAutoCompleteManager = serviceFactory.GetTypeAutoCompleteManager
            (
                configureFunctionsForm,
                txtTypeName
            );
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureFunctionsForm = configureFunctionsForm;
            Initialize();
        }

        private readonly HelpProvider helpProvider = new();
        private readonly RadToolTip toolTip = new();

        public RadDropDownList CmbFunctionCategory => cmbFunctionCategory;
        public RadDropDownList CmbReferenceCategory => cmbReferenceCategory;
        public RadDropDownList CmbReferenceDefinition => cmbReferenceDefinition;
        public RadLabel LblFunctionName => lblFunctionName;
        public RadLabel LblMemberName => lblMemberName;
        public RadLabel LblTypeName => lblTypeName;
        public RadTextBox TxtCastReferenceAs => txtCastReferenceAs;
        public RadTextBox TxtFunctionName => txtFunctionName;
        public RadTextBox TxtMemberName => txtMemberName;
        public RadTextBox TxtReferenceName => txtReferenceName;
        public AutoCompleteRadDropDownList TxtTypeName => txtTypeName;
        public RadTreeView TreeView => configureFunctionsForm.TreeView;
        public XmlDocument XmlDocument => configureFunctionsForm.XmlDocument;

        public void ClearMessage() => configureFunctionsForm.ClearMessage();

        public void SetControlValues(RadTreeNode treeNode)
        {
            if (!_treeViewService.IsMethodNode(treeNode))
                throw _exceptionHelper.CriticalException("{6A99F438-5C43-401A-B7D9-21120EE533CE}");

            RemoveEventHandlers();

            XmlElement functionElement = _xmlDocumentHelpers.SelectSingleElement(XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(functionElement).ToDictionary(e => e.Name);

            txtFunctionName.Text = functionElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE);
            txtFunctionName.Select();
            txtFunctionName.SelectAll();
            txtMemberName.Text = elements[XmlDataConstants.MEMBERNAMEELEMENT].InnerText;
            cmbFunctionCategory.SelectedValue = _enumHelper.ParseEnumText<FunctionCategories>(elements[XmlDataConstants.FUNCTIONCATEGORYELEMENT].InnerText);
            txtTypeName.Text = elements[XmlDataConstants.TYPENAMEELEMENT].InnerText;
            txtReferenceName.Text = elements[XmlDataConstants.REFERENCENAMEELEMENT].InnerText;
            cmbReferenceDefinition.Text = string.Join
            (
                MiscellaneousConstants.PERIODSTRING,
                _enumHelper.ToVisibleDropdownValues
                (
                    _stringHelper.SplitWithQuoteQualifier
                    (
                        elements[XmlDataConstants.REFERENCEDEFINITIONELEMENT].InnerText,
                        MiscellaneousConstants.PERIODSTRING
                    )
                )
            );
            txtCastReferenceAs.Text = elements[XmlDataConstants.CASTREFERENCEASELEMENT].InnerText;
            cmbReferenceCategory.SelectedValue = _enumHelper.ParseEnumText<ReferenceCategories>(elements[XmlDataConstants.REFERENCECATEGORYELEMENT].InnerText);
            cmbParametersLayout.SelectedValue = _enumHelper.ParseEnumText<ParametersLayout>(elements[XmlDataConstants.PARAMETERSLAYOUTELEMENT].InnerText);
            txtSummary.Text = elements[XmlDataConstants.SUMMARYELEMENT].InnerText;

            AddEventHandlers();
        }

        public void SetErrorMessage(string message) => configureFunctionsForm.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => configureFunctionsForm.SetMessage(message, title);

        public void UpdateXmlDocument(RadTreeNode treeNode)
        {
            if (TreeView.SelectedNode == null)
                return;

            if (!_treeViewService.IsMethodNode(treeNode))
                throw _exceptionHelper.CriticalException("{5BD6DE33-6065-427F-B9AE-1946207F2CA4}");

            _functionControlsValidator.ValidateInputBoxes();

            XmlElement functionElement = _xmlDocumentHelpers.SelectSingleElement(XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(functionElement).ToDictionary(e => e.Name);
            string newNameAttributeValue = txtFunctionName.Text.Trim();

            elements[XmlDataConstants.MEMBERNAMEELEMENT].InnerText = txtMemberName.Text.Trim();
            elements[XmlDataConstants.FUNCTIONCATEGORYELEMENT].InnerText = Enum.GetName(typeof(FunctionCategories), cmbFunctionCategory.SelectedValue)!;
            elements[XmlDataConstants.TYPENAMEELEMENT].InnerText = txtTypeName.Text.Trim();
            elements[XmlDataConstants.REFERENCENAMEELEMENT].InnerText = txtReferenceName.Text.Trim();
            elements[XmlDataConstants.REFERENCEDEFINITIONELEMENT].InnerText = _enumHelper.BuildValidReferenceDefinition(cmbReferenceDefinition.Text);
            elements[XmlDataConstants.CASTREFERENCEASELEMENT].InnerText = txtCastReferenceAs.Text.Trim();
            elements[XmlDataConstants.REFERENCECATEGORYELEMENT].InnerText = Enum.GetName(typeof(ReferenceCategories), cmbReferenceCategory.SelectedValue)!;
            elements[XmlDataConstants.PARAMETERSLAYOUTELEMENT].InnerText = Enum.GetName(typeof(ParametersLayout), cmbParametersLayout.SelectedValue)!;
            elements[XmlDataConstants.SUMMARYELEMENT].InnerText = txtSummary.Text.Trim();
            functionElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value = newNameAttributeValue;

            configureFunctionsForm.ValidateXmlDocument();

            if (treeNode.Expanded && configureFunctionsForm.ExpandedNodes.ContainsKey(treeNode.Name))
                configureFunctionsForm.ExpandedNodes.Remove(treeNode.Name);

            treeNode.Name = $"{treeNode.Parent.Name}/{XmlDataConstants.FUNCTIONELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{newNameAttributeValue}\"]";
            treeNode.Text = newNameAttributeValue;
            if (treeNode.Expanded)
                configureFunctionsForm.ExpandedNodes.Add(treeNode.Name, newNameAttributeValue);

            _configureFunctionsStateImageSetter.SetImage(functionElement, (StateImageRadTreeNode)treeNode, configureFunctionsForm.Application);
            configureFunctionsForm.RenameChildNodes(treeNode);
        }

        public void ValidateFields()
        {
            _functionControlsValidator.ValidateInputBoxes();
        }

        public void ValidateXmlDocument()
        {
            configureFunctionsForm.ValidateXmlDocument();
        }

        private void AddEventHandlers()
        {
            cmbReferenceDefinition.Validating += CmbReferenceDefinition_Validating;
            txtCastReferenceAs.Validating += TxtCastReferenceAs_Validating;
            txtFunctionName.TextChanged += TxtFunctionName_TextChanged;
            txtFunctionName.Validating += TxtFunctionName_Validating;
            txtMemberName.Validating += TxtMemberName_Validating;
            txtReferenceName.Validating += TxtReferenceName_Validating;
            txtTypeName.Validating += TxtTypeName_Validating;

        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private static void CollapsePanelBorder(RadScrollablePanel radPanel)
            => radPanel.PanelElement.Border.Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            //AddEventHandlers(); Causes exception at 125% scaling: CmbReferenceCategory.SelectedValue is still null in FunctionControlsValidator.ValidateTypeName().
            //Event handlers shouldn't run until all contols have been populated in SetControlValues().
            CollapsePanelBorder(radScrollablePanelFunction);
            CollapsePanelBorder(radPanelTableParent);
            InitializeFunctionControls();
            InitializeClickCommands();
            LoadFunctionDropDownLists();
            _txtTypeNameTypeAutoCompleteManager.Setup();
        }

        private void InitializeClickCommands()
        {
            InitializeHelperButtonCommand
            (
                txtGenericArguments,
                _configureFunctionControlCommandFactory.GetEditFunctionGenericArgumentsCommand(this)
            );

            InitializeHelperButtonCommand
            (
                txtReturnType,
                _configureFunctionControlCommandFactory.GetConfigureFunctionReturnTypeCommand(this)
            );
        }

        private void InitializeFunctionControls()
        {
            InitializeReadOnlyTextBox(txtGenericArguments, Strings.configurationFormIndicatorText);
            InitializeReadOnlyTextBox(txtReturnType, Strings.configurationFormIndicatorText);
            helpProvider.SetHelpString(txtFunctionName, Strings.funcConfigFunctionNameHelp);
            helpProvider.SetHelpString(txtMemberName, Strings.funcConfigMemberNameHelp);
            helpProvider.SetHelpString(cmbFunctionCategory, Strings.funcConfigFunctionCategoryHelp);
            helpProvider.SetHelpString(txtTypeName, Strings.funcConfigTypeNameHelp);
            helpProvider.SetHelpString(txtReferenceName, Strings.funcConfigReferenceNameHelp);
            helpProvider.SetHelpString(cmbReferenceDefinition, Strings.funcConfigReferenceDefinitionHelp);
            helpProvider.SetHelpString(txtCastReferenceAs, Strings.funcConfigCastReferenceAsHelp);
            helpProvider.SetHelpString(cmbReferenceCategory, Strings.funcConfigReferenceCategoryHelp);
            helpProvider.SetHelpString(cmbParametersLayout, Strings.funcConfigParametersLayoutHelp);
            helpProvider.SetHelpString(txtGenericArguments, Strings.funcConfigGenericArgumentsHelp);
            helpProvider.SetHelpString(txtReturnType, Strings.funcConfigReturnTypeHelp);
            helpProvider.SetHelpString(txtSummary, Strings.funcConfigSummaryHelp);
            toolTip.SetToolTip(lblFunctionName, Strings.funcConfigFunctionNameHelp);
            toolTip.SetToolTip(lblMemberName, Strings.funcConfigMemberNameHelp);
            toolTip.SetToolTip(lblFunctionCategory, Strings.funcConfigFunctionCategoryHelp);
            toolTip.SetToolTip(lblTypeName, Strings.funcConfigTypeNameHelp);
            toolTip.SetToolTip(lblReferenceName, Strings.funcConfigReferenceNameHelp);
            toolTip.SetToolTip(lblReferenceDefinition, Strings.funcConfigReferenceDefinitionHelp);
            toolTip.SetToolTip(lblCastReferenceAs, Strings.funcConfigCastReferenceAsHelp);
            toolTip.SetToolTip(lblReferenceCategory, Strings.funcConfigReferenceCategoryHelp);
            toolTip.SetToolTip(lblParametersLayout, Strings.funcConfigParametersLayoutHelp);
            toolTip.SetToolTip(lblGenericArguments, Strings.funcConfigGenericArgumentsHelp);
            toolTip.SetToolTip(lblReturnType, Strings.funcConfigReturnTypeHelp);
            toolTip.SetToolTip(lblSummary, Strings.funcConfigSummaryHelp);
        }

        private static void InitializeHelperButtonCommand(HelperButtonTextBox helperButtonTextBox, IClickCommand command)
        {
            helperButtonTextBox.ButtonClick += (sender, args) => command.Execute();
        }

        private static void InitializeReadOnlyTextBox(HelperButtonTextBox helperButtonTextBox, string text)
        {
            helperButtonTextBox.Font = new Font(helperButtonTextBox.Font, FontStyle.Bold);
            helperButtonTextBox.ReadOnly = true;
            helperButtonTextBox.Text = text;
            helperButtonTextBox.SetPaddingType(HelperButtonTextBox.PaddingType.Bold);
        }

        private void LoadFunctionDropDownLists()
        {
            _radDropDownListHelper.LoadComboItems(cmbFunctionCategory, RadDropDownStyle.DropDownList, new FunctionCategories[] { FunctionCategories.Assert, FunctionCategories.Retract, FunctionCategories.RuleChainingUpdate, FunctionCategories.Unknown, FunctionCategories.Cast });
            _radDropDownListHelper.LoadComboItems<ReferenceCategories>(cmbReferenceCategory);
            _radDropDownListHelper.LoadComboItems<ParametersLayout>(cmbParametersLayout);
            _radDropDownListHelper.LoadComboItems<ValidIndirectReference>(cmbReferenceDefinition, RadDropDownStyle.DropDown);
        }

        private void RemoveEventHandlers()
        {
            cmbReferenceDefinition.Validating -= CmbReferenceDefinition_Validating;
            txtCastReferenceAs.Validating -= TxtCastReferenceAs_Validating;
            txtFunctionName.TextChanged -= TxtFunctionName_TextChanged;
            txtFunctionName.Validating -= TxtFunctionName_Validating;
            txtMemberName.Validating -= TxtMemberName_Validating;
            txtReferenceName.Validating -= TxtReferenceName_Validating;
            txtTypeName.Validating -= TxtTypeName_Validating;
        }

        #region Event Handlers
        private void CmbReferenceDefinition_Validating(object? sender, CancelEventArgs e)
        {
            try
            {
                _functionControlsValidator.CmbReferenceDefinitionValidating();
            }
            catch (LogicBuilderException ex)
            {
                SetErrorMessage(ex.Message);
            }
        }

        private void TxtCastReferenceAs_Validating(object? sender, CancelEventArgs e)
        {
            try
            {
                _functionControlsValidator.TxtCastReferenceAsValidating();
            }
            catch (LogicBuilderException ex)
            {
                SetErrorMessage(ex.Message);
            }
        }

        private void TxtFunctionName_TextChanged(object? sender, EventArgs e)
        {
            try
            {
                UpdateXmlDocument(TreeView.SelectedNode);
            }
            catch (XmlException ex)
            {
                SetErrorMessage(ex.Message);
            }
            catch (LogicBuilderException ex)
            {
                SetErrorMessage(ex.Message);
            }
        }

        private void TxtFunctionName_Validating(object? sender, CancelEventArgs e)
        {
            try
            {
                _functionControlsValidator.TxtFunctionNameValidating();
            }
            catch (LogicBuilderException ex)
            {
                SetErrorMessage(ex.Message);
            }
        }
        private void TxtMemberName_Validating(object? sender, CancelEventArgs e)
        {
            try
            {
                _functionControlsValidator.TxtMemberNameValidating();
            }
            catch (LogicBuilderException ex)
            {
                SetErrorMessage(ex.Message);
            }
        }

        private void TxtReferenceName_Validating(object? sender, CancelEventArgs e)
        {
            try
            {
                _functionControlsValidator.TxtReferenceNameValidating();
            }
            catch (LogicBuilderException ex)
            {
                SetErrorMessage(ex.Message);
            }
        }

        private void TxtTypeName_Validating(object? sender, CancelEventArgs e)
        {
            try
            {
                _functionControlsValidator.TxtTypeNameValidating();
            }
            catch (LogicBuilderException ex)
            {
                SetErrorMessage(ex.Message);
            }
        }
        #endregion  Event Handlers
    }
}
