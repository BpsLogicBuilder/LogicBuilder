using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.Helpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.StateImageSetters;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureObjectParameter
{
    internal partial class ConfigureObjectParameterControl : UserControl, IConfigureObjectParameterControl
    {
        private readonly IConfigureParametersStateImageSetter _configureParametersStateImageSetter;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IObjectParameterControlValidator _objectParameterControlValidator;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly ITypeAutoCompleteManager _cmbCpObjectTypeTypeAutoCompleteManager;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigurationForm configurationForm;

        public ConfigureObjectParameterControl(
            IConfigureParametersStateImageSetter configureParametersStateImageSetter,
            IExceptionHelper exceptionHelper,
            IParameterControlValidatorFactory parameterControlValidatorFactory,
            IRadDropDownListHelper radDropDownListHelper,
            IServiceFactory serviceFactory,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigurationForm configurationForm)
        {
            InitializeComponent();
            _cmbCpObjectTypeTypeAutoCompleteManager = serviceFactory.GetTypeAutoCompleteManager
            (
                configurationForm,
                cmbCpObjectType
            );
            _configureParametersStateImageSetter = configureParametersStateImageSetter;
            _exceptionHelper = exceptionHelper;
            _objectParameterControlValidator = parameterControlValidatorFactory.GetObjectParameterControlValidator(this);
            _radDropDownListHelper = radDropDownListHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configurationForm = configurationForm;
            Initialize();
        }

        private readonly HelpProvider helpProvider = new();
        private readonly RadToolTip toolTip = new();

        public RadLabel LblCpName => lblCpName;
        public RadTextBox TxtCpName => txtCpName;
        public RadTreeView TreeView => configurationForm.TreeView;
        public XmlDocument XmlDocument => configurationForm.XmlDocument;

        public void ClearMessage() => configurationForm.ClearMessage();

        public void SetControlValues(RadTreeNode treeNode)
        {
            if (!_treeViewService.IsObjectTypeNode(treeNode))
                throw _exceptionHelper.CriticalException("{54A37476-9C88-49A6-B10E-A7D7C419493D}");

            RemoveEventHandlers();
            XmlElement parameterElement = _xmlDocumentHelpers.SelectSingleElement(XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(parameterElement).ToDictionary(e => e.Name);

            txtCpName.Text = parameterElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE);
            txtCpName.Select();
            txtCpName.SelectAll();
            cmbCpObjectType.Text = elements[XmlDataConstants.OBJECTTYPEELEMENT].InnerText;
            cmbCpOptional.SelectedValue = bool.Parse(elements[XmlDataConstants.OPTIONALELEMENT].InnerText);
            cmbCpUseForEquality.SelectedValue = bool.Parse(elements[XmlDataConstants.USEFOREQUALITYELEMENT].InnerText);
            cmbCpUseForHashCode.SelectedValue = bool.Parse(elements[XmlDataConstants.USEFORHASHCODEELEMENT].InnerText);
            cmbCpUseForToString.SelectedValue = bool.Parse(elements[XmlDataConstants.USEFORTOSTRINGELEMENT].InnerText);
            txtCpComments.Text = elements[XmlDataConstants.COMMENTSELEMENT].InnerText;

            AddEventHandlers();
        }

        public void SetErrorMessage(string message) => configurationForm.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => configurationForm.SetMessage(message, title);

        public void UpdateXmlDocument(RadTreeNode treeNode)
        {
            if (TreeView.SelectedNode == null)
                return;

            if (!_treeViewService.IsObjectTypeNode(treeNode))
                throw _exceptionHelper.CriticalException("{17C8A4C5-A59C-4641-9F7C-B49577E553FA}");

            _objectParameterControlValidator.ValidateInputBoxes();

            XmlElement parameterElement = _xmlDocumentHelpers.SelectSingleElement(XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(parameterElement).ToDictionary(e => e.Name);
            string newNameAttributeValue = txtCpName.Text.Trim();

            elements[XmlDataConstants.OBJECTTYPEELEMENT].InnerText = cmbCpObjectType.Text;
            elements[XmlDataConstants.OPTIONALELEMENT].InnerText = bool.Parse(cmbCpOptional.SelectedValue.ToString()!).ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
            elements[XmlDataConstants.USEFOREQUALITYELEMENT].InnerText = bool.Parse(cmbCpUseForEquality.SelectedValue.ToString()!).ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
            elements[XmlDataConstants.USEFORHASHCODEELEMENT].InnerText = bool.Parse(cmbCpUseForHashCode.SelectedValue.ToString()!).ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
            elements[XmlDataConstants.USEFORTOSTRINGELEMENT].InnerText = bool.Parse(cmbCpUseForToString.SelectedValue.ToString()!).ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
            elements[XmlDataConstants.COMMENTSELEMENT].InnerText = txtCpComments.Text.Trim();
            parameterElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value = newNameAttributeValue;

            configurationForm.ValidateXmlDocument();

            treeNode.Name = $"{treeNode.Parent.Name}/{XmlDataConstants.PARAMETERSELEMENT}/{XmlDataConstants.OBJECTPARAMETERELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{newNameAttributeValue}\"]";
            treeNode.Text = newNameAttributeValue;
            _configureParametersStateImageSetter.SetImage(parameterElement, (StateImageRadTreeNode)treeNode, configurationForm.Application);
        }

        public void ValidateFields() => _objectParameterControlValidator.ValidateInputBoxes();

        public void ValidateXmlDocument() => configurationForm.ValidateXmlDocument();

        private void AddEventHandlers()
        {
            txtCpName.TextChanged += TxtCpName_TextChanged;
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
            _cmbCpObjectTypeTypeAutoCompleteManager.Setup();
        }

        private void InitializeParameterControls()
        {
            helpProvider.SetHelpString(txtCpName, Strings.constrConfigParameterNameHelp);
            helpProvider.SetHelpString(cmbCpObjectType, Strings.constrConfigObjectTypeHelp);
            helpProvider.SetHelpString(cmbCpOptional, Strings.constrConfigOptionalHelp);
            helpProvider.SetHelpString(cmbCpUseForEquality, Strings.constrConfigUseForEqualityHelp);
            helpProvider.SetHelpString(cmbCpUseForHashCode, Strings.constrConfigUseForHashCodeHelp);
            helpProvider.SetHelpString(cmbCpUseForToString, Strings.constrConfigUseForToStringHelp);
            helpProvider.SetHelpString(txtCpComments, Strings.constrConfigCommentsHelp);
            toolTip.SetToolTip(lblCpName, Strings.constrConfigParameterNameHelp);
            toolTip.SetToolTip(lblCpObjectType, Strings.constrConfigObjectTypeHelp);
            toolTip.SetToolTip(lblCpOptional, Strings.constrConfigOptionalHelp);
            toolTip.SetToolTip(lblCpUseForEquality, Strings.constrConfigUseForEqualityHelp);
            toolTip.SetToolTip(lblCpUseForHashCode, Strings.constrConfigUseForHashCodeHelp);
            toolTip.SetToolTip(lblCpUseForToString, Strings.constrConfigUseForToStringHelp);
            toolTip.SetToolTip(lblCpComments, Strings.constrConfigCommentsHelp);
        }

        private void InitializeTableLayoutPanel()
        {
            ControlsLayoutUtility.LayoutControls
            (
                groupBoxParameter,
                radPanelParameter,
                radPanelTableParent,
                tableLayoutPanel,
                7
            );
        }

        private void LoadParameterDropDownLists()
        {
            _radDropDownListHelper.LoadBooleans(cmbCpOptional);
            _radDropDownListHelper.LoadBooleans(cmbCpUseForEquality);
            _radDropDownListHelper.LoadBooleans(cmbCpUseForHashCode);
            _radDropDownListHelper.LoadBooleans(cmbCpUseForToString);
        }

        private void RemoveEventHandlers()
        {
            txtCpName.TextChanged -= TxtCpName_TextChanged;
        }

        private void TxtCpName_TextChanged(object? sender, EventArgs e)
        {
            try
            {
                _objectParameterControlValidator.ValidateInputBoxes();
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
