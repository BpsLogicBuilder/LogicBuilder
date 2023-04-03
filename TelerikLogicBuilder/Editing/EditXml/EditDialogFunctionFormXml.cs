﻿using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
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
    internal partial class EditDialogFunctionFormXml : Telerik.WinControls.UI.RadForm, IEditDialogFunctionFormXml
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IConfigurationService _configurationService;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IFormInitializer _formInitializer;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IFunctionHelper _functionHelper;
        private readonly IFunctionsDataParser _functionsDataParser;
        private readonly IFunctionElementValidator _functionElementValidator;
        private readonly IRefreshVisibleTextHelper _refreshVisibleTextHelper;
        private readonly EditXmlRichTextBoxPanel _richTextBoxPanel;
        private readonly IValidateXmlTextHelper _validateXmlTextHelper;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IXmlValidator _xmlValidator;

        private ApplicationTypeInfo _application;

        public EditDialogFunctionFormXml(
            IConfigurationService configurationService,
            IDialogFormMessageControl dialogFormMessageControl,
            IEditXmlHelperFactory editXmlHelperFactory,
            IFormInitializer formInitializer,
            IFunctionDataParser functionDataParser,
            IFunctionHelper functionHelper,
            IFunctionsDataParser functionsDataParser,
            IFunctionElementValidator functionElementValidator,
            IRefreshVisibleTextHelper refreshVisibleTextHelper,
            EditXmlRichTextBoxPanel richTextBoxPanel,
            IServiceFactory serviceFactory,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlValidatorFactory xmlValidatorFactory,
            string xml)
        {
            InitializeComponent();
            _dialogFormMessageControl = dialogFormMessageControl;//_applicationDropDownList may try to set messages so do this first
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _configurationService = configurationService;
            _formInitializer = formInitializer;
            _functionDataParser = functionDataParser;
            _functionElementValidator = functionElementValidator;
            _functionHelper = functionHelper;
            _functionsDataParser = functionsDataParser;
            _refreshVisibleTextHelper = refreshVisibleTextHelper;
            _richTextBoxPanel = richTextBoxPanel;
            _validateXmlTextHelper = editXmlHelperFactory.GetValidateXmlTextHelper(this);
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _xmlValidator = xmlValidatorFactory.GetXmlValidator(Schema);
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

        public SchemaName Schema => SchemaName.FunctionsDataSchema;

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
            FunctionsData functionsData = _functionsDataParser.Parse
            (
                _xmlDocumentHelpers.ToXmlElement
                (
                    _xmlDataHelper.BuildFunctionsXml(FormattedXmlElement.OuterXml)
                )
            );

            if (functionsData.FunctionElements.Count != 1)
                throw new LogicBuilderException(Strings.dialogShapesOneDialogFunction);
            if (functionsData.FunctionElements[0].Name != XmlDataConstants.FUNCTIONELEMENT)//<not /> element is not applicable for a dialog function.
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.invalidXmlRootElementFormat, functionsData.FunctionElements[0].Name, XmlDataConstants.FUNCTIONELEMENT));

            FunctionData functionData = _functionDataParser.Parse(functionsData.FunctionElements[0]);
            if (!_configurationService.FunctionList.Functions.TryGetValue(functionData.Name, out Function? function))
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.functionNotConfiguredFormat, functionData.Name));
            if (!_functionHelper.IsDialog(function))
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.functionIncorrectlyConfiguredFormat, functionData.Name));

            List<string> errors = new();
            _functionElementValidator.Validate
            (
                UnFormattedXmlElement,
                typeof(object),//Dialog function should be void but does not matter.
                Application,
                errors
            );
            if (errors.Count > 0)
                throw new LogicBuilderException(string.Join(Environment.NewLine, errors));
        }

        public void ValidateSchema()
        {
            var response = _xmlValidator.Validate
            (
                _xmlDataHelper.BuildFunctionsXml(FormattedXmlElement.OuterXml)
            );
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
            this.radGroupBoxXml.Controls.Add(_richTextBoxPanel);

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