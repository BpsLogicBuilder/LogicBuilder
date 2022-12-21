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

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureObjectVariable
{
    internal partial class ConfigureObjectVariableControl : UserControl, IConfigureObjectVariableControl
    {
        private readonly IConfigureVariablesStateImageSetter _configureVariablesStateImageSetter;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IObjectVariableControlsValidator _objectVariableControlsValidator;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly IStringHelper _stringHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly ITypeAutoCompleteManager _cmbCvObjectTypeTypeAutoCompleteManager;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureVariablesForm configureVariablesForm;

        public ConfigureObjectVariableControl(
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
            _cmbCvObjectTypeTypeAutoCompleteManager = serviceFactory.GetTypeAutoCompleteManager
            (
                configureVariablesForm,
                cmbCvObjectType
            );
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _objectVariableControlsValidator = variableControlValidatorFactory.GetObjectVariableControlsValidator(this);
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

        public RadLabel LblName => lblCvName;
        public RadTextBox TxtName => txtCvName;
        public RadTextBox TxtMemberName => txtCvMemberName;
        public RadDropDownList CmbVariableCategory => cmbCvVariableCategory;
        public RadLabel LblCastVariableAs => lblCvCastVariableAs;
        public RadTextBox TxtCastVariableAs => txtCvCastVariableAs;
        public RadLabel LblTypeName => lblCvTypeName;
        public RadTextBox TxtTypeName => txtCvTypeName;
        public RadTextBox TxtReferenceName => txtCvReferenceName;
        public RadDropDownList CmbReferenceDefinition => cmbCvReferenceDefinition;
        public RadTextBox TxtCastReferenceAs => txtCvCastReferenceAs;
        public RadDropDownList CmbReferenceCategory => cmbCvReferenceCategory;
        public RadLabel LblObjectType => lblCvObjectType;
        public AutoCompleteRadDropDownList CmbObjectType => cmbCvObjectType;
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
            if (!_treeViewService.IsObjectTypeNode(treeNode))
                throw _exceptionHelper.CriticalException("{AE1252F6-969D-41AB-B97C-F5C3D2996C03}");

            RemoveEventHandlers();
            XmlElement variableElement = _xmlDocumentHelpers.SelectSingleElement(XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(variableElement).ToDictionary(e => e.Name);

            txtCvName.Text = variableElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE);
            txtCvName.Select();
            txtCvName.SelectAll();
            txtCvMemberName.Text = elements[XmlDataConstants.MEMBERNAMEELEMENT].InnerText;
            cmbCvVariableCategory.SelectedValue = _enumHelper.ParseEnumText<VariableCategory>(elements[XmlDataConstants.VARIABLECATEGORYELEMENT].InnerText);
            txtCvCastVariableAs.Text = elements[XmlDataConstants.CASTVARIABLEASELEMENT].InnerText;
            txtCvTypeName.Text = elements[XmlDataConstants.TYPENAMEELEMENT].InnerText;
            txtCvReferenceName.Text = elements[XmlDataConstants.REFERENCENAMEELEMENT].InnerText;
            cmbCvReferenceDefinition.Text = string.Join
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
            txtCvCastReferenceAs.Text = elements[XmlDataConstants.CASTREFERENCEASELEMENT].InnerText;
            cmbCvReferenceCategory.SelectedValue = _enumHelper.ParseEnumText<ReferenceCategories>(elements[XmlDataConstants.REFERENCECATEGORYELEMENT].InnerText);
            txtCvComments.Text = elements[XmlDataConstants.COMMENTSELEMENT].InnerText;
            cmbCvObjectType.Text = elements[XmlDataConstants.OBJECTTYPEELEMENT].InnerText;
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

            if (!_treeViewService.IsObjectTypeNode(treeNode))
                throw _exceptionHelper.CriticalException("{1E8F5698-5C95-46C2-BC8F-7B6B6252AB32}");

            _objectVariableControlsValidator.ValidateInputBoxes();

            XmlElement variableElement = _xmlDocumentHelpers.SelectSingleElement(XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(variableElement).ToDictionary(e => e.Name);
            string currentNameAttributeValue = variableElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE);
            string newNameAttributeValue = txtCvName.Text.Trim();

            _objectVariableControlsValidator.ValidateForExistingVariableName(currentNameAttributeValue);

            elements[XmlDataConstants.MEMBERNAMEELEMENT].InnerText = txtCvMemberName.Text.Trim();
            elements[XmlDataConstants.VARIABLECATEGORYELEMENT].InnerText = Enum.GetName(typeof(VariableCategory), cmbCvVariableCategory.SelectedValue)!;
            elements[XmlDataConstants.CASTVARIABLEASELEMENT].InnerText = txtCvCastVariableAs.Text.Trim();
            elements[XmlDataConstants.TYPENAMEELEMENT].InnerText = txtCvTypeName.Text.Trim();
            elements[XmlDataConstants.REFERENCENAMEELEMENT].InnerText = txtCvReferenceName.Text.Trim();
            elements[XmlDataConstants.REFERENCEDEFINITIONELEMENT].InnerText = _enumHelper.BuildValidReferenceDefinition(cmbCvReferenceDefinition.Text);
            elements[XmlDataConstants.CASTREFERENCEASELEMENT].InnerText = txtCvCastReferenceAs.Text.Trim();
            elements[XmlDataConstants.REFERENCECATEGORYELEMENT].InnerText = Enum.GetName(typeof(ReferenceCategories), cmbCvReferenceCategory.SelectedValue)!;
            elements[XmlDataConstants.COMMENTSELEMENT].InnerText = txtCvComments.Text.Trim();
            elements[XmlDataConstants.OBJECTTYPEELEMENT].InnerText = cmbCvObjectType.Text;
            variableElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value = newNameAttributeValue;
            //always update name attribute last because the other elements depend on it

            configureVariablesForm.ValidateXmlDocument();

            treeNode.Name = $"{treeNode.Parent.Name}/{XmlDataConstants.OBJECTVARIABLEELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{newNameAttributeValue}\"]";
            treeNode.Text = newNameAttributeValue;
            _configureVariablesStateImageSetter.SetImage(variableElement, (StateImageRadTreeNode)treeNode, Application);
        }

        public void ValidateXmlDocument()
        {
            configureVariablesForm.ValidateXmlDocument();
        }

        private void AddEventHandlers()
        {
            txtCvName.Validating += TxtName_Validating;
            txtCvName.TextChanged += TxtName_TextChanged;
            txtCvMemberName.Validating += TxtMemberName_Validating;
            txtCvCastVariableAs.Validating += TxtCastVariableAs_Validating;
            txtCvTypeName.Validating += TxtTypeName_Validating;
            txtCvReferenceName.Validating += TxtReferenceName_Validating;
            cmbCvReferenceDefinition.Validating += CmbReferenceDefinition_Validating;
            txtCvCastReferenceAs.Validating += TxtCastReferenceAs_Validating;
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
            _cmbCvObjectTypeTypeAutoCompleteManager.Setup();
        }

        private void InitializeVariableControls()
        {
            helpProvider.SetHelpString(this.txtCvName, Strings.varConfigNameHelp);
            helpProvider.SetHelpString(this.txtCvMemberName, Strings.varConfigVariableNameHelp);
            helpProvider.SetHelpString(this.cmbCvVariableCategory, Strings.varConfigVariableCategoryHelp);
            helpProvider.SetHelpString(this.txtCvCastVariableAs, Strings.varConfigCastVariableAsHelp);
            helpProvider.SetHelpString(this.txtCvTypeName, Strings.varConfigTypeNameHelp);
            helpProvider.SetHelpString(this.txtCvReferenceName, Strings.varConfigReferenceNameHelp);
            helpProvider.SetHelpString(this.cmbCvReferenceDefinition, Strings.varConfigReferenceDefinitionHelp);
            helpProvider.SetHelpString(this.txtCvCastReferenceAs, Strings.varConfigCastReferenceAsHelp);
            helpProvider.SetHelpString(this.cmbCvReferenceCategory, Strings.varConfigReferenceCategoryHelp);
            helpProvider.SetHelpString(this.txtCvComments, Strings.varConfigCommentsHelp);
            helpProvider.SetHelpString(this.cmbCvObjectType, Strings.varConfigObjectTypeHelp);
            toolTip.SetToolTip(this.lblCvName, Strings.varConfigNameHelp);
            toolTip.SetToolTip(this.lblCvMemberName, Strings.varConfigVariableNameHelp);
            toolTip.SetToolTip(this.lblCvVariableCategory, Strings.varConfigVariableCategoryHelp);
            toolTip.SetToolTip(this.lblCvCastVariableAs, Strings.varConfigCastVariableAsHelp);
            toolTip.SetToolTip(this.lblCvTypeName, Strings.varConfigTypeNameHelp);
            toolTip.SetToolTip(this.lblCvReferenceName, Strings.varConfigReferenceNameHelp);
            toolTip.SetToolTip(this.lblCvReferenceDefinition, Strings.varConfigReferenceDefinitionHelp);
            toolTip.SetToolTip(this.lblCvCastReferenceAs, Strings.varConfigCastReferenceAsHelp);
            toolTip.SetToolTip(this.lblCvReferenceCategory, Strings.varConfigReferenceCategoryHelp);
            toolTip.SetToolTip(this.lblCvComments, Strings.varConfigCommentsHelp);
            toolTip.SetToolTip(this.lblCvObjectType, Strings.varConfigObjectTypeHelp);
        }

        private void LoadVariableDropDownLists()
        {
            _radDropDownListHelper.LoadComboItems<VariableCategory>(cmbCvVariableCategory);
            _radDropDownListHelper.LoadComboItems<ValidIndirectReference>(cmbCvReferenceDefinition, RadDropDownStyle.DropDown);
            _radDropDownListHelper.LoadComboItems(cmbCvReferenceCategory, RadDropDownStyle.DropDownList, new ReferenceCategories[] { ReferenceCategories.None });
        }

        private void RemoveEventHandlers()
        {
            txtCvName.Validating -= TxtName_Validating;
            txtCvName.TextChanged -= TxtName_TextChanged;
            txtCvMemberName.Validating -= TxtMemberName_Validating;
            txtCvCastVariableAs.Validating -= TxtCastVariableAs_Validating;
            txtCvTypeName.Validating -= TxtTypeName_Validating;
            txtCvReferenceName.Validating -= TxtReferenceName_Validating;
            cmbCvReferenceDefinition.Validating -= CmbReferenceDefinition_Validating;
            txtCvCastReferenceAs.Validating -= TxtCastReferenceAs_Validating;
        }

        #region Event Handlers
        private void CmbReferenceDefinition_Validating(object? sender, CancelEventArgs e)
        {
            try
            {
                _objectVariableControlsValidator.CmbReferenceDefinitionValidating();
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
                _objectVariableControlsValidator.TxtCastReferenceAsValidating();
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
                _objectVariableControlsValidator.TxtCastVariableAsValidating();
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
                _objectVariableControlsValidator.TxtMemberNameValidating();
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
                _objectVariableControlsValidator.TxtNameChanged();
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
                _objectVariableControlsValidator.TxtNameValidating();
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
                _objectVariableControlsValidator.TxtReferenceNameValidating();
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
                _objectVariableControlsValidator.TxtTypeNameValidating();
            }
            catch (LogicBuilderException ex)
            {
                SetErrorMessage(ex.Message);
            };
        }
        #endregion Event Handlers
    }
}
