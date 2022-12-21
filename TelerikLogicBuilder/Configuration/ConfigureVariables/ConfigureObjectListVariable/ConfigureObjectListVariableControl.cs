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
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureObjectListVariable
{
    internal partial class ConfigureObjectListVariableControl : UserControl, IConfigureObjectListVariableControl
    {
        private readonly IConfigureVariablesStateImageSetter _configureVariablesStateImageSetter;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IObjectListVariableControlsValidator _objectListVariableControlsValidator;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly IStringHelper _stringHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly ITypeAutoCompleteManager _cmbCvListObjectTypeTypeAutoCompleteManager;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureVariablesForm configureVariablesForm;

        public ConfigureObjectListVariableControl(
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
            _cmbCvListObjectTypeTypeAutoCompleteManager = serviceFactory.GetTypeAutoCompleteManager
            (
                configureVariablesForm,
                cmbCvListObjectType
            );
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _objectListVariableControlsValidator = variableControlValidatorFactory.GetObjectListVariableControlsValidator(this);
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
        private RadTreeView TreeView => configureVariablesForm.TreeView;
        private XmlDocument XmlDocument => configureVariablesForm.XmlDocument;

        public AutoCompleteRadDropDownList CmbObjectType => cmbCvListObjectType;
        public RadLabel LblObjectType => lblCvListObjectType;
        public RadLabel LblName => lblCvListName;
        public RadTextBox TxtName => txtCvListName;
        public RadTextBox TxtMemberName => txtCvListMemberName;
        public RadDropDownList CmbVariableCategory => cmbCvListVariableCategory;
        public RadLabel LblCastVariableAs => lblCvListCastVariableAs;
        public RadTextBox TxtCastVariableAs => txtCvListCastVariableAs;
        public RadLabel LblTypeName => lblCvListTypeName;
        public RadTextBox TxtTypeName => txtCvListTypeName;
        public RadTextBox TxtReferenceName => txtCvListReferenceName;
        public RadDropDownList CmbReferenceDefinition => cmbCvListReferenceDefinition;
        public RadTextBox TxtCastReferenceAs => txtCvListCastReferenceAs;
        public RadDropDownList CmbReferenceCategory => cmbCvListReferenceCategory;
        public ApplicationTypeInfo Application => configureVariablesForm.Application;
        public IDictionary<string, VariableBase> VariablesDictionary => configureVariablesForm.VariablesDictionary;
        public HashSet<string> VariableNames => configureVariablesForm.VariableNames;
        #endregion Properties

        public void ClearMessage()
        {
            this.configureVariablesForm.ClearMessage();
        }

        public void SetControlValues(RadTreeNode treeNode)
        {
            if (!_treeViewService.IsListOfObjectsTypeNode(treeNode))
                throw _exceptionHelper.CriticalException("{B54C9E53-53A0-4890-A2BC-FA7A645B252E}");

            RemoveEventHandlers();
            XmlElement variableElement = _xmlDocumentHelpers.SelectSingleElement(this.XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(variableElement).ToDictionary(e => e.Name);

            txtCvListName.Text = variableElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE);
            txtCvListName.Select();
            txtCvListName.SelectAll();
            txtCvListMemberName.Text = elements[XmlDataConstants.MEMBERNAMEELEMENT].InnerText;
            cmbCvListVariableCategory.SelectedValue = _enumHelper.ParseEnumText<VariableCategory>(elements[XmlDataConstants.VARIABLECATEGORYELEMENT].InnerText);
            txtCvListCastVariableAs.Text = elements[XmlDataConstants.CASTVARIABLEASELEMENT].InnerText;
            txtCvListTypeName.Text = elements[XmlDataConstants.TYPENAMEELEMENT].InnerText;
            txtCvListReferenceName.Text = elements[XmlDataConstants.REFERENCENAMEELEMENT].InnerText;
            cmbCvListReferenceDefinition.Text = string.Join
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
            txtCvListCastReferenceAs.Text = elements[XmlDataConstants.CASTREFERENCEASELEMENT].InnerText;
            cmbCvListReferenceCategory.SelectedValue = _enumHelper.ParseEnumText<ReferenceCategories>(elements[XmlDataConstants.REFERENCECATEGORYELEMENT].InnerText);
            txtCvListComments.Text = elements[XmlDataConstants.COMMENTSELEMENT].InnerText;
            cmbCvListObjectType.Text = elements[XmlDataConstants.OBJECTTYPEELEMENT].InnerText;
            cmbCvListListType.SelectedValue = _enumHelper.ParseEnumText<ListType>(elements[XmlDataConstants.LISTTYPEELEMENT].InnerText);
            cmbCvListListControl.SelectedValue = _enumHelper.ParseEnumText<ListVariableInputStyle>(elements[XmlDataConstants.CONTROLELEMENT].InnerText);
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

            if (!_treeViewService.IsListOfObjectsTypeNode(treeNode))
                throw _exceptionHelper.CriticalException("{D567560B-24C9-4123-87CA-69050A5F54CE}");

            _objectListVariableControlsValidator.ValidateInputBoxes();

            XmlElement variableElement = _xmlDocumentHelpers.SelectSingleElement(this.XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(variableElement).ToDictionary(e => e.Name);
            string currentNameAttributeValue = variableElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE);
            string newNameAttributeValue = txtCvListName.Text.Trim();

            _objectListVariableControlsValidator.ValidateForExistingVariableName(currentNameAttributeValue);

            elements[XmlDataConstants.MEMBERNAMEELEMENT].InnerText = txtCvListMemberName.Text.Trim();
            elements[XmlDataConstants.VARIABLECATEGORYELEMENT].InnerText = Enum.GetName(typeof(VariableCategory), cmbCvListVariableCategory.SelectedValue)!;
            elements[XmlDataConstants.CASTVARIABLEASELEMENT].InnerText = txtCvListCastVariableAs.Text.Trim();
            elements[XmlDataConstants.TYPENAMEELEMENT].InnerText = txtCvListTypeName.Text.Trim();
            elements[XmlDataConstants.REFERENCENAMEELEMENT].InnerText = txtCvListReferenceName.Text.Trim();
            elements[XmlDataConstants.REFERENCEDEFINITIONELEMENT].InnerText = _enumHelper.BuildValidReferenceDefinition(cmbCvListReferenceDefinition.Text);
            elements[XmlDataConstants.CASTREFERENCEASELEMENT].InnerText = txtCvListCastReferenceAs.Text.Trim();
            elements[XmlDataConstants.REFERENCECATEGORYELEMENT].InnerText = Enum.GetName(typeof(ReferenceCategories), cmbCvListReferenceCategory.SelectedValue)!;
            elements[XmlDataConstants.COMMENTSELEMENT].InnerText = txtCvListComments.Text.Trim();
            elements[XmlDataConstants.OBJECTTYPEELEMENT].InnerText = cmbCvListObjectType.Text;
            elements[XmlDataConstants.LISTTYPEELEMENT].InnerText = Enum.GetName(typeof(ListType), cmbCvListListType.SelectedValue)!;
            elements[XmlDataConstants.CONTROLELEMENT].InnerText = Enum.GetName(typeof(ListVariableInputStyle), cmbCvListListControl.SelectedValue)!;
            variableElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value = newNameAttributeValue;
            //always update name attribute last because the other elements depend on it

            configureVariablesForm.ValidateXmlDocument();

            treeNode.Name = string.Concat(treeNode.Parent.Name, "/", XmlDataConstants.OBJECTLISTVARIABLEELEMENT, "[@", XmlDataConstants.NAMEATTRIBUTE, "=\"", newNameAttributeValue, "\"]");
            treeNode.Text = newNameAttributeValue;
            _configureVariablesStateImageSetter.SetImage(variableElement, (StateImageRadTreeNode)treeNode, Application);
        }

        public void ValidateXmlDocument()
        {
            configureVariablesForm.ValidateXmlDocument();
        }

        private void AddEventHandlers()
        {
            txtCvListName.Validating += TxtName_Validating;
            txtCvListName.TextChanged += TxtName_TextChanged;
            txtCvListMemberName.Validating += TxtMemberName_Validating;
            txtCvListCastVariableAs.Validating += TxtCastVariableAs_Validating;
            txtCvListTypeName.Validating += TxtTypeName_Validating;
            txtCvListReferenceName.Validating += TxtReferenceName_Validating;
            cmbCvListReferenceDefinition.Validating += CmbReferenceDefinition_Validating;
            txtCvListCastReferenceAs.Validating += TxtCastReferenceAs_Validating;
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            radPanelVariable.AutoScroll = true;
            CmbReferenceDefinition.Validating += CmbReferenceDefinition_Validating;
            TxtCastReferenceAs.Validating += TxtCastReferenceAs_Validating;
            TxtCastVariableAs.Validating += TxtCastVariableAs_Validating;
            TxtMemberName.Validating += TxtMemberName_Validating;
            TxtName.TextChanged += TxtName_TextChanged;
            TxtName.Validating += TxtName_Validating;
            TxtReferenceName.Validating += TxtReferenceName_Validating;
            TxtTypeName.Validating += TxtTypeName_Validating;
            CollapsePanelBorder(radPanelVariable);
            CollapsePanelBorder(radPanelTableParent);
            InitializeVariableControls();
            LoadVariableDropDownLists();
            _cmbCvListObjectTypeTypeAutoCompleteManager.Setup();
        }

        private void InitializeVariableControls()
        {
            helpProvider.SetHelpString(this.txtCvListName, Strings.varConfigNameHelp);
            helpProvider.SetHelpString(this.txtCvListMemberName, Strings.varConfigVariableNameHelp);
            helpProvider.SetHelpString(this.cmbCvListVariableCategory, Strings.varConfigVariableCategoryHelp);
            helpProvider.SetHelpString(this.txtCvListCastVariableAs, Strings.varConfigCastVariableAsHelp);
            helpProvider.SetHelpString(this.txtCvListTypeName, Strings.varConfigTypeNameHelp);
            helpProvider.SetHelpString(this.txtCvListReferenceName, Strings.varConfigReferenceNameHelp);
            helpProvider.SetHelpString(this.cmbCvListReferenceDefinition, Strings.varConfigReferenceDefinitionHelp);
            helpProvider.SetHelpString(this.txtCvListCastReferenceAs, Strings.varConfigCastReferenceAsHelp);
            helpProvider.SetHelpString(this.cmbCvListReferenceCategory, Strings.varConfigReferenceCategoryHelp);
            helpProvider.SetHelpString(this.txtCvListComments, Strings.varConfigCommentsHelp);
            helpProvider.SetHelpString(this.cmbCvListObjectType, Strings.varConfigObjectTypeHelp);
            helpProvider.SetHelpString(this.cmbCvListListType, Strings.varConfigListTypeHelp);
            helpProvider.SetHelpString(this.cmbCvListListControl, Strings.varConfigListControlHelp);
            toolTip.SetToolTip(this.lblCvListName, Strings.varConfigNameHelp);
            toolTip.SetToolTip(this.lblCvListMemberName, Strings.varConfigVariableNameHelp);
            toolTip.SetToolTip(this.lblCvListVariableCategory, Strings.varConfigVariableCategoryHelp);
            toolTip.SetToolTip(this.lblCvListCastVariableAs, Strings.varConfigCastVariableAsHelp);
            toolTip.SetToolTip(this.lblCvListTypeName, Strings.varConfigTypeNameHelp);
            toolTip.SetToolTip(this.lblCvListReferenceName, Strings.varConfigReferenceNameHelp);
            toolTip.SetToolTip(this.lblCvListReferenceDefinition, Strings.varConfigReferenceDefinitionHelp);
            toolTip.SetToolTip(this.lblCvListCastReferenceAs, Strings.varConfigCastReferenceAsHelp);
            toolTip.SetToolTip(this.lblCvListReferenceCategory, Strings.varConfigReferenceCategoryHelp);
            toolTip.SetToolTip(this.lblCvListComments, Strings.varConfigCommentsHelp);
            toolTip.SetToolTip(this.lblCvListObjectType, Strings.varConfigObjectTypeHelp);
            toolTip.SetToolTip(this.lblCvListListType, Strings.varConfigListTypeHelp);
            toolTip.SetToolTip(this.lblCvListListControl, Strings.varConfigListControlHelp);
        }

        private void LoadVariableDropDownLists()
        {
            _radDropDownListHelper.LoadComboItems<VariableCategory>(cmbCvListVariableCategory);
            _radDropDownListHelper.LoadComboItems<ValidIndirectReference>(cmbCvListReferenceDefinition, RadDropDownStyle.DropDown);
            _radDropDownListHelper.LoadComboItems(cmbCvListReferenceCategory, RadDropDownStyle.DropDownList, new ReferenceCategories[] { ReferenceCategories.None });
            _radDropDownListHelper.LoadComboItems<ListType>(cmbCvListListType);
            _radDropDownListHelper.LoadComboItems<ListVariableInputStyle>(cmbCvListListControl);
        }

        private void RemoveEventHandlers()
        {
            txtCvListName.Validating -= TxtName_Validating;
            txtCvListName.TextChanged -= TxtName_TextChanged;
            txtCvListMemberName.Validating -= TxtMemberName_Validating;
            txtCvListCastVariableAs.Validating -= TxtCastVariableAs_Validating;
            txtCvListTypeName.Validating -= TxtTypeName_Validating;
            txtCvListReferenceName.Validating -= TxtReferenceName_Validating;
            cmbCvListReferenceDefinition.Validating -= CmbReferenceDefinition_Validating;
            txtCvListCastReferenceAs.Validating -= TxtCastReferenceAs_Validating;
        }

        #region Event Handlers
        private void CmbReferenceDefinition_Validating(object? sender, CancelEventArgs e)
        {
            try
            {
                _objectListVariableControlsValidator.CmbReferenceDefinitionValidating();
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
                _objectListVariableControlsValidator.TxtCastReferenceAsValidating();
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
                _objectListVariableControlsValidator.TxtCastVariableAsValidating();
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
                _objectListVariableControlsValidator.TxtMemberNameValidating();
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
                _objectListVariableControlsValidator.TxtNameChanged();
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
                _objectListVariableControlsValidator.TxtNameValidating();
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
                _objectListVariableControlsValidator.TxtReferenceNameValidating();
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
                _objectListVariableControlsValidator.TxtTypeNameValidating();
            }
            catch (LogicBuilderException ex)
            {
                SetErrorMessage(ex.Message);
            };
        }
        #endregion Event Handlers
    }
}
