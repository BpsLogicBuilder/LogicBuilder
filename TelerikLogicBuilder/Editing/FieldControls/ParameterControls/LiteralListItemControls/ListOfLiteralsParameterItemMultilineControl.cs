﻿using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ParameterControls.LiteralListItemControls
{
    internal partial class ListOfLiteralsParameterItemMultilineControl : UserControl, IListOfLiteralsParameterItemMultilineControl
    {
        private readonly RadButton btnConstructor;
        private readonly RadButton btnFunction;
        private readonly RadButton btnVariable;

        private readonly IEnumHelper _enumHelper;
        private readonly ICreateRichInputBoxContextMenu _createRichInputBoxContextMenu;
        private readonly IFieldControlCommandFactory _fieldControlCommandFactory;
        private readonly IImageListService _imageListService;
        private readonly ILayoutFieldControlButtons _layoutFieldControlButtons;
        private readonly ILiteralListItemRichInputBoxEventsHelper _richInputBoxEventsHelper;
        private readonly IUpdateRichInputBoxXml _updateRichInputBoxXml;
        private readonly RichInputBox _richInputBox;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IDataGraphEditingControl dataGraphEditingControl;
        private readonly ListOfLiteralsParameter literalListParameter;
        private Type? _assignedTo;

        public ListOfLiteralsParameterItemMultilineControl(
            IEditingControlHelperFactory editingControlHelperFactory,
            IEnumHelper enumHelper,
            IFieldControlCommandFactory fieldControlCommandFactory,
            IFieldControlHelperFactory fieldControlHelperFactory,
            IImageListService imageListService,
            ILayoutFieldControlButtons layoutFieldControlButtons,
            IUpdateRichInputBoxXml updateRichInputBoxXml,
            RichInputBox richInputBox,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IDataGraphEditingControl dataGraphEditingControl,
            ListOfLiteralsParameter literalListParameter)
        {
            InitializeComponent();
            _enumHelper = enumHelper;
            _imageListService = imageListService;
            _fieldControlCommandFactory = fieldControlCommandFactory;
            _layoutFieldControlButtons = layoutFieldControlButtons;
            _updateRichInputBoxXml = updateRichInputBoxXml;
            _richInputBox = richInputBox;
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.dataGraphEditingControl = dataGraphEditingControl;
            this.literalListParameter = literalListParameter;
            _richInputBoxEventsHelper = fieldControlHelperFactory.GetLiteralListItemRichInputBoxEventsHelper(this);
            _createRichInputBoxContextMenu = editingControlHelperFactory.GetCreateRichInputBoxContextMenu(this);
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

        public IList<RadButton> CommandButtons => new RadButton[] { btnVariable, btnFunction, btnConstructor };

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
                    Type type = _enumHelper.GetSystemType(literalListParameter.LiteralType);

                    //string types convert multiple items (mixed xml) of different types to a format string so accepts all types.
                    //parent control field validation will handle single child cases.  Single items where type != typeof(string) are not valid.
                    return type == typeof(string) ? typeof(object) : type;
                }
            }
        }

        public bool IsEmpty => false;

        public string MixedXml => _richInputBox.GetMixedXml();

        public string VisibleText => _richInputBox.GetVisibleText();

        public XmlElement? XmlElement => _xmlDocumentHelpers.ToXmlElement
        (
            _xmlDataHelper.BuildLiteralXml(MixedXml)
        );

        public void DisableControls() => Enable(false);

        public void EnableControls() => Enable(true);

        public void ClearMessage() => dataGraphEditingControl.ClearMessage();

        public void HideControls() => ShowControls(false);

        public void InvokeChanged() => Changed?.Invoke(this, EventArgs.Empty);

        public void ResetControl() => _richInputBox.Clear();

        void IValueControl.Focus() => _richInputBox.Select();

        //string types convert multiple items (mixed xml) of different types to a format string so accepts all types.
        //parent control field validation will handle single child cases.  Single items where type != typeof(string) are not valid.
        public void SetAssignedToType(Type type) => _assignedTo = type == typeof(string) ? typeof(object) : type;

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
            toolTip.SetToolTip(btnConstructor, toolTipText);
            toolTip.SetToolTip(btnFunction, toolTipText);
            toolTip.SetToolTip(btnVariable, toolTipText);
        }

        public void ShowControls() => ShowControls(true);

        public void Update(XmlElement xmlElement) => _updateRichInputBoxXml.Update(xmlElement, _richInputBox);

        private static void AddButtonClickCommand(RadButton radButton, IClickCommand command)
        {
            radButton.Click += (sender, args) => command.Execute();
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private void Enable(bool enable)
        {
            //Enable displays the wrong color.
            _richInputBox.Visible = enable;//With ReadOnly CommandButtons become enabled on enter.
            //The radPanelRichInputBox border remains visible so the appearance is about right.

            foreach (RadButton button in CommandButtons)
                button.Enabled = enable;
        }

        private void Initialize()
        {
            InitializeRichInputBox();
            InitializeButtons();
            CollapsePanelBorder(radPanelRight);

            AddButtonClickCommand(btnConstructor, _fieldControlCommandFactory.GetEditRichInputBoxConstructorCommand(this));
            AddButtonClickCommand(btnFunction, _fieldControlCommandFactory.GetEditRichInputBoxFunctionCommand(this));
            AddButtonClickCommand(btnVariable, _fieldControlCommandFactory.GetEditRichInputBoxVariableCommand(this));

            _richInputBoxEventsHelper.Setup();
            _createRichInputBoxContextMenu.Create();
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

        private static void SetPanelBorderForeColor(RadPanel radPanel, Color color)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).ForeColor = color;

        private void ShowControls(bool show)
        {
            _richInputBox.Visible = show;
            radPanelRichInputBox.Visible = show;
            foreach (RadButton button in CommandButtons)
                button.Visible = show;
        }
    }
}