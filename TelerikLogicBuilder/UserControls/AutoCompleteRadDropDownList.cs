using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Components.Helpers;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
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

        public new event EventHandler? TextChanged;

        public new event MouseEventHandler? MouseDown;

        public RadDropDownList RadDropDownList => radDropDownList1;

        public string SelectedText { get => radDropDownList1.SelectedText; set => radDropDownList1.SelectedText = value; }

        public new string Text { get => radDropDownList1.Text; set => radDropDownList1.Text = value; }

        public void EnableAddUpdateGenericArguments(bool enable)
        {
            radButtonHelper.Enabled = enable;
        }

        public void ResetTypesList(IList<string>? items)
        {
            radDropDownList1.Items.Clear();
            if (items != null)
                radDropDownList1.Items.AddRange(items);
        }

        public void SetAddUpdateGenericArgumentsCommand(IClickCommand command)
        {
            radButtonHelper.Click += (sender, args) => command.Execute();
        }

        public void SetContextMenus(RadContextMenuManager radContextMenuManager, RadContextMenu radContextMenu)
        {
            radContextMenuManager.SetRadContextMenu(this, radContextMenu);
            radContextMenuManager.SetRadContextMenu(this.RadDropDownList, radContextMenu);
            radContextMenuManager.SetRadContextMenu(this.RadDropDownList.DropDownListElement.EditableElement.TextBox.TextBoxItem.TextBoxControl, radContextMenu);
            
            this.ContextMenuStrip = null;
            this.RadDropDownList.ContextMenuStrip = null;
            this.RadDropDownList.DropDownListElement.EditableElement.TextBox.TextBoxItem.TextBoxControl.ShortcutsEnabled = false;
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            radButtonHelper.Image = Properties.Resources.more;
            radButtonHelper.ImageAlignment = ContentAlignment.MiddleCenter;

            this.RadDropDownList.DropDownListElement.EditableElement.TextBox.TextBoxItem.TextBoxControl.MouseDown += TextBoxControl_MouseDown;
            this.radButtonHelper.MouseDown += RadButtonHelper_MouseDown;
            radDropDownList1.MouseDown += RadDropDownList1_MouseDown;
            radDropDownList1.TextChanged += RadDropDownList1_TextChanged;
            radDropDownList1.DropDownStyle = RadDropDownStyle.DropDown;
            radDropDownList1.AutoCompleteMode = AutoCompleteMode.None;
            radDropDownList1.DropDownListElement.AutoCompleteSuggest = new CustomAutoCompleteSuggestHelper(radDropDownList1.DropDownListElement);

            CollapsePanelBorder(radPanelDropDownList);
            CollapsePanelBorder(radPanelButton);
        }

        private void TextBoxControl_MouseDown(object? sender, MouseEventArgs e)
        {
            MouseDown?.Invoke(this, e);
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
            TextChanged?.Invoke(this, e);
        }
    }
}
