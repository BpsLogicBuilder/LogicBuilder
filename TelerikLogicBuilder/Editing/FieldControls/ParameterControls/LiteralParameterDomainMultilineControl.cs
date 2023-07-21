using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Components.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
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

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ParameterControls
{
    internal partial class LiteralParameterDomainMultilineControl : UserControl, ILiteralParameterDomainMultilineControl
    {
        private readonly RadButton btnDomain;
        private readonly RadButton btnVariable;
        private readonly RadButton btnFunction;
        private readonly RadButton btnConstructor;

        private readonly IEnumHelper _enumHelper;
        private readonly ICreateLiteralParameterXmlElement _createLiteralParameterXmlElement;
        private readonly ICreateRichInputBoxContextMenu _createRichInputBoxContextMenu;
        private readonly IFieldControlCommandFactory _fieldControlCommandFactory;
        private readonly IImageListService _imageListService;
        private readonly ILayoutFieldControlButtons _layoutFieldControlButtons;
        private readonly IParameterRichInputBoxEventsHelper _richInputBoxEventsHelper;
        private readonly IUpdateRichInputBoxXml _updateRichInputBoxXml;
        private readonly RichInputBox _richInputBox;

        private readonly IDataGraphEditingControl dataGraphEditingControl;
        private readonly LiteralParameter literalParameter;
        private Type? _assignedTo;
        private EventHandler btnDomainClickHandler;
        private EventHandler btnVariableClickHandler;
        private EventHandler btnFunctionClickHandler;
        private EventHandler btnConstructorClickHandler;

        public LiteralParameterDomainMultilineControl(
            IComponentFactory componentFactory,
            ICreateLiteralParameterXmlElement createLiteralParameterXmlElement,
            IEditingControlHelperFactory editingControlHelperFactory,
            IEnumHelper enumHelper,
            IFieldControlCommandFactory fieldControlCommandFactory,
            IFieldControlHelperFactory fieldControlHelperFactory,
            IImageListService imageListService,
            ILayoutFieldControlButtons layoutFieldControlButtons,
            IUpdateRichInputBoxXml updateRichInputBoxXml,
            IDataGraphEditingControl dataGraphEditingControl,
            LiteralParameter literalParameter)
        {
            InitializeComponent();
            _createLiteralParameterXmlElement = createLiteralParameterXmlElement;
            _enumHelper = enumHelper;
            _imageListService = imageListService;
            _fieldControlCommandFactory = fieldControlCommandFactory;
            _layoutFieldControlButtons = layoutFieldControlButtons;
            _updateRichInputBoxXml = updateRichInputBoxXml;
            _richInputBox = componentFactory.GetRichInputBox();
            this.dataGraphEditingControl = dataGraphEditingControl;
            this.literalParameter = literalParameter;
            _richInputBoxEventsHelper = fieldControlHelperFactory.GetParameterRichInputBoxEventsHelper(this);
            _createRichInputBoxContextMenu = editingControlHelperFactory.GetCreateRichInputBoxContextMenu(this);
            btnDomain = new()
            {
                Name = "btnDomain",
                ImageList = _imageListService.ImageList,
                ImageAlignment = ContentAlignment.MiddleCenter,
                Padding = new Padding(0),
                Margin = new Padding(1, 0, 1, 0),
                ImageIndex = ImageIndexes.MOREIMAGEINDEX,
                Dock = DockStyle.Fill
            };
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

            Initialize();
        }

        private readonly RadMenuItem mnuItemInsert = new(Strings.mnuItemInsertText);
        private readonly RadMenuItem mnuItemInsertConstructor = new(Strings.mnuItemInsertConstructorText);
        private readonly RadMenuItem mnuItemInsertFunction = new(Strings.mnuItemInsertFunctionText);
        private readonly RadMenuItem mnuItemInsertVariable = new(Strings.mnuItemInsertVariableText);
        private readonly RadMenuItem mnuItemDelete = new(Strings.mnuItemDeleteText);
        private readonly RadMenuItem mnuItemClear = new(Strings.mnuItemClearText);
        private readonly RadMenuItem mnuItemCopy = new(Strings.mnuItemCopyText) { ImageIndex = ImageIndexes.COPYIMAGEINDEX };
        private readonly RadMenuItem mnuItemCut = new(Strings.mnuItemCutText) { ImageIndex = ImageIndexes.CUTIMAGEINDEX };
        private readonly RadMenuItem mnuItemPaste = new(Strings.mnuItemPasteText);
        private readonly RadMenuItem mnuItemToCamelCase = new(Strings.mnuItemToCamelCaseText);

        private readonly HelpProvider helpProvider = new();
        private readonly RadToolTip toolTip = new();

        public event EventHandler? Changed;

        public IList<RadButton> CommandButtons => new RadButton[] { btnDomain, btnVariable, btnFunction, btnConstructor };

        public RadMenuItem MnuItemInsert => mnuItemInsert;
        public RadMenuItem MnuItemInsertConstructor => mnuItemInsertConstructor;
        public RadMenuItem MnuItemInsertFunction => mnuItemInsertFunction;
        public RadMenuItem MnuItemInsertVariable => mnuItemInsertVariable;
        public RadMenuItem MnuItemDelete => mnuItemDelete;
        public RadMenuItem MnuItemClear => mnuItemClear;
        public RadMenuItem MnuItemCopy => mnuItemCopy;
        public RadMenuItem MnuItemCut => mnuItemCut;
        public RadMenuItem MnuItemPaste => mnuItemPaste;
        public RadMenuItem MnuItemToCamelCase => mnuItemToCamelCase;

        public RichInputBox RichInputBox => _richInputBox;

        public ApplicationTypeInfo Application => dataGraphEditingControl.Application;

        public Type AssignedTo
        {
            get
            {
                return _assignedTo ??= GetAssignedTo();

                Type GetAssignedTo()
                {
                    Type type = _enumHelper.GetSystemType(literalParameter.LiteralType);

                    //string types convert multiple items (mixed xml) of different types to a format string so accepts all types.
                    //parent control field validation will handle single child cases.  Single items where type != typeof(string) are not valid.
                    return type == typeof(string) ? typeof(object) : type;
                }
            }
        }

        public bool IsEmpty => false;

        public string MixedXml => _richInputBox.GetMixedXml();

        public string VisibleText => _richInputBox.GetVisibleText();

        public XmlElement? XmlElement => _createLiteralParameterXmlElement.Create(literalParameter, MixedXml);

        public string Comments => literalParameter.Comments;

        public IList<string> Domain => literalParameter.Domain;

        public void ClearMessage() => dataGraphEditingControl.ClearMessage();

        public void HideControls() => ShowControls(false);

        public void InvokeChanged() => Changed?.Invoke(this, EventArgs.Empty);

        public void RequestDocumentUpdate() => dataGraphEditingControl.RequestDocumentUpdate();

        public void ResetControl() => _richInputBox.Clear();

        public void SetErrorBackColor()
        {
            Color errorColor = ForeColorUtility.GetGroupBoxBorderErrorColor();
            SetPanelBorderForeColor(radPanelRichInputBox, errorColor);
        }

        public void SetErrorMessage(string message) => dataGraphEditingControl.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => dataGraphEditingControl.SetMessage(message, title);

        public void SetNormalBackColor()
        {
            Color normalColor = ForeColorUtility.GetGroupBoxBorderColor(ThemeResolutionService.ApplicationThemeName);
            SetPanelBorderForeColor(radPanelRichInputBox, normalColor);
        }

        public void SetToolTipHelp(string toolTipText)
        {
            helpProvider.SetHelpString(_richInputBox, toolTipText);
            toolTip.SetToolTip(_richInputBox, toolTipText);
            foreach (RadButton button in CommandButtons)
                toolTip.SetToolTip(button, toolTipText);
        }

        public void ShowControls() => ShowControls(true);

        public void Update(XmlElement xmlElement) => _updateRichInputBoxXml.Update(xmlElement, _richInputBox);

        void IValueControl.Focus() => _richInputBox.Select();

        private static EventHandler AddButtonClickCommand(IClickCommand command)
        {
            return (sender, args) => command.Execute();
        }

        private void AddClickCommands()
        {
            RemoveClickCommands();
            btnDomain.Click += btnDomainClickHandler;
            btnVariable.Click += btnVariableClickHandler;
            btnFunction.Click += btnFunctionClickHandler;
            btnConstructor.Click += btnConstructorClickHandler;
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [MemberNotNull(nameof(btnDomainClickHandler),
        nameof(btnVariableClickHandler),
        nameof(btnFunctionClickHandler),
        nameof(btnConstructorClickHandler))]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        private void Initialize()
        {
            InitializeRichInputBox();
            InitializeButtons();
            CollapsePanelBorder(radPanelRight);

            Disposed += LiteralParameterDomainMultilineControl_Disposed;

            btnDomainClickHandler = AddButtonClickCommand(_fieldControlCommandFactory.GetSelectDomainItemCommand(this));
            btnVariableClickHandler = AddButtonClickCommand(_fieldControlCommandFactory.GetEditRichInputBoxVariableCommand(this));
            btnFunctionClickHandler = AddButtonClickCommand(_fieldControlCommandFactory.GetEditRichInputBoxFunctionCommand(this));
            btnConstructorClickHandler = AddButtonClickCommand(_fieldControlCommandFactory.GetEditRichInputBoxConstructorCommand(this));

            _richInputBoxEventsHelper.Setup();
            _createRichInputBoxContextMenu.Create();
            AddClickCommands();
        }

        private void InitializeButtons()
        {
            int buttonWidth = PerFontSizeConstants.CommandButtonWidth;
            int rightPanelWidth = CommandButtons.Count * buttonWidth;

            ((ISupportInitialize)this.radPanelRight).BeginInit();
            this.radPanelRight.SuspendLayout();

            radPanelCommandBar.Size = new Size(rightPanelWidth, buttonWidth);
            radPanelRight.Size = new Size(rightPanelWidth, radPanelRight.Height);
            _layoutFieldControlButtons.Layout
            (
                radPanelCommandBar,
                CommandButtons,
                false
            );

            ((ISupportInitialize)this.radPanelRight).EndInit();
            this.radPanelRight.ResumeLayout(true);
        }

        private void InitializeRichInputBox()
        {
            ((ISupportInitialize)this.radPanelRichInputBox).BeginInit();
            this.radPanelRichInputBox.SuspendLayout();

            _richInputBox.Name = "richInputBox";
            _richInputBox.Dock = DockStyle.Fill;
            _richInputBox.BorderStyle = BorderStyle.None;
            ControlsLayoutUtility.SetRichTextBoxPadding(this.radPanelRichInputBox);//shows the panel border instead
            _richInputBox.Margin = new Padding(0);
            _richInputBox.Location = new Point(0, 0);
            _richInputBox.DetectUrls = false;
            _richInputBox.HideSelection = false;
            _richInputBox.Multiline = true;
            _richInputBox.DenySpecialCharacters = dataGraphEditingControl.DenySpecialCharacters;

            this.radPanelRichInputBox.Controls.Add(_richInputBox);
            ((ISupportInitialize)this.radPanelRichInputBox).EndInit();
            this.radPanelRichInputBox.ResumeLayout(true);
        }

        private void RemoveClickCommands()
        {
            btnDomain.Click -= btnDomainClickHandler;
            btnVariable.Click -= btnVariableClickHandler;
            btnFunction.Click -= btnFunctionClickHandler;
            btnConstructor.Click -= btnConstructorClickHandler;
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
            _richInputBox.Visible = show;
            radPanelRichInputBox.Visible = show;
            foreach (RadButton button in CommandButtons)
                button.Visible = show;
        }

        #region Event Handlers
        private void LiteralParameterDomainMultilineControl_Disposed(object? sender, EventArgs e)
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
