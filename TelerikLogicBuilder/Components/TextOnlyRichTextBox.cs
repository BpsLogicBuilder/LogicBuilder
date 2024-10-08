﻿using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Components
{
    internal class TextOnlyRichTextBox : RichTextBox
    {
        public TextOnlyRichTextBox()
        {
            this.BackColor = ForeColorUtility.GetTextBoxBackColor(Telerik.WinControls.ThemeResolutionService.ApplicationThemeName);
            this.ForeColor = ForeColorUtility.GetTextBoxForeColor(Telerik.WinControls.ThemeResolutionService.ApplicationThemeName);

            Telerik.WinControls.ThemeResolutionService.ApplicationThemeChanged += ThemeResolutionService_ApplicationThemeChanged;
            this.Disposed += TextOnlyRichTextBox_Disposed;

            this.Font = ForeColorUtility.GetDefaultFont(Telerik.WinControls.ThemeResolutionService.ApplicationThemeName);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x100;
            const int WM_SYSKEYDOWN = 0x104;
            if (msg.Msg == WM_KEYDOWN || msg.Msg == WM_SYSKEYDOWN)
            {
                switch (keyData)
                {
                    case Keys.Control | Keys.V:
                    case Keys.Shift | Keys.Insert:
                        if (Clipboard.GetDataObject()?.GetDataPresent(DataFormats.Text, true) == true)
                        {
                            Clipboard.SetDataObject(Clipboard.GetDataObject()!.GetData(DataFormats.Text, true)!);
                        }
                        break;
                    default:
                        break;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #region EventHandlers
        private void TextOnlyRichTextBox_Disposed(object? sender, System.EventArgs e)
        {
            Telerik.WinControls.ThemeResolutionService.ApplicationThemeChanged -= ThemeResolutionService_ApplicationThemeChanged;
        }

        private void ThemeResolutionService_ApplicationThemeChanged(object sender, Telerik.WinControls.ThemeChangedEventArgs args)
        {
            if (this.IsDisposed)
                return;

            this.BackColor = ForeColorUtility.GetTextBoxBackColor(args.ThemeName);
            this.ForeColor = ForeColorUtility.GetTextBoxForeColor(args.ThemeName);
            this.Font = ForeColorUtility.GetDefaultFont(args.ThemeName);
        }
        #endregion EventHandlers
    }
}
