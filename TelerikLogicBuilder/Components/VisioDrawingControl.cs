namespace ABIS.LogicBuilder.FlowBuilder.Components
{
    internal class VisioDrawingControl : AxMicrosoft.Office.Interop.VisOcx.AxDrawingControl
    {
        private bool bCtrlPressed;
        public override bool PreProcessMessage(ref System.Windows.Forms.Message m)
        {
            const int WM_KEYDOWN = 256;
            const int WM_KEYUP = 257;
            const int VK_CONTROL = 17;
            const int VK_F = 70;
            //System.Diagnostics.Debug.WriteLine(m.Msg.ToString() + ": " + m.WParam.ToInt32() + ": " + m.LParam.ToInt32());
            if (WM_KEYDOWN == m.Msg && VK_CONTROL == m.WParam.ToInt32())
                bCtrlPressed = true;
            if (WM_KEYUP == m.Msg && VK_CONTROL == m.WParam.ToInt32())
                bCtrlPressed = false;
            if (bCtrlPressed && WM_KEYDOWN == m.Msg && VK_F == m.WParam.ToInt32())
                return true;
            return base.PreProcessMessage(ref m);
        }
    }
}
