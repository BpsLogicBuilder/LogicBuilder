using ABIS.LogicBuilder.FlowBuilder.Native;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class FormInitializer : IFormInitializer
    {
        public Icon GetLogicBuilderIcon() 
            => Properties.Resources.Simple;

        public void SetCenterScreen(Form form)
        {
            float dpiX, dpiY;
            Graphics graphics = form.CreateGraphics();
            dpiX = graphics.DpiX / 100;
            dpiY = graphics.DpiY / 100;

            var workingArea = NativeMethods.GetScreenArea();

            var centerX = (workingArea.Width / 2) - (form.DesktopBounds.Width * dpiX / 2);
            var centerY = (workingArea.Height / 2) - (form.DesktopBounds.Height * dpiY / 2);
            form.Location = new Point((int)centerX, (int)centerY);
        }

        public void SetFormDefaults(Form form, int minHeight)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            Rectangle area = NativeMethods.GetScreenArea();

            int maxHeight = area.Height - 100;
            int maxWidth = area.Width - 150;
            int minWidth = 0;

            form.Icon = GetLogicBuilderIcon();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.FormBorderStyle = FormBorderStyle.Sizable;
            form.MaximumSize = new Size(maxWidth, maxHeight);
            form.MinimumSize = new Size(GetMinWidth(), GetMinHeight());
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Size = new Size(area.Width - 450, form.Height);

            int GetMinHeight() => minHeight < maxHeight ? minHeight : maxHeight;
            int GetMinWidth() => minWidth < maxWidth ? minWidth : maxWidth;
        }

        public void SetProgressFormDefaults(Form form, int minHeight)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            int maxHeight = 2000;
            int maxWidth = 2000;
            int minWidth = 0;

            form.Icon = GetLogicBuilderIcon();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.FormBorderStyle = FormBorderStyle.Sizable;
            form.MaximumSize = new Size(maxWidth, maxHeight);
            form.MinimumSize = new Size(minWidth, minHeight);
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.ControlBox = false;
        }

        public void SetToConfigFragmentSize(Form form)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            Rectangle area = NativeMethods.GetScreenArea();

            form.Size = new Size(form.Width, area.Height - 100);
        }

        public void SetToEditSize(Form form)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            Rectangle area = NativeMethods.GetScreenArea();

            form.Size = new Size(area.Width - 450, area.Height - 100);
        }

        public void SetToolTipDefaults(ToolTip toolTip)
        {
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 250;
            toolTip.ReshowDelay = 250;
            toolTip.ShowAlways = true;
        }
    }
}
