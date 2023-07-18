using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
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
using System.Diagnostics.CodeAnalysis;
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
    internal partial class ConstructorGenericParametersControl : UserControl, IConstructorGenericParametersControl
    {
        private readonly RadButton btnHelper;

        private readonly IConfigurationService _configurationService;
        private readonly IConstructorDataParser _constructorDataParser;
        private readonly IConstructorGenericsConfigrationValidator _constructorGenericsConfigrationValidator;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFieldControlCommandFactory _fieldControlCommandFactory;
        private readonly IImageListService _imageListService;
        private readonly ILayoutFieldControlButtons _layoutFieldControlButtons;
        private readonly ObjectRichTextBox _objectRichTextBox;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IEditConstructorControl editConstructorControl;
        private EventHandler btnHelperClickHandler;

        public ConstructorGenericParametersControl(
            IConfigurationService configurationService,
            IConstructorDataParser constructorDataParser,
            IConstructorGenericsConfigrationValidator constructorGenericsConfigrationValidator,
            IExceptionHelper exceptionHelper,
            IFieldControlCommandFactory fieldControlCommandFactory,
            IImageListService imageListService,
            ILayoutFieldControlButtons layoutFieldControlButtons,
            ObjectRichTextBox objectRichTextBox,
            ITypeLoadHelper typeLoadHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditConstructorControl editConstructorControl)
        {
            InitializeComponent();
            _configurationService = configurationService;
            _constructorDataParser = constructorDataParser;
            _constructorGenericsConfigrationValidator = constructorGenericsConfigrationValidator;
            _exceptionHelper = exceptionHelper;
            _fieldControlCommandFactory = fieldControlCommandFactory;
            _imageListService = imageListService;
            _layoutFieldControlButtons = layoutFieldControlButtons;
            _objectRichTextBox = objectRichTextBox;
            _typeLoadHelper = typeLoadHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;

            this.editConstructorControl = editConstructorControl;

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

        public ApplicationTypeInfo Application => editConstructorControl.Application;

        public Constructor Constructor => editConstructorControl.Constructor;

        public XmlDocument XmlDocument => editConstructorControl.XmlDocument;

        public void ResetControls() 
            => editConstructorControl.ResetControls();

        public void UpdateValidState()
        {
            Constructor constructor = _configurationService.ConstructorList.Constructors[Constructor.Name];

            if (!_typeLoadHelper.TryGetSystemType(constructor.TypeName, Application, out Type? constructorType))
            {
                SetInvalid
                (
                    string.Format(CultureInfo.CurrentCulture, Strings.genericTypeDescriptionFormat, constructor.TypeName, string.Join(Strings.itemsCommaSeparator, constructor.GenericArguments)),
                    string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeFormat2, constructor.TypeName)
                );
                return;
            }

            if (!constructorType.IsGenericTypeDefinition)
            {
                SetInvalid
                (
                    string.Format(CultureInfo.CurrentCulture, Strings.genericTypeDescriptionFormat, constructorType.Name, string.Join(Strings.itemsCommaSeparator, constructor.GenericArguments)),
                    string.Format(CultureInfo.CurrentCulture, Strings.constructorGenericArgsMisMatchFormat2, constructorType.Name, string.Join(Strings.itemsCommaSeparator, constructor.GenericArguments))
                );
                return;
            }

            ConstructorData constructorData = _constructorDataParser.Parse(_xmlDocumentHelpers.GetDocumentElement(XmlDocument));
            List<string> errors = new();
            if 
            (  
                !_constructorGenericsConfigrationValidator.Validate
                (
                    constructor,
                    constructorData.GenericArguments,
                    editConstructorControl.Application,
                    errors
                )
            )
            {
                SetInvalid
                (
                    string.Format(CultureInfo.CurrentCulture, Strings.genericTypeDescriptionFormat, constructorType.Name, string.Join(Strings.itemsCommaSeparator, constructor.GenericArguments)),
                    string.Join(Strings.itemsCommaSeparator, errors)
                );
                return;
            }

            SetValid
            (
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.genericTypeDescriptionFormat, constructorType.Name,
                    string.Join
                    (
                        Strings.itemsCommaSeparator,
                        constructorData.GenericArguments.Select
                        (
                            ar =>
                            {
                                if (!_typeLoadHelper.TryGetSystemType(ar, editConstructorControl.Application, out Type? type))
                                    throw _exceptionHelper.CriticalException("{8853EBF9-9053-494E-80C5-60D6E984B35E}");

                                return type.Name;
                            }
                        )
                    )
                )
            );
        }

        private static EventHandler AddButtonClickCommand(IClickCommand command)
        {
            return (sender, args) => command.Execute();
        }

        private void AddClickCommands()
        {
            RemoveClickCommands();
            btnHelper.Click += btnHelperClickHandler;
        }

        [MemberNotNull(nameof(btnHelperClickHandler))]
        private void Initialize()
        {
            InitializeRichTextBox();
            InitializeButton();

            Disposed += ConstructorGenericParametersControl_Disposed;

            btnHelperClickHandler = AddButtonClickCommand(_fieldControlCommandFactory.GetAddUpdateConstructorGenericArgumentsCommand(this));

            UpdateValidState();
            AddClickCommands();
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

        private void RemoveClickCommands()
        {
            btnHelper.Click -= btnHelperClickHandler;
        }

        private void SetErrorBorderColor()
        {
            Color errorColor = ForeColorUtility.GetGroupBoxBorderErrorColor();
            SetPanelBorderForeColor(radPanelRichTextBox, errorColor);
        }

        private void SetInvalid(string text, string errors)
        {
            _objectRichTextBox.Text = text;
            editConstructorControl.SetErrorMessage(errors);
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
            editConstructorControl.ClearMessage();
            SetNormalBorderColor();
        }

        private static void SetPanelBorderForeColor(RadPanel radPanel, Color color)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).ForeColor = color;

        #region Event Handlers
        private void ConstructorGenericParametersControl_Disposed(object? sender, EventArgs e)
        {
            btnHelper.ImageList = null;
            RemoveClickCommands();
        }
        #endregion Event Handlers
    }
}
