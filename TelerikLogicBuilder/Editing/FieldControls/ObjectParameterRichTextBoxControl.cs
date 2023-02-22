using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
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

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls
{
    internal partial class ObjectParameterRichTextBoxControl : UserControl, IObjectParameterRichTextBoxControl
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
        private readonly ObjectRichTextBox _objectRichTextBox;
        private readonly IObjectRichTextBoxEventsHelper _objectRichTextBoxEventsHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IUpdateObjectRichTextBoxXml _updateObjectRichTextBoxXml;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IEditingControl editingControl;
        private readonly ObjectParameter objectParameter;
        private Type? _assignedTo;

        public ObjectParameterRichTextBoxControl(
            IFieldControlCommandFactory fieldControlCommandFactory,
            IFieldControlHelperFactory fieldControlHelperFactory,
            IGetObjectRichTextBoxVisibleText getObjectRichTextBoxVisibleText,
            IImageListService imageListService,
            ILayoutFieldControlButtons layoutFieldControlButtons,
            ObjectRichTextBox objectRichTextBox,
            ITypeLoadHelper typeLoadHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditingControl editingControl,
            ObjectParameter objectParameter)
        {
            InitializeComponent();
            _fieldControlCommandFactory = fieldControlCommandFactory;
            _getObjectRichTextBoxVisibleText = getObjectRichTextBoxVisibleText;
            _imageListService = imageListService;
            _layoutFieldControlButtons = layoutFieldControlButtons;
            _objectRichTextBox = objectRichTextBox;
            _typeLoadHelper = typeLoadHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.editingControl = editingControl;
            this.objectParameter = objectParameter;
            _objectRichTextBoxEventsHelper = fieldControlHelperFactory.GetObjectRichTextBoxEventsHelper(this);
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
                    _typeLoadHelper.TryGetSystemType(objectParameter, Application, out Type? type);
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
                        XmlElement.OuterXml,//OuterXml element is objectParameter
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

        public event EventHandler? Changed;

        public void HideControls() => ShowControls(false);

        public void InvokeChanged() => Changed?.Invoke(this, EventArgs.Empty);

        public void RequestDocumentUpdate() => editingControl.RequestDocumentUpdate();

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
                xmlTextWriter.WriteStartElement(XmlDataConstants.OBJECTPARAMETERELEMENT);
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, objectParameter.Name);
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

        private void Initialize()
        {
            InitializeRichTextBox();
            InitializeButtons();
            ResetControl();

            AddButtonClickCommand(btnVariable, _fieldControlCommandFactory.GetEditObjectRichTextBoxVariableCommand(this));
            AddButtonClickCommand(btnFunction, _fieldControlCommandFactory.GetEditObjectRichTextBoxFunctionCommand(this));
            AddButtonClickCommand(btnConstructor, _fieldControlCommandFactory.GetEditObjectRichTextBoxConstructorCommand(this));
            AddButtonClickCommand(btnLiteralList, _fieldControlCommandFactory.GetEditObjectRichTextBoxLiteralListCommand(this));
            AddButtonClickCommand(btnObjectList, _fieldControlCommandFactory.GetEditObjectRichTextBoxObjectListCommand(this));

            _objectRichTextBoxEventsHelper.Setup();
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
            this.radPanelRichTextBox.Padding = new Padding(1);//shows the panel border instead
            _objectRichTextBox.Margin = new Padding(0);
            _objectRichTextBox.Location = new Point(0, 0);
            _objectRichTextBox.DetectUrls = false;
            _objectRichTextBox.HideSelection = false;
            _objectRichTextBox.Multiline = false;

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
