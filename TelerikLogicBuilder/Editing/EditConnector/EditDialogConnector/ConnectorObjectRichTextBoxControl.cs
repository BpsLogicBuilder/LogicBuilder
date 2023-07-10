using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConnector.EditDialogConnector
{
    internal partial class ConnectorObjectRichTextBoxControl : UserControl, IConnectorObjectRichTextBoxControl
    {
        private readonly RadButton btnVariable;
        private readonly RadButton btnFunction;
        private readonly RadButton btnConstructor;
        private readonly RadButton btnLiteralList;
        private readonly RadButton btnObjectList;

        private readonly IConstructorTypeHelper _constructorTypeHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFieldControlCommandFactory _fieldControlCommandFactory;
        private readonly IGetObjectRichTextBoxVisibleText _getObjectRichTextBoxVisibleText;
        private readonly IImageListService _imageListService;
        private readonly ILayoutFieldControlButtons _layoutFieldControlButtons;
        private readonly ILiteralListDataParser _literalListDataParser;
        private readonly ILiteralListParameterElementInfoHelper _literalListParameterElementInfoHelper;
        private readonly IObjectListDataParser _objectListDataParser;
        private readonly IObjectListParameterElementInfoHelper _objectListParameterElementInfoHelper;
        private readonly ObjectRichTextBox _objectRichTextBox;
        private readonly IParameterObjectRichTextBoxEventsHelper _parameterObjectRichTextBoxEventsHelper;
        private readonly ITypeHelper _typeHelper;
        private readonly IUpdateObjectRichTextBoxXml _updateObjectRichTextBoxXml;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IEditingControl editingControl;
        private Type? _assignedTo;
        private EventHandler btnVariableClickHandler;
        private EventHandler btnFunctionClickHandler;
        private EventHandler btnConstructorClickHandler;
        private EventHandler btnLiteralListClickHandler;
        private EventHandler btnObjectListClickHandler;

        public ConnectorObjectRichTextBoxControl(
            IConstructorTypeHelper constructorTypeHelper,
            IExceptionHelper exceptionHelper,
            IFieldControlCommandFactory fieldControlCommandFactory,
            IFieldControlHelperFactory fieldControlHelperFactory,
            IGetObjectRichTextBoxVisibleText getObjectRichTextBoxVisibleText,
            IImageListService imageListService,
            ILayoutFieldControlButtons layoutFieldControlButtons,
            ILiteralListDataParser literalListDataParser,
            ILiteralListParameterElementInfoHelper literalListParameterElementInfoHelper,
            IObjectListDataParser objectListDataParser,
            IObjectListParameterElementInfoHelper objectListParameterElementInfoHelper,
            ObjectRichTextBox objectRichTextBox,
            ITypeHelper typeHelper,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditingControl editingControl)
        {
            InitializeComponent();
            _constructorTypeHelper = constructorTypeHelper;
            _exceptionHelper = exceptionHelper;
            _fieldControlCommandFactory = fieldControlCommandFactory;
            _getObjectRichTextBoxVisibleText = getObjectRichTextBoxVisibleText;
            _imageListService = imageListService;
            _layoutFieldControlButtons = layoutFieldControlButtons;
            _literalListDataParser = literalListDataParser;
            _literalListParameterElementInfoHelper = literalListParameterElementInfoHelper;
            _objectListDataParser = objectListDataParser;
            _objectListParameterElementInfoHelper = objectListParameterElementInfoHelper;
            _objectRichTextBox = objectRichTextBox;
            _typeHelper = typeHelper;
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.editingControl = editingControl;

            _parameterObjectRichTextBoxEventsHelper = fieldControlHelperFactory.GetParameterObjectRichTextBoxEventsHelper(this);
            _updateObjectRichTextBoxXml = fieldControlHelperFactory.GetUpdateObjectRichTextBoxXml(this);

            btnVariable = new()
            {
                Name = "btnVariable",
                ImageList = _imageListService.ImageList,
                ImageAlignment = ContentAlignment.MiddleCenter,
                Padding = new Padding(0),
                Margin = new Padding(1, 0, 1, 0),
                ImageIndex = ImageIndexes.VARIABLEIMAGEINDEX,
                Dock = DockStyle.Fill
            };
            btnFunction = new()
            {
                Name = "btnFunction",
                ImageList = _imageListService.ImageList,
                ImageAlignment = ContentAlignment.MiddleCenter,
                Padding = new Padding(0),
                Margin = new Padding(1, 0, 1, 0),
                ImageIndex = ImageIndexes.METHODIMAGEINDEX,
                Dock = DockStyle.Fill
            };
            btnConstructor = new()
            {
                Name = "btnConstructor",
                ImageList = _imageListService.ImageList,
                ImageAlignment = ContentAlignment.MiddleCenter,
                Padding = new Padding(0),
                Margin = new Padding(1, 0, 1, 0),
                ImageIndex = ImageIndexes.CONSTRUCTORIMAGEINDEX,
                Dock = DockStyle.Fill
            };
            btnLiteralList = new()
            {
                Name = "btnLiteralList",
                ImageList = _imageListService.ImageList,
                ImageAlignment = ContentAlignment.MiddleCenter,
                Padding = new Padding(0),
                Margin = new Padding(1, 0, 1, 0),
                ImageIndex = ImageIndexes.LITERALLISTCONSTRUCTORIMAGEINDEX,
                Dock = DockStyle.Fill
            };
            btnObjectList = new()
            {
                Name = "btnObjectList",
                ImageList = _imageListService.ImageList,
                ImageAlignment = ContentAlignment.MiddleCenter,
                Padding = new Padding(0),
                Margin = new Padding(1, 0, 1, 0),
                ImageIndex = ImageIndexes.OBJECTLISTCONSTRUCTORIMAGEINDEX,
                Dock = DockStyle.Fill
            };

            Initialize();
        }

        private readonly HelpProvider helpProvider = new();
        private readonly RadToolTip toolTip = new();

        private IList<RadButton> CommandButtons => new RadButton[] { btnVariable, btnFunction, btnConstructor, btnLiteralList, btnObjectList };

        public LiteralListParameterElementInfo LiteralListElementInfo
        {
            get
            {
                XmlElement? childElement = XmlElement == null ? null : _xmlDocumentHelpers.GetSingleOrDefaultChildElement(XmlElement);
                if (childElement?.Name == XmlDataConstants.LITERALLISTELEMENT)
                {
                    return _literalListParameterElementInfoHelper.GetLiteralListElementInfo
                    (
                        _literalListDataParser.Parse(childElement)
                    );
                }

                return _literalListParameterElementInfoHelper.GetDefaultLiteralListElementInfo();
            }
        }

        public ObjectListParameterElementInfo ObjectListElementInfo
        {
            get
            {
                XmlElement? childElement = XmlElement == null ? null : _xmlDocumentHelpers.GetSingleOrDefaultChildElement(XmlElement);
                if (childElement?.Name == XmlDataConstants.OBJECTLISTELEMENT)
                {
                    return _objectListParameterElementInfoHelper.GetObjectListElementInfo
                    (
                        _objectListDataParser.Parse(childElement)
                    );
                }

                return _objectListParameterElementInfoHelper.GetDefaultObjectListElementInfo();
            }
        }

        public ApplicationTypeInfo Application => editingControl.Application;

        public Type? AssignedTo => _assignedTo ?? throw _exceptionHelper.CriticalException("{A37A940C-7672-4340-86F0-B02F1A51DAF1}");

        public ObjectRichTextBox RichTextBox => _objectRichTextBox;

        public bool IsEmpty => XmlElement == null || _xmlDocumentHelpers.GetSingleOrDefaultChildElement(XmlElement) == null;

        public string MixedXml => XmlElement?.InnerXml ?? string.Empty;

        public string VisibleText
        {
            get
            {
                if (XmlElement?.InnerXml == null)
                    return string.Empty;

                XmlElement = _xmlDocumentHelpers.ToXmlElement
                (
                    _getObjectRichTextBoxVisibleText.RefreshVisibleTexts
                    (
                        XmlElement.OuterXml,//OuterXml element is metaObject
                        Application
                    )
                );

                return _getObjectRichTextBoxVisibleText.GetVisibleText
                (
                    XmlElement.InnerXml,
                    Application
                );
            }
        }

        public XmlElement? XmlElement { get; private set; }

        public event EventHandler? Changed;

        public void ClearMessage() => editingControl.ClearMessage();

        public void DisableControls() => Enable(false);

        public void EnableControls() => Enable(true);

        public void HideControls() => ShowControls(false);

        public void InvokeChanged() => Changed?.Invoke(this, EventArgs.Empty);

        public void RequestDocumentUpdate() => Changed?.Invoke(this, EventArgs.Empty);

        public void ResetControl()
        {
            XmlElement = null;
            RichTextBox.SetDefaultFormat();
            RichTextBox.Text = Strings.popupObjectNullDescription;
        }

        public void SetAssignedToType(Type type) => _assignedTo = type;

        public void SetErrorBackColor()
        {
            Color errorColor = ForeColorUtility.GetGroupBoxBorderErrorColor();
            SetPanelBorderForeColor(radPanelRichTextBox, errorColor);
        }

        public void SetErrorMessage(string message) => editingControl.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => editingControl.SetMessage(message, title);

        public void SetNormalBackColor()
        {
            Color normalColor = ForeColorUtility.GetGroupBoxBorderColor(ThemeResolutionService.ApplicationThemeName);
            SetPanelBorderForeColor(radPanelRichTextBox, normalColor);
        }

        public void SetToolTipHelp(string toolTipText)
        {
            helpProvider.SetHelpString(_objectRichTextBox, toolTipText);
            toolTip.SetToolTip(_objectRichTextBox, toolTipText);
            foreach (RadButton button in CommandButtons)
                toolTip.SetToolTip(button, toolTipText);
        }

        public void SetupDefaultElement(Type type)
        {
            _assignedTo = type;

            ClosedConstructor? constructor = _constructorTypeHelper.GetConstructor(_assignedTo, Application) ?? throw _exceptionHelper.CriticalException("{5FA3EFC6-D10A-45F8-BACB-0BB507999681}");
            XmlElement = _xmlDocumentHelpers.ToXmlElement
            (
                _xmlDataHelper.BuildMetaObjectXml
                (
                    _typeHelper.ToId(_assignedTo),
                    _xmlDataHelper.BuildDefaultConstructorXml(constructor)
                )
            );

            RichTextBox.SetLinkFormat();
            RichTextBox.Text = VisibleText;
        }

        public void ShowControls() => ShowControls(true);

        public void Update(XmlElement xmlElement)
            => _updateObjectRichTextBoxXml.Update(xmlElement);

        public void UpdateXmlElement(string innerXml)
        {
            if (AssignedTo == null)
                throw _exceptionHelper.CriticalException("{F00E4610-E35E-494D-B5E0-FB49B4916B37}");

            XmlElement = _xmlDocumentHelpers.ToXmlElement
            (
                _xmlDataHelper.BuildMetaObjectXml
                (
                    _typeHelper.ToId(AssignedTo),
                    innerXml
                )
            );
        }

        void IValueControl.Focus() => _objectRichTextBox.Select();

        private static EventHandler AddButtonClickCommand(IClickCommand command)
        {
            return (sender, args) => command.Execute();
        }

        private void AddClickCommands()
        {
            RemoveClickCommands();
            btnVariable.Click += btnVariableClickHandler;
            btnFunction.Click += btnFunctionClickHandler;
            btnConstructor.Click += btnConstructorClickHandler;
            btnLiteralList.Click += btnLiteralListClickHandler;
            btnObjectList.Click += btnObjectListClickHandler;
        }

        private void Enable(bool enable)
        {
            //Enable displays the wrong color.
            _objectRichTextBox.Visible = enable;//With ReadOnly CommandButtons become enabled on enter.
            //The radPanelRichInputBox border remains visible so the appearance is about right.

            foreach (RadButton button in CommandButtons)
                button.Enabled = enable;
        }

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [MemberNotNull(nameof(btnVariableClickHandler),
        nameof(btnFunctionClickHandler),
        nameof(btnConstructorClickHandler),
        nameof(btnLiteralListClickHandler),
        nameof(btnObjectListClickHandler))]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        private void Initialize()
        {
            InitializeRichTextBox();
            InitializeButtons();
            ResetControl();

            Disposed += ConnectorObjectRichTextBoxControl_Disposed;

            btnVariableClickHandler = AddButtonClickCommand(_fieldControlCommandFactory.GetEditObjectRichTextBoxVariableCommand(this));
            btnFunctionClickHandler = AddButtonClickCommand(_fieldControlCommandFactory.GetEditObjectRichTextBoxFunctionCommand(this));
            btnConstructorClickHandler = AddButtonClickCommand(_fieldControlCommandFactory.GetEditObjectRichTextBoxConstructorCommand(this));
            btnLiteralListClickHandler = AddButtonClickCommand(_fieldControlCommandFactory.GetEditParameterObjectRichTextBoxLiteralListCommand(this));
            btnObjectListClickHandler = AddButtonClickCommand(_fieldControlCommandFactory.GetEditParameterObjectRichTextBoxObjectListCommand(this));

            _parameterObjectRichTextBoxEventsHelper.Setup();
            AddClickCommands();
        }

        private void InitializeButtons()
            => _layoutFieldControlButtons.Layout
            (
                radPanelCommandBar,
                CommandButtons
            );

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
            btnVariable.Click -= btnVariableClickHandler;
            btnFunction.Click -= btnFunctionClickHandler;
            btnConstructor.Click -= btnConstructorClickHandler;
            btnLiteralList.Click -= btnLiteralListClickHandler;
            btnObjectList.Click -= btnObjectListClickHandler;
        }

        private static void SetPanelBorderForeColor(RadPanel radPanel, Color color)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).ForeColor = color;

        private void ShowControls(bool show)
        {
            _objectRichTextBox.Visible = show;
            radPanelRichTextBox.Visible = show;
            foreach (RadButton button in CommandButtons)
                button.Visible = show;
        }

        #region Event Handlers
        private void ConnectorObjectRichTextBoxControl_Disposed(object? sender, EventArgs e)
        {
            RemoveClickCommands();
        }
        #endregion Event Handlers
    }
}
