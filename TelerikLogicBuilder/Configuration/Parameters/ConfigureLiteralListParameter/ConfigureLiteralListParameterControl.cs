using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralListParameter.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.Helpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.StateImageSetters;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralListParameter
{
    internal partial class ConfigureLiteralListParameterControl : UserControl, IConfigureLiteralListParameterControl
    {
        private readonly IConfigureLiteralListParameterCommandFactory _configureLiteralListParameterCommandFactory;
        private readonly IConfigureParametersStateImageSetter _configureParametersStateImageSetter;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ILiteralListParameterControlValidator _literalListParameterControlValidator;
        private readonly IParametersXmlParser _parametersXmlParser;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly ITypeAutoCompleteManager _cmbListLpPropertySourceTypeAutoCompleteManager;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigurationForm configurationForm;

        public ConfigureLiteralListParameterControl(
            IConfigureLiteralListParameterCommandFactory configureLiteralListParameterCommandFactory,
            IConfigureParametersStateImageSetter configureParametersStateImageSetter,
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IParameterControlValidatorFactory parameterControlValidatorFactory,
            IParametersXmlParser parametersXmlParser,
            IRadDropDownListHelper radDropDownListHelper,
            IServiceFactory serviceFactory,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigurationForm configurationForm)
        {
            InitializeComponent();
            _configureLiteralListParameterCommandFactory = configureLiteralListParameterCommandFactory;
            _configureParametersStateImageSetter = configureParametersStateImageSetter;
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _cmbListLpPropertySourceTypeAutoCompleteManager = serviceFactory.GetTypeAutoCompleteManager
            (
                configurationForm,
                cmbListLpPropertySource
            );
            _literalListParameterControlValidator = parameterControlValidatorFactory.GetLiteralListParameterControlValidator(this);
            _parametersXmlParser = parametersXmlParser;
            _radDropDownListHelper = radDropDownListHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configurationForm = configurationForm;
            Initialize();
        }

        private readonly HelpProvider helpProvider = new();
        private readonly RadToolTip toolTip = new();

        public AutoCompleteRadDropDownList CmbListLpPropertySource => cmbListLpPropertySource;
        public RadDropDownList CmbListLpElementControl => cmbListLpElementControl;
        public RadDropDownList CmbListLpLiteralType => cmbListLpLiteralType;
        public RadDropDownList CmbListLpPropertySourceParameter => cmbListLpPropertySourceParameter;
        public RadLabel LblListLpElementControl => lblListLpElementControl;
        public RadLabel LblListLpName => lblListLpName;
        public RadLabel LblListLpPropertySource => lblListLpPropertySource;
        public RadLabel LblListLpPropertySourceParameter => lblListLpPropertySourceParameter;
        public RadTextBox TxtListLpName => txtListLpName;
        public RadTreeView TreeView => configurationForm.TreeView;
        public XmlDocument XmlDocument => configurationForm.XmlDocument;

        public void ClearMessage() => configurationForm.ClearMessage();

        public void SetControlValues(RadTreeNode treeNode)
        {
            if (!_treeViewService.IsListOfLiteralsTypeNode(treeNode))
                throw _exceptionHelper.CriticalException("{4DD717A5-039B-470A-916D-8E6B9FA815BD}");

            RemoveEventHandlers();
            LoadPropertySourceParameterComboBox(treeNode);
            XmlElement parameterElement = _xmlDocumentHelpers.SelectSingleElement(XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(parameterElement).ToDictionary(e => e.Name);

            txtListLpName.Text = parameterElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE);
            txtListLpName.Select();
            txtListLpName.SelectAll();
            cmbListLpLiteralType.SelectedValue = _enumHelper.ParseEnumText<LiteralParameterType>(elements[XmlDataConstants.LITERALTYPEELEMENT].InnerText);
            cmbListLpListType.SelectedValue = _enumHelper.ParseEnumText<ListType>(elements[XmlDataConstants.LISTTYPEELEMENT].InnerText);
            cmbListLpControl.SelectedValue = _enumHelper.ParseEnumText<ListParameterInputStyle>(elements[XmlDataConstants.CONTROLELEMENT].InnerText);
            cmbListLpElementControl.SelectedValue = _enumHelper.ParseEnumText<LiteralParameterInputStyle>(elements[XmlDataConstants.ELEMENTCONTROLELEMENT].InnerText);
            cmbListLpOptional.SelectedValue = bool.Parse(elements[XmlDataConstants.OPTIONALELEMENT].InnerText);
            cmbListLpPropertySource.Text = elements[XmlDataConstants.PROPERTYSOURCEELEMENT].InnerText;
            cmbListLpPropertySourceParameter.Text = elements[XmlDataConstants.PROPERTYSOURCEPARAMETERELEMENT].InnerText;
            txtListLpComments.Text = elements[XmlDataConstants.COMMENTSELEMENT].InnerText;

            AddEventHandlers();
        }

        public void SetErrorMessage(string message) => configurationForm.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => configurationForm.SetMessage(message, title);

        public void UpdateXmlDocument(RadTreeNode treeNode)
        {
            if (TreeView.SelectedNode == null)
                return;

            if (!_treeViewService.IsListOfLiteralsTypeNode(treeNode))
                throw _exceptionHelper.CriticalException("{AFA78CB3-1400-43D6-BC15-87E60869720C}");

            _literalListParameterControlValidator.ValidateInputBoxes();

            XmlElement parameterElement = _xmlDocumentHelpers.SelectSingleElement(XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(parameterElement).ToDictionary(e => e.Name);
            string newNameAttributeValue = txtListLpName.Text.Trim();

            elements[XmlDataConstants.LITERALTYPEELEMENT].InnerText = Enum.GetName(typeof(LiteralParameterType), cmbListLpLiteralType.SelectedValue)!;
            elements[XmlDataConstants.LISTTYPEELEMENT].InnerText = Enum.GetName(typeof(ListType), cmbListLpListType.SelectedValue)!;
            elements[XmlDataConstants.CONTROLELEMENT].InnerText = Enum.GetName(typeof(ListParameterInputStyle), cmbListLpControl.SelectedValue)!;
            elements[XmlDataConstants.ELEMENTCONTROLELEMENT].InnerText = Enum.GetName(typeof(LiteralParameterInputStyle), cmbListLpElementControl.SelectedValue)!;
            elements[XmlDataConstants.OPTIONALELEMENT].InnerText = bool.Parse(cmbListLpOptional.SelectedValue.ToString()!).ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
            elements[XmlDataConstants.PROPERTYSOURCEELEMENT].InnerText = cmbListLpPropertySource.Text.Trim();
            elements[XmlDataConstants.PROPERTYSOURCEPARAMETERELEMENT].InnerText = cmbListLpPropertySourceParameter.Text.Trim();
            parameterElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value = newNameAttributeValue;

            configurationForm.ValidateXmlDocument();

            treeNode.Name = $"{treeNode.Parent.Name}/{XmlDataConstants.PARAMETERSELEMENT}/{XmlDataConstants.LITERALLISTPARAMETERELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{newNameAttributeValue}\"]";
            treeNode.Text = newNameAttributeValue;
            _configureParametersStateImageSetter.SetImage(parameterElement, (StateImageRadTreeNode)treeNode, configurationForm.Application);
        }

        public void ValidateFields() => _literalListParameterControlValidator.ValidateInputBoxes();

        public void ValidateXmlDocument() => configurationForm.ValidateXmlDocument();

        private void AddEventHandlers()
        {
            txtListLpName.TextChanged += TxtListLpName_TextChanged;
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private static void CollapsePanelBorder(RadScrollablePanel radPanel)
            => radPanel.PanelElement.Border.Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            radPanelParameter.VerticalScrollBarState = ScrollState.AlwaysShow;
            InitializeTableLayoutPanel();
            CollapsePanelBorder(radPanelParameter);
            CollapsePanelBorder(radPanelTableParent);
            InitializeParameterControls();
            LoadParameterDropDownLists();
            InitializeClickCommands();
            _cmbListLpPropertySourceTypeAutoCompleteManager.Setup();
        }

        private void InitializeClickCommands()
        {
            InitializeHelperButtonCommand
            (
                txtListLpDomain,
                _configureLiteralListParameterCommandFactory.GetUpdateLiteralListParameterDomainCommand(this)
            );
            InitializeHelperButtonCommand
            (
                txtListLpDefaultValue,
                _configureLiteralListParameterCommandFactory.GetUpdateLiteralListParameterDefaultValueCommand(this)
            );
        }

        private static void InitializeHelperButtonCommand(HelperButtonTextBox helperButtonTextBox, IClickCommand command)
        {
            helperButtonTextBox.ButtonClick += (sender, args) => command.Execute();
        }

        private void InitializeParameterControls()
        {
            InitializeReadOnlyTextBox(txtListLpDefaultValue, Strings.collectionIndicatorText);
            InitializeReadOnlyTextBox(txtListLpDomain, Strings.collectionIndicatorText);
            helpProvider.SetHelpString(txtListLpName, Strings.constrConfigParameterNameHelp);
            helpProvider.SetHelpString(cmbListLpLiteralType, Strings.constrConfigListLiteralTypeHelp);
            helpProvider.SetHelpString(cmbListLpListType, Strings.constrConfigListTypeHelp);
            helpProvider.SetHelpString(cmbListLpControl, Strings.constrConfigControlHelp);
            helpProvider.SetHelpString(cmbListLpElementControl, Strings.constrConfigElementControlHelp);
            helpProvider.SetHelpString(cmbListLpOptional, Strings.constrConfigOptionalHelp);
            helpProvider.SetHelpString(cmbListLpPropertySource, string.Format(CultureInfo.CurrentCulture, Strings.constrConfigPropertySourceHelpFormat, lblListLpElementControl.Text, Strings.dropdownTextPropertyInput));
            helpProvider.SetHelpString(cmbListLpPropertySourceParameter, string.Format(CultureInfo.CurrentCulture, Strings.constrConfigPropertySourceParameterHelpFormat, lblListLpElementControl.Text, Strings.dropdownTextParameterSourcedPropertyInput));
            helpProvider.SetHelpString(txtListLpDefaultValue, Strings.constrConfigDefaultValueHelp);
            helpProvider.SetHelpString(txtListLpDomain, Strings.constrConfigDomainHelp);
            helpProvider.SetHelpString(txtListLpComments, Strings.constrConfigCommentsHelp);
            toolTip.SetToolTip(lblListLpName, Strings.constrConfigParameterNameHelp);
            toolTip.SetToolTip(lblListLpLiteralType, Strings.constrConfigListLiteralTypeHelp);
            toolTip.SetToolTip(lblListLpListType, Strings.constrConfigListTypeHelp);
            toolTip.SetToolTip(lblListLpControl, Strings.constrConfigControlHelp);
            toolTip.SetToolTip(lblListLpElementControl, Strings.constrConfigElementControlHelp);
            toolTip.SetToolTip(lblListLpOptional, Strings.constrConfigOptionalHelp);
            toolTip.SetToolTip(lblListLpPropertySource, string.Format(CultureInfo.CurrentCulture, Strings.constrConfigPropertySourceHelpFormat, lblListLpElementControl.Text, Strings.dropdownTextPropertyInput));
            toolTip.SetToolTip(lblListLpPropertySourceParameter, string.Format(CultureInfo.CurrentCulture, Strings.constrConfigPropertySourceParameterHelpFormat, lblListLpElementControl.Text, Strings.dropdownTextParameterSourcedPropertyInput));
            toolTip.SetToolTip(lblListLpDefaultValue, Strings.constrConfigDefaultValueHelp);
            toolTip.SetToolTip(lblListLpDomain, Strings.constrConfigDomainHelp);
            toolTip.SetToolTip(lblListLpComments, Strings.constrConfigCommentsHelp);
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
                groupBoxParameter,
                radPanelParameter,
                radPanelTableParent,
                tableLayoutPanel,
                11
            );
        }

        private void LoadParameterDropDownLists()
        {
            _radDropDownListHelper.LoadComboItems<LiteralParameterType>(cmbListLpLiteralType, RadDropDownStyle.DropDownList);
            _radDropDownListHelper.LoadComboItems<ListType>(cmbListLpListType);
            _radDropDownListHelper.LoadComboItems(cmbListLpControl, RadDropDownStyle.DropDownList, new ListParameterInputStyle[] { ListParameterInputStyle.Connectors });
            _radDropDownListHelper.LoadComboItems<LiteralParameterInputStyle>(cmbListLpElementControl);
            _radDropDownListHelper.LoadBooleans(cmbListLpOptional);
        }

        private void LoadPropertySourceParameterComboBox(RadTreeNode parameterTreeNode)
        {
            cmbListLpPropertySourceParameter.Items.Clear();
            cmbListLpPropertySourceParameter.Items.AddRange
            (
                _xmlDocumentHelpers.GetSiblingParameterElements
                (
                    _xmlDocumentHelpers.SelectSingleElement(XmlDocument, parameterTreeNode.Name)
                )
                .Select(e => _parametersXmlParser.Parse(e).Name)
                .OrderBy(n => n)
            );
        }

        private void RemoveEventHandlers()
        {
            txtListLpName.TextChanged -= TxtListLpName_TextChanged;
        }

        private void TxtListLpName_TextChanged(object? sender, System.EventArgs e)
        {
            try
            {
                _literalListParameterControlValidator.ValidateInputBoxes();
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
    }
}
