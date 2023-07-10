using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.Helpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
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

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureGenericListParameter
{
    internal partial class ConfigureGenericListParameterControl : UserControl, IConfigureGenericListParameterControl
    {
        private readonly IConfigureParametersStateImageSetter _configureParametersStateImageSetter;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IGenericListParameterControlValidator _genericListParameterControlValidator;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigurationForm configurationForm;

        public ConfigureGenericListParameterControl(
            IConfigureParametersStateImageSetter configureParametersStateImageSetter,
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IParameterControlValidatorFactory parameterControlValidatorFactory,
            IRadDropDownListHelper radDropDownListHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigurationForm configurationForm)
        {
            InitializeComponent();
            _configureParametersStateImageSetter = configureParametersStateImageSetter;
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _genericListParameterControlValidator = parameterControlValidatorFactory.GetGenericListParameterControlValidator(this);
            _radDropDownListHelper = radDropDownListHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configurationForm = configurationForm;
            Initialize();
        }

        private readonly HelpProvider helpProvider = new();
        private readonly RadToolTip toolTip = new();

        public RadDropDownList CmbListGpGenericArgumentName => cmbListGpGenericArgumentName;
        public RadLabel LblListGpGenericArgumentName => lblListGpGenericArgumentName;
        public RadTreeView TreeView => configurationForm.TreeView;
        public XmlDocument XmlDocument => configurationForm.XmlDocument;

        public void ClearMessage() => configurationForm.ClearMessage();

        public void SetControlValues(RadTreeNode treeNode)
        {
            if (!_treeViewService.IsListOfGenericsTypeNode(treeNode))
                throw _exceptionHelper.CriticalException("{123A063D-FA1C-4333-9496-47513CBA4515}");

            RemoveEventHandlers();
            LoadGenericArgumentsComboBox(treeNode);
            XmlElement parameterElement = _xmlDocumentHelpers.SelectSingleElement(XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(parameterElement).ToDictionary(e => e.Name);

            txtListGpName.Text = parameterElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value;
            txtListGpName.Select();
            txtListGpName.SelectAll();
            cmbListGpGenericArgumentName.SelectedValue = elements[XmlDataConstants.GENERICARGUMENTNAMEELEMENT].InnerText;
            cmbListGpListType.SelectedValue = _enumHelper.ParseEnumText<ListType>(elements[XmlDataConstants.LISTTYPEELEMENT].InnerText);
            cmbListGpControl.SelectedValue = _enumHelper.ParseEnumText<ListParameterInputStyle>(elements[XmlDataConstants.CONTROLELEMENT].InnerText);
            cmbListGpOptional.SelectedValue = bool.Parse(elements[XmlDataConstants.OPTIONALELEMENT].InnerText);
            txtListGpComments.Text = elements[XmlDataConstants.COMMENTSELEMENT].InnerText;
            AddEventHandlers();
        }

        public void SetErrorMessage(string message) => configurationForm.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => configurationForm.SetMessage(message, title);

        public void UpdateXmlDocument(RadTreeNode treeNode)
        {
            if (TreeView.SelectedNode == null)
                return;

            if (!_treeViewService.IsListOfGenericsTypeNode(treeNode))
                throw _exceptionHelper.CriticalException("{506E77E2-AC68-4488-8B03-E0D364695B09}");

            _genericListParameterControlValidator.ValidateInputBoxes();

            XmlElement parameterElement = _xmlDocumentHelpers.SelectSingleElement(XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(parameterElement).ToDictionary(e => e.Name);
            string newNameAttributeValue = txtListGpName.Text.Trim();

            elements[XmlDataConstants.GENERICARGUMENTNAMEELEMENT].InnerText = cmbListGpGenericArgumentName.SelectedItem.Text;
            elements[XmlDataConstants.LISTTYPEELEMENT].InnerText = Enum.GetName(typeof(ListType), cmbListGpListType.SelectedValue)!;
            elements[XmlDataConstants.CONTROLELEMENT].InnerText = Enum.GetName(typeof(ListParameterInputStyle), cmbListGpControl.SelectedValue)!;
            elements[XmlDataConstants.OPTIONALELEMENT].InnerText = bool.Parse(cmbListGpOptional.SelectedValue.ToString()!).ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
            elements[XmlDataConstants.COMMENTSELEMENT].InnerText = txtListGpComments.Text.Trim();
            parameterElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value = newNameAttributeValue;

            configurationForm.ValidateXmlDocument();

            treeNode.Name = $"{treeNode.Parent.Name}/{XmlDataConstants.PARAMETERSELEMENT}/{XmlDataConstants.GENERICLISTPARAMETERELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{newNameAttributeValue}\"]";
            treeNode.Text = newNameAttributeValue;
            _configureParametersStateImageSetter.SetImage(parameterElement, (StateImageRadTreeNode)treeNode, configurationForm.Application);
        }

        public void ValidateFields() => _genericListParameterControlValidator.ValidateInputBoxes();

        public void ValidateXmlDocument() => configurationForm.ValidateXmlDocument();

        private void AddEventHandlers()
        {
            txtListGpName.TextChanged += TxtListGpName_TextChanged;
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private static void CollapsePanelBorder(RadScrollablePanel radPanel)
            => radPanel.PanelElement.Border.Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            Disposed += ConfigureGenericListParameterControl_Disposed;
            radPanelParameter.VerticalScrollBarState = ScrollState.AlwaysShow;
            InitializeTableLayoutPanel();
            CollapsePanelBorder(radPanelParameter);
            CollapsePanelBorder(radPanelTableParent);
            InitializeParameterControls();
            LoadParameterDropDownLists();
        }

        private void InitializeParameterControls()
        {
            helpProvider.SetHelpString(txtListGpName, Strings.constrConfigParameterNameHelp);
            helpProvider.SetHelpString(cmbListGpGenericArgumentName, Strings.constrConfigGenericArgumentNameHelp);
            helpProvider.SetHelpString(cmbListGpListType, Strings.constrConfigListTypeHelp);
            helpProvider.SetHelpString(cmbListGpControl, Strings.constrConfigControlHelp);
            helpProvider.SetHelpString(cmbListGpOptional, Strings.constrConfigOptionalHelp);
            helpProvider.SetHelpString(txtListGpComments, Strings.constrConfigCommentsHelp);
            toolTip.SetToolTip(lblListGpName, Strings.constrConfigParameterNameHelp);
            toolTip.SetToolTip(lblListGpGenericArgumentName, Strings.constrConfigGenericArgumentNameHelp);
            toolTip.SetToolTip(lblListGpListType, Strings.constrConfigListTypeHelp);
            toolTip.SetToolTip(lblListGpControl, Strings.constrConfigControlHelp);
            toolTip.SetToolTip(lblListGpOptional, Strings.constrConfigOptionalHelp);
            toolTip.SetToolTip(lblListGpComments, Strings.constrConfigCommentsHelp);
        }

        private void InitializeTableLayoutPanel()
        {
            ControlsLayoutUtility.LayoutControls
            (
                groupBoxParameter,
                radPanelParameter,
                radPanelTableParent,
                tableLayoutPanel,
                6
            );
        }

        private void LoadGenericArgumentsComboBox(RadTreeNode parameterTreeNode)
        {
            cmbListGpGenericArgumentName.DropDownStyle = RadDropDownStyle.DropDownList;
            cmbListGpGenericArgumentName.Items.Clear();
            cmbListGpGenericArgumentName.Items.AddRange
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
            _radDropDownListHelper.LoadComboItems<ListType>(cmbListGpListType);
            _radDropDownListHelper.LoadComboItems(cmbListGpControl, RadDropDownStyle.DropDownList, new ListParameterInputStyle[] { ListParameterInputStyle.Connectors });
            _radDropDownListHelper.LoadBooleans(cmbListGpOptional);
        }

        private void RemoveEventHandlers()
        {
            txtListGpName.TextChanged -= TxtListGpName_TextChanged;
        }

        #region Event Handlers
        private void ConfigureGenericListParameterControl_Disposed(object? sender, EventArgs e)
        {
            toolTip.RemoveAll();
            toolTip.Dispose();
            helpProvider.Dispose();
            RemoveEventHandlers();
        }

        private void TxtListGpName_TextChanged(object? sender, EventArgs e)
        {
            try
            {
                _genericListParameterControlValidator.ValidateInputBoxes();
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
        #endregion Event Handlers
    }
}
