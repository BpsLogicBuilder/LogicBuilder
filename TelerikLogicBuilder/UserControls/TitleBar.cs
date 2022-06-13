using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal class TitleBar : Telerik.WinControls.UI.RadTitleBar
    {
        public TitleBar()
        {
            this.TitleBarElement.CloseButton.Click += new EventHandler(CloseButton_Click);
            this.ContextMenuStrip = new ContextMenuStrip();
            this.ContextMenuStrip.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripMenuItem(Strings.titleBarCloseContextMenuText, null, new EventHandler(CloseButton_Click))
            });
        }

        public delegate void CloseClickHandler();
        public event CloseClickHandler? CloseClick;

        protected override void OnLoad(System.Drawing.Size desiredSize)
        {
            this.CanManageOwnerForm = false;
            this.TitleBarElement.MaximizeButton.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            this.TitleBarElement.MinimizeButton.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;

            this.TitleBarElement.TitleBarFill.BackColor4 = System.Drawing.SystemColors.ActiveCaption;
            this.TitleBarElement.TitlePrimitive.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            base.OnLoad(desiredSize);
        }

        void CloseButton_Click(object? sender, EventArgs e)
        {
            CloseClick?.Invoke();
        }
    }
}
