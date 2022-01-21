using ABIS.LogicBuilder.FlowBuilder.Native;
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
    }
}
