using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace ABIS.LogicBuilder.FlowBuilder.Native
{
    internal static class NativeMethods
    {
        #region Methods
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
        [DllImport("user32.dll")]
        internal static extern IntPtr LockWindowUpdate(IntPtr hWnd);

        [DllImport("User32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("User32.dll")]
        static extern int ReleaseDC(IntPtr hwnd, IntPtr dc);

        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        public static System.Drawing.Rectangle GetScreenArea()
        {
            IntPtr primary = GetDC(IntPtr.Zero);
            int DESKTOPVERTRES = 117;
            int DESKTOPHORZRES = 118;
            int actualPixelsX = GetDeviceCaps(primary, DESKTOPHORZRES);
            int actualPixelsY = GetDeviceCaps(primary, DESKTOPVERTRES);
            if (ReleaseDC(IntPtr.Zero, primary) == 1)
            {
                return new System.Drawing.Rectangle
                {
                    Height = actualPixelsY,
                    Width = actualPixelsX
                };
            }
            else
            {
                throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{D98119B0-D5C3-4255-8327-124889C19E01}"));
            }
        }

        private const int WM_SETREDRAW = 11;
        public static void SuspendDrawing(System.Windows.Forms.Control parent)
        {
            //Changing Curser here causes "loud" refresh
            //parent.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            _ = SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
        }

        public static void ResumeDrawing(System.Windows.Forms.Control parent)
        {
            _ = SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
            //parent.Cursor = System.Windows.Forms.Cursors.Default;
            parent.Refresh();
        }
        #endregion Methods
    }
}
