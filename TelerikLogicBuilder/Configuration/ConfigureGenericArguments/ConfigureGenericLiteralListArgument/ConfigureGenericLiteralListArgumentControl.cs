﻿using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralListArgument.Factories;
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

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralListArgument
{
    internal partial class ConfigureGenericLiteralListArgumentControl : UserControl, IConfigureGenericLiteralListArgumentControl
    {
        private readonly IConfigureGenericLiteralListArgumentCommandFactory _configureGenericLiteralListArgumentCommandFactory;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly ITypeAutoCompleteManager _cmbListLpPropertySourceTypeAutoCompleteManager;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureGenericArgumentsForm configureGenericArgumentsForm;

        public ConfigureGenericLiteralListArgumentControl(
            IConfigureGenericLiteralListArgumentCommandFactory configureGenericLiteralListArgumentCommandFactory,
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IRadDropDownListHelper radDropDownListHelper,
            IServiceFactory serviceFactory,
            ITreeViewService treeViewService,
            ITypeLoadHelper typeLoadHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureGenericArgumentsForm configureGenericArgumentsForm)
        {
            InitializeComponent();
            _cmbListLpPropertySourceTypeAutoCompleteManager = serviceFactory.GetTypeAutoCompleteManager
            (
                configureGenericArgumentsForm,
                cmbListLpPropertySource
            );
            _configureGenericLiteralListArgumentCommandFactory = configureGenericLiteralListArgumentCommandFactory;
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _treeViewService = treeViewService;
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
        public HelperButtonTextBox TxtListLpDomain => txtListLpDomain;
        public HelperButtonTextBox TxtListLpDefaultValue => txtListLpDefaultValue;
        #endregion Properties

        public void SetControlValues(RadTreeNode treeNode)
        {
            XmlElement parameterElement = _xmlDocumentHelpers.SelectSingleElement(this.XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(parameterElement).ToDictionary(e => e.Name);
            
            cmbListLpGenericArgumentName.SelectedValue = parameterElement.GetAttribute(XmlDataConstants.GENERICARGUMENTNAMEATTRIBUTE);
            cmbListLpLiteralType.SelectedValue = _enumHelper.ParseEnumText<LiteralParameterType>(elements[XmlDataConstants.LITERALTYPEELEMENT].InnerText);
            cmbListLpListType.SelectedValue = _enumHelper.ParseEnumText<ListType>(elements[XmlDataConstants.LISTTYPEELEMENT].InnerText);
            cmbListLpControl.SelectedValue = _enumHelper.ParseEnumText<ListParameterInputStyle>(elements[XmlDataConstants.CONTROLELEMENT].InnerText);
            cmbListLpElementControl.SelectedValue = _enumHelper.ParseEnumText<LiteralParameterInputStyle>(elements[XmlDataConstants.ELEMENTCONTROLELEMENT].InnerText);
            cmbListLpPropertySource.Text = elements[XmlDataConstants.PROPERTYSOURCEELEMENT].InnerText;
            cmbListLpPropertySourceParameter.Text = elements[XmlDataConstants.PROPERTYSOURCEPARAMETERELEMENT].InnerText;
        }

        public void UpdateXmlDocument(RadTreeNode treeNode)
        {
            if (!_treeViewService.IsGenericArgumentParameterNode(treeNode))
                throw _exceptionHelper.CriticalException("{43A15371-B21B-4A5E-8633-43AA184611F3}");

            List<string> errors = ValidateFields();
            if (errors.Count > 0)
                throw new LogicBuilderException(string.Join(Environment.NewLine, errors));

            XmlElement parameterElement = _xmlDocumentHelpers.SelectSingleElement(this.XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(parameterElement).ToDictionary(e => e.Name);

            elements[XmlDataConstants.LITERALTYPEELEMENT].InnerText = Enum.GetName(typeof(LiteralParameterType), cmbListLpLiteralType.SelectedValue)!;
            elements[XmlDataConstants.LISTTYPEELEMENT].InnerText = Enum.GetName(typeof(ListType), cmbListLpListType.SelectedValue)!;
            elements[XmlDataConstants.CONTROLELEMENT].InnerText = Enum.GetName(typeof(ListParameterInputStyle), cmbListLpControl.SelectedValue)!;
            elements[XmlDataConstants.ELEMENTCONTROLELEMENT].InnerText = Enum.GetName(typeof(LiteralParameterInputStyle), cmbListLpElementControl.SelectedValue)!;
            elements[XmlDataConstants.PROPERTYSOURCEELEMENT].InnerText = cmbListLpPropertySource.Text.Trim();
            elements[XmlDataConstants.PROPERTYSOURCEPARAMETERELEMENT].InnerText = cmbListLpPropertySourceParameter.Text.Trim();

            configureGenericArgumentsForm.ValidateXmlDocument();
        }

        private List<string> ValidateFields()
        {
            List<string> errors = new();
            ValidateListLpPropertySource
            (
                (LiteralParameterInputStyle)cmbListLpElementControl.SelectedValue, 
                errors
            );

            ValidateListLpPropertySourceParameter
            (
                (LiteralParameterInputStyle)cmbListLpElementControl.SelectedValue,
                configureGenericArgumentsForm.MemberParameters
                    .Select(m => m.Name)
                    .ToHashSet(),
                errors
            );
            return errors;
        }

        private void ValidateListLpPropertySource(LiteralParameterInputStyle inputStyle, List<string> errors)
        {
            if 
            (
                inputStyle == LiteralParameterInputStyle.PropertyInput
                    && !_typeLoadHelper.TryGetSystemType(cmbListLpPropertySource.Text, configureGenericArgumentsForm.Application, out Type? _)
            )
            {
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadFieldSourceFormat2, lblListLpPropertySource.Text, cmbListLpPropertySource.Text, lblListLpElementControl.Text, Strings.dropdownTextPropertyInput));
            }

            if (inputStyle != LiteralParameterInputStyle.PropertyInput && cmbListLpPropertySource.Text.Trim().Length > 0)
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.fieldSourceMustBeEmptyFormat, lblListLpPropertySource.Text, lblListLpElementControl.Text, Strings.dropdownTextPropertyInput));
        }

        private void ValidateListLpPropertySourceParameter(LiteralParameterInputStyle inputStyle, HashSet<string> siblingNames, List<string> errors)
        {
            if (inputStyle == LiteralParameterInputStyle.ParameterSourcedPropertyInput)
            {
                if (!siblingNames.Contains(cmbListLpPropertySourceParameter.Text.Trim()))
                    errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadPropertySourceParameterFormat3,
                        lblListLpPropertySourceParameter.Text,
                        cmbListLpPropertySourceParameter.Text,
                        string.Join(Strings.itemsCommaSeparator, siblingNames),
                        lblListLpElementControl.Text,
                        Strings.dropdownTextParameterSourcedPropertyInput));
            }

            if (inputStyle != LiteralParameterInputStyle.ParameterSourcedPropertyInput && cmbListLpPropertySourceParameter.Text.Trim().Length > 0)
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.fieldSourceMustBeEmptyFormat, lblListLpPropertySourceParameter.Text, lblListLpElementControl.Text, Strings.dropdownTextParameterSourcedPropertyInput));
        }

        private void Initialize()
        {
            this.cmbListLpGenericArgumentName.ReadOnly = true;
            radPanelParameter.AutoScroll = true;
            ((BorderPrimitive)radPanelParameter.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;
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
                _configureGenericLiteralListArgumentCommandFactory.GetUpdateGenericLiteralListDomainCommand(this)
            );

            InitializeHelperButtonCommand
            (
                txtListLpDefaultValue,
                _configureGenericLiteralListArgumentCommandFactory.GetUpdateGenericLiteralListDefaultValueCommand(this)
            );
        }

        private static void InitializeHelperButtonCommand(HelperButtonTextBox helperButtonTextBox, IClickCommand command)
        {
            helperButtonTextBox.ButtonClick += (sender, args) => command.Execute();
        }

        private void InitializeParameterControls()
        {
            InitializeReadOnlyTextBox(txtListLpDomain, Strings.collectionIndicatorText);
            InitializeReadOnlyTextBox(txtListLpDefaultValue, Strings.collectionIndicatorText);
            helpProvider.SetHelpString(this.cmbListLpGenericArgumentName, Strings.constrConfigGenericArgumentNameHelp);
            helpProvider.SetHelpString(this.cmbListLpLiteralType, Strings.constrConfigListLiteralTypeHelp);
            helpProvider.SetHelpString(this.cmbListLpListType, Strings.constrConfigListTypeHelp);
            helpProvider.SetHelpString(this.cmbListLpControl, Strings.constrConfigControlHelp);
            helpProvider.SetHelpString(this.cmbListLpElementControl, Strings.constrConfigElementControlHelp);
            helpProvider.SetHelpString(this.cmbListLpPropertySource, string.Format(CultureInfo.CurrentCulture, Strings.constrConfigPropertySourceHelpFormat, lblListLpElementControl.Text, Strings.dropdownTextPropertyInput));
            helpProvider.SetHelpString(this.cmbListLpPropertySourceParameter, string.Format(CultureInfo.CurrentCulture, Strings.constrConfigPropertySourceParameterHelpFormat, lblListLpElementControl.Text, Strings.dropdownTextParameterSourcedPropertyInput));
            helpProvider.SetHelpString(this.txtListLpDefaultValue, Strings.constrConfigDefaultValueHelp);
            helpProvider.SetHelpString(this.txtListLpDomain, Strings.constrConfigDomainHelp);
            toolTip.SetToolTip(this.lblListLpGenericArgumentName, Strings.constrConfigGenericArgumentNameHelp);
            toolTip.SetToolTip(this.lblListLpLiteralType, Strings.constrConfigListLiteralTypeHelp);
            toolTip.SetToolTip(this.lblListLpListType, Strings.constrConfigListTypeHelp);
            toolTip.SetToolTip(this.lblListLpControl, Strings.constrConfigControlHelp);
            toolTip.SetToolTip(this.lblListLpElementControl, Strings.constrConfigElementControlHelp);
            toolTip.SetToolTip(this.lblListLpPropertySource, string.Format(CultureInfo.CurrentCulture, Strings.constrConfigPropertySourceHelpFormat, lblListLpElementControl.Text, Strings.dropdownTextPropertyInput));
            toolTip.SetToolTip(this.lblListLpPropertySourceParameter, string.Format(CultureInfo.CurrentCulture, Strings.constrConfigPropertySourceParameterHelpFormat, lblListLpElementControl.Text, Strings.dropdownTextParameterSourcedPropertyInput));
            toolTip.SetToolTip(this.lblListLpDefaultValue, Strings.constrConfigDefaultValueHelp);
            toolTip.SetToolTip(this.lblListLpDomain, Strings.constrConfigDomainHelp);
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
            _radDropDownListHelper.LoadTextItems(cmbListLpGenericArgumentName, configureGenericArgumentsForm.ConfiguredGenericArgumentNames);
            _radDropDownListHelper.LoadComboItems<LiteralParameterType>(cmbListLpLiteralType);
            _radDropDownListHelper.LoadComboItems<ListType>(cmbListLpListType);
            _radDropDownListHelper.LoadComboItems(cmbListLpControl, RadDropDownStyle.DropDownList, new ListParameterInputStyle[] { ListParameterInputStyle.Connectors });
            _radDropDownListHelper.LoadComboItems<LiteralParameterInputStyle>(cmbListLpElementControl);
            if (configureGenericArgumentsForm.Application.AssemblyAvailable)
                _radDropDownListHelper.LoadTextItems(cmbListLpPropertySource.RadDropDownList, configureGenericArgumentsForm.Application.AllTypesList, RadDropDownStyle.DropDown);

            _radDropDownListHelper.LoadTextItems
            (
                cmbListLpPropertySourceParameter, 
                configureGenericArgumentsForm.MemberParameters
                    .OrderBy(m => m.Name)
                    .Select(m => m.Name), 
                RadDropDownStyle.DropDown
            );
        }
    }
}