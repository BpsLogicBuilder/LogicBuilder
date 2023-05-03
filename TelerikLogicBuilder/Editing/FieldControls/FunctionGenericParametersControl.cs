using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls
{
    internal partial class FunctionGenericParametersControl : UserControl, IFunctionGenericParametersControl
    {
        private readonly RadButton btnHelper;

        private readonly IConfigurationService _configurationService;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IFunctionGenericsConfigrationValidator _functionGenericsConfigrationValidator;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFieldControlCommandFactory _fieldControlCommandFactory;
        private readonly IImageListService _imageListService;
        private readonly ILayoutFieldControlButtons _layoutFieldControlButtons;
        private readonly ObjectRichTextBox _objectRichTextBox;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IEditFunctionControl editFunctionControl;

        public FunctionGenericParametersControl(
            IConfigurationService configurationService,
            IFunctionDataParser constructorDataParser,
            IFunctionGenericsConfigrationValidator constructorGenericsConfigrationValidator,
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IFieldControlCommandFactory fieldControlCommandFactory,
            IImageListService imageListService,
            ILayoutFieldControlButtons layoutFieldControlButtons,
            ObjectRichTextBox objectRichTextBox,
            ITypeLoadHelper typeLoadHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditFunctionControl editFunctionControl)
        {
            InitializeComponent();
            _configurationService = configurationService;
            _functionDataParser = constructorDataParser;
            _functionGenericsConfigrationValidator = constructorGenericsConfigrationValidator;
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _fieldControlCommandFactory = fieldControlCommandFactory;
            _imageListService = imageListService;
            _layoutFieldControlButtons = layoutFieldControlButtons;
            _objectRichTextBox = objectRichTextBox;
            _typeLoadHelper = typeLoadHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;

            this.editFunctionControl = editFunctionControl;

            btnHelper = new()
            {
                Name = "btnHelper",
                ImageList = _imageListService.ImageList,
                ImageAlignment = ContentAlignment.MiddleCenter,
                Padding = new Padding(0),
                Margin = new Padding(1, 0, 1, 0),
                ImageIndex = ImageIndexes.MOREIMAGEINDEX,
                Dock = DockStyle.Fill
            };

            Initialize();

        }

        public ApplicationTypeInfo Application => editFunctionControl.Application;

        public Function Function => editFunctionControl.Function;

        public XmlDocument XmlDocument => editFunctionControl.XmlDocument;

        public void ResetControls() => editFunctionControl.ResetControls();

        public void UpdateValidState()
        {
            Function function = _configurationService.FunctionList.Functions[Function.Name];
            if (!_typeLoadHelper.TryGetSystemType(function.TypeName, Application, out Type? functionType))
            {
                SetInvalid
                (
                    string.Format(CultureInfo.CurrentCulture, Strings.genericTypeDescriptionFormat, function.TypeName, string.Join(Strings.itemsCommaSeparator, function.GenericArguments)),
                    string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeFormat2, function.TypeName)
                );
                return;
            }

            if (!functionType.IsGenericTypeDefinition)
            {
                SetInvalid
                (
                    string.Format(CultureInfo.CurrentCulture, Strings.genericTypeDescriptionFormat, functionType.Name, string.Join(Strings.itemsCommaSeparator, function.GenericArguments)),
                    string.Format(CultureInfo.CurrentCulture, Strings.functionGenericArgsMisMatchFormat2, _enumHelper.GetVisibleEnumText(ReferenceCategories.Type), functionType.Name, string.Join(Strings.itemsCommaSeparator, function.GenericArguments))
                );
                return;
            }

            FunctionData functionData = _functionDataParser.Parse(_xmlDocumentHelpers.GetDocumentElement(XmlDocument));
            List<string> errors = new();
            if
            (   
                !_functionGenericsConfigrationValidator.Validate
                (
                    function,
                    functionData.GenericArguments,
                    editFunctionControl.Application,
                    errors
                )
            )
            {
                SetInvalid
                (
                    string.Format(CultureInfo.CurrentCulture, Strings.genericTypeDescriptionFormat, functionType.Name, string.Join(Strings.itemsCommaSeparator, function.GenericArguments)),
                    string.Join(Strings.itemsCommaSeparator, errors)
                );
                return;
            }

            SetValid
            (
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.genericTypeDescriptionFormat, functionType.Name,
                    string.Join
                    (
                        Strings.itemsCommaSeparator,
                        functionData.GenericArguments.Select
                        (
                            ar =>
                            {
                                if (!_typeLoadHelper.TryGetSystemType(ar, editFunctionControl.Application, out Type? type))
                                    throw _exceptionHelper.CriticalException("{9FA50420-1C3E-48FF-9DB1-DAB8B03B8CCA}");

                                return type.Name;
                            }
                        )
                    )
                )
            );
        }

        private static void AddButtonClickCommand(RadButton radButton, IClickCommand command)
        {
            radButton.Click += (sender, args) => command.Execute();
        }

        private void Initialize()
        {
            InitializeRichTextBox();
            InitializeButton();

            AddButtonClickCommand(btnHelper, _fieldControlCommandFactory.GetAddUpdateFunctionGenericArgumentsCommand(this));

            UpdateValidState();
        }

        private void InitializeButton()
            => _layoutFieldControlButtons.Layout(radPanelButton, new RadButton[] { btnHelper });

        private void InitializeRichTextBox()
        {
            ((ISupportInitialize)this.radPanelRichTextBox).BeginInit();
            this.radPanelRichTextBox.SuspendLayout();

            _objectRichTextBox.Name = "objectRichTextBox";
            _objectRichTextBox.Dock = DockStyle.Fill;
            _objectRichTextBox.BorderStyle = BorderStyle.None;
            ControlsLayoutUtility.SetRichTextBoxPadding(this.radPanelRichTextBox);//shows the panel border instead
            _objectRichTextBox.Margin = new Padding(0);
            _objectRichTextBox.Location = new Point(0, 0);
            _objectRichTextBox.DetectUrls = false;
            _objectRichTextBox.HideSelection = false;
            _objectRichTextBox.Multiline = false;
            _objectRichTextBox.ReadOnly = true;

            this.radPanelRichTextBox.Controls.Add(_objectRichTextBox);
            ((ISupportInitialize)this.radPanelRichTextBox).EndInit();
            this.radPanelRichTextBox.ResumeLayout(true);
        }

        private void SetErrorBorderColor()
        {
            Color errorColor = ForeColorUtility.GetGroupBoxBorderErrorColor();
            SetPanelBorderForeColor(radPanelRichTextBox, errorColor);
        }

        private void SetInvalid(string text, string errors)
        {
            _objectRichTextBox.Text = text;
            editFunctionControl.SetErrorMessage(errors);
            SetErrorBorderColor();
        }

        private void SetNormalBorderColor()
        {
            Color normalColor = ForeColorUtility.GetGroupBoxBorderColor(ThemeResolutionService.ApplicationThemeName);
            SetPanelBorderForeColor(radPanelRichTextBox, normalColor);
        }

        private void SetValid(string text)
        {
            _objectRichTextBox.Text = text;
            editFunctionControl.ClearMessage();
            SetNormalBorderColor();
        }

        private static void SetPanelBorderForeColor(RadPanel radPanel, Color color)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).ForeColor = color;
    }
}
