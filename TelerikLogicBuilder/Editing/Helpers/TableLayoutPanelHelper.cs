using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Helpers
{
    internal class TableLayoutPanelHelper : ITableLayoutPanelHelper
    {
        public void SetUp(TableLayoutPanel tableLayoutPanel, RadPanel radPanelTableParent, IList<ParameterBase> parameters, bool hasGenericArguments)
        {
            tableLayoutPanel.ColumnStyles.Clear();
            tableLayoutPanel.RowStyles.Clear();

            float boundaryWidth = PerFontSizeConstants.BoundarySize;
            float singleLineHeight = PerFontSizeConstants.SingleLineHeight;
            float multiLineHeight = PerFontSizeConstants.MultiLineHeight;
            float separatorLineHeight = PerFontSizeConstants.SeparatorLineHeight;
            int rowCount = 4;//top + bottom + constructorName rows;

            foreach (ParameterBase parameter in parameters)
                rowCount += 2;

            if (hasGenericArguments)
            {
                rowCount += 2;
            }

            float totalHeight = (2 * boundaryWidth) + singleLineHeight + separatorLineHeight; //top + bottom + constructor/function name rows;
            foreach (ParameterBase parameter in parameters)
            {
                totalHeight += parameter is LiteralParameter literalParameter && literalParameter.Control == Enums.LiteralParameterInputStyle.MultipleLineTextBox
                            ? multiLineHeight
                            : singleLineHeight;

                totalHeight += separatorLineHeight;
            }

            if (hasGenericArguments)
            {
                totalHeight += singleLineHeight;
                totalHeight += separatorLineHeight;
            }

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
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31F));//parameter label column - 2
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 63F));//entry column column - 3
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 3F));//boundary column - 4

            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Location = new Point(0, 0);
            tableLayoutPanel.Margin = new Padding(0);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = rowCount;

            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, boundaryWidth));//boundary row
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, singleLineHeight));//Constructor Name
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, separatorLineHeight));

            if (hasGenericArguments)
            {
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, singleLineHeight));//Generic Arguments
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, separatorLineHeight));
            }

            foreach (ParameterBase parameter in parameters)
            {
                float size = parameter is LiteralParameter literalParameter && literalParameter.Control == Enums.LiteralParameterInputStyle.MultipleLineTextBox
                            ? multiLineHeight
                            : singleLineHeight;

                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, size));//parameter
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, separatorLineHeight));
            }

            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, boundaryWidth));//boundary row
            tableLayoutPanel.Size = new Size(851, totalTableLayoutHeight);
            tableLayoutPanel.TabIndex = 0;
        }
    }
}
