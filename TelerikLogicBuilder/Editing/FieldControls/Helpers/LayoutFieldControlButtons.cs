using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal class LayoutFieldControlButtons : ILayoutFieldControlButtons
    {
        public void Layout(RadPanel panelButtons, IList<RadButton> buttons, bool performLayout = true, int buttonWidth = 30)
        {
            TableLayoutPanel tableLayoutPanel = new();
            ((ISupportInitialize)panelButtons).BeginInit();
            panelButtons.SuspendLayout();
            panelButtons.Size = new Size(buttons.Count * buttonWidth, panelButtons.Height);

            tableLayoutPanel.SuspendLayout();
            tableLayoutPanel.Padding = new Padding(0);
            tableLayoutPanel.Margin = new Padding(0);
            tableLayoutPanel.Dock = DockStyle.Fill;

            tableLayoutPanel.ColumnCount = buttons.Count;
            for (int i = 0; i < buttons.Count; i++)
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / buttons.Count));

            tableLayoutPanel.RowCount = 1;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            for (int i = 0; i < buttons.Count; i++)
                tableLayoutPanel.Controls.Add(buttons[i], i, 0);

            panelButtons.Controls.Add(tableLayoutPanel);

            tableLayoutPanel.ResumeLayout(false);

            ((ISupportInitialize)panelButtons).EndInit();
            panelButtons.ResumeLayout(performLayout);
        }
    }
}
