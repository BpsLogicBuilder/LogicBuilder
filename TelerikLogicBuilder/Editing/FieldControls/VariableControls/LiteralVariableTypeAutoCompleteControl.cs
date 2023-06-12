using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls
{
    internal partial class LiteralVariableTypeAutoCompleteControl : UserControl, ILiteralVariableTypeAutoCompleteControl
    {
        private readonly RadButton btnHelper;

        private readonly IImageListService _imageListService;
        private readonly ILayoutFieldControlButtons _layoutFieldControlButtons;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IDataGraphEditingControl dataGraphEditingControl;
        private readonly LiteralVariable literalVariable;
        private RadDropDownList radDropDownList;
        private bool modified;

        public LiteralVariableTypeAutoCompleteControl(
            IImageListService imageListService,
            ILayoutFieldControlButtons layoutFieldControlButtons,
            IRadDropDownListHelper radDropDownListHelper,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IDataGraphEditingControl dataGraphEditingControl,
            LiteralVariable literalVariable)
        {
            InitializeComponent();
            _imageListService = imageListService;
            _layoutFieldControlButtons = layoutFieldControlButtons;
            _radDropDownListHelper = radDropDownListHelper;
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.dataGraphEditingControl = dataGraphEditingControl;
            this.literalVariable = literalVariable;

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

        private readonly HelpProvider helpProvider = new();
        private readonly RadToolTip toolTip = new();

        #region ITypeAutoCompleteTextControl
        public new event EventHandler? TextChanged;
        public new event EventHandler? Validated;
        public new event CancelEventHandler? Validating;
        public new event MouseEventHandler? MouseDown;
        public new bool Enabled
        {
            get => base.Enabled;
            set
            {
                radDropDownList.Enabled = value;
                btnHelper.Enabled = value;
            }
        }
        public new string Text { get => radDropDownList.Text; set => radDropDownList.Text = value; }
        public string SelectedText { get => radDropDownList.SelectedText; set => radDropDownList.SelectedText = value; }
        #endregion ITypeAutoCompleteTextControl

        public bool IsEmpty => false;

        public string MixedXml
        {
            get
            {
                StringBuilder stringBuilder = new();
                using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateFragmentXmlWriter(stringBuilder))
                {
                    xmlTextWriter.WriteString(radDropDownList.Text);
                    xmlTextWriter.Flush();
                }

                return stringBuilder.ToString();
            }
        }

        public string VisibleText => radDropDownList.Text;

        public XmlElement? XmlElement => _xmlDocumentHelpers.ToXmlElement
        (
            _xmlDataHelper.BuildVariableValueXml(literalVariable, MixedXml)
        );

        public event EventHandler? Changed;

        #region ITypeAutoCompleteTextControl
        public void EnableAddUpdateGenericArguments(bool enable)
        {
            btnHelper.Enabled = enable;
        }

        public void ResetTypesList(IList<string>? items)
        {
            string existingText = radDropDownList.Text;
            radDropDownList.Items.Clear();
            if (items != null)
            {
                radDropDownList.Items.AddRange(items);
                radDropDownList.Text = new HashSet<string>(items).Contains(existingText) ? existingText : string.Empty;
            }
        }

        public void SetAddUpdateGenericArgumentsCommand(IClickCommand command)
        {
            btnHelper.Click += (sender, args) => command.Execute();
        }

        public void SetContextMenus(RadContextMenuManager radContextMenuManager, RadContextMenu radContextMenu)
        {
            radContextMenuManager.SetRadContextMenu(this, radContextMenu);
            radContextMenuManager.SetRadContextMenu(radDropDownList, radContextMenu);
            radContextMenuManager.SetRadContextMenu(radDropDownList.DropDownListElement.EditableElement.TextBox.TextBoxItem.TextBoxControl, radContextMenu);

            this.ContextMenuStrip = null;
            radDropDownList.ContextMenuStrip = null;
            radDropDownList.DropDownListElement.EditableElement.TextBox.TextBoxItem.TextBoxControl.ShortcutsEnabled = false;
        }
        #endregion ITypeAutoCompleteTextControl

        public void HideControls() => ShowControls(false);

        public void InvokeChanged() => Changed?.Invoke(this, EventArgs.Empty);

        public void RequestDocumentUpdate() => dataGraphEditingControl.RequestDocumentUpdate();

        public void ResetControl() => radDropDownList.Text = string.Empty;

        public void SetErrorBackColor()
            => SetDropDownBorderForeColor
            (
                ForeColorUtility.GetGroupBoxBorderErrorColor()
            );

        public void SetNormalBackColor()
            => SetDropDownBorderForeColor
            (
                ForeColorUtility.GetGroupBoxBorderColor(ThemeResolutionService.ApplicationThemeName)
            );

        public void SetToolTipHelp(string toolTipText)
        {
            helpProvider.SetHelpString(radPanelDropDownList, toolTipText);
            toolTip.SetToolTip(radPanelDropDownList, toolTipText);
        }

        public void ShowControls() => ShowControls(true);

        public void Update(XmlElement xmlElement)
        {
            radDropDownList.Text = xmlElement.InnerText;
            modified = false;
        }

        void IValueControl.Focus() => radDropDownList.Select();

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        [MemberNotNull(nameof(radDropDownList))]
        private void Initialize()
        {
            InitializeDropDownList();
            InitializeButton();

            radDropDownList.DropDownListElement.EditableElement.TextBox.TextBoxItem.TextBoxControl.MouseDown += TextBoxControl_MouseDown;
            btnHelper.MouseDown += BtnHelper_MouseDown;
            radDropDownList.Disposed += _radDropDownListHelper.DisposedHandler;
            radDropDownList.MouseDown += RadDropDownList_MouseDown;
            radDropDownList.TextChanged += RadDropDownList_TextChanged;
            radDropDownList.Validated += RadDropDownList_Validated;
            radDropDownList.Validating += RadDropDownList_Validating;
            radDropDownList.DropDownListElement.UseDefaultDisabledPaint = false;
            ControlsLayoutUtility.SetDropDownListPadding(radDropDownList);

            CollapsePanelBorder(radPanelDropDownList);
            CollapsePanelBorder(radPanelButton);

            _radDropDownListHelper.SetAutoCompleteSuggestHelper(radDropDownList);
            radDropDownList.DropDownListElement.AutoCompleteSuggest.DropDownList.ListElement.VisualItemFormatting += RadDropDownList_VisualListItemFormatting;
        }

        private void InitializeButton()
            => _layoutFieldControlButtons.Layout(radPanelButton, new RadButton[] { btnHelper });

        [MemberNotNull(nameof(radDropDownList))]
        private void InitializeDropDownList()
        {
            this.radDropDownList = new RadDropDownList();

            ((ISupportInitialize)this.radPanelDropDownList).BeginInit();
            this.radPanelDropDownList.SuspendLayout();
            this.radPanelDropDownList.Padding = new Padding(0, 1, 3, 0);

            ((ISupportInitialize)this.radDropDownList).BeginInit();
            this.radDropDownList.Dock = DockStyle.Fill;
            this.radDropDownList.AutoSize = false;
            this.radDropDownList.DropDownAnimationEnabled = true;
            this.radDropDownList.Location = new Point(0, 0);
            this.radDropDownList.Name = "radDropDownList";
            this.radDropDownList.Size = new Size(350, 28);
            this.radDropDownList.TabIndex = 0;
            ((ISupportInitialize)this.radDropDownList).EndInit();

            this.radPanelDropDownList.Controls.Add(radDropDownList);
            ((ISupportInitialize)this.radPanelDropDownList).EndInit();
            this.radPanelDropDownList.ResumeLayout(true);
        }

        private void SetDropDownBorderForeColor(Color color)
            => ((BorderPrimitive)radDropDownList.DropDownListElement.Children[0]).ForeColor = color;

        private void ShowControls(bool show)
        {
            radDropDownList.Visible = show;
            btnHelper.Visible = show;
        }

        #region Event Handlers
        private void BtnHelper_MouseDown(object? sender, MouseEventArgs e)
        {
            MouseDown?.Invoke(this, e);
        }

        private void RadDropDownList_MouseDown(object? sender, MouseEventArgs e)
        {
            MouseDown?.Invoke(this, e);
        }

        private void RadDropDownList_TextChanged(object? sender, EventArgs e)
        {
            modified = true;
            Changed?.Invoke(this, e);
            TextChanged?.Invoke(this, e);
        }

        private void RadDropDownList_Validated(object? sender, EventArgs e)
        {
            Validated?.Invoke(this, e);
            if (!modified)
                return;

            modified = false;
            RequestDocumentUpdate();
        }

        private void RadDropDownList_Validating(object? sender, CancelEventArgs e)
        {
            Validating?.Invoke(this, e);
        }

        private void RadDropDownList_VisualListItemFormatting(object sender, VisualItemFormattingEventArgs args)
        {
            args.VisualItem.TextWrap = false;
        }

        private void TextBoxControl_MouseDown(object? sender, MouseEventArgs e)
        {
            MouseDown?.Invoke(this, e);
        }
        #endregion Event Handlers
    }
}
