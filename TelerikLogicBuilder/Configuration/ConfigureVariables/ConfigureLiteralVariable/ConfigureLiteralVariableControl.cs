using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralVariable.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.StateImageSetters;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralVariable
{
    internal partial class ConfigureLiteralVariableControl : UserControl, IConfigureLiteralVariableControl
    {
        private readonly IConfigureLiteralVariableCommandFactory _configureLiteralVariableCommandFactory;
        private readonly IConfigureVariablesStateImageSetter _configureVariablesStateImageSetter;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ILiteralVariableControlsValidator _literalVariableControlsValidator;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly IStringHelper _stringHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly ITypeAutoCompleteManager _cmbLvPropertySourceTypeAutoCompleteManager;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureVariablesForm configureVariablesForm;
        public ConfigureLiteralVariableControl(
            IConfigureLiteralVariableCommandFactory configureLiteralVariableCommandFactory,
            IConfigureVariablesStateImageSetter configureVariablesStateImageSetter,
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IRadDropDownListHelper radDropDownListHelper,
            IServiceFactory serviceFactory,
            IStringHelper stringHelper,
            ITreeViewService treeViewService,
            IVariableControlValidatorFactory variableControlValidatorFactory,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureVariablesForm configureVariablesForm)
        {
            InitializeComponent();
            _configureVariablesStateImageSetter = configureVariablesStateImageSetter;
            _configureLiteralVariableCommandFactory = configureLiteralVariableCommandFactory;
            _cmbLvPropertySourceTypeAutoCompleteManager = serviceFactory.GetTypeAutoCompleteManager
            (
                configureVariablesForm,
                cmbLvPropertySource
            );
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _literalVariableControlsValidator = variableControlValidatorFactory.GetLiteralVariableControlsValidator(this);
            _radDropDownListHelper = radDropDownListHelper;
            _stringHelper = stringHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureVariablesForm = configureVariablesForm;
            Initialize();
        }

        private readonly HelpProvider helpProvider = new();
        private readonly RadToolTip toolTip = new();

        #region Properties
        public RadLabel LblName => lblLvName;
        public RadTextBox TxtName => txtLvName;
        public RadTextBox TxtMemberName => txtLvMemberName;
        public RadDropDownList CmbVariableCategory => cmbLvVariableCategory;
        public RadLabel LblCastVariableAs => lblLvCastVariableAs;
        public RadTextBox TxtCastVariableAs => txtLvCastVariableAs;
        public RadLabel LblTypeName => lblLvTypeName;
        public RadTextBox TxtTypeName => txtLvTypeName;
        public RadTextBox TxtReferenceName => txtLvReferenceName;
        public RadDropDownList CmbReferenceDefinition => cmbLvReferenceDefinition;
        public RadTextBox TxtCastReferenceAs => txtLvCastReferenceAs;
        public RadDropDownList CmbReferenceCategory => cmbLvReferenceCategory;
        public RadDropDownList CmbLiteralType => cmbLvLiteralType;
        public RadLabel LblControl => lblLvControl;
        public RadDropDownList CmbControl => cmbLvControl;
        public RadLabel LblPropertySource => lblLvPropertySource;
        public AutoCompleteRadDropDownList CmbPropertySource => cmbLvPropertySource;
        public RadLabel LblDefaultValue => lblLvDefaultValue;
        public RadTextBox TxtDefaultValue => txtLvDefaultValue;
        public ApplicationTypeInfo Application => configureVariablesForm.Application;
        public IDictionary<string, VariableBase> VariablesDictionary => configureVariablesForm.VariablesDictionary;
        public HashSet<string> VariableNames => configureVariablesForm.VariableNames;
        public RadTreeView TreeView => configureVariablesForm.TreeView;
        public XmlDocument XmlDocument => configureVariablesForm.XmlDocument;
        #endregion Properties

        public void ClearMessage()
        {
            this.configureVariablesForm.ClearMessage();
        }

        public void SetControlValues(RadTreeNode treeNode)
        {
            if (!_treeViewService.IsLiteralTypeNode(treeNode))
                throw _exceptionHelper.CriticalException("{2F52E3A1-76F0-4FE2-8F93-5663DA1F314A}");

            RemoveEventHandlers();
            XmlElement variableElement = _xmlDocumentHelpers.SelectSingleElement(this.XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(variableElement).ToDictionary(e => e.Name);

            txtLvName.Text = variableElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE);
            txtLvName.Select();
            txtLvName.SelectAll();
            txtLvMemberName.Text = elements[XmlDataConstants.MEMBERNAMEELEMENT].InnerText;
            cmbLvVariableCategory.SelectedValue = _enumHelper.ParseEnumText<VariableCategory>(elements[XmlDataConstants.VARIABLECATEGORYELEMENT].InnerText);
            txtLvCastVariableAs.Text = elements[XmlDataConstants.CASTVARIABLEASELEMENT].InnerText;
            txtLvTypeName.Text = elements[XmlDataConstants.TYPENAMEELEMENT].InnerText;
            txtLvReferenceName.Text = elements[XmlDataConstants.REFERENCENAMEELEMENT].InnerText;
            cmbLvReferenceDefinition.Text = string.Join
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
            txtLvCastReferenceAs.Text = elements[XmlDataConstants.CASTREFERENCEASELEMENT].InnerText;
            cmbLvReferenceCategory.SelectedValue = _enumHelper.ParseEnumText<ReferenceCategories>(elements[XmlDataConstants.REFERENCECATEGORYELEMENT].InnerText);
            txtLvComments.Text = elements[XmlDataConstants.COMMENTSELEMENT].InnerText;
            cmbLvLiteralType.SelectedValue = _enumHelper.ParseEnumText<LiteralVariableType>(elements[XmlDataConstants.LITERALTYPEELEMENT].InnerText);
            cmbLvControl.SelectedValue = _enumHelper.ParseEnumText<LiteralVariableInputStyle>(elements[XmlDataConstants.CONTROLELEMENT].InnerText);
            cmbLvPropertySource.Text = elements[XmlDataConstants.PROPERTYSOURCEELEMENT].InnerText;
            txtLvDefaultValue.Text = elements[XmlDataConstants.DEFAULTVALUEELEMENT].InnerText;
            AddEventHandlers();
        }

        public void SetErrorMessage(string message)
        {
            this.configureVariablesForm.SetErrorMessage(message);
        }

        public void SetMessage(string message, string title = "")
        {
            this.configureVariablesForm.SetMessage(message, title);
        }

        public void UpdateXmlDocument(RadTreeNode treeNode)
        {
            if (TreeView.SelectedNode == null)
                return;

            if (!_treeViewService.IsLiteralTypeNode(treeNode))
                throw _exceptionHelper.CriticalException("{A81673C6-DD3E-4E44-848D-B41A11C1B40D}");

            _literalVariableControlsValidator.ValidateInputBoxes();

            XmlElement variableElement = _xmlDocumentHelpers.SelectSingleElement(XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(variableElement).ToDictionary(e => e.Name);
            string currentNameAttributeValue = variableElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE);
            string newNameAttributeValue = txtLvName.Text.Trim();

            _literalVariableControlsValidator.ValidateForExistingVariableName(currentNameAttributeValue);

            elements[XmlDataConstants.MEMBERNAMEELEMENT].InnerText = txtLvMemberName.Text.Trim();
            elements[XmlDataConstants.VARIABLECATEGORYELEMENT].InnerText = Enum.GetName(typeof(VariableCategory), cmbLvVariableCategory.SelectedValue)!;
            elements[XmlDataConstants.CASTVARIABLEASELEMENT].InnerText = txtLvCastVariableAs.Text.Trim();
            elements[XmlDataConstants.TYPENAMEELEMENT].InnerText = txtLvTypeName.Text.Trim();
            elements[XmlDataConstants.REFERENCENAMEELEMENT].InnerText = txtLvReferenceName.Text.Trim();
            elements[XmlDataConstants.REFERENCEDEFINITIONELEMENT].InnerText = _enumHelper.BuildValidReferenceDefinition(cmbLvReferenceDefinition.Text);
            elements[XmlDataConstants.CASTREFERENCEASELEMENT].InnerText = txtLvCastReferenceAs.Text.Trim();
            elements[XmlDataConstants.REFERENCECATEGORYELEMENT].InnerText = Enum.GetName(typeof(ReferenceCategories), cmbLvReferenceCategory.SelectedValue)!;
            elements[XmlDataConstants.COMMENTSELEMENT].InnerText = txtLvComments.Text.Trim();
            elements[XmlDataConstants.LITERALTYPEELEMENT].InnerText = Enum.GetName(typeof(LiteralVariableType), cmbLvLiteralType.SelectedValue)!;
            elements[XmlDataConstants.CONTROLELEMENT].InnerText = Enum.GetName(typeof(LiteralVariableInputStyle), cmbLvControl.SelectedValue)!;
            elements[XmlDataConstants.PROPERTYSOURCEELEMENT].InnerText = cmbLvPropertySource.Text.Trim();
            elements[XmlDataConstants.DEFAULTVALUEELEMENT].InnerText = txtLvDefaultValue.Text.Trim();
            variableElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value = newNameAttributeValue;
            //always update name attribute last because the other elements may depend on it

            configureVariablesForm.ValidateXmlDocument();

            treeNode.Name = $"{treeNode.Parent.Name}/{XmlDataConstants.LITERALVARIABLEELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{newNameAttributeValue}\"]";
            treeNode.Text = newNameAttributeValue;
            _configureVariablesStateImageSetter.SetImage(variableElement, (StateImageRadTreeNode)treeNode, Application);
        }

        public void ValidateFields()
        {
            _literalVariableControlsValidator.ValidateInputBoxes();
        }

        public void ValidateXmlDocument()
        {
            configureVariablesForm.ValidateXmlDocument();
        }

        private void AddEventHandlers()
        {
            txtLvName.Validating += TxtName_Validating;
            txtLvName.TextChanged += TxtName_TextChanged;
            txtLvMemberName.Validating += TxtMemberName_Validating;
            txtLvCastVariableAs.Validating += TxtCastVariableAs_Validating;
            txtLvTypeName.Validating += TxtTypeName_Validating;
            txtLvReferenceName.Validating += TxtReferenceName_Validating;
            cmbLvReferenceDefinition.Validating += CmbReferenceDefinition_Validating;
            txtLvCastReferenceAs.Validating += TxtCastReferenceAs_Validating;
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private static void CollapsePanelBorder(RadScrollablePanel radPanel)
            => radPanel.PanelElement.Border.Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            CollapsePanelBorder(radPanelVariable);
            CollapsePanelBorder(radPanelTableParent);
            InitializeVariableControls();
            LoadVariableDropDownLists();
            InitializeClickCommands();
            _cmbLvPropertySourceTypeAutoCompleteManager.Setup();
        }

        private void InitializeClickCommands()
        {
            InitializeHelperButtonCommand
            (
                txtLvDomain,
                _configureLiteralVariableCommandFactory.GetUpdateLiteralVariableDomainCommand(this)
            );
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

        private void InitializeVariableControls()
        {
            InitializeReadOnlyTextBox(txtLvDomain, Strings.collectionIndicatorText);
            helpProvider.SetHelpString(this.txtLvName, Strings.varConfigNameHelp);
            helpProvider.SetHelpString(this.txtLvMemberName, Strings.varConfigVariableNameHelp);
            helpProvider.SetHelpString(this.cmbLvVariableCategory, Strings.varConfigVariableCategoryHelp);
            helpProvider.SetHelpString(this.txtLvCastVariableAs, Strings.varConfigCastVariableAsHelp);
            helpProvider.SetHelpString(this.txtLvTypeName, Strings.varConfigTypeNameHelp);
            helpProvider.SetHelpString(this.txtLvReferenceName, Strings.varConfigReferenceNameHelp);
            helpProvider.SetHelpString(this.cmbLvReferenceDefinition, Strings.varConfigReferenceDefinitionHelp);
            helpProvider.SetHelpString(this.txtLvCastReferenceAs, Strings.varConfigCastReferenceAsHelp);
            helpProvider.SetHelpString(this.cmbLvReferenceCategory, Strings.varConfigReferenceCategoryHelp);
            helpProvider.SetHelpString(this.txtLvComments, Strings.varConfigCommentsHelp);
            helpProvider.SetHelpString(this.cmbLvLiteralType, Strings.varConfigLiteralTypeHelp);
            helpProvider.SetHelpString(this.cmbLvControl, Strings.varConfigLiteralControlHelp);
            helpProvider.SetHelpString(this.cmbLvPropertySource, string.Format(CultureInfo.CurrentCulture, Strings.varConfigPropertySourceHelpFormat, lblLvControl.Text, Strings.dropdownTextPropertyInput));
            helpProvider.SetHelpString(this.txtLvDefaultValue, Strings.varConfigDefaultValueHelp);
            helpProvider.SetHelpString(this.txtLvDomain, Strings.varConfigDomainHelp);
            toolTip.SetToolTip(this.lblLvName, Strings.varConfigNameHelp);
            toolTip.SetToolTip(this.lblLvMemberName, Strings.varConfigVariableNameHelp);
            toolTip.SetToolTip(this.lblLvVariableCategory, Strings.varConfigVariableCategoryHelp);
            toolTip.SetToolTip(this.lblLvCastVariableAs, Strings.varConfigCastVariableAsHelp);
            toolTip.SetToolTip(this.lblLvTypeName, Strings.varConfigTypeNameHelp);
            toolTip.SetToolTip(this.lblLvReferenceName, Strings.varConfigReferenceNameHelp);
            toolTip.SetToolTip(this.lblLvReferenceDefinition, Strings.varConfigReferenceDefinitionHelp);
            toolTip.SetToolTip(this.lblLvCastReferenceAs, Strings.varConfigCastReferenceAsHelp);
            toolTip.SetToolTip(this.lblLvReferenceCategory, Strings.varConfigReferenceCategoryHelp);
            toolTip.SetToolTip(this.lblLvComments, Strings.varConfigCommentsHelp);
            toolTip.SetToolTip(this.lblLvLiteralType, Strings.varConfigLiteralTypeHelp);
            toolTip.SetToolTip(this.lblLvControl, Strings.varConfigLiteralControlHelp);
            toolTip.SetToolTip(this.lblLvPropertySource, string.Format(CultureInfo.CurrentCulture, Strings.varConfigPropertySourceHelpFormat, lblLvControl.Text, Strings.dropdownTextPropertyInput));
            toolTip.SetToolTip(this.lblLvDefaultValue, Strings.varConfigDefaultValueHelp);
            toolTip.SetToolTip(this.lblLvDomain, Strings.varConfigDomainHelp);
        }

        private void LoadVariableDropDownLists()
        {
            _radDropDownListHelper.LoadComboItems<VariableCategory>(cmbLvVariableCategory);
            _radDropDownListHelper.LoadComboItems<ValidIndirectReference>(cmbLvReferenceDefinition, RadDropDownStyle.DropDown);
            _radDropDownListHelper.LoadComboItems(cmbLvReferenceCategory, RadDropDownStyle.DropDownList, new ReferenceCategories[] { ReferenceCategories.None });
            _radDropDownListHelper.LoadComboItems<LiteralVariableType>(cmbLvLiteralType);
            _radDropDownListHelper.LoadComboItems<LiteralVariableInputStyle>(cmbLvControl);
        }

        private void RemoveEventHandlers()
        {
            txtLvName.Validating -= TxtName_Validating;
            txtLvName.TextChanged -= TxtName_TextChanged;
            txtLvMemberName.Validating -= TxtMemberName_Validating;
            txtLvCastVariableAs.Validating -= TxtCastVariableAs_Validating;
            txtLvTypeName.Validating -= TxtTypeName_Validating;
            txtLvReferenceName.Validating -= TxtReferenceName_Validating;
            cmbLvReferenceDefinition.Validating -= CmbReferenceDefinition_Validating;
            txtLvCastReferenceAs.Validating -= TxtCastReferenceAs_Validating;
        }

        #region Event Handlers
        private void CmbReferenceDefinition_Validating(object? sender, CancelEventArgs e)
        {
            try
            {
                _literalVariableControlsValidator.CmbReferenceDefinitionValidating();
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
                _literalVariableControlsValidator.TxtCastReferenceAsValidating();
            }
            catch (LogicBuilderException ex)
            {
                SetErrorMessage(ex.Message);
            }
        }

        private void TxtCastVariableAs_Validating(object? sender, CancelEventArgs e)
        {
            try
            {
                _literalVariableControlsValidator.TxtCastVariableAsValidating();
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
                _literalVariableControlsValidator.TxtMemberNameValidating();
            }
            catch (LogicBuilderException ex)
            {
                SetErrorMessage(ex.Message);
            };
        }

        private void TxtName_TextChanged(object? sender, EventArgs e)
        {
            try
            {
                _literalVariableControlsValidator.TxtNameChanged();
                if (TreeView.SelectedNode != null)
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

        private void TxtName_Validating(object? sender, CancelEventArgs e)
        {
            try
            {
                _literalVariableControlsValidator.TxtNameValidating();
            }
            catch (LogicBuilderException ex)
            {
                SetErrorMessage(ex.Message);
            };
        }

        private void TxtReferenceName_Validating(object? sender, CancelEventArgs e)
        {
            try
            {
                _literalVariableControlsValidator.TxtReferenceNameValidating();
            }
            catch (LogicBuilderException ex)
            {
                SetErrorMessage(ex.Message);
            };
        }

        private void TxtTypeName_Validating(object? sender, CancelEventArgs e)
        {
            try
            {
                _literalVariableControlsValidator.TxtTypeNameValidating();
            }
            catch (LogicBuilderException ex)
            {
                SetErrorMessage(ex.Message);
            };
        }
        #endregion Event Handlers
    }
}
