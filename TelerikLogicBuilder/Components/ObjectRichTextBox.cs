using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Components
{
    internal class ObjectRichTextBox : RichTextBox
    {
        public ObjectRichTextBox()
        {
            this.BackColor = ForeColorUtility.GetTextBoxBackColor(Telerik.WinControls.ThemeResolutionService.ApplicationThemeName);
            this.ForeColor = ForeColorUtility.GetTextBoxForeColor(Telerik.WinControls.ThemeResolutionService.ApplicationThemeName);

            Telerik.WinControls.ThemeResolutionService.ApplicationThemeChanged += ThemeResolutionService_ApplicationThemeChanged;
            this.Disposed += ObjectRichTextBox_Disposed;

            //the font will be replaced (links erased) on theme changes if it hasn't been set
            this.Font = new Font(this.Font, this.Font.Style);
        }

        public void SetLinkFormat()
        {
            this.Cursor = Cursors.Hand;
            this.Font = new Font(this.Font, GetFontStyle());
            this.ForeColor = ForeColorUtility.GetLinkBoundaryColor(Telerik.WinControls.ThemeResolutionService.ApplicationThemeName);

            FontStyle GetFontStyle()
            {
                FontStyle fontStyle = this.Font.Style;
                if (!this.Font.Underline)
                    fontStyle |= FontStyle.Underline;

                return fontStyle;
            }
        }

        public void SetDefaultFormat()
        {
            this.Cursor = Cursors.Default;
            this.Font = new Font(this.Font, GetFontStyle());
            this.ForeColor = ForeColorUtility.GetTextBoxForeColor(Telerik.WinControls.ThemeResolutionService.ApplicationThemeName);

            FontStyle GetFontStyle()
            {
                FontStyle fontStyle = this.Font.Style;
                if (this.Font.Underline)
                    fontStyle &= ~FontStyle.Underline;

                return fontStyle;
            }
        }

        private void ObjectRichTextBox_Disposed(object? sender, EventArgs e)
        {
            Telerik.WinControls.ThemeResolutionService.ApplicationThemeChanged -= ThemeResolutionService_ApplicationThemeChanged;
        }

        private void ThemeResolutionService_ApplicationThemeChanged(object sender, Telerik.WinControls.ThemeChangedEventArgs args)
        {
            if (this.IsDisposed)
                return;

            this.BackColor = ForeColorUtility.GetTextBoxBackColor(args.ThemeName);
            this.ForeColor = ForeColorUtility.GetTextBoxForeColor(args.ThemeName);
        }
    }
}
