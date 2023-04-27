using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConnector.EditDialogConnector.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConnector.EditDialogConnector
{
    internal partial class EditDialogConnectorControl : UserControl, IEditDialogConnectorControl
    {
        private readonly IConfigurationService _configurationService;
        private readonly IConnectorDataParser _connectorDataParser;
        private readonly IConnectorObjectTypeAutoCompleteManager _connectorObjectTypeAutoCompleteManager;
        private readonly IConstructorTypeHelper _constructorTypeHelper;
        private readonly IEditDialogConnectorFieldControlFactory _editDialogConnectorFieldControlFactory;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMetaObjectDataParser _metaObjectDataParser;
        private readonly IMetaObjectElementValidator _metaMetaObjectElementValidator;
        private readonly IRefreshVisibleTextHelper _refreshVisibleTextHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IEditDialogConnectorForm editDialogConnectorForm;

        private IConnectorObjectRichTextBoxControl connectorObjectRichTextBoxControl;
        private IConnectorTextRichInputBoxControl connectorTextRichInputBoxControl;

        public EditDialogConnectorControl(
            IConfigurationService configurationService,
            IConnectorDataParser connectorDataParser,
            IConstructorTypeHelper constructorTypeHelper,
            IEditDialogConnectorFieldControlFactory editDialogConnectorFieldControlFactory,
            IExceptionHelper exceptionHelper,
            IMetaObjectDataParser metaObjectDataParser,
            IMetaObjectElementValidator metaObjectElementValidator,
            IRefreshVisibleTextHelper refreshVisibleTextHelper,
            IServiceFactory serviceFactory,
            ITypeLoadHelper typeLoadHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditDialogConnectorForm editDialogConnectorForm,
            short connectorIndexToSelect,
            XmlDocument? connectorXmlDocument)
        {
            InitializeComponent();
            _configurationService = configurationService;
            _connectorDataParser = connectorDataParser;
            _constructorTypeHelper = constructorTypeHelper;
            _editDialogConnectorFieldControlFactory = editDialogConnectorFieldControlFactory;
            _exceptionHelper = exceptionHelper;
            _metaObjectDataParser = metaObjectDataParser;
            _metaMetaObjectElementValidator = metaObjectElementValidator;
            _refreshVisibleTextHelper = refreshVisibleTextHelper;
            _typeLoadHelper = typeLoadHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _connectorObjectTypeAutoCompleteManager = serviceFactory.GetConnectorObjectTypeAutoCompleteManager(editDialogConnectorForm, cmbConnectorObjectType);

            this.editDialogConnectorForm = editDialogConnectorForm;
            Initialize();
            UpdateSelection(connectorIndexToSelect, connectorXmlDocument);
        }

        private RadButton RadButtonOk => editDialogConnectorForm.RadButtonOk;

        public ApplicationTypeInfo Application => editDialogConnectorForm.Application;

        public bool IsValid
        {
            get
            {
                List<string> errors = new();
                errors.AddRange(ValidateControls(false));
                if (errors.Count > 0)
                    return false;

                return true;
            }
        }

        public XmlElement XmlResult
        {
            get
            {
                StringBuilder stringBuilder = new();
                using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
                {
                    xmlTextWriter.WriteStartElement(XmlDataConstants.CONNECTORELEMENT);
                        xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, ((short)cmbIndex.SelectedValue).ToString(CultureInfo.InvariantCulture));
                        xmlTextWriter.WriteAttributeString(XmlDataConstants.CONNECTORCATEGORYATTRIBUTE, ((short)ConnectorCategory.Dialog).ToString(CultureInfo.InvariantCulture));
                        xmlTextWriter.WriteRaw(connectorTextRichInputBoxControl.XmlElement!.OuterXml);
                        xmlTextWriter.WriteRaw(connectorObjectRichTextBoxControl.XmlElement!.OuterXml);
                    xmlTextWriter.WriteEndElement();
                    xmlTextWriter.Flush();
                }

                return _refreshVisibleTextHelper.RefreshAllVisibleTexts
                (
                    _xmlDocumentHelpers.ToXmlElement(stringBuilder.ToString()),
                    Application
                );
            }
        }

        public string VisibleText => string.Format(CultureInfo.CurrentCulture, Strings.dialogConnectorFormat, cmbIndex.SelectedValue.ToString(), connectorTextRichInputBoxControl.VisibleText);

        public void ClearMessage() => editDialogConnectorForm.ClearMessage();

        public void RequestDocumentUpdate() {}

        public void SetErrorMessage(string message) => editDialogConnectorForm.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => editDialogConnectorForm.SetMessage(message, title);

        public void ValidateFields()
        {
            List<string> errors = new();
            errors.AddRange(ValidateControls());
            if (errors.Count > 0)
                throw new LogicBuilderException(string.Join(Environment.NewLine, errors));
        }

        private void SetupConnectorObjectControl()
        {
            ClearMessage();
            connectorTextRichInputBoxControl.SetNormalBackColor();
            connectorObjectRichTextBoxControl.SetNormalBackColor();
            cmbConnectorObjectType.SetNormalBackColor();

            if (!_typeLoadHelper.TryGetSystemType(cmbConnectorObjectType.Text,Application,out Type? type))
            {
                SetErrorMessage
                (
                    string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeFormat2, cmbConnectorObjectType.Text)
                );
                cmbConnectorObjectType.SetErrorBackColor();
                connectorObjectRichTextBoxControl.DisableControls();
                return;
            }

            if (type.ContainsGenericParameters)
            {
                SetErrorMessage
                (
                    string.Format(CultureInfo.CurrentCulture, Strings.cannotContainGenericParametersFormat, type.ToString())
                );
                cmbConnectorObjectType.SetErrorBackColor();
                connectorObjectRichTextBoxControl.DisableControls();
                return;
            }

            ClosedConstructor? constructor = _constructorTypeHelper.GetConstructor(type, Application);
            if (constructor == null)
            {
                SetErrorMessage
                (
                    string.Format(CultureInfo.CurrentCulture,  Strings.constructorNotConfiguredForObjectTypeFormat, type.ToString())
                );
                cmbConnectorObjectType.SetErrorBackColor();
                connectorObjectRichTextBoxControl.DisableControls();
                return;
            }

            connectorObjectRichTextBoxControl.EnableControls();
            connectorObjectRichTextBoxControl.SetupDefaultElement(type);
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private static void CollapsePanelBorder(RadScrollablePanel radPanel)
            => radPanel.PanelElement.Border.Visibility = ElementVisibility.Collapsed;

        [MemberNotNull(nameof(connectorObjectRichTextBoxControl), nameof(connectorTextRichInputBoxControl))]
        private void Initialize()
        {
            InitializeObjectRichTextBoxControl();
            InitializeTextRichInputBoxControl();
            InitializeTableLayoutPanel();

            CollapsePanelBorder(radPanelConnector);
            CollapsePanelBorder(radPanelTableParent);

            LoadIndexComboBox();

            connectorObjectRichTextBoxControl.Changed += ConnectorObjectRichTextBoxControl_Changed;
            connectorTextRichInputBoxControl.Changed += ConnectorTextRichInputBoxControl_Changed;
            cmbConnectorObjectType.TextChanged += CmbConnectorObjectType_TextChanged;
            _connectorObjectTypeAutoCompleteManager.Setup();

            SetCmbConnectorObjectType(_configurationService.ProjectProperties.ConnectorObjectTypes.First());
            SetupConnectorObjectControl();
            ValidateOk(false);
        }

        [MemberNotNull(nameof(connectorObjectRichTextBoxControl))]
        private void InitializeObjectRichTextBoxControl()
        {
            ((ISupportInitialize)radPanelConnectorObject).BeginInit();
            radPanelConnectorObject.SuspendLayout();

            connectorObjectRichTextBoxControl = _editDialogConnectorFieldControlFactory.GetConnectorObjectRichTextBoxControl(this);
            connectorObjectRichTextBoxControl.Dock = DockStyle.Fill;
            connectorObjectRichTextBoxControl.Location = new Point(0, 0);
            radPanelConnectorObject.Controls.Add((Control)connectorObjectRichTextBoxControl);

            ((ISupportInitialize)radPanelConnectorObject).EndInit();
            radPanelConnectorObject.ResumeLayout(true);
        }

        private void InitializeTableLayoutPanel()
        {
            ControlsLayoutUtility.LayoutControls
            (
                groupBoxConnector,
                radPanelConnector,
                radPanelTableParent,
                tableLayoutPanel,
                4
            );
        }

        [MemberNotNull(nameof(connectorTextRichInputBoxControl))]
        private void InitializeTextRichInputBoxControl()
        {
            ((ISupportInitialize)radPanelText).BeginInit();
            radPanelText.SuspendLayout();

            connectorTextRichInputBoxControl = _editDialogConnectorFieldControlFactory.GetConnectorTextRichInputBoxControl(this);
            connectorTextRichInputBoxControl.Dock = DockStyle.Fill;
            connectorTextRichInputBoxControl.Location = new Point(0, 0);
            radPanelText.Controls.Add((Control)connectorTextRichInputBoxControl);

            ((ISupportInitialize)radPanelText).EndInit();
            radPanelText.ResumeLayout(true);
        }

        private void LoadIndexComboBox()
        {
            cmbIndex.DropDownStyle = RadDropDownStyle.DropDownList;
            cmbIndex.Items.Clear();
            for (short i = 1; i <= EditingConstants.MAXIMUMCHOICES; i++)
                cmbIndex.Items.Add(new RadListDataItem(i.ToString(CultureInfo.CurrentCulture), i));
        }

        private void UpdateSelection(short connectorIndexToSelect, XmlDocument? connectorXmlDocument)
        {
            UpdateFields();
            ValidateOk(false);

            void UpdateFields()
            {
                cmbIndex.SelectedValue = connectorIndexToSelect;

                if (connectorXmlDocument == null)
                    return;

                ConnectorData connectorData = _connectorDataParser.Parse
                (
                    _xmlDocumentHelpers.GetDocumentElement(connectorXmlDocument)
                );
                if (connectorData.ConnectorCategory != ConnectorCategory.Dialog)
                    return;

                cmbIndex.SelectedValue = connectorData.Index;
                connectorTextRichInputBoxControl.Update(connectorData.TextXmlNode);

                if (connectorData.MetaObjectDataXmlNode == null)
                    throw _exceptionHelper.CriticalException("{9269C9BD-757F-4874-B1F6-6CBABFAE6A8D}");

                MetaObjectData metaObjectData = _metaObjectDataParser.Parse(connectorData.MetaObjectDataXmlNode);
                if (!_configurationService.ProjectProperties.ConnectorObjectTypes.ToHashSet().Contains(metaObjectData.ObjectType))
                    return;

                SetCmbConnectorObjectType(metaObjectData.ObjectType);
                connectorObjectRichTextBoxControl.Update(connectorData.MetaObjectDataXmlNode);
            }
        }

        private void SetCmbConnectorObjectType(string connectorObjectType)
        {
            cmbConnectorObjectType.TextChanged -= CmbConnectorObjectType_TextChanged;
            cmbConnectorObjectType.Text = connectorObjectType;
            cmbConnectorObjectType.TextChanged += CmbConnectorObjectType_TextChanged;
        }

        private IList<string> ValidateControls(bool showErrors = true)
        {
            connectorTextRichInputBoxControl.SetNormalBackColor();
            connectorObjectRichTextBoxControl.SetNormalBackColor();
            cmbConnectorObjectType.SetNormalBackColor();

            List<string> errors = new();
            if(connectorTextRichInputBoxControl.MixedXml.Trim().Length == 0)
            {
                if (showErrors)
                    connectorTextRichInputBoxControl.SetErrorBackColor();
                errors.Add(Strings.dialogConnectorTextIsEmpty);
                return errors;
            }

            if (!_typeLoadHelper.TryGetSystemType(cmbConnectorObjectType.Text, Application, out Type? type))
            {
                errors.Add
                (
                    string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeFormat2, cmbConnectorObjectType.Text)
                );

                if (showErrors)
                    cmbConnectorObjectType.SetErrorBackColor();
                connectorObjectRichTextBoxControl.DisableControls();
                return errors;
            }

            if (type.ContainsGenericParameters)
            {
                errors.Add
                (
                    string.Format(CultureInfo.CurrentCulture, Strings.cannotContainGenericParametersFormat, type.ToString())
                );
                if (showErrors)
                    cmbConnectorObjectType.SetErrorBackColor();
                connectorObjectRichTextBoxControl.DisableControls();
                return errors;
            }

            if (connectorObjectRichTextBoxControl.IsEmpty)
            {
                if (showErrors)
                    connectorObjectRichTextBoxControl.SetErrorBackColor();
                errors.Add(Strings.dialogConnectorObjectIsEmpty);
                return errors;
            }

            _metaMetaObjectElementValidator.Validate
            (
                connectorObjectRichTextBoxControl.XmlElement!, 
                Application, 
                errors
            );

            if (showErrors && errors.Count > 0)
            {
                connectorObjectRichTextBoxControl.SetErrorBackColor();
            }

            return errors;
        }

        private void ValidateOk(bool showErrors = true)
        {
            ClearMessage();

            List<string> errors = new();
            errors.AddRange(ValidateControls(showErrors));
            if (showErrors && errors.Count > 0)
            {
                SetErrorMessage(string.Join(Environment.NewLine, errors));
            }

            RadButtonOk.Enabled = errors.Count == 0;
        }

        #region Event Handlers
        private void CmbConnectorObjectType_TextChanged(object? sender, EventArgs e)
        {
            SetupConnectorObjectControl();
            ValidateOk();
        }

        private void ConnectorObjectRichTextBoxControl_Changed(object? sender, EventArgs e)
        {
            ValidateOk();
        }

        private void ConnectorTextRichInputBoxControl_Changed(object? sender, EventArgs e)
        {
            ValidateOk();
        }
        #endregion Event Handlers
    }
}
