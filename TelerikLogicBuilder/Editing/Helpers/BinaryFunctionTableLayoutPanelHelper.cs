﻿using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Helpers
{
    internal class BinaryFunctionTableLayoutPanelHelper : IBinaryFunctionTableLayoutPanelHelper
    {
        private readonly IExceptionHelper _exceptionHelper;

        public BinaryFunctionTableLayoutPanelHelper(IExceptionHelper exceptionHelper)
        {
            _exceptionHelper = exceptionHelper;
        }

        public void SetUp(TableLayoutPanel tableLayoutPanel, RadPanel radPanelTableParent, IList<ParameterBase> parameters, bool hasGenericArguments)
        {
            if (parameters.Count != 2)
                throw _exceptionHelper.CriticalException("{54A3276D-273B-4A17-9698-202328DAB699}");

            tableLayoutPanel.ColumnStyles.Clear();
            tableLayoutPanel.RowStyles.Clear();

            float boundaryWidth = PerFontSizeConstants.BoundarySize;
            float singleLineHeight = PerFontSizeConstants.SingleLineHeight;
            float multiLineHeight = PerFontSizeConstants.MultiLineHeight;
            float separatorLineHeight = PerFontSizeConstants.SeparatorLineHeight;
            int rowCount = 8;//top + bottom + functionName rows + parameter rows;

            if (hasGenericArguments)
                rowCount += 2;

            float totalHeight = (2 * boundaryWidth) + singleLineHeight + separatorLineHeight; //top + bottom + functionName rows;

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
            if (hasGenericArguments)
            {
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, singleLineHeight));//Generic Arguments
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, separatorLineHeight));
            }

            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, GetParameterRowHeight(parameters[0])));//First parameter
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, separatorLineHeight));

            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, singleLineHeight));//function name
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, separatorLineHeight));//boundary row

            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, GetParameterRowHeight(parameters[1])));//Second parameter
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, separatorLineHeight));

            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, boundaryWidth));//boundary row
            tableLayoutPanel.Size = new Size(851, totalTableLayoutHeight);
            tableLayoutPanel.TabIndex = 0;

            float GetParameterRowHeight(ParameterBase parameter)
            {
                return parameter is LiteralParameter literalParameter && literalParameter.Control == Enums.LiteralParameterInputStyle.MultipleLineTextBox
                            ? multiLineHeight
                            : singleLineHeight;
            }
        }
    }
}
