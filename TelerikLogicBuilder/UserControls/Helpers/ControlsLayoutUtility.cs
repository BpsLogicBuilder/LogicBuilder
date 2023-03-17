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
        public static void LayoutAddUpdateButtonPanel(RadPanel radPanel, TableLayoutPanel tableLayoutPanelAddUpdate, bool performLayout = true)
        {
            ((ISupportInitialize)radPanel).BeginInit();
            radPanel.SuspendLayout();
            tableLayoutPanelAddUpdate.SuspendLayout();

            tableLayoutPanelAddUpdate.Dock = DockStyle.Top;
            tableLayoutPanelAddUpdate.Size = new Size(tableLayoutPanelAddUpdate.Width, PerFontSizeConstants.BottomPanelHeight / 5);//Keeps this height the same as a button on ManagedListBoxControl and bottom panel group.
                                                                                                                                    //Button height on the bottom panel is 20 percent of the total.
            radPanel.Size = new Size(PerFontSizeConstants.OkCancelButtonPanelWidth, radPanel.Height);

            tableLayoutPanelAddUpdate.ResumeLayout(false);
            ((ISupportInitialize)radPanel).EndInit();
            radPanel.ResumeLayout(performLayout);
        }

        public static void LayoutAddUpdateItemGroupBox(Control groupBoxParent, RadGroupBox radGroupBox, bool performLayout = true)
        {
            ((ISupportInitialize)radGroupBox).BeginInit();
            radGroupBox.SuspendLayout();
            groupBoxParent.SuspendLayout();

            radGroupBox.Size = new Size(radGroupBox.Width, PerFontSizeConstants.AddUpdateItemGroupBoxHeight);
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

        public static void LayoutBottomPanel(RadPanel radPanelBottom, RadPanel radPanelMessages, RadPanel radPanelButtons, IDialogFormMessageControl dialogFormMessageControl, bool performLayout = true)
        {
            ((ISupportInitialize)radPanelMessages).BeginInit();
            radPanelMessages.SuspendLayout();
            ((ISupportInitialize)radPanelButtons).BeginInit();
            radPanelButtons.SuspendLayout();
            ((ISupportInitialize)radPanelBottom).BeginInit();
            radPanelBottom.SuspendLayout();

            dialogFormMessageControl.Dock = DockStyle.Fill;
            dialogFormMessageControl.Location = new Point(0, 0);
            radPanelMessages.Controls.Add((Control)dialogFormMessageControl);

            radPanelButtons.Size = new Size(PerFontSizeConstants.OkCancelButtonPanelWidth, radPanelButtons.Height);
            radPanelBottom.Size = new Size(radPanelBottom.Width, PerFontSizeConstants.BottomPanelHeight);

            ((ISupportInitialize)radPanelMessages).EndInit();
            radPanelMessages.ResumeLayout(false);
            ((ISupportInitialize)radPanelButtons).EndInit();
            radPanelButtons.ResumeLayout(false);
            ((ISupportInitialize)radPanelBottom).EndInit();
            radPanelBottom.ResumeLayout(performLayout);
        }

        public static void LayoutBottomPanel(RadPanel radPanelBottom, RadPanel radPanelMessages, RadPanel radPanelButtons, bool performLayout = true)
        {
            ((ISupportInitialize)radPanelMessages).BeginInit();
            radPanelMessages.SuspendLayout();
            ((ISupportInitialize)radPanelButtons).BeginInit();
            radPanelButtons.SuspendLayout();
            ((ISupportInitialize)radPanelBottom).BeginInit();
            radPanelBottom.SuspendLayout();

            radPanelButtons.Size = new Size(PerFontSizeConstants.OkCancelButtonPanelWidth, radPanelButtons.Height);
            radPanelBottom.Size = new Size(radPanelBottom.Width, PerFontSizeConstants.BottomPanelHeight);

            ((ISupportInitialize)radPanelMessages).EndInit();
            radPanelMessages.ResumeLayout(false);
            ((ISupportInitialize)radPanelButtons).EndInit();
            radPanelButtons.ResumeLayout(false);
            ((ISupportInitialize)radPanelBottom).EndInit();
            radPanelBottom.ResumeLayout(performLayout);
        }

        public static void LayoutControls(RadGroupBox groupBoxItem, RadScrollablePanel radScrollablePanelItem, RadPanel radPanelTableParent, TableLayoutPanel tableLayoutPanel, int controlsRowCount)
        {
            tableLayoutPanel.ColumnStyles.Clear();
            tableLayoutPanel.RowStyles.Clear();

            float boundaryWidth = PerFontSizeConstants.BoundarySize;
            float singleLineHeight = PerFontSizeConstants.SingleLineHeight;
            float separatorLineHeight = PerFontSizeConstants.SeparatorLineHeight;
            int rowCount = 2 + (controlsRowCount * 2);/*boundary rows plus control rows with separator rows*/
            float totalHeight = (2 * boundaryWidth) + (controlsRowCount * (singleLineHeight + separatorLineHeight));
            float size_Boundary = boundaryWidth / totalHeight * 100;
            float size_SingleLine = singleLineHeight / totalHeight * 100;
            float size_Separator = separatorLineHeight / totalHeight * 100;
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

            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, size_Boundary));//boundary row
            for (int i = 0; i < controlsRowCount; i++)
            {
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, size_SingleLine));
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, size_Separator));
            }
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, size_Boundary));//boundary row
            tableLayoutPanel.Size = new Size(851, totalTableLayoutHeight);

            ((ISupportInitialize)groupBoxItem).EndInit();
            radScrollablePanelItem.PanelContainer.ResumeLayout(false);
            ((ISupportInitialize)radScrollablePanelItem).EndInit();
            radScrollablePanelItem.ResumeLayout(false);
            ((ISupportInitialize)radPanelTableParent).EndInit();
            radPanelTableParent.ResumeLayout(false);
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            groupBoxItem.ResumeLayout(true);
        }

        public static void LayoutManagedListBoxEditButtons(RadPanel rightDockedPanel, RadPanel radPanelTableParent, bool performLayout = true)
        {
            ((ISupportInitialize)rightDockedPanel).BeginInit();
            rightDockedPanel.SuspendLayout();
            ((ISupportInitialize)radPanelTableParent).BeginInit();
            radPanelTableParent.SuspendLayout();

            radPanelTableParent.Size = new Size(PerFontSizeConstants.OkCancelButtonPanelWidth, PerFontSizeConstants.BottomPanelHeight);
            rightDockedPanel.Size = new Size(PerFontSizeConstants.OkCancelButtonPanelWidth, rightDockedPanel.Height);

            ((ISupportInitialize)radPanelTableParent).EndInit();
            radPanelTableParent.ResumeLayout(false);
            ((ISupportInitialize)rightDockedPanel).EndInit();
            rightDockedPanel.ResumeLayout(performLayout);
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
    }
}
