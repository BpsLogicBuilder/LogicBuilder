using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Components.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal partial class AutoCompleteRadDropDownList : UserControl, ITypeAutoCompleteTextControl
    {
        public AutoCompleteRadDropDownList()
        {
            InitializeComponent();
            Initialize();
        }

        private EventHandler? radButtonHelperClickHandler;

        public new event EventHandler? TextChanged;

        public new event EventHandler? Validated;

        public new event CancelEventHandler? Validating;

        public new event MouseEventHandler? MouseDown;

        public RadDropDownList RadDropDownList => radDropDownList1;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool Enabled
        {
            get => base.Enabled;
            set
            {
                radDropDownList1.Enabled = value;
                radButtonHelper.Enabled = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedText { get => radDropDownList1.SelectedText; set => radDropDownList1.SelectedText = value; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new string Text { get => radDropDownList1.Text; set => radDropDownList1.Text = value; }

        public void EnableAddUpdateGenericArguments(bool enable)
        {
            radButtonHelper.Enabled = enable;
        }

        public void ResetTypesList(IList<string>? items)
        {
            string existingText = radDropDownList1.Text;
            radDropDownList1.Items.Clear();
            if (items != null)
            {
                radDropDownList1.Items.AddRange(items);
                radDropDownList1.Text = new HashSet<string>(items).Contains(existingText) ? existingText : string.Empty;
            }
        }

        public void SetAddUpdateGenericArgumentsCommand(IClickCommand command)
        {
            radButtonHelperClickHandler = (sender, args) => command.Execute();
            AddClickCommands();
        }

        public void SetContextMenus(RadContextMenuManager radContextMenuManager, RadContextMenu radContextMenu)
        {
            radContextMenuManager.SetRadContextMenu(this, radContextMenu);
            radContextMenuManager.SetRadContextMenu(this.RadDropDownList, radContextMenu);
            radContextMenuManager.SetRadContextMenu(this.RadDropDownList.DropDownListElement.EditableElement.TextBox.TextBoxItem.TextBoxControl, radContextMenu);
            
            this.ContextMenuStrip = null;
            this.RadDropDownList.ContextMenuStrip = null;
            this.RadDropDownList.DropDownListElement.EditableElement.TextBox.TextBoxItem.TextBoxControl.ShortcutsEnabled = false;/*This prevents CTRL+V from working*/
        }

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

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private void AddClickCommands()
        {
            RemoveClickCommands();
            radButtonHelper.Click += radButtonHelperClickHandler;
        }

        private void Initialize()
        {
            radButtonHelper.TabStop = false;
            radButtonHelper.Image = Properties.Resources.more;
            radButtonHelper.ImageAlignment = ContentAlignment.MiddleCenter;

            this.Disposed += AutoCompleteRadDropDownList_Disposed;
            ControlsLayoutUtility.SetDropDownListPadding(RadDropDownList);
            this.RadDropDownList.DropDownListElement.EditableElement.TextBox.TextBoxItem.TextBoxControl.MouseDown += TextBoxControl_MouseDown;
            this.radButtonHelper.MouseDown += RadButtonHelper_MouseDown;
            radDropDownList1.Disposed += RadDropDownList1_Disposed;
            radDropDownList1.MouseDown += RadDropDownList1_MouseDown;
            radDropDownList1.TextChanged += RadDropDownList1_TextChanged;
            radDropDownList1.Validated += RadDropDownList1_Validated;
            radDropDownList1.Validating += RadDropDownList1_Validating;
            radDropDownList1.DropDownStyle = RadDropDownStyle.DropDown;
            radDropDownList1.DropDownListElement.UseDefaultDisabledPaint = false;
            radDropDownList1.AutoCompleteMode = AutoCompleteMode.None;
            radDropDownList1.DropDownListElement.AutoCompleteSuggest = new CustomAutoCompleteSuggestHelper(radDropDownList1.DropDownListElement);
            radDropDownList1.DropDownListElement.AutoCompleteSuggest.DropDownList.ListElement.VisualItemFormatting += RadDropDownList1_VisualListItemFormatting;

            ResetButtonPanel();
            CollapsePanelBorder(radPanelDropDownList);
            CollapsePanelBorder(radPanelButton);
        }

        private void RemoveClickCommands()
        {
            if (radButtonHelperClickHandler == null)
                return;

            radButtonHelper.Click -= radButtonHelperClickHandler;
        }

        private void RemoveEventHandlers()
        {
            this.RadDropDownList.DropDownListElement.EditableElement.TextBox.TextBoxItem.TextBoxControl.MouseDown -= TextBoxControl_MouseDown;
            this.radButtonHelper.MouseDown -= RadButtonHelper_MouseDown;
            radDropDownList1.MouseDown -= RadDropDownList1_MouseDown;
            radDropDownList1.TextChanged -= RadDropDownList1_TextChanged;
            radDropDownList1.Validated -= RadDropDownList1_Validated;
            radDropDownList1.Validating -= RadDropDownList1_Validating;
        }

        private void ResetButtonPanel()
        {
            ((ISupportInitialize)this.radPanelButton).BeginInit();
            this.radPanelButton.SuspendLayout();
            SuspendLayout();
            this.radPanelButton.Size = new Size(PerFontSizeConstants.CommandButtonWidth, this.radPanelButton.Height);
            ((ISupportInitialize)this.radPanelButton).EndInit();
            this.radPanelButton.ResumeLayout(false);
            this.ResumeLayout(true);
        }

        private void SetDropDownBorderForeColor(Color color)
            => ((BorderPrimitive)radDropDownList1.DropDownListElement.Children[0]).ForeColor = color;

        #region Event Handlers
        private void AutoCompleteRadDropDownList_Disposed(object? sender, EventArgs e)
        {
            RemoveEventHandlers();
            RemoveClickCommands();
        }

        private void TextBoxControl_MouseDown(object? sender, MouseEventArgs e)
        {
            MouseDown?.Invoke(this, e);
        }

        private void RadDropDownList1_Disposed(object? sender, EventArgs e)
        {
            radDropDownList1.Items.Clear();
        }

        private void RadButtonHelper_MouseDown(object? sender, MouseEventArgs e)
        {
            MouseDown?.Invoke(this, e);
        }

        private void RadDropDownList1_MouseDown(object? sender, MouseEventArgs e)
        {
            MouseDown?.Invoke(this, e);
        }

        private void RadDropDownList1_TextChanged(object? sender, EventArgs e)
        {
            if (radDropDownList1.Disposing
                || radDropDownList1.IsDisposed) 
                return;

            TextChanged?.Invoke(this, e);
        }

        private void RadDropDownList1_Validated(object? sender, EventArgs e)
        {
            Validated?.Invoke(this, e);
        }

        private void RadDropDownList1_Validating(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            Validating?.Invoke(this, e);
        }

        private void RadDropDownList1_VisualListItemFormatting(object sender, VisualItemFormattingEventArgs args)
        {
            args.VisualItem.TextWrap = false;
        } 
        #endregion Event Handlers
    }
}
