﻿using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.ItemEditorControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.ItemEditorControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
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

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.ItemEditorControls
{
    internal partial class ListOfLiteralsItemParameterSourcedPropertyRichInputBoxControl : UserControl, IListOfLiteralsItemParameterSourcedPropertyRichInputBoxControl
    {
        private readonly RadButton btnHelper;
        private readonly RadButton btnDomain;
        private readonly RadButton btnVariable;
        private readonly RadButton btnFunction;
        private readonly RadButton btnConstructor;

        private readonly ICreateRichInputBoxContextMenu _createRichInputBoxContextMenu;
        private readonly IEnumHelper _enumHelper;
        private readonly IFieldControlCommandFactory _fieldControlCommandFactory;
        private readonly IImageListService _imageListService;
        private readonly ILayoutFieldControlButtons _layoutFieldControlButtons;
        private readonly ILiteralListItemRichInputBoxEventsHelper _richInputBoxEventsHelper;
        private readonly IUpdateRichInputBoxXml _updateRichInputBoxXml;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly RichInputBox _richInputBox;

        private readonly IDataGraphEditingControl dataGraphEditingControl;
        private readonly LiteralListParameterElementInfo listInfo;
        private Type? _assignedTo;

        public ListOfLiteralsItemParameterSourcedPropertyRichInputBoxControl(
            IEditingControlHelperFactory editingControlHelperFactory,
            IEnumHelper enumHelper,
            IFieldControlCommandFactory fieldControlCommandFactory,
            IImageListService imageListService,
            ILayoutFieldControlButtons layoutFieldControlButtons,
            ILiteralListItemControlHelperFactory literalListItemControlHelperFactory,
            IUpdateRichInputBoxXml updateRichInputBoxXml,
            RichInputBox richInputBox,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IDataGraphEditingControl dataGraphEditingControl,
            LiteralListParameterElementInfo listInfo)
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
            this.listInfo = listInfo;
            _richInputBoxEventsHelper = literalListItemControlHelperFactory.GetLiteralListItemRichInputBoxEventsHelper(this);
            _createRichInputBoxContextMenu = editingControlHelperFactory.GetCreateRichInputBoxContextMenu(this);
            btnHelper = new()
            {
                Name = "btnHelper",
                ImageList = _imageListService.ImageList,
                ImageAlignment = ContentAlignment.MiddleCenter,
                Padding = new Padding(0),
                Margin = new Padding(1, 0, 1, 0),
                ImageIndex = ImageIndexes.HELPFILEWMAGEINDEX,
                Dock = DockStyle.Fill
            };
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

        public ApplicationTypeInfo Application => dataGraphEditingControl.Application;

        public string Comments => listInfo.Parameter?.Comments ?? string.Empty;

        public string? SourceClassName => this.listInfo.ParameterSourceClassName;

        public Type AssignedTo
        {
            get
            {
                return _assignedTo ??= GetAssignedTo();

                Type GetAssignedTo()
                {
                    Type type = _enumHelper.GetSystemType(listInfo.LiteralType);

                    //string types convert multiple items (mixed xml) of different types to a format string so accepts all types.
                    //parent control field validation will handle single child cases.  Single items where type != typeof(string) are not valid.
                    return type == typeof(string) ? typeof(object) : type;
                }
            }
        }

        public IList<RadButton> CommandButtons => new RadButton[] { btnHelper, btnDomain, btnVariable, btnFunction, btnConstructor };

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

        public bool IsEmpty => false;

        public string MixedXml => _richInputBox.GetMixedXml();

        public string VisibleText => _richInputBox.GetVisibleText();

        public XmlElement? XmlElement => _xmlDocumentHelpers.ToXmlElement
        (
            _xmlDataHelper.BuildLiteralXml(MixedXml)
        );

        public void DisableControls() => Enable(false);

        public void EnableControls() => Enable(true);

        public void HideControls() => ShowControls(false);

        public void InvokeChanged() => Changed?.Invoke(this, EventArgs.Empty);

        public void ResetControl() => _richInputBox.Clear();

        //string types convert multiple items (mixed xml) of different types to a format string so accepts all types.
        //parent control field validation will handle single child cases.  Single items where type != typeof(string) are not valid.
        public void SetAssignedToType(Type type) => _assignedTo = type == typeof(string) ? typeof(object) : type;

        public void SetErrorBackColor()
        {
            Color errorColor = ForeColorUtility.GetGroupBoxBorderErrorColor();
            SetPanelBorderForeColor(radPanelRichInputBox, errorColor);
        }

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

        private static void AddButtonClickCommand(RadButton radButton, IClickCommand command)
        {
            radButton.Click += (sender, args) => command.Execute();
        }

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

            AddButtonClickCommand(btnHelper, _fieldControlCommandFactory.GetSelectItemFromReferencesTreeViewCommand(this));
            AddButtonClickCommand(btnDomain, _fieldControlCommandFactory.GetSelectItemFromPropertyListCommand(this));
            AddButtonClickCommand(btnVariable, _fieldControlCommandFactory.GetEditRichInputBoxVariableCommand(this));
            AddButtonClickCommand(btnFunction, _fieldControlCommandFactory.GetEditRichInputBoxFunctionCommand(this));
            AddButtonClickCommand(btnConstructor, _fieldControlCommandFactory.GetEditRichInputBoxConstructorCommand(this));

            _richInputBoxEventsHelper.Setup();
            _createRichInputBoxContextMenu.Create();
        }

        private void InitializeButtons()
            => _layoutFieldControlButtons.Layout
            (
                radPanelCommandBar,
                CommandButtons
            );

        private void InitializeRichInputBox()
        {
            ((ISupportInitialize)this.radPanelRichInputBox).BeginInit();
            this.radPanelRichInputBox.SuspendLayout();

            _richInputBox.Name = "richInputBox";
            _richInputBox.Dock = DockStyle.Fill;
            _richInputBox.BorderStyle = BorderStyle.None;
            this.radPanelRichInputBox.Padding = new Padding(1);//shows the panel border instead
            _richInputBox.Margin = new Padding(0);
            _richInputBox.Location = new Point(0, 0);
            _richInputBox.DetectUrls = false;
            _richInputBox.HideSelection = false;
            _richInputBox.Multiline = false;
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
