using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.Helpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
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

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureGenericParameter
{
    internal partial class ConfigureGenericParameterControl : UserControl, IConfigureGenericParameterControl
    {
        private readonly IConfigureParametersStateImageSetter _configureParametersStateImageSetter;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IGenericParameterControlValidator _genericParameterControlValidator;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigurationForm configurationForm;

        public ConfigureGenericParameterControl(
            IConfigureParametersStateImageSetter configureParametersStateImageSetter,
            IExceptionHelper exceptionHelper,
            IParameterControlValidatorFactory parameterControlValidatorFactory,
            IRadDropDownListHelper radDropDownListHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigurationForm configurationForm)
        {
            InitializeComponent();
            _configureParametersStateImageSetter = configureParametersStateImageSetter;
            _exceptionHelper = exceptionHelper;
            _genericParameterControlValidator = parameterControlValidatorFactory.GetGenericParameterControlValidator(this);
            _radDropDownListHelper = radDropDownListHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configurationForm = configurationForm;
            Initialize();
        }

        private readonly HelpProvider helpProvider = new();
        private readonly RadToolTip toolTip = new();

        public RadDropDownList CmbGpGenericArgumentName => cmbGpGenericArgumentName;
        public RadLabel LblGpGenericArgumentName => lblGpGenericArgumentName;
        public RadTreeView TreeView => configurationForm.TreeView;
        public XmlDocument XmlDocument => configurationForm.XmlDocument;

        public void ClearMessage() => configurationForm.ClearMessage();

        public void SetControlValues(RadTreeNode treeNode)
        {
            if (!_treeViewService.IsGenericTypeNode(treeNode))
                throw _exceptionHelper.CriticalException("{A4C3A301-EC48-4B2F-8F42-F1C76A6509A8}");

            RemoveEventHandlers();
            LoadGenericArgumentsComboBox(treeNode);
            XmlElement parameterElement = _xmlDocumentHelpers.SelectSingleElement(XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(parameterElement).ToDictionary(e => e.Name);
            txtGpName.Text = parameterElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE);
            txtGpName.Select();
            txtGpName.SelectAll();
            cmbGpGenericArgumentName.SelectedValue = elements[XmlDataConstants.GENERICARGUMENTNAMEELEMENT].InnerText;
            cmbGpOptional.SelectedValue = bool.Parse(elements[XmlDataConstants.OPTIONALELEMENT].InnerText);
            txtGpComments.Text = elements[XmlDataConstants.COMMENTSELEMENT].InnerText;
            AddEventHandlers();
        }

        public void SetErrorMessage(string message) => configurationForm.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => configurationForm.SetMessage(message, title);

        public void UpdateXmlDocument(RadTreeNode treeNode)
        {
            if (TreeView.SelectedNode == null)
                return;

            if (!_treeViewService.IsGenericTypeNode(treeNode))
                throw _exceptionHelper.CriticalException("{93F4D830-ACAF-432E-BD53-239CCBA4E6F3}");

            _genericParameterControlValidator.ValidateInputBoxes();

            XmlElement parameterElement = _xmlDocumentHelpers.SelectSingleElement(XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(parameterElement).ToDictionary(e => e.Name);
            string newNameAttributeValue = txtGpName.Text.Trim();

            elements[XmlDataConstants.GENERICARGUMENTNAMEELEMENT].InnerText = cmbGpGenericArgumentName.SelectedItem.Text;
            elements[XmlDataConstants.OPTIONALELEMENT].InnerText = bool.Parse(cmbGpOptional.SelectedValue.ToString()!).ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
            elements[XmlDataConstants.COMMENTSELEMENT].InnerText = txtGpComments.Text.Trim();
            parameterElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value = newNameAttributeValue;

            configurationForm.ValidateXmlDocument();

            treeNode.Name = $"{treeNode.Parent.Name}/{XmlDataConstants.PARAMETERSELEMENT}/{XmlDataConstants.GENERICPARAMETERELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{newNameAttributeValue}\"]";
            treeNode.Text = newNameAttributeValue;
            _configureParametersStateImageSetter.SetImage(parameterElement, (StateImageRadTreeNode)treeNode, configurationForm.Application);
        }

        public void ValidateFields() => _genericParameterControlValidator.ValidateInputBoxes();

        public void ValidateXmlDocument() => configurationForm.ValidateXmlDocument();

        private void AddEventHandlers()
        {
            txtGpName.TextChanged += TxtGpName_TextChanged;
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            radPanelParameter.AutoScroll = true;
            AddEventHandlers();
            CollapsePanelBorder(radPanelParameter);
            CollapsePanelBorder(radPanelTableParent);
            InitializeParameterControls();
            LoadParameterDropDownLists();
        }

        private void InitializeParameterControls()
        {
            helpProvider.SetHelpString(txtGpName, Strings.constrConfigParameterNameHelp);
            helpProvider.SetHelpString(cmbGpGenericArgumentName, Strings.constrConfigGenericArgumentNameHelp);
            helpProvider.SetHelpString(cmbGpOptional, Strings.constrConfigOptionalHelp);
            helpProvider.SetHelpString(txtGpComments, Strings.constrConfigCommentsHelp);
            toolTip.SetToolTip(lblGpName, Strings.constrConfigParameterNameHelp);
            toolTip.SetToolTip(lblGpGenericArgumentName, Strings.constrConfigGenericArgumentNameHelp);
            toolTip.SetToolTip(lblGpOptional, Strings.constrConfigOptionalHelp);
            toolTip.SetToolTip(lblGpComments, Strings.constrConfigCommentsHelp);
        }

        private void LoadGenericArgumentsComboBox(RadTreeNode parameterTreeNode)
        {
            cmbGpGenericArgumentName.DropDownStyle = RadDropDownStyle.DropDownList;
            cmbGpGenericArgumentName.Items.Clear();
            cmbGpGenericArgumentName.Items.AddRange
            (
                _xmlDocumentHelpers.GetGenericArguments
                (
                    XmlDocument, 
                    $"{parameterTreeNode.Parent.Name}/{XmlDataConstants.GENERICARGUMENTSELEMENT}"
                )
            );
        }

        private void LoadParameterDropDownLists()
        {
            _radDropDownListHelper.LoadBooleans(cmbGpOptional);
        }

        private void RemoveEventHandlers()
        {
            txtGpName.TextChanged += TxtGpName_TextChanged;
        }

        private void TxtGpName_TextChanged(object? sender, EventArgs e)
        {
            try
            {
                _genericParameterControlValidator.ValidateInputBoxes();
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
