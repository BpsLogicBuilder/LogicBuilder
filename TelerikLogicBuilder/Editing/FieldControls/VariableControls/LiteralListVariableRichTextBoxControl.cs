using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Components.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
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

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls
{
    internal partial class LiteralListVariableRichTextBoxControl : UserControl, ILiteralListVariableRichTextBoxControl
    {
        private readonly RadButton btnVariable;
        private readonly RadButton btnFunction;
        private readonly RadButton btnConstructor;
        private readonly RadButton btnLiteralList;
        private readonly RadButton btnObjectList;

        private readonly IFieldControlCommandFactory _fieldControlCommandFactory;
        private readonly IGetObjectRichTextBoxVisibleText _getObjectRichTextBoxVisibleText;
        private readonly IImageListService _imageListService;
        private readonly ILayoutFieldControlButtons _layoutFieldControlButtons;
        private readonly ILiteralListVariableElementInfoHelper _literalListVariableElementInfoHelper;
        private readonly IObjectListDataParser _objectListDataParser;
        private readonly IObjectListVariableElementInfoHelper _objectListVariableElementInfoHelper;
        private readonly ObjectRichTextBox _objectRichTextBox;
        private readonly IVariableObjectRichTextBoxEventsHelper _variableObjectRichTextBoxEventsHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IUpdateObjectRichTextBoxXml _updateObjectRichTextBoxXml;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IDataGraphEditingControl dataGraphEditingControl;
        private readonly ListOfLiteralsVariable listOfLiteralsVariable;
        private Type? _assignedTo;
        private EventHandler btnVariableClickHandler;
        private EventHandler btnFunctionClickHandler;
        private EventHandler btnConstructorClickHandler;
        private EventHandler btnLiteralListClickHandler;
        private EventHandler btnObjectListClickHandler;

        public LiteralListVariableRichTextBoxControl(
            IComponentFactory componentFactory,
            IFieldControlCommandFactory fieldControlCommandFactory,
            IFieldControlHelperFactory fieldControlHelperFactory,
            IGetObjectRichTextBoxVisibleText getObjectRichTextBoxVisibleText,
            IImageListService imageListService,
            ILayoutFieldControlButtons layoutFieldControlButtons,
            ILiteralListVariableElementInfoHelper literalListVariableElementInfoHelper,
            IObjectListDataParser objectListDataParser,
            IObjectListVariableElementInfoHelper objectListVariableElementInfoHelper,
            ITypeLoadHelper typeLoadHelper,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IDataGraphEditingControl dataGraphEditingControl,
            ListOfLiteralsVariable listOfLiteralsVariable)
        {
            InitializeComponent();
            _fieldControlCommandFactory = fieldControlCommandFactory;
            _getObjectRichTextBoxVisibleText = getObjectRichTextBoxVisibleText;
            _imageListService = imageListService;
            _layoutFieldControlButtons = layoutFieldControlButtons;
            _literalListVariableElementInfoHelper = literalListVariableElementInfoHelper;
            _objectListDataParser = objectListDataParser;
            _objectListVariableElementInfoHelper = objectListVariableElementInfoHelper;
            _objectRichTextBox = componentFactory.GetObjectRichTextBox();
            _typeLoadHelper = typeLoadHelper;
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.dataGraphEditingControl = dataGraphEditingControl;
            this.listOfLiteralsVariable = listOfLiteralsVariable;
            _variableObjectRichTextBoxEventsHelper = fieldControlHelperFactory.GetVariableObjectRichTextBoxEventsHelper(this);
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

        public ApplicationTypeInfo Application => dataGraphEditingControl.Application;

        public Type? AssignedTo
        {
            get
            {
                return _assignedTo ??= GetAssignedTo();

                Type? GetAssignedTo()
                {
                    _typeLoadHelper.TryGetSystemType(listOfLiteralsVariable, Application, out Type? type);
                    return type;
                }
            }
        }

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
                        XmlElement.OuterXml,//OuterXml element is literalListVariable
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

        public LiteralListVariableElementInfo LiteralListElementInfo => _literalListVariableElementInfoHelper.GetLiteralListElementInfo(listOfLiteralsVariable);

        public ObjectListVariableElementInfo ObjectListElementInfo
        {
            get
            {
                XmlElement? childElement = XmlElement == null ? null : _xmlDocumentHelpers.GetSingleOrDefaultChildElement(XmlElement);
                if (childElement?.Name == XmlDataConstants.OBJECTLISTELEMENT)
                {
                    return _objectListVariableElementInfoHelper.GetObjectListElementInfo
                    (
                        _objectListDataParser.Parse(childElement)
                    );
                }

                return _objectListVariableElementInfoHelper.GetDefaultObjectListElementInfo();
            }
        }

        public event EventHandler? Changed;

        public void ClearMessage() => dataGraphEditingControl.ClearMessage();

        public void HideControls() => ShowControls(false);

        public void InvokeChanged() => Changed?.Invoke(this, EventArgs.Empty);

        public void RequestDocumentUpdate() => dataGraphEditingControl.RequestDocumentUpdate();

        public void ResetControl()
        {
            XmlElement = null;
            RichTextBox.SetDefaultFormat();
            RichTextBox.Text = Strings.popupObjectNullDescription;
        }

        public void SetErrorBackColor()
        {
            Color errorColor = ForeColorUtility.GetGroupBoxBorderErrorColor();
            SetPanelBorderForeColor(radPanelRichTextBox, errorColor);
        }

        public void SetErrorMessage(string message) => dataGraphEditingControl.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => dataGraphEditingControl.SetMessage(message, title);

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

        public void ShowControls() => ShowControls(true);

        public void Update(XmlElement xmlElement)
            => _updateObjectRichTextBoxXml.Update(xmlElement);

        public void UpdateXmlElement(string innerXml)
        {
            XmlElement = _xmlDocumentHelpers.ToXmlElement
            (
                _xmlDataHelper.BuildVariableValueXml(listOfLiteralsVariable, innerXml)
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

            Disposed += LiteralListVariableRichTextBoxControl_Disposed;

            btnVariableClickHandler = AddButtonClickCommand(_fieldControlCommandFactory.GetEditObjectRichTextBoxVariableCommand(this));
            btnFunctionClickHandler = AddButtonClickCommand(_fieldControlCommandFactory.GetEditObjectRichTextBoxFunctionCommand(this));
            btnConstructorClickHandler = AddButtonClickCommand(_fieldControlCommandFactory.GetEditObjectRichTextBoxConstructorCommand(this));
            btnLiteralListClickHandler = AddButtonClickCommand(_fieldControlCommandFactory.GetEditVariableObjectRichTextBoxLiteralListCommand(this));
            btnObjectListClickHandler = AddButtonClickCommand(_fieldControlCommandFactory.GetEditVariableObjectRichTextBoxObjectListCommand(this));

            _variableObjectRichTextBoxEventsHelper.Setup();
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

        private void RemoveImageLists()
        {
            foreach (RadButton button in CommandButtons)
                button.ImageList = null;
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
        private void LiteralListVariableRichTextBoxControl_Disposed(object? sender, EventArgs e)
        {
            toolTip.RemoveAll();
            toolTip.Dispose();
            helpProvider.Dispose();
            RemoveImageLists();
            RemoveClickCommands();
        }
        #endregion Event Handlers
    }
}
