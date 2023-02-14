using ABIS.LogicBuilder.FlowBuilder.Enums;
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
            HashSet<LiteralParameterInputStyle> implemented = new()
            {
                LiteralParameterInputStyle.DomainAutoComplete,
                LiteralParameterInputStyle.DropDown,
                LiteralParameterInputStyle.MultipleLineTextBox,
                LiteralParameterInputStyle.SingleLineTextBox,
                LiteralParameterInputStyle.TypeAutoComplete
            };
            tableLayoutPanel.ColumnStyles.Clear();
            tableLayoutPanel.RowStyles.Clear();

            const float boundaryWidth = 20F;
            const float singleLineHeight = 33F;
            const float multiLineHeight = 100F;
            const float separatorLineHeight = 3F;
            int rowCount = 4;//top + bottom + constructorName rows;

            foreach (ParameterBase parameter in parameters)
            {
                if (parameter is not LiteralParameter literalParameter 
                    || !implemented.Contains(literalParameter.Control))
                    continue;

                rowCount += 2;
            }

            if (hasGenericArguments)
            {
                rowCount += 2;
            }

            float totalHeight = (2 * boundaryWidth) + singleLineHeight + separatorLineHeight; //top + bottom + constructorName rows;
            foreach (ParameterBase parameter in parameters)
            {
                if (parameter is not LiteralParameter literalParameter 
                    || !implemented.Contains(literalParameter.Control))
                    continue;

                totalHeight += literalParameter.Control == Enums.LiteralParameterInputStyle.MultipleLineTextBox
                            ? multiLineHeight
                            : singleLineHeight;

                totalHeight += separatorLineHeight;
            }

            if (hasGenericArguments)
            {
                totalHeight += singleLineHeight;
                totalHeight += separatorLineHeight;
            }

            float size_Boundary = boundaryWidth / totalHeight * 100;
            float size_SingleLine = singleLineHeight / totalHeight * 100;
            float size_MultiLine = multiLineHeight / totalHeight * 100;
            float size_Separator = separatorLineHeight / totalHeight * 100;
            int totalTableLayoutHeight = (int)Math.Ceiling(totalHeight);

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

            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, size_Boundary));//boundary row
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, size_SingleLine));//Constructor Name
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, size_Separator));

            if (hasGenericArguments)
            {
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, size_SingleLine));//Generic Arguments
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, size_Separator));
            }

            foreach (ParameterBase parameter in parameters)
            {
                if (parameter is not LiteralParameter literalParameter 
                    || !implemented.Contains(literalParameter.Control))
                    continue;

                float size = literalParameter.Control == Enums.LiteralParameterInputStyle.MultipleLineTextBox
                            ? size_MultiLine
                            : size_SingleLine;

                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, size));//parameter
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, size_Separator));
            }

            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, size_Boundary));//boundary row
            tableLayoutPanel.Size = new Size(851, totalTableLayoutHeight);
            tableLayoutPanel.TabIndex = 0;
        }
    }
}
