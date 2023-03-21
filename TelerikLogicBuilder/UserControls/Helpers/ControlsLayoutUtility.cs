using ABIS.LogicBuilder.FlowBuilder.Constants;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers
{
    internal static class ControlsLayoutUtility
    {
        public static void LayoutAddUpdateButtonPanel(RadPanel radPanelAddButton, TableLayoutPanel tableLayoutPanelAddUpdate, bool performLayout = true)
        {
            ((ISupportInitialize)radPanelAddButton).BeginInit();
            radPanelAddButton.SuspendLayout();
            tableLayoutPanelAddUpdate.SuspendLayout();

            tableLayoutPanelAddUpdate.Dock = DockStyle.Top;
            tableLayoutPanelAddUpdate.Size = new Size(tableLayoutPanelAddUpdate.Width, (int)PerFontSizeConstants.SingleLineHeight);
            radPanelAddButton.Size = new Size(PerFontSizeConstants.OkCancelButtonPanelWidth, radPanelAddButton.Height);

            tableLayoutPanelAddUpdate.ResumeLayout(false);
            ((ISupportInitialize)radPanelAddButton).EndInit();
            radPanelAddButton.ResumeLayout(performLayout);
        }

        public static void LayoutAddUpdateItemGroupBox(Control groupBoxParent, RadGroupBox radGroupBox, bool performLayout = true)
        {
            ((ISupportInitialize)radGroupBox).BeginInit();
            radGroupBox.SuspendLayout();
            groupBoxParent.SuspendLayout();

            radGroupBox.Size = new Size(radGroupBox.Width, PerFontSizeConstants.ApplicationGroupBoxHeight);
            radGroupBox.Margin = new Padding(0);
            radGroupBox.Padding = PerFontSizeConstants.AddUpdateItemGroupBoxPadding;

            ((ISupportInitialize)radGroupBox).EndInit();
            radGroupBox.ResumeLayout(false);
            groupBoxParent.ResumeLayout(performLayout);
        }

        public static void LayoutApplicationGroupBox(Form applicationForm, RadPanel radPanelApplication, RadGroupBox radGroupBoxApplication, IApplicationDropDownList applicationDropDownList)
        {
            ((ISupportInitialize)radGroupBoxApplication).BeginInit();
            radGroupBoxApplication.SuspendLayout();
            ((ISupportInitialize)radPanelApplication).BeginInit();
            radPanelApplication.SuspendLayout();
            applicationForm.SuspendLayout();

            radPanelApplication.Size = new Size(radPanelApplication.Width, PerFontSizeConstants.ApplicationGroupBoxHeight);
            radGroupBoxApplication.Margin = new Padding(0);
            radGroupBoxApplication.Padding = PerFontSizeConstants.ApplicationGroupBoxPadding;

            applicationDropDownList.AutoSize = false;
            applicationDropDownList.Margin = new Padding(0);
            applicationDropDownList.Dock = DockStyle.Fill;
            applicationDropDownList.Location = new Point(0, 0);
            radGroupBoxApplication.Controls.Add((Control)applicationDropDownList);

            ((ISupportInitialize)radGroupBoxApplication).EndInit();
            radGroupBoxApplication.ResumeLayout(false);
            ((ISupportInitialize)radPanelApplication).EndInit();
            radPanelApplication.ResumeLayout(false);
            applicationForm.ResumeLayout(true);
        }

        public static void LayoutApplicationPanel(RadPanel radPanelApplication, IApplicationDropDownList applicationDropDownList)
        {
            ((ISupportInitialize)radPanelApplication).BeginInit();
            radPanelApplication.SuspendLayout();

            applicationDropDownList.AutoSize = false;
            applicationDropDownList.Margin = new Padding(0);
            applicationDropDownList.Dock = DockStyle.Fill;
            applicationDropDownList.Location = new Point(0, 0);
            radPanelApplication.Controls.Add((Control)applicationDropDownList);

            ((ISupportInitialize)radPanelApplication).EndInit();
            radPanelApplication.ResumeLayout(true);
        }

        public static void LayoutBottomPanel(RadPanel radPanelBottom, RadPanel radPanelMessages, RadPanel radPanelButtons, TableLayoutPanel tableLayoutPanelButtons, IDialogFormMessageControl dialogFormMessageControl, bool performLayout = true)
        {
            ((ISupportInitialize)radPanelMessages).BeginInit();
            radPanelMessages.SuspendLayout();
            ((ISupportInitialize)radPanelButtons).BeginInit();
            radPanelButtons.SuspendLayout();
            tableLayoutPanelButtons.SuspendLayout();
            ((ISupportInitialize)radPanelBottom).BeginInit();
            radPanelBottom.SuspendLayout();

            float separatorLineHeight = PerFontSizeConstants.SeparatorLineHeight;
            float singleLineHeight = PerFontSizeConstants.SingleLineHeight;
            float totalHeight = (5 * separatorLineHeight) + (4 * singleLineHeight);
            int totalTableLayoutHeight = (int)Math.Round(totalHeight);

            tableLayoutPanelButtons.ColumnStyles.Clear();
            tableLayoutPanelButtons.RowStyles.Clear();

            tableLayoutPanelButtons.ColumnCount = 3;
            tableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));

            tableLayoutPanelButtons.RowCount = 9;
            tableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, separatorLineHeight));
            tableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, singleLineHeight));
            tableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, separatorLineHeight));
            tableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, singleLineHeight));
            tableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, separatorLineHeight));
            tableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, singleLineHeight));
            tableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, separatorLineHeight));
            tableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, singleLineHeight));
            tableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, separatorLineHeight));

            dialogFormMessageControl.Dock = DockStyle.Fill;
            dialogFormMessageControl.Location = new Point(0, 0);
            radPanelMessages.Controls.Add((Control)dialogFormMessageControl);

            tableLayoutPanelButtons.Margin = new Padding(0);
            radPanelButtons.Margin = new Padding(0);
            radPanelButtons.Size = new Size(PerFontSizeConstants.OkCancelButtonPanelWidth, totalTableLayoutHeight);
            radPanelBottom.Size = new Size(radPanelBottom.Width, totalTableLayoutHeight);

            ((ISupportInitialize)radPanelMessages).EndInit();
            radPanelMessages.ResumeLayout(false);
            tableLayoutPanelButtons.ResumeLayout(false);
            ((ISupportInitialize)radPanelButtons).EndInit();
            radPanelButtons.ResumeLayout(false);
            ((ISupportInitialize)radPanelBottom).EndInit();
            radPanelBottom.ResumeLayout(performLayout);
        }

        public static void LayoutBottomPanel(RadPanel radPanelBottom, RadPanel radPanelMessages, RadPanel radPanelButtons, TableLayoutPanel tableLayoutPanelButtons, bool performLayout = true)
        {
            ((ISupportInitialize)radPanelMessages).BeginInit();
            radPanelMessages.SuspendLayout();
            ((ISupportInitialize)radPanelButtons).BeginInit();
            radPanelButtons.SuspendLayout();
            tableLayoutPanelButtons.SuspendLayout();
            ((ISupportInitialize)radPanelBottom).BeginInit();
            radPanelBottom.SuspendLayout();

            float separatorLineHeight = PerFontSizeConstants.SeparatorLineHeight;
            float singleLineHeight = PerFontSizeConstants.SingleLineHeight;
            float totalHeight = (5 * separatorLineHeight) + (4 * singleLineHeight);
            int totalTableLayoutHeight = (int)Math.Round(totalHeight);

            tableLayoutPanelButtons.ColumnStyles.Clear();
            tableLayoutPanelButtons.RowStyles.Clear();

            tableLayoutPanelButtons.ColumnCount = 3;
            tableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));

            tableLayoutPanelButtons.RowCount = 9;
            tableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, separatorLineHeight));
            tableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, singleLineHeight));
            tableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, separatorLineHeight));
            tableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, singleLineHeight));
            tableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, separatorLineHeight));
            tableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, singleLineHeight));
            tableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, separatorLineHeight));
            tableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, singleLineHeight));
            tableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, separatorLineHeight));

            tableLayoutPanelButtons.Margin = new Padding(0);
            radPanelButtons.Margin = new Padding(0);
            radPanelButtons.Size = new Size(PerFontSizeConstants.OkCancelButtonPanelWidth, totalTableLayoutHeight);
            radPanelBottom.Size = new Size(radPanelBottom.Width, totalTableLayoutHeight);

            ((ISupportInitialize)radPanelMessages).EndInit();
            radPanelMessages.ResumeLayout(false);
            tableLayoutPanelButtons.ResumeLayout(false);
            ((ISupportInitialize)radPanelButtons).EndInit();
            radPanelButtons.ResumeLayout(false);
            ((ISupportInitialize)radPanelBottom).EndInit();
            radPanelBottom.ResumeLayout(performLayout);
        }

        public static void LayoutControls(RadGroupBox groupBoxItem, RadScrollablePanel radScrollablePanelItem, RadPanel radPanelTableParent, TableLayoutPanel tableLayoutPanel, int controlsRowCount, bool performLayout = true)
        {
            tableLayoutPanel.ColumnStyles.Clear();
            tableLayoutPanel.RowStyles.Clear();

            float boundaryWidth = PerFontSizeConstants.BoundarySize;
            float singleLineHeight = PerFontSizeConstants.SingleLineHeight;
            float separatorLineHeight = PerFontSizeConstants.SeparatorLineHeight;
            int rowCount = 2 + (controlsRowCount * 2);/*boundary rows plus control rows with separator rows*/
            float totalHeight = (2 * boundaryWidth) + (controlsRowCount * (singleLineHeight + separatorLineHeight));
            int totalTableLayoutHeight = (int)Math.Round(totalHeight);//totalHeight height should always be a whole number

            ((ISupportInitialize)groupBoxItem).BeginInit();
            groupBoxItem.SuspendLayout();
            ((ISupportInitialize)radScrollablePanelItem).BeginInit();
            radScrollablePanelItem.PanelContainer.SuspendLayout();
            radScrollablePanelItem.SuspendLayout();
            ((ISupportInitialize)radPanelTableParent).BeginInit();
            radPanelTableParent.SuspendLayout();
            tableLayoutPanel.SuspendLayout();

            groupBoxItem.Padding = PerFontSizeConstants.GroupBoxPadding;

            // 
            // radPanelTableParent
            // 
            radPanelTableParent.Controls.Add(tableLayoutPanel);
            radPanelTableParent.Dock = DockStyle.Top;
            radPanelTableParent.Location = new Point(0, 0);
            radPanelTableParent.Margin = new Padding(0);
            radPanelTableParent.Name = "radPanelTableParent";
            radPanelTableParent.Size = new Size(851, totalTableLayoutHeight);
            radPanelTableParent.TabIndex = 0;

            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 4;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 3F));//boundary column - 0
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31F));//parameter label column - 1
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 63F));//entry column column - 2
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 3F));//boundary column - 3

            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Location = new Point(0, 0);
            tableLayoutPanel.Margin = new Padding(0);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = rowCount;

            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, boundaryWidth));//boundary row
            for (int i = 0; i < controlsRowCount; i++)
            {
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, singleLineHeight));
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, separatorLineHeight));
            }
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, boundaryWidth));//boundary row
            tableLayoutPanel.Size = new Size(851, totalTableLayoutHeight);

            ((ISupportInitialize)groupBoxItem).EndInit();
            radScrollablePanelItem.PanelContainer.ResumeLayout(false);
            ((ISupportInitialize)radScrollablePanelItem).EndInit();
            radScrollablePanelItem.ResumeLayout(false);
            ((ISupportInitialize)radPanelTableParent).EndInit();
            radPanelTableParent.ResumeLayout(false);
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            groupBoxItem.ResumeLayout(performLayout);
        }

        public static void LayoutListItemGroupBox(Control groupBoxParent, RadGroupBox radGroupBoxEdit, RadPanel radPanelEdit, RadPanel radPanelEditControl, Control control, bool multiLine, bool performLayout = true)
        {
            ((ISupportInitialize)radPanelEdit).BeginInit();
            radPanelEdit.SuspendLayout();
            ((ISupportInitialize)radPanelEditControl).BeginInit();
            radPanelEditControl.SuspendLayout();
            ((ISupportInitialize)radGroupBoxEdit).BeginInit();
            radGroupBoxEdit.SuspendLayout();
            groupBoxParent.SuspendLayout();

            radPanelEditControl.Margin = new Padding(0);
            radPanelEditControl.Padding = new Padding(0);
            radPanelEditControl.Dock = DockStyle.Top;
            radPanelEditControl.Size = new Size(radPanelEditControl.Width, multiLine ? (int)PerFontSizeConstants.MultiLineHeight : (int)PerFontSizeConstants.SingleLineHeight);
            radPanelEdit.Margin = new Padding(0);
            radPanelEdit.Padding = new Padding(0);
            radPanelEdit.Dock = DockStyle.Fill;
            radGroupBoxEdit.Size = new Size(radGroupBoxEdit.Width, multiLine ? PerFontSizeConstants.MultiLineAddUpdateItemGroupBoxHeight : PerFontSizeConstants.ApplicationGroupBoxHeight);
            radGroupBoxEdit.Padding = PerFontSizeConstants.AddUpdateItemGroupBoxPadding;

            control.Name = "valueControl";
            control.Dock = DockStyle.Fill;
            control.Margin = new Padding(0);
            control.Location = new Point(0, 0);

            radPanelEditControl.Controls.Add(control);
            ((ISupportInitialize)radPanelEditControl).EndInit();
            radPanelEditControl.ResumeLayout(false);
            ((ISupportInitialize)radPanelEdit).EndInit();
            radPanelEdit.ResumeLayout(false);
            ((ISupportInitialize)radGroupBoxEdit).EndInit();
            radGroupBoxEdit.ResumeLayout(false);
            groupBoxParent.ResumeLayout(performLayout);
        }

        public static void LayoutGroupBox(Control groupBoxParent, RadGroupBox radGroupBox, bool performLayout = true)
        {
            ((ISupportInitialize)radGroupBox).BeginInit();
            radGroupBox.SuspendLayout();
            groupBoxParent.SuspendLayout();

            radGroupBox.Margin = new Padding(0);
            radGroupBox.Padding = PerFontSizeConstants.GroupBoxPadding;

            ((ISupportInitialize)radGroupBox).EndInit();
            radGroupBox.ResumeLayout(false);
            groupBoxParent.ResumeLayout(performLayout);
        }

        public static void LayoutManagedListBoxEditButtons(RadPanel rightDockedPanel, RadPanel radPanelTableParent, TableLayoutPanel tableLayoutPanel, bool performLayout = true)
        {
            ((ISupportInitialize)rightDockedPanel).BeginInit();
            rightDockedPanel.SuspendLayout();
            ((ISupportInitialize)radPanelTableParent).BeginInit();
            radPanelTableParent.SuspendLayout();
            tableLayoutPanel.SuspendLayout();

            tableLayoutPanel.ColumnStyles.Clear();
            tableLayoutPanel.RowStyles.Clear();

            tableLayoutPanel.ColumnCount = 3;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));

            float separatorLineHeight = PerFontSizeConstants.SeparatorLineHeight;
            float singleLineHeight = PerFontSizeConstants.SingleLineHeight;
            float totalHeight = (5 * separatorLineHeight) + (4 * singleLineHeight);
            int totalTableLayoutHeight = (int)Math.Round(totalHeight);
            tableLayoutPanel.RowCount = 9;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, separatorLineHeight));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, singleLineHeight));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, separatorLineHeight));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, singleLineHeight));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, separatorLineHeight));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, singleLineHeight));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, separatorLineHeight));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, singleLineHeight));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, separatorLineHeight));

            tableLayoutPanel.Margin = new Padding(0);
            radPanelTableParent.Margin = new Padding(0);
            radPanelTableParent.Size = new Size(PerFontSizeConstants.OkCancelButtonPanelWidth, totalTableLayoutHeight);
            rightDockedPanel.Size = new Size(PerFontSizeConstants.OkCancelButtonPanelWidth, rightDockedPanel.Height);

            tableLayoutPanel.ResumeLayout(false);
            ((ISupportInitialize)radPanelTableParent).EndInit();
            radPanelTableParent.ResumeLayout(false);
            ((ISupportInitialize)rightDockedPanel).EndInit();
            rightDockedPanel.ResumeLayout(performLayout);
        }

        public static void LayoutSelectConfiguredItemGroupBox(Form editForm, RadPanel radPanelConfiguredItem, RadGroupBox radGroupBoxConfiguredItem, HelperButtonDropDownList helperButtonDropDownList)
        {
            ((ISupportInitialize)radGroupBoxConfiguredItem).BeginInit();
            radGroupBoxConfiguredItem.SuspendLayout();
            ((ISupportInitialize)radPanelConfiguredItem).BeginInit();
            radPanelConfiguredItem.SuspendLayout();
            editForm.SuspendLayout();

            radPanelConfiguredItem.Size = new Size(radPanelConfiguredItem.Width, PerFontSizeConstants.ApplicationGroupBoxHeight);
            radGroupBoxConfiguredItem.Margin = new Padding(0);
            radGroupBoxConfiguredItem.Padding = PerFontSizeConstants.SelectConfiguredItemGroupBoxPadding;

            helperButtonDropDownList.AutoSize = false;
            helperButtonDropDownList.Margin = new Padding(0);
            helperButtonDropDownList.Dock = DockStyle.Fill;
            helperButtonDropDownList.Location = new Point(0, 0);
            radGroupBoxConfiguredItem.Controls.Add(helperButtonDropDownList);

            ((ISupportInitialize)radGroupBoxConfiguredItem).EndInit();
            radGroupBoxConfiguredItem.ResumeLayout(false);
            ((ISupportInitialize)radPanelConfiguredItem).EndInit();
            radPanelConfiguredItem.ResumeLayout(false);
            editForm.ResumeLayout(true);
        }

        public static void LayoutSingleRowGroupBox(RadPanel radPanelGroupBoxParent, RadGroupBox radGroupBox, bool performLayout = true)
        {
            ((ISupportInitialize)radGroupBox).BeginInit();
            radGroupBox.SuspendLayout();
            radPanelGroupBoxParent.SuspendLayout();

            radPanelGroupBoxParent.Size = new Size(radGroupBox.Width, PerFontSizeConstants.SingleRowGroupBoxHeight);
            radGroupBox.Size = new Size(radGroupBox.Width, PerFontSizeConstants.SingleRowGroupBoxHeight);
            radGroupBox.Margin = new Padding(0);
            radGroupBox.Padding = PerFontSizeConstants.SingleRowGroupBoxPadding;

            ((ISupportInitialize)radGroupBox).EndInit();
            radGroupBox.ResumeLayout(false);
            radPanelGroupBoxParent.ResumeLayout(performLayout);
        }

        public static void LayoutTwoRowGroupBox(Control groupBoxParent, RadGroupBox radGroupBox, bool performLayout = true)
        {
            ((ISupportInitialize)radGroupBox).BeginInit();
            radGroupBox.SuspendLayout();
            groupBoxParent.SuspendLayout();

            radGroupBox.Margin = new Padding(0);
            radGroupBox.Padding = PerFontSizeConstants.GroupBoxPadding;
            radGroupBox.Size = new Size(radGroupBox.Width, PerFontSizeConstants.TwoRowGroupBoxHeight);

            ((ISupportInitialize)radGroupBox).EndInit();
            radGroupBox.ResumeLayout(false);
            groupBoxParent.ResumeLayout(performLayout);
        }
    }
}
