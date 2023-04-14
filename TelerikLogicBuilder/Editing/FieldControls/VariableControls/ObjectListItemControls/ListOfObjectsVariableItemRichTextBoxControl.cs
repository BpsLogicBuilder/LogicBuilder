﻿using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls.ObjectListItemControls
{
    internal partial class ListOfObjectsVariableItemRichTextBoxControl : UserControl, IListOfObjectsVariableItemRichTextBoxControl
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
        private readonly ILiteralListDataParser _literalListDataParser;
        private readonly ILiteralListVariableElementInfoHelper _literalListVariableElementInfoHelper;
        private readonly IObjectListDataParser _objectListDataParser;
        private readonly IObjectListVariableElementInfoHelper _objectListVariableElementInfoHelper;
        private readonly ObjectRichTextBox _objectRichTextBox;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IUpdateObjectRichTextBoxXml _updateObjectRichTextBoxXml;
        private readonly IVariableObjectRichTextBoxEventsHelper _variableObjectRichTextBoxEventsHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IEditingControl editingControl;
        private readonly ObjectListVariableElementInfo listInfo;
        private Type? _assignedTo;

        public ListOfObjectsVariableItemRichTextBoxControl(
            IFieldControlCommandFactory fieldControlCommandFactory,
            IFieldControlHelperFactory fieldControlHelperFactory,
            IGetObjectRichTextBoxVisibleText getObjectRichTextBoxVisibleText,
            IImageListService imageListService,
            ILayoutFieldControlButtons layoutFieldControlButtons,
            ILiteralListDataParser literalListDataParser,
            ILiteralListVariableElementInfoHelper literalListVariableElementInfoHelper,
            IObjectListDataParser objectListDataParser,
            IObjectListVariableElementInfoHelper objectListVariableElementInfoHelper,
            ObjectRichTextBox objectRichTextBox,
            ITypeLoadHelper typeLoadHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditingControl editingControl,
            ObjectListVariableElementInfo listInfo)
        {
            InitializeComponent();
            _fieldControlCommandFactory = fieldControlCommandFactory;
            _getObjectRichTextBoxVisibleText = getObjectRichTextBoxVisibleText;
            _imageListService = imageListService;
            _layoutFieldControlButtons = layoutFieldControlButtons;
            _literalListDataParser = literalListDataParser;
            _literalListVariableElementInfoHelper = literalListVariableElementInfoHelper;
            _objectListDataParser = objectListDataParser;
            _objectListVariableElementInfoHelper = objectListVariableElementInfoHelper;
            _objectRichTextBox = objectRichTextBox;
            _typeLoadHelper = typeLoadHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.editingControl = editingControl;
            this.listInfo = listInfo;
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

        public ApplicationTypeInfo Application => editingControl.Application;

        public Type? AssignedTo
        {
            get
            {
                return _assignedTo ??= GetAssignedTo();

                Type? GetAssignedTo()
                {
                    _typeLoadHelper.TryGetSystemType(listInfo.ObjectType, Application, out Type? type);
                    return type;
                }
            }
        }

        public ObjectRichTextBox RichTextBox => _objectRichTextBox;

        public XmlElement? XmlElement { get; private set; }

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
                        XmlElement.OuterXml,//OuterXml element is <object />
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

        public LiteralListVariableElementInfo LiteralListElementInfo
        {
            get
            {
                XmlElement? childElement = XmlElement == null ? null : _xmlDocumentHelpers.GetSingleOrDefaultChildElement(XmlElement);
                if (childElement?.Name == XmlDataConstants.LITERALLISTELEMENT)
                {
                    return _literalListVariableElementInfoHelper.GetLiteralListElementInfo
                    (
                        _literalListDataParser.Parse(childElement)
                    );
                }

                return _literalListVariableElementInfoHelper.GetDefaultLiteralListElementInfo();
            }
        }

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

        public void DisableControls() => Enable(false);

        public void EnableControls() => Enable(true);

        public void ClearMessage() => editingControl.ClearMessage();

        public void HideControls() => ShowControls(false);

        public void InvokeChanged() => Changed?.Invoke(this, EventArgs.Empty);

        public void RequestDocumentUpdate() => editingControl.RequestDocumentUpdate();

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

        public void ShowControls() => ShowControls(true);

        public void Update(XmlElement xmlElement)
            => _updateObjectRichTextBoxXml.Update(xmlElement);

        public void UpdateXmlElement(string innerXml)
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.OBJECTELEMENT);
                    xmlTextWriter.WriteRaw(innerXml);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
            }

            XmlElement = _xmlDocumentHelpers.ToXmlElement(stringBuilder.ToString());
        }

        void IValueControl.Focus() => _objectRichTextBox.Select();

        private static void AddButtonClickCommand(RadButton radButton, IClickCommand command)
        {
            radButton.Click += (sender, args) => command.Execute();
        }

        private void Enable(bool enable)
        {
            //Enable displays the wrong color.
            _objectRichTextBox.Visible = enable;//With ReadOnly CommandButtons become enabled on enter.
            //The radPanelRichInputBox border remains visible so the appearance is about right.

            foreach (RadButton button in CommandButtons)
                button.Enabled = enable;
        }

        private void Initialize()
        {
            InitializeRichTextBox();
            InitializeButtons();
            ResetControl();

            AddButtonClickCommand(btnVariable, _fieldControlCommandFactory.GetEditObjectRichTextBoxVariableCommand(this));
            AddButtonClickCommand(btnFunction, _fieldControlCommandFactory.GetEditObjectRichTextBoxFunctionCommand(this));
            AddButtonClickCommand(btnConstructor, _fieldControlCommandFactory.GetEditObjectRichTextBoxConstructorCommand(this));
            AddButtonClickCommand(btnLiteralList, _fieldControlCommandFactory.GetEditVariableObjectRichTextBoxLiteralListCommand(this));
            AddButtonClickCommand(btnObjectList, _fieldControlCommandFactory.GetEditVariableObjectRichTextBoxObjectListCommand(this));

            _variableObjectRichTextBoxEventsHelper.Setup();
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

        private static void SetPanelBorderForeColor(RadPanel radPanel, Color color)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).ForeColor = color;

        private void ShowControls(bool show)
        {
            _objectRichTextBox.Visible = show;
            radPanelRichTextBox.Visible = show;
            foreach (RadButton button in CommandButtons)
                button.Visible = show;
        }
    }
}
