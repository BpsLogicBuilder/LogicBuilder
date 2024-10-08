﻿using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DialogFormMessageControlHelpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditXml
{
    internal partial class EditObjectListFormXml : Telerik.WinControls.UI.RadForm, IEditObjectListFormXml
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IFormInitializer _formInitializer;
        private readonly IObjectListElementValidator _objectListElementValidator;
        private readonly IRefreshVisibleTextHelper _refreshVisibleTextHelper;
        private readonly IEditXmlRichTextBoxPanel _richTextBoxPanel;
        private readonly IValidateXmlTextHelper _validateXmlTextHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IXmlValidator _xmlValidator;
        private readonly Type assignedTo;

        private ApplicationTypeInfo _application;

        public EditObjectListFormXml(
            IDialogFormMessageControlFactory dialogFormMessageControlFactory,
            IEditXmlHelperFactory editXmlHelperFactory,
            IFormInitializer formInitializer,
            IObjectListElementValidator objectListElementValidator,
            IRefreshVisibleTextHelper refreshVisibleTextHelper,
            IServiceFactory serviceFactory,
            IUserControlFactory userControlFactory,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlValidatorFactory xmlValidatorFactory,
            string xml,
            Type assignedTo)
        {
            InitializeComponent();
            _dialogFormMessageControl = dialogFormMessageControlFactory.GetDialogFormMessageControl();//_applicationDropDownList may try to set messages so do this first
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _formInitializer = formInitializer;
            _objectListElementValidator = objectListElementValidator;
            _refreshVisibleTextHelper = refreshVisibleTextHelper;
            _richTextBoxPanel = userControlFactory.GetEditXmlRichTextBoxPanel();
            _validateXmlTextHelper = editXmlHelperFactory.GetValidateXmlTextHelper(this);
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _xmlValidator = xmlValidatorFactory.GetXmlValidator(Schema);
            this.assignedTo = assignedTo;
            RichTextBox.Lines = new string[] { xml };
            Initialize();
        }

        public RadButton BtnOk => btnOk;

        public XmlElement FormattedXmlElement => _xmlDocumentHelpers.ToXmlElement
        (
            string.Join(Environment.NewLine, RichTextBox.Lines),
            true
        );

        public RichTextBox RichTextBox => _richTextBoxPanel.RichTextBox;

        public SchemaName Schema => SchemaName.ParametersDataSchema;

        public XmlElement UnFormattedXmlElement => _xmlDocumentHelpers.ToXmlElement
        (
            string.Join(string.Empty, RichTextBox.Lines),
            false
        );

        public string XmlResult
            => _refreshVisibleTextHelper.RefreshAllVisibleTexts(UnFormattedXmlElement, Application).OuterXml;

        public ApplicationTypeInfo Application => _application;

        public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        public void ClearMessage() => _dialogFormMessageControl.ClearMessage();

        public void SetErrorMessage(string message)
           => _dialogFormMessageControl.SetErrorMessage(message);

        public void SetMessage(string message, string title = "")
            => _dialogFormMessageControl.SetMessage(message, title);

        public void ValidateElement()
        {
            if (this.UnFormattedXmlElement.Name != XmlDataConstants.OBJECTLISTELEMENT)
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.invalidXmlRootElementFormat, this.UnFormattedXmlElement.Name, XmlDataConstants.OBJECTLISTELEMENT));

            List<string> errors = new();
            _objectListElementValidator.Validate
            (
                UnFormattedXmlElement,
                this.assignedTo,
                Application,
                errors
            );
            if (errors.Count > 0)
                throw new LogicBuilderException(string.Join(Environment.NewLine, errors));
        }

        public void ValidateSchema()
        {
            var response = _xmlValidator.Validate(FormattedXmlElement.OuterXml);
            if (response.Success == false)
                throw new LogicBuilderException(string.Join(Environment.NewLine, response.Errors));
        }

        private void Initialize()
        {
            InitializeApplicationDropDownList();
            InitializeDialogFormMessageControl();
            InitializeRichTextBoxPanelControl();

            _applicationDropDownList.ApplicationChanged += ApplicationDropDownList_ApplicationChanged;

            _formInitializer.SetFormDefaults(this, 719);
            _formInitializer.SetToEditSize(this);
            btnCancel.CausesValidation = false;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            ControlsLayoutUtility.CollapsePanelBorder(radPanelApplication);
            ControlsLayoutUtility.CollapsePanelBorder(radPanelBottom);
            ControlsLayoutUtility.CollapsePanelBorder(radPanelButtons);
            ControlsLayoutUtility.CollapsePanelBorder(radPanelFill);
            ControlsLayoutUtility.CollapsePanelBorder(radPanelMessages);

            _validateXmlTextHelper.Setup();
            _validateXmlTextHelper.ValidateXml();
        }

        private void InitializeApplicationDropDownList()
        {
            ControlsLayoutUtility.LayoutApplicationGroupBox(this, radPanelApplication, radGroupBoxApplication, _applicationDropDownList);
        }

        private void InitializeDialogFormMessageControl()
        {
            ControlsLayoutUtility.LayoutBottomPanel(radPanelBottom, radPanelMessages, radPanelButtons, tableLayoutPanelButtons, _dialogFormMessageControl);
        }

        private void InitializeRichTextBoxPanelControl()
        {
            ((ISupportInitialize)this.radGroupBoxXml).BeginInit();
            this.radGroupBoxXml.SuspendLayout();

            radGroupBoxXml.Padding = PerFontSizeConstants.GroupBoxPadding;

            _richTextBoxPanel.Dock = DockStyle.Fill;
            _richTextBoxPanel.Location = new Point(0, 0);
            this.radGroupBoxXml.Controls.Add((Control)_richTextBoxPanel);

            ((ISupportInitialize)this.radGroupBoxXml).EndInit();
            this.radGroupBoxXml.ResumeLayout(true);
        }

        #region Event Handlers
        private void ApplicationDropDownList_ApplicationChanged(object? sender, ApplicationChangedEventArgs e)
        {
            _application = e.Application;
            ApplicationChanged?.Invoke(this, e);
        }
        #endregion Event Handlers
    }
}
