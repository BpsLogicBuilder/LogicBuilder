using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralArgument.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralArgument
{
    internal partial class ConfigureGenericLiteralArgumentControl : UserControl, IConfigureGenericLiteralArgumentControl
    {
        private readonly IConfigureGenericLiteralArgumentCommandFactory _configureGenericLiteralArgumentCommandFactory;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly ITypeAutoCompleteManager _cmbLpPropertySourceTypeAutoCompleteManager;
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureGenericArgumentsForm configureGenericArgumentsForm;

        public ConfigureGenericLiteralArgumentControl(
            IConfigureGenericLiteralArgumentCommandFactory configureGenericLiteralArgumentCommandFactory,
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IRadDropDownListHelper radDropDownListHelper,
            IServiceFactory serviceFactory,
            ITreeViewService treeViewService,
            ITypeHelper typeHelper,
            ITypeLoadHelper typeLoadHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureGenericArgumentsForm configureGenericArgumentsForm)
        {
            InitializeComponent();
            _cmbLpPropertySourceTypeAutoCompleteManager = serviceFactory.GetTypeAutoCompleteManager
            (
                configureGenericArgumentsForm,
                cmbLpPropertySource
            );
            _configureGenericLiteralArgumentCommandFactory = configureGenericLiteralArgumentCommandFactory;
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _treeViewService = treeViewService;
            _typeHelper = typeHelper;
            _typeLoadHelper = typeLoadHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _radDropDownListHelper = radDropDownListHelper;
            this.configureGenericArgumentsForm = configureGenericArgumentsForm;
            Initialize();
        }

        private readonly HelpProvider helpProvider = new();
        private readonly RadToolTip toolTip = new();

        #region Properties
        public RadTreeView TreeView => configureGenericArgumentsForm.TreeView;
        public XmlDocument XmlDocument => configureGenericArgumentsForm.XmlDocument;
        public RadDropDownList CmbLpLiteralType => cmbLpLiteralType;
        #endregion Properties

        public void SetControlValues(RadTreeNode treeNode)
        {
            XmlElement parameterElement = _xmlDocumentHelpers.SelectSingleElement(this.XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(parameterElement).ToDictionary(e => e.Name);

            cmbLpGenericArgumentName.SelectedValue = parameterElement.GetAttribute(XmlDataConstants.GENERICARGUMENTNAMEATTRIBUTE);
            cmbLpLiteralType.SelectedValue = _enumHelper.ParseEnumText<LiteralParameterType>(elements[XmlDataConstants.LITERALTYPEELEMENT].InnerText);
            cmbLpControl.SelectedValue = _enumHelper.ParseEnumText<LiteralParameterInputStyle>(elements[XmlDataConstants.CONTROLELEMENT].InnerText);
            cmbLpUseForEquality.SelectedValue = bool.Parse(elements[XmlDataConstants.USEFOREQUALITYELEMENT].InnerText);
            cmbLpUseForHashCode.SelectedValue = bool.Parse(elements[XmlDataConstants.USEFORHASHCODEELEMENT].InnerText);
            cmbLpUseForToString.SelectedValue = bool.Parse(elements[XmlDataConstants.USEFORTOSTRINGELEMENT].InnerText);
            cmbLpPropertySource.Text = elements[XmlDataConstants.PROPERTYSOURCEELEMENT].InnerText;
            cmbLpPropertySourceParameter.Text = elements[XmlDataConstants.PROPERTYSOURCEPARAMETERELEMENT].InnerText;
            txtLpDefaultValue.Text = elements[XmlDataConstants.DEFAULTVALUEELEMENT].InnerText;
        }

        public void UpdateXmlDocument(RadTreeNode treeNode)
        {
            if (!_treeViewService.IsGenericArgumentParameterNode(treeNode))
                throw _exceptionHelper.CriticalException("{E04C2AD7-EFE1-46B4-A5DA-4E4EFE4095CF}");

            List<string> errors = ValidateFields();
            if (errors.Count > 0)
                throw new LogicBuilderException(string.Join(Environment.NewLine, errors));

            XmlElement parameterElement = _xmlDocumentHelpers.SelectSingleElement(this.XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(parameterElement).ToDictionary(e => e.Name);

            elements[XmlDataConstants.LITERALTYPEELEMENT].InnerText = Enum.GetName(typeof(LiteralParameterType), cmbLpLiteralType.SelectedValue)!;
            elements[XmlDataConstants.CONTROLELEMENT].InnerText = Enum.GetName(typeof(LiteralParameterInputStyle), cmbLpControl.SelectedValue)!;
            elements[XmlDataConstants.USEFOREQUALITYELEMENT].InnerText = bool.Parse(cmbLpUseForEquality.SelectedValue.ToString()!).ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
            elements[XmlDataConstants.USEFORHASHCODEELEMENT].InnerText = bool.Parse(cmbLpUseForHashCode.SelectedValue.ToString()!).ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
            elements[XmlDataConstants.USEFORTOSTRINGELEMENT].InnerText = bool.Parse(cmbLpUseForToString.SelectedValue.ToString()!).ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
            elements[XmlDataConstants.PROPERTYSOURCEELEMENT].InnerText = cmbLpPropertySource.Text.Trim();
            elements[XmlDataConstants.PROPERTYSOURCEPARAMETERELEMENT].InnerText = cmbLpPropertySourceParameter.Text.Trim();
            elements[XmlDataConstants.DEFAULTVALUEELEMENT].InnerText = txtLpDefaultValue.Text.Trim();

            configureGenericArgumentsForm.ValidateXmlDocument();
        }

        private void Initialize()
        {
            this.cmbLpGenericArgumentName.ReadOnly = true;
            radPanelParameter.AutoScroll = true;
            ((BorderPrimitive)radPanelParameter.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;
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
                _configureGenericLiteralArgumentCommandFactory.GetUpdateGenericLiteralDomainCommand(this)
            );
        }

        private static void InitializeHelperButtonCommand(HelperButtonTextBox helperButtonTextBox, IClickCommand command)
        {
            helperButtonTextBox.ButtonClick += (sender, args) => command.Execute();
        }

        private void InitializeParameterControls()
        {
            InitializeReadOnlyTextBox(txtLpDomain, Strings.collectionIndicatorText);
            helpProvider.SetHelpString(this.cmbLpGenericArgumentName, Strings.constrConfigGenericArgumentNameHelp);
            helpProvider.SetHelpString(this.cmbLpLiteralType, Strings.constrConfigLiteralTypeHelp);
            helpProvider.SetHelpString(this.cmbLpControl, Strings.constrConfigControlHelp);
            helpProvider.SetHelpString(this.cmbLpUseForEquality, Strings.constrConfigUseForEqualityHelp);
            helpProvider.SetHelpString(this.cmbLpUseForHashCode, Strings.constrConfigUseForHashCodeHelp);
            helpProvider.SetHelpString(this.cmbLpUseForToString, Strings.constrConfigUseForToStringHelp);
            helpProvider.SetHelpString(this.cmbLpPropertySource, string.Format(CultureInfo.CurrentCulture, Strings.constrConfigPropertySourceHelpFormat, lblLpControl.Text, Strings.dropdownTextPropertyInput));
            helpProvider.SetHelpString(this.cmbLpPropertySourceParameter, string.Format(CultureInfo.CurrentCulture, Strings.constrConfigPropertySourceParameterHelpFormat, lblLpControl.Text, Strings.dropdownTextParameterSourcedPropertyInput));
            helpProvider.SetHelpString(this.txtLpDefaultValue, Strings.constrConfigDefaultValueHelp);
            helpProvider.SetHelpString(this.txtLpDomain, Strings.constrConfigDomainHelp);
            toolTip.SetToolTip(this.lblLpGenericArgumentName, Strings.constrConfigGenericArgumentNameHelp);
            toolTip.SetToolTip(this.lblLpLiteralType, Strings.constrConfigLiteralTypeHelp);
            toolTip.SetToolTip(this.lblLpControl, Strings.constrConfigControlHelp);
            toolTip.SetToolTip(this.lblLpUseForEquality, Strings.constrConfigUseForEqualityHelp);
            toolTip.SetToolTip(this.lblLpUseForHashCode, Strings.constrConfigUseForHashCodeHelp);
            toolTip.SetToolTip(this.lblLpUseForToString, Strings.constrConfigUseForToStringHelp);
            toolTip.SetToolTip(this.lblLpPropertySource, string.Format(CultureInfo.CurrentCulture, Strings.constrConfigPropertySourceHelpFormat, lblLpControl.Text, Strings.dropdownTextPropertyInput));
            toolTip.SetToolTip(this.lblLpPropertySourceParameter, string.Format(CultureInfo.CurrentCulture, Strings.constrConfigPropertySourceParameterHelpFormat, lblLpControl.Text, Strings.dropdownTextParameterSourcedPropertyInput));
            toolTip.SetToolTip(this.lblLpDefaultValue, Strings.constrConfigDefaultValueHelp);
            toolTip.SetToolTip(this.lblLpDomain, Strings.constrConfigDomainHelp);
        }

        private static void InitializeReadOnlyTextBox(HelperButtonTextBox helperButtonTextBox, string text)
        {
            helperButtonTextBox.Font = new Font(helperButtonTextBox.Font, FontStyle.Bold);
            helperButtonTextBox.ReadOnly = true;
            helperButtonTextBox.Text = text;
            helperButtonTextBox.SetPaddingType(HelperButtonTextBox.PaddingType.Bold);
        }

        private void LoadParameterDropDownLists()
        {
            _radDropDownListHelper.LoadTextItems(cmbLpGenericArgumentName, configureGenericArgumentsForm.ConfiguredGenericArgumentNames);
            _radDropDownListHelper.LoadComboItems<LiteralParameterType>(cmbLpLiteralType);
            _radDropDownListHelper.LoadComboItems<LiteralParameterInputStyle>(cmbLpControl);
            _radDropDownListHelper.LoadBooleans(cmbLpUseForEquality);
            _radDropDownListHelper.LoadBooleans(cmbLpUseForHashCode);
            _radDropDownListHelper.LoadBooleans(cmbLpUseForToString);
            if (configureGenericArgumentsForm.Application.AssemblyAvailable)
                _radDropDownListHelper.LoadTextItems(cmbLpPropertySource.RadDropDownList, configureGenericArgumentsForm.Application.AllTypesList, RadDropDownStyle.DropDown);

            _radDropDownListHelper.LoadTextItems
            (
                cmbLpPropertySourceParameter, 
                configureGenericArgumentsForm.MemberParameters
                    .OrderBy(m => m.Name)
                    .Select(m => m.Name), 
                RadDropDownStyle.DropDown
            );
        }

        private List<string> ValidateFields()
        {
            List<string> errors = new();
            ValidateLpPropertySource
            (
                (LiteralParameterInputStyle)cmbLpControl.SelectedValue,
                errors
            );

            ValidateLpPropertySourceParameter
            (
                (LiteralParameterInputStyle)cmbLpControl.SelectedValue,
                configureGenericArgumentsForm.MemberParameters
                    .Select(m => m.Name)
                    .ToHashSet(),
                errors
            );

            ValidateLpDefaultValue
            (
                _enumHelper.GetSystemType((LiteralParameterType)cmbLpLiteralType.SelectedValue),
                errors
            );
            return errors;
        }

        private void ValidateLpDefaultValue(Type type, List<string> errors)
        {
            if (!string.IsNullOrEmpty(txtLpDefaultValue.Text) && !_typeHelper.TryParse(txtLpDefaultValue.Text, type, out object? _))
            {
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.invalidDomainFormat, lblLpDefaultValue.Text, txtLpDefaultValue.Text, type.Name));
            }
        }

        private void ValidateLpPropertySource(LiteralParameterInputStyle inputStyle, List<string> errors)
        {
            if 
            (
                inputStyle == LiteralParameterInputStyle.PropertyInput
                    && !_typeLoadHelper.TryGetSystemType(cmbLpPropertySource.Text, configureGenericArgumentsForm.Application, out Type? _)
            )
            {
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadFieldSourceFormat2, lblLpPropertySource.Text, cmbLpPropertySource.Text, lblLpControl.Text, Strings.dropdownTextPropertyInput));
            }

            if (inputStyle != LiteralParameterInputStyle.PropertyInput && cmbLpPropertySource.Text.Trim().Length > 0)
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.fieldSourceMustBeEmptyFormat, lblLpPropertySource.Text, lblLpControl.Text, Strings.dropdownTextPropertyInput));
        }

        private void ValidateLpPropertySourceParameter(LiteralParameterInputStyle inputStyle, HashSet<string> siblingNames, List<string> errors)
        {
            if (inputStyle == LiteralParameterInputStyle.ParameterSourcedPropertyInput)
            {
                if (!siblingNames.Contains(cmbLpPropertySourceParameter.Text.Trim()))
                    errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadPropertySourceParameterFormat3,
                        lblLpPropertySourceParameter.Text,
                        cmbLpPropertySourceParameter.Text,
                        string.Join(", ", siblingNames),
                        lblLpControl.Text,
                        Strings.dropdownTextParameterSourcedPropertyInput));
            }

            if (inputStyle != LiteralParameterInputStyle.ParameterSourcedPropertyInput && cmbLpPropertySourceParameter.Text.Trim().Length > 0)
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.fieldSourceMustBeEmptyFormat, lblLpPropertySourceParameter.Text, lblLpControl.Text, Strings.dropdownTextParameterSourcedPropertyInput));
        }
    }
}
