using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers
{
    internal static class NavigationUtility
    {
        public static void Navigate(nint handle, RadPanel radPanelFields, Control newControl)
        {
            Native.NativeMethods.LockWindowUpdate(handle);
            ((ISupportInitialize)radPanelFields).BeginInit();
            radPanelFields.SuspendLayout();

            ClearFieldControls();
            newControl.Dock = DockStyle.Fill;
            newControl.Location = new Point(0, 0);
            radPanelFields.Controls.Add(newControl);

            ((ISupportInitialize)radPanelFields).EndInit();
            radPanelFields.ResumeLayout(true);

            Native.NativeMethods.LockWindowUpdate(IntPtr.Zero);

            void ClearFieldControls()
            {
                for (int i = radPanelFields.Controls.Count - 1; i > -1; i--)
                {
                    Control control = radPanelFields.Controls[i];
                    control.Visible = false;
                    if (!control.IsDisposed)
                        control.Dispose();
                }
            }
        }
    }
}
