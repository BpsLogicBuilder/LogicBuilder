using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralListVariable.Factories;
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

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralListVariable
{
    internal partial class ConfigureLiteralListVariableControl : UserControl, IConfigureLiteralListVariableControl
    {
        private readonly IConfigureLiteralListVariableCommandFactory _configureLiteralListVariableCommandFactory;
        private readonly IConfigureVariablesStateImageSetter _configureVariablesStateImageSetter;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ILiteralListVariableControlsValidator _literalListVariableControlsValidator;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly IStringHelper _stringHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly ITypeAutoCompleteManager _cmbLvListPropertySourceTypeAutoCompleteManager;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureVariablesForm configureVariablesForm;

       public ConfigureLiteralListVariableControl(
           IConfigureLiteralListVariableCommandFactory configureLiteralListVariableCommandFactory,
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
            _cmbLvListPropertySourceTypeAutoCompleteManager = serviceFactory.GetTypeAutoCompleteManager
            (
                configureVariablesForm,
                cmbLvListPropertySource
            );
            _configureLiteralListVariableCommandFactory = configureLiteralListVariableCommandFactory;
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _literalListVariableControlsValidator = variableControlValidatorFactory.GetLiteralListVariableControlsValidator(this);
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
        public RadLabel LblElementControl => lblLvListElementControl;
        public RadDropDownList CmbElementControl => cmbLvListElementControl;
        public RadLabel LblPropertySource => lblLvListPropertySource;
        public AutoCompleteRadDropDownList CmbPropertySource => cmbLvListPropertySource;
        public RadLabel LblName => lblLvListName;
        public RadTextBox TxtName => txtLvListName;
        public RadTextBox TxtMemberName => txtLvListMemberName;
        public RadDropDownList CmbVariableCategory => cmbLvListVariableCategory;
        public RadLabel LblCastVariableAs => lblLvListCastVariableAs;
        public RadTextBox TxtCastVariableAs => txtLvListCastVariableAs;
        public RadLabel LblTypeName => lblLvListTypeName;
        public RadTextBox TxtTypeName => txtLvListTypeName;
        public RadTextBox TxtReferenceName => txtLvListReferenceName;
        public RadDropDownList CmbReferenceDefinition => cmbLvListReferenceDefinition;
        public RadTextBox TxtCastReferenceAs => txtLvListCastReferenceAs;
        public RadDropDownList CmbReferenceCategory => cmbLvListReferenceCategory;
        public RadDropDownList CmbLiteralType => cmbLvListLiteralType;
        public ApplicationTypeInfo Application => configureVariablesForm.Application;
        public IDictionary<string, VariableBase> VariablesDictionary => configureVariablesForm.VariablesDictionary;
        public HashSet<string> VariableNames => configureVariablesForm.VariableNames;
        public RadTreeView TreeView => configureVariablesForm.TreeView;
        public XmlDocument XmlDocument => configureVariablesForm.XmlDocument;
        #endregion Properties

        public void ClearMessage()
        {
            configureVariablesForm.ClearMessage();
        }

        public void SetControlValues(RadTreeNode treeNode)
        {
            if (!_treeViewService.IsListOfLiteralsTypeNode(treeNode))
                throw _exceptionHelper.CriticalException("{1138CDED-5116-4CCC-AF0E-6B82B7427B2D}");

            RemoveEventHandlers();
            XmlElement variableElement = _xmlDocumentHelpers.SelectSingleElement(this.XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(variableElement).ToDictionary(e => e.Name);

            txtLvListName.Text = variableElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE);
            txtLvListName.Select();
            txtLvListName.SelectAll();
            txtLvListMemberName.Text = elements[XmlDataConstants.MEMBERNAMEELEMENT].InnerText;
            cmbLvListVariableCategory.SelectedValue = _enumHelper.ParseEnumText<VariableCategory>(elements[XmlDataConstants.VARIABLECATEGORYELEMENT].InnerText);
            txtLvListCastVariableAs.Text = elements[XmlDataConstants.CASTVARIABLEASELEMENT].InnerText;
            txtLvListTypeName.Text = elements[XmlDataConstants.TYPENAMEELEMENT].InnerText;
            txtLvListReferenceName.Text = elements[XmlDataConstants.REFERENCENAMEELEMENT].InnerText;
            cmbLvListReferenceDefinition.Text = string.Join
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
            txtLvListCastReferenceAs.Text = elements[XmlDataConstants.CASTREFERENCEASELEMENT].InnerText;
            cmbLvListReferenceCategory.SelectedValue = _enumHelper.ParseEnumText<ReferenceCategories>(elements[XmlDataConstants.REFERENCECATEGORYELEMENT].InnerText);
            txtLvListComments.Text = elements[XmlDataConstants.COMMENTSELEMENT].InnerText;

            cmbLvListLiteralType.SelectedValue = _enumHelper.ParseEnumText<LiteralVariableType>(elements[XmlDataConstants.LITERALTYPEELEMENT].InnerText);
            cmbLvListListType.SelectedValue = _enumHelper.ParseEnumText<ListType>(elements[XmlDataConstants.LISTTYPEELEMENT].InnerText);
            cmbLvListListControl.SelectedValue = _enumHelper.ParseEnumText<ListVariableInputStyle>(elements[XmlDataConstants.CONTROLELEMENT].InnerText);
            cmbLvListElementControl.SelectedValue = _enumHelper.ParseEnumText<LiteralVariableInputStyle>(elements[XmlDataConstants.ELEMENTCONTROLELEMENT].InnerText);
            cmbLvListPropertySource.Text = elements[XmlDataConstants.PROPERTYSOURCEELEMENT].InnerText;
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

            if (!_treeViewService.IsListOfLiteralsTypeNode(treeNode))
                throw _exceptionHelper.CriticalException("{3E917DA3-58E8-4DB7-9311-E72135CAE99E}");

            _literalListVariableControlsValidator.ValidateInputBoxes();

            XmlElement variableElement = _xmlDocumentHelpers.SelectSingleElement(XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(variableElement).ToDictionary(e => e.Name);
            string currentNameAttributeValue = variableElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE);
            string newNameAttributeValue = txtLvListName.Text.Trim();

            _literalListVariableControlsValidator.ValidateForExistingVariableName(currentNameAttributeValue);

            elements[XmlDataConstants.MEMBERNAMEELEMENT].InnerText = txtLvListMemberName.Text.Trim();
            elements[XmlDataConstants.VARIABLECATEGORYELEMENT].InnerText = Enum.GetName(typeof(VariableCategory), cmbLvListVariableCategory.SelectedValue)!;
            elements[XmlDataConstants.CASTVARIABLEASELEMENT].InnerText = txtLvListCastVariableAs.Text.Trim();
            elements[XmlDataConstants.TYPENAMEELEMENT].InnerText = txtLvListTypeName.Text.Trim();
            elements[XmlDataConstants.REFERENCENAMEELEMENT].InnerText = txtLvListReferenceName.Text.Trim();
            elements[XmlDataConstants.REFERENCEDEFINITIONELEMENT].InnerText = _enumHelper.BuildValidReferenceDefinition(cmbLvListReferenceDefinition.Text);
            elements[XmlDataConstants.CASTREFERENCEASELEMENT].InnerText = txtLvListCastReferenceAs.Text.Trim();
            elements[XmlDataConstants.REFERENCECATEGORYELEMENT].InnerText = Enum.GetName(typeof(ReferenceCategories), cmbLvListReferenceCategory.SelectedValue)!;
            elements[XmlDataConstants.COMMENTSELEMENT].InnerText = txtLvListComments.Text.Trim();

            elements[XmlDataConstants.LITERALTYPEELEMENT].InnerText = Enum.GetName(typeof(LiteralVariableType), cmbLvListLiteralType.SelectedValue)!;
            elements[XmlDataConstants.LISTTYPEELEMENT].InnerText = Enum.GetName(typeof(ListType), cmbLvListListType.SelectedValue)!;
            elements[XmlDataConstants.CONTROLELEMENT].InnerText = Enum.GetName(typeof(ListVariableInputStyle), cmbLvListListControl.SelectedValue)!;
            elements[XmlDataConstants.ELEMENTCONTROLELEMENT].InnerText = Enum.GetName(typeof(LiteralVariableInputStyle), cmbLvListElementControl.SelectedValue)!;
            elements[XmlDataConstants.PROPERTYSOURCEELEMENT].InnerText = cmbLvListPropertySource.Text.Trim();
            variableElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value = newNameAttributeValue;
            //always update name attribute last because the other elements depend on it

            configureVariablesForm.ValidateXmlDocument();

            treeNode.Name = $"{treeNode.Parent.Name}/{XmlDataConstants.LITERALLISTVARIABLEELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{newNameAttributeValue}\"]";
            treeNode.Text = newNameAttributeValue;
            _configureVariablesStateImageSetter.SetImage(variableElement, (StateImageRadTreeNode)treeNode, Application);
        }

        public void ValidateFields()
        {
            _literalListVariableControlsValidator.ValidateInputBoxes();
        }

        public void ValidateXmlDocument()
        {
            configureVariablesForm.ValidateXmlDocument();
        }

        private void AddEventHandlers()
        {
            txtLvListName.Validating += TxtName_Validating;
            txtLvListName.TextChanged += TxtName_TextChanged;
            txtLvListMemberName.Validating += TxtMemberName_Validating;
            txtLvListCastVariableAs.Validating += TxtCastVariableAs_Validating;
            txtLvListTypeName.Validating += TxtTypeName_Validating;
            txtLvListReferenceName.Validating += TxtReferenceName_Validating;
            cmbLvListReferenceDefinition.Validating += CmbReferenceDefinition_Validating;
            txtLvListCastReferenceAs.Validating += TxtCastReferenceAs_Validating;
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private static void CollapsePanelBorder(RadScrollablePanel radPanel)
            => radPanel.PanelElement.Border.Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            radPanelVariable.VerticalScrollBarState = ScrollState.AlwaysShow;
            InitializeTableLayoutPanel();
            CollapsePanelBorder(radPanelVariable);
            CollapsePanelBorder(radPanelTableParent);
            InitializeVariableControls();
            LoadVariableDropDownLists();
            InitializeClickCommands();
            _cmbLvListPropertySourceTypeAutoCompleteManager.Setup();
        }

        private void InitializeClickCommands()
        {
            InitializeHelperButtonCommand
            (
                txtLvListDefaultValue,
                _configureLiteralListVariableCommandFactory.GetUpdateLiteralListVariableDefaultValueCommand(this)
            );

            InitializeHelperButtonCommand
            (
                txtLvListDomain,
                _configureLiteralListVariableCommandFactory.GetUpdateLiteralListVariableDomainCommand(this)
            );
        }

        private static void InitializeHelperButtonCommand(HelperButtonTextBox helperButtonTextBox, IClickCommand command)
        {
            helperButtonTextBox.ButtonClick += (sender, args) => command.Execute();
        }

        private static void InitializeReadOnlyTextBox(HelperButtonTextBox helperButtonTextBox, string text)
        {
            helperButtonTextBox.Font = new Font
            (
                ForeColorUtility.GetDefaultFont(ThemeResolutionService.ApplicationThemeName),
                FontStyle.Bold
            );
            helperButtonTextBox.ReadOnly = true;
            helperButtonTextBox.Text = text;
            helperButtonTextBox.SetPaddingType(HelperButtonTextBox.PaddingType.Bold);
        }

        private void InitializeTableLayoutPanel()
        {
            ControlsLayoutUtility.LayoutControls
            (
                groupBoxVariable,
                radPanelVariable,
                radPanelTableParent,
                tableLayoutPanel,
                17
            );
        }

        private void InitializeVariableControls()
        {
            InitializeReadOnlyTextBox(txtLvListDefaultValue, Strings.collectionIndicatorText);
            InitializeReadOnlyTextBox(txtLvListDomain, Strings.collectionIndicatorText);

            helpProvider.SetHelpString(this.txtLvListName, Strings.varConfigNameHelp);
            helpProvider.SetHelpString(this.txtLvListMemberName, Strings.varConfigVariableNameHelp);
            helpProvider.SetHelpString(this.cmbLvListVariableCategory, Strings.varConfigVariableCategoryHelp);
            helpProvider.SetHelpString(this.txtLvListCastVariableAs, Strings.varConfigCastVariableAsHelp);
            helpProvider.SetHelpString(this.txtLvListTypeName, Strings.varConfigTypeNameHelp);
            helpProvider.SetHelpString(this.txtLvListReferenceName, Strings.varConfigReferenceNameHelp);
            helpProvider.SetHelpString(this.cmbLvListReferenceDefinition, Strings.varConfigReferenceDefinitionHelp);
            helpProvider.SetHelpString(this.txtLvListCastReferenceAs, Strings.varConfigCastReferenceAsHelp);
            helpProvider.SetHelpString(this.cmbLvListReferenceCategory, Strings.varConfigReferenceCategoryHelp);
            helpProvider.SetHelpString(this.txtLvListComments, Strings.varConfigCommentsHelp);
            helpProvider.SetHelpString(this.cmbLvListLiteralType, Strings.varConfigLiteralTypeHelp);
            helpProvider.SetHelpString(this.cmbLvListListType, Strings.varConfigListTypeHelp);
            helpProvider.SetHelpString(this.cmbLvListListControl, Strings.varConfigListControlHelp);
            helpProvider.SetHelpString(this.cmbLvListElementControl, Strings.varConfigElementControlHelp);
            helpProvider.SetHelpString(this.cmbLvListPropertySource, string.Format(CultureInfo.CurrentCulture, Strings.varConfigPropertySourceHelpFormat, lblLvListListControl.Text, Strings.dropdownTextPropertyInput));
            helpProvider.SetHelpString(this.txtLvListDefaultValue, Strings.varConfigListDefaultValueHelp);
            helpProvider.SetHelpString(this.txtLvListDomain, Strings.varConfigDomainHelp);
            toolTip.SetToolTip(this.lblLvListName, Strings.varConfigNameHelp);
            toolTip.SetToolTip(this.lblLvListMemberName, Strings.varConfigVariableNameHelp);
            toolTip.SetToolTip(this.lblLvListVariableCategory, Strings.varConfigVariableCategoryHelp);
            toolTip.SetToolTip(this.lblLvListCastVariableAs, Strings.varConfigCastVariableAsHelp);
            toolTip.SetToolTip(this.lblLvListTypeName, Strings.varConfigTypeNameHelp);
            toolTip.SetToolTip(this.lblLvListReferenceName, Strings.varConfigReferenceNameHelp);
            toolTip.SetToolTip(this.lblLvListReferenceDefinition, Strings.varConfigReferenceDefinitionHelp);
            toolTip.SetToolTip(this.lblLvListCastReferenceAs, Strings.varConfigCastReferenceAsHelp);
            toolTip.SetToolTip(this.lblLvListReferenceCategory, Strings.varConfigReferenceCategoryHelp);
            toolTip.SetToolTip(this.lblLvListComments, Strings.varConfigCommentsHelp);
            toolTip.SetToolTip(this.lblLvListLiteralType, Strings.varConfigLiteralTypeHelp);
            toolTip.SetToolTip(this.lblLvListListType, Strings.varConfigListTypeHelp);
            toolTip.SetToolTip(this.lblLvListListControl, Strings.varConfigListControlHelp);
            toolTip.SetToolTip(this.lblLvListElementControl, Strings.varConfigElementControlHelp);
            toolTip.SetToolTip(this.lblLvListPropertySource, string.Format(CultureInfo.CurrentCulture, Strings.varConfigPropertySourceHelpFormat, lblLvListListControl.Text, Strings.dropdownTextPropertyInput));
            toolTip.SetToolTip(this.lblLvListDefaultValue, Strings.varConfigListDefaultValueHelp);
            toolTip.SetToolTip(this.lblLvListDomain, Strings.varConfigDomainHelp);
        }

        private void LoadVariableDropDownLists()
        {
            _radDropDownListHelper.LoadComboItems<VariableCategory>(cmbLvListVariableCategory);
            _radDropDownListHelper.LoadComboItems<ValidIndirectReference>(cmbLvListReferenceDefinition, RadDropDownStyle.DropDown);
            _radDropDownListHelper.LoadComboItems(cmbLvListReferenceCategory, RadDropDownStyle.DropDownList, new ReferenceCategories[] { ReferenceCategories.None });
            _radDropDownListHelper.LoadComboItems<LiteralVariableType>(cmbLvListLiteralType);
            _radDropDownListHelper.LoadComboItems<ListType>(cmbLvListListType);
            _radDropDownListHelper.LoadComboItems<ListVariableInputStyle>(cmbLvListListControl);
            _radDropDownListHelper.LoadComboItems<LiteralVariableInputStyle>(cmbLvListElementControl);
        }

        private void RemoveEventHandlers()
        {
            txtLvListName.Validating -= TxtName_Validating;
            txtLvListName.TextChanged -= TxtName_TextChanged;
            txtLvListMemberName.Validating -= TxtMemberName_Validating;
            txtLvListCastVariableAs.Validating -= TxtCastVariableAs_Validating;
            txtLvListTypeName.Validating -= TxtTypeName_Validating;
            txtLvListReferenceName.Validating -= TxtReferenceName_Validating;
            cmbLvListReferenceDefinition.Validating -= CmbReferenceDefinition_Validating;
            txtLvListCastReferenceAs.Validating -= TxtCastReferenceAs_Validating;
        }

        #region Event Handlers
        private void CmbReferenceDefinition_Validating(object? sender, CancelEventArgs e)
        {
            try
            {
                _literalListVariableControlsValidator.CmbReferenceDefinitionValidating();
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
                _literalListVariableControlsValidator.TxtCastReferenceAsValidating();
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
                _literalListVariableControlsValidator.TxtCastVariableAsValidating();
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
                _literalListVariableControlsValidator.TxtMemberNameValidating();
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
                _literalListVariableControlsValidator.TxtNameChanged();
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
                _literalListVariableControlsValidator.TxtNameValidating();
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
                _literalListVariableControlsValidator.TxtReferenceNameValidating();
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
                _literalListVariableControlsValidator.TxtTypeNameValidating();
            }
            catch (LogicBuilderException ex)
            {
                SetErrorMessage(ex.Message);
            };
        }
        #endregion Event Handlers
    }
}
