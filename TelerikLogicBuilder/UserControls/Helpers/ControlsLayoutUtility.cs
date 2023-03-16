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
        public static void LayoutApplicationGroupBox(Form applicationForm, RadPanel radPanelApplication, RadGroupBox radGroupBoxApplication, IApplicationDropDownList applicationDropDownList)
        {
            ((ISupportInitialize)radGroupBoxApplication).BeginInit();
            radGroupBoxApplication.SuspendLayout();
            ((ISupportInitialize)radPanelApplication).BeginInit();
            radPanelApplication.SuspendLayout();
            applicationForm.SuspendLayout();

            radPanelApplication.Size = new Size(radPanelApplication.Width, PerFontSizeConstants.ApplicationGroupBoxPanelHeight);
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
            //groupBoxItem.ResumeLayout(false);
            radScrollablePanelItem.PanelContainer.ResumeLayout(false);
            ((ISupportInitialize)radScrollablePanelItem).EndInit();
            radScrollablePanelItem.ResumeLayout(false);
            ((ISupportInitialize)radPanelTableParent).EndInit();
            radPanelTableParent.ResumeLayout(false);
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            groupBoxItem.ResumeLayout(true);
        }
    }
}
