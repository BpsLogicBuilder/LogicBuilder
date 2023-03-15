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
                foreach (Control control in radPanelFields.Controls)
                {
                    control.Visible = false;
                    if (!control.IsDisposed)
                        control.Dispose();
                }

                radPanelFields.Controls.Clear();
            }
        }
    }
}
