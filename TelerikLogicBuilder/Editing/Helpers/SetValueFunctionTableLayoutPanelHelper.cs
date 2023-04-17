using ABIS.LogicBuilder.FlowBuilder.Constants;
using System;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Helpers
{
    internal class SetValueFunctionTableLayoutPanelHelper : ISetValueFunctionTableLayoutPanelHelper
    {
        public void SetUp(TableLayoutPanel tableLayoutPanel, RadPanel radPanelTableParent, bool isMultiLine)
        {
            tableLayoutPanel.ColumnStyles.Clear();
            tableLayoutPanel.RowStyles.Clear();

            float boundaryWidth = PerFontSizeConstants.BoundarySize;
            float singleLineHeight = PerFontSizeConstants.SingleLineHeight;
            float multiLineHeight = PerFontSizeConstants.MultiLineHeight;
            float separatorLineHeight = PerFontSizeConstants.SeparatorLineHeight;
            int rowCount = 6;//top + bottom + functionName row + separator + varibleAndValue row + separator;

            float totalHeight = (2 * boundaryWidth) + singleLineHeight + separatorLineHeight; //top + bottom + function name rows;
            totalHeight += isMultiLine ? multiLineHeight : singleLineHeight;
            totalHeight += separatorLineHeight;
            int totalTableLayoutHeight = (int)Math.Round(totalHeight);//totalHeight height should always be a whole number

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
            tableLayoutPanel.ColumnCount = 5;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 3F));//boundary column - 0
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30F));//image column - 1
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31F));//variable column - 2
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 63F));//entry column column - 3
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 3F));//boundary column - 4

            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Location = new Point(0, 0);
            tableLayoutPanel.Margin = new Padding(0);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = rowCount;

            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, boundaryWidth));//boundary row
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, singleLineHeight));//Constructor/Function Name
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, separatorLineHeight));

            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, isMultiLine ? multiLineHeight : singleLineHeight));//parameter
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, separatorLineHeight));

            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, boundaryWidth));//boundary row
            tableLayoutPanel.Size = new Size(851, totalTableLayoutHeight);
            tableLayoutPanel.TabIndex = 0;
        }
    }
}
