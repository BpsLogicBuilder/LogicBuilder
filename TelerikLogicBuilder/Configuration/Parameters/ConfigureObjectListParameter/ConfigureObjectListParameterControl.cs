using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.Helpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
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

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureObjectListParameter
{
    internal partial class ConfigureObjectListParameterControl : UserControl, IConfigureObjectListParameterControl
    {
        private readonly IConfigureParametersStateImageSetter _configureParametersStateImageSetter;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IObjectListParameterControlValidator _objectListParameterControlValidator;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly ITypeAutoCompleteManager _cmbListCpObjectTypeTypeAutoCompleteManager;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigurationForm configurationForm;

        public ConfigureObjectListParameterControl(
            IConfigureParametersStateImageSetter configureParametersStateImageSetter,
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IParameterControlValidatorFactory parameterControlValidatorFactory,
            IRadDropDownListHelper radDropDownListHelper,
            IServiceFactory serviceFactory,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigurationForm configurationForm)
        {
            InitializeComponent();
            _cmbListCpObjectTypeTypeAutoCompleteManager = serviceFactory.GetTypeAutoCompleteManager
            (
                configurationForm,
                cmbListCpObjectType
            );
            _configureParametersStateImageSetter = configureParametersStateImageSetter;
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _objectListParameterControlValidator = parameterControlValidatorFactory.GetObjectListParameterControlValidator(this);
            _radDropDownListHelper = radDropDownListHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configurationForm = configurationForm;
            Initialize();
        }

        private readonly HelpProvider helpProvider = new();
        private readonly RadToolTip toolTip = new();

        public RadLabel LblListCpName => lblListCpName;
        public RadTextBox TxtListCpName => txtListCpName;
        public RadTreeView TreeView => configurationForm.TreeView;
        public XmlDocument XmlDocument => configurationForm.XmlDocument;

        public void ClearMessage() => configurationForm.ClearMessage();

        public void SetControlValues(RadTreeNode treeNode)
        {
            if (!_treeViewService.IsListOfObjectsTypeNode(treeNode))
                throw _exceptionHelper.CriticalException("{46B3CAC4-7EA3-4D5D-B472-DDEDA8AD0244}");

            RemoveEventHandlers();
            XmlElement parameterElement = _xmlDocumentHelpers.SelectSingleElement(XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(parameterElement).ToDictionary(e => e.Name);

            txtListCpName.Text = parameterElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE);
            txtListCpName.Select();
            txtListCpName.SelectAll();
            cmbListCpObjectType.Text = elements[XmlDataConstants.OBJECTTYPEELEMENT].InnerText;
            cmbListCpListType.SelectedValue = _enumHelper.ParseEnumText<ListType>(elements[XmlDataConstants.LISTTYPEELEMENT].InnerText);
            cmbListCpControl.SelectedValue = _enumHelper.ParseEnumText<ListParameterInputStyle>(elements[XmlDataConstants.CONTROLELEMENT].InnerText);
            cmbListCpOptional.SelectedValue = bool.Parse(elements[XmlDataConstants.OPTIONALELEMENT].InnerText);
            txtListCpComments.Text = elements[XmlDataConstants.COMMENTSELEMENT].InnerText;

            AddEventHandlers();
        }

        public void SetErrorMessage(string message) => configurationForm.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => configurationForm.SetMessage(message, title);

        public void UpdateXmlDocument(RadTreeNode treeNode)
        {
            if (TreeView.SelectedNode == null)
                return;

            if (!_treeViewService.IsListOfObjectsTypeNode(treeNode))
                throw _exceptionHelper.CriticalException("{242E173C-96FD-4CE9-9500-CB10A110B670}");

            _objectListParameterControlValidator.ValidateInputBoxes();

            XmlElement parameterElement = _xmlDocumentHelpers.SelectSingleElement(XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(parameterElement).ToDictionary(e => e.Name);
            string newNameAttributeValue = txtListCpName.Text.Trim();

            elements[XmlDataConstants.OBJECTTYPEELEMENT].InnerText = cmbListCpObjectType.Text;
            elements[XmlDataConstants.LISTTYPEELEMENT].InnerText = Enum.GetName(typeof(ListType), cmbListCpListType.SelectedValue)!;
            elements[XmlDataConstants.CONTROLELEMENT].InnerText = Enum.GetName(typeof(ListParameterInputStyle), cmbListCpControl.SelectedValue)!;
            elements[XmlDataConstants.OPTIONALELEMENT].InnerText = bool.Parse(cmbListCpOptional.SelectedValue.ToString()!).ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
            elements[XmlDataConstants.COMMENTSELEMENT].InnerText = txtListCpComments.Text.Trim();
            parameterElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value = newNameAttributeValue;

            configurationForm.ValidateXmlDocument();

            treeNode.Name = $"{treeNode.Parent.Name}/{XmlDataConstants.PARAMETERSELEMENT}/{XmlDataConstants.OBJECTLISTPARAMETERELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{newNameAttributeValue}\"]";
            treeNode.Text = newNameAttributeValue;
            _configureParametersStateImageSetter.SetImage(parameterElement, (StateImageRadTreeNode)treeNode, configurationForm.Application);
        }

        public void ValidateFields() => _objectListParameterControlValidator.ValidateInputBoxes();

        public void ValidateXmlDocument() => configurationForm.ValidateXmlDocument();

        private void AddEventHandlers()
        {
            txtListCpName.TextChanged += TxtListCpName_TextChanged;
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
            _cmbListCpObjectTypeTypeAutoCompleteManager.Setup();
        }

        private void InitializeParameterControls()
        {
            helpProvider.SetHelpString(txtListCpName, Strings.constrConfigParameterNameHelp);
            helpProvider.SetHelpString(cmbListCpObjectType, Strings.constrConfigListObjectTypeHelp);
            helpProvider.SetHelpString(cmbListCpListType, Strings.constrConfigListTypeHelp);
            helpProvider.SetHelpString(cmbListCpControl, Strings.constrConfigControlHelp);
            helpProvider.SetHelpString(cmbListCpOptional, Strings.constrConfigOptionalHelp);
            helpProvider.SetHelpString(txtListCpComments, Strings.constrConfigCommentsHelp);
            toolTip.SetToolTip(lblListCpName, Strings.constrConfigParameterNameHelp);
            toolTip.SetToolTip(lblListCpObjectType, Strings.constrConfigListObjectTypeHelp);
            toolTip.SetToolTip(lblListCpListType, Strings.constrConfigListTypeHelp);
            toolTip.SetToolTip(lblListCpControl, Strings.constrConfigControlHelp);
            toolTip.SetToolTip(lblListCpOptional, Strings.constrConfigOptionalHelp);
            toolTip.SetToolTip(lblListCpComments, Strings.constrConfigCommentsHelp);
        }

        private void LoadParameterDropDownLists()
        {
            _radDropDownListHelper.LoadComboItems<ListType>(cmbListCpListType);
            _radDropDownListHelper.LoadComboItems(cmbListCpControl, RadDropDownStyle.DropDownList, new ListParameterInputStyle[] { ListParameterInputStyle.Connectors });
            _radDropDownListHelper.LoadBooleans(cmbListCpOptional);
        }

        private void RemoveEventHandlers()
        {
            txtListCpName.TextChanged -= TxtListCpName_TextChanged;
        }

        private void TxtListCpName_TextChanged(object? sender, EventArgs e)
        {
            try
            {
                _objectListParameterControlValidator.ValidateInputBoxes();
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
