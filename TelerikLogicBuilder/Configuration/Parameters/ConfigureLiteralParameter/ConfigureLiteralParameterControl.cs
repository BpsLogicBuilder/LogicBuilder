using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralParameter.Factories;
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralParameter
{
    internal partial class ConfigureLiteralParameterControl : UserControl, IConfigureLiteralParameterControl
    {
        private readonly IConfigureLiteralParameterCommandFactory _configureLiteralParameterCommandFactory;
        private readonly IConfigureParametersStateImageSetter _configureParametersStateImageSetter;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ILiteralParameterControlValidator _literalParameterControlValidator;
        private readonly IParametersXmlParser _parametersXmlParser;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly ITypeAutoCompleteManager _cmbLpPropertySourceTypeAutoCompleteManager;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigurationForm configurationForm;

        public ConfigureLiteralParameterControl(
            IConfigureLiteralParameterCommandFactory configureLiteralParameterCommandFactory,
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
            _configureLiteralParameterCommandFactory = configureLiteralParameterCommandFactory;
            _configureParametersStateImageSetter = configureParametersStateImageSetter;
            _cmbLpPropertySourceTypeAutoCompleteManager = serviceFactory.GetTypeAutoCompleteManager
            (
                configurationForm,
                cmbLpPropertySource
            );
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _literalParameterControlValidator = parameterControlValidatorFactory.GetLiteralParameterControlValidator(this);
            _parametersXmlParser = parametersXmlParser;
            _radDropDownListHelper = radDropDownListHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configurationForm = configurationForm;
            Initialize();
        }

        private readonly HelpProvider helpProvider = new();
        private readonly RadToolTip toolTip = new();

        public RadDropDownList CmbLpControl => cmbLpControl;
        public RadDropDownList CmbLpLiteralType => cmbLpLiteralType;
        public AutoCompleteRadDropDownList CmbLpPropertySource => cmbLpPropertySource;
        public RadDropDownList CmbLpPropertySourceParameter => cmbLpPropertySourceParameter;
        public RadLabel LblLpControl => lblLpControl;
        public RadLabel LblLpDefaultValue => lblLpDefaultValue;
        public RadLabel LblLpName => lblLpName;
        public RadLabel LblLpPropertySource => lblLpPropertySource;
        public RadLabel LblLpPropertySourceParameter => lblLpPropertySourceParameter;
        public RadTextBox TxtLpDefaultValue => txtLpDefaultValue;
        public RadTextBox TxtLpName => txtLpName;
        public RadTreeView TreeView => configurationForm.TreeView;
        public XmlDocument XmlDocument => configurationForm.XmlDocument;

        public void ClearMessage() => configurationForm.ClearMessage();

        public void SetControlValues(RadTreeNode treeNode)
        {
            if (!_treeViewService.IsLiteralTypeNode(treeNode))
                throw _exceptionHelper.CriticalException("{909CD55D-8B4E-4989-BA35-99B7E8D6F771}");

            RemoveEventHandlers();
            LoadPropertySourceParameterComboBox(treeNode);
            XmlElement parameterElement = _xmlDocumentHelpers.SelectSingleElement(XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(parameterElement).ToDictionary(e => e.Name);

            txtLpName.Text = parameterElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE);
            txtLpName.Select();
            txtLpName.SelectAll();
            cmbLpLiteralType.SelectedValue = _enumHelper.ParseEnumText<LiteralParameterType>(elements[XmlDataConstants.LITERALTYPEELEMENT].InnerText);
            cmbLpControl.SelectedValue = _enumHelper.ParseEnumText<LiteralParameterInputStyle>(elements[XmlDataConstants.CONTROLELEMENT].InnerText);
            cmbLpOptional.SelectedValue = bool.Parse(elements[XmlDataConstants.OPTIONALELEMENT].InnerText);
            cmbLpUseForEquality.SelectedValue = bool.Parse(elements[XmlDataConstants.USEFOREQUALITYELEMENT].InnerText);
            cmbLpUseForHashCode.SelectedValue = bool.Parse(elements[XmlDataConstants.USEFORHASHCODEELEMENT].InnerText);
            cmbLpUseForToString.SelectedValue = bool.Parse(elements[XmlDataConstants.USEFORTOSTRINGELEMENT].InnerText);
            cmbLpPropertySource.Text = elements[XmlDataConstants.PROPERTYSOURCEELEMENT].InnerText;
            cmbLpPropertySourceParameter.Text = elements[XmlDataConstants.PROPERTYSOURCEPARAMETERELEMENT].InnerText;
            txtLpDefaultValue.Text = elements[XmlDataConstants.DEFAULTVALUEELEMENT].InnerText;
            txtLpComments.Text = elements[XmlDataConstants.COMMENTSELEMENT].InnerText;

            AddEventHandlers();
        }

        public void SetErrorMessage(string message) => configurationForm.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => configurationForm.SetMessage(message, title);

        public void UpdateXmlDocument(RadTreeNode treeNode)
        {
            if (TreeView.SelectedNode == null)
                return;

            if (!_treeViewService.IsLiteralTypeNode(treeNode))
                throw _exceptionHelper.CriticalException("{4A40E250-AB7E-4C24-92F4-972E9E5EF5A3}");

            _literalParameterControlValidator.ValidateInputBoxes();

            XmlElement parameterElement = _xmlDocumentHelpers.SelectSingleElement(XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(parameterElement).ToDictionary(e => e.Name);
            string newNameAttributeValue = txtLpName.Text.Trim();

            elements[XmlDataConstants.LITERALTYPEELEMENT].InnerText = Enum.GetName(typeof(LiteralParameterType), cmbLpLiteralType.SelectedValue)!;
            elements[XmlDataConstants.CONTROLELEMENT].InnerText = Enum.GetName(typeof(LiteralParameterInputStyle), cmbLpControl.SelectedValue)!;
            elements[XmlDataConstants.OPTIONALELEMENT].InnerText = bool.Parse(cmbLpOptional.SelectedValue.ToString()!).ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
            elements[XmlDataConstants.USEFOREQUALITYELEMENT].InnerText = bool.Parse(cmbLpUseForEquality.SelectedValue.ToString()!).ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
            elements[XmlDataConstants.USEFORHASHCODEELEMENT].InnerText = bool.Parse(cmbLpUseForHashCode.SelectedValue.ToString()!).ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
            elements[XmlDataConstants.USEFORTOSTRINGELEMENT].InnerText = bool.Parse(cmbLpUseForToString.SelectedValue.ToString()!).ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
            elements[XmlDataConstants.PROPERTYSOURCEELEMENT].InnerText = cmbLpPropertySource.Text.Trim();
            elements[XmlDataConstants.PROPERTYSOURCEPARAMETERELEMENT].InnerText = cmbLpPropertySourceParameter.Text.Trim();
            elements[XmlDataConstants.DEFAULTVALUEELEMENT].InnerText = txtLpDefaultValue.Text.Trim();
            elements[XmlDataConstants.COMMENTSELEMENT].InnerText = txtLpComments.Text.Trim();
            parameterElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value = newNameAttributeValue;

            configurationForm.ValidateXmlDocument();

            treeNode.Name = $"{treeNode.Parent.Name}/{XmlDataConstants.PARAMETERSELEMENT}/{XmlDataConstants.LITERALPARAMETERELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{newNameAttributeValue}\"]";
            treeNode.Text = newNameAttributeValue;
            _configureParametersStateImageSetter.SetImage(parameterElement, (StateImageRadTreeNode)treeNode, configurationForm.Application);
        }

        public void ValidateFields() => _literalParameterControlValidator.ValidateInputBoxes();

        public void ValidateXmlDocument() => configurationForm.ValidateXmlDocument();

        private void AddEventHandlers()
        {
            txtLpName.TextChanged += TxtLpName_TextChanged;
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
            _cmbLpPropertySourceTypeAutoCompleteManager.Setup();
        }

        private void InitializeClickCommands()
        {
            InitializeHelperButtonCommand
            (
                txtLpDomain,
                _configureLiteralParameterCommandFactory.GetUpdateLiteralParameterDomainCommand(this)
            );
        }

        private static void InitializeHelperButtonCommand(HelperButtonTextBox helperButtonTextBox, IClickCommand command)
        {
            helperButtonTextBox.ButtonClick += (sender, args) => command.Execute();
        }

        private void InitializeParameterControls()
        {
            InitializeReadOnlyTextBox(txtLpDomain, Strings.collectionIndicatorText);
            helpProvider.SetHelpString(this.txtLpName, Strings.constrConfigParameterNameHelp);
            helpProvider.SetHelpString(this.cmbLpLiteralType, Strings.constrConfigLiteralTypeHelp);
            helpProvider.SetHelpString(this.cmbLpControl, Strings.constrConfigControlHelp);
            helpProvider.SetHelpString(this.cmbLpOptional, Strings.constrConfigOptionalHelp);
            helpProvider.SetHelpString(this.cmbLpUseForEquality, Strings.constrConfigUseForEqualityHelp);
            helpProvider.SetHelpString(this.cmbLpUseForHashCode, Strings.constrConfigUseForHashCodeHelp);
            helpProvider.SetHelpString(this.cmbLpUseForToString, Strings.constrConfigUseForToStringHelp);
            helpProvider.SetHelpString(this.cmbLpPropertySource, string.Format(CultureInfo.CurrentCulture, Strings.constrConfigPropertySourceHelpFormat, lblLpControl.Text, Strings.dropdownTextPropertyInput));
            helpProvider.SetHelpString(this.cmbLpPropertySourceParameter, string.Format(CultureInfo.CurrentCulture, Strings.constrConfigPropertySourceParameterHelpFormat, lblLpControl.Text, Strings.dropdownTextParameterSourcedPropertyInput));
            helpProvider.SetHelpString(this.txtLpDefaultValue, Strings.constrConfigDefaultValueHelp);
            helpProvider.SetHelpString(this.txtLpDomain, Strings.constrConfigDomainHelp);
            helpProvider.SetHelpString(this.txtLpComments, Strings.constrConfigCommentsHelp);
            toolTip.SetToolTip(this.lblLpName, Strings.constrConfigParameterNameHelp);
            toolTip.SetToolTip(this.lblLpLiteralType, Strings.constrConfigLiteralTypeHelp);
            toolTip.SetToolTip(this.lblLpControl, Strings.constrConfigControlHelp);
            toolTip.SetToolTip(this.lblLpOptional, Strings.constrConfigOptionalHelp);
            toolTip.SetToolTip(this.lblLpUseForEquality, Strings.constrConfigUseForEqualityHelp);
            toolTip.SetToolTip(this.lblLpUseForHashCode, Strings.constrConfigUseForHashCodeHelp);
            toolTip.SetToolTip(this.lblLpUseForToString, Strings.constrConfigUseForToStringHelp);
            toolTip.SetToolTip(this.lblLpPropertySource, string.Format(CultureInfo.CurrentCulture, Strings.constrConfigPropertySourceHelpFormat, lblLpControl.Text, Strings.dropdownTextPropertyInput));
            toolTip.SetToolTip(this.lblLpPropertySourceParameter, string.Format(CultureInfo.CurrentCulture, Strings.constrConfigPropertySourceParameterHelpFormat, lblLpControl.Text, Strings.dropdownTextParameterSourcedPropertyInput));
            toolTip.SetToolTip(this.lblLpDefaultValue, Strings.constrConfigDefaultValueHelp);
            toolTip.SetToolTip(this.lblLpDomain, Strings.constrConfigDomainHelp);
            toolTip.SetToolTip(this.lblLpComments, Strings.constrConfigCommentsHelp);
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
            float size_20 = 20F / 472 * 100;
            float size_30 = 30F / 472 * 100;
            float size_6 = 6F / 472 * 100;

            ((ISupportInitialize)this.radPanelTableParent).BeginInit();
            this.radPanelTableParent.SuspendLayout();

            this.tableLayoutPanel.RowStyles[0] = new RowStyle(SizeType.Percent, size_20);
            this.tableLayoutPanel.RowStyles[1] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[2] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[3] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[4] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[5] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[6] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[7] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[8] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[9] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[10] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[11] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[12] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[13] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[14] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[15] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[16] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[17] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[18] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[19] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[20] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[21] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[22] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[23] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[24] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[25] = new RowStyle(SizeType.Percent, size_20);

            ((ISupportInitialize)this.radPanelTableParent).EndInit();
            this.radPanelTableParent.ResumeLayout(true);
        }

        private void LoadParameterDropDownLists()
        {
            _radDropDownListHelper.LoadComboItems<LiteralParameterType>(cmbLpLiteralType, RadDropDownStyle.DropDownList);
            _radDropDownListHelper.LoadComboItems<LiteralParameterInputStyle>(cmbLpControl);
            _radDropDownListHelper.LoadBooleans(cmbLpOptional);
            _radDropDownListHelper.LoadBooleans(cmbLpUseForEquality);
            _radDropDownListHelper.LoadBooleans(cmbLpUseForHashCode);
            _radDropDownListHelper.LoadBooleans(cmbLpUseForToString);
        }

        private void LoadPropertySourceParameterComboBox(RadTreeNode parameterTreeNode)
        {
            cmbLpPropertySourceParameter.Items.Clear();
            cmbLpPropertySourceParameter.Items.AddRange
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
            txtLpName.TextChanged -= TxtLpName_TextChanged;
        }

        private void TxtLpName_TextChanged(object? sender, EventArgs e)
        {
            try
            {
                _literalParameterControlValidator.ValidateInputBoxes();
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
