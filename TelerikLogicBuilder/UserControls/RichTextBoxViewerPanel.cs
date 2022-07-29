using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    public partial class RichTextBoxViewerPanel : UserControl
    {
        public RichTextBoxViewerPanel()
        {
            InitializeComponent();
            Initialize();
        }

        public RichTextBox RichTextBox => this.richTextBox1;

        private void Initialize()
        {
            this.richTextBox1.WordWrap = false;
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.BorderStyle = BorderStyle.None;
            this.richTextBox1.ScrollBars = RichTextBoxScrollBars.Both;
            this.richTextBox1.Select(0, 0);

            this.richTextBox1.BackColor = ForeColorUtility.GetTextBoxBackColor(ThemeResolutionService.ApplicationThemeName);
            this.richTextBox1.ForeColor = ForeColorUtility.GetTextBoxForeColor(ThemeResolutionService.ApplicationThemeName);

            ThemeResolutionService.ApplicationThemeChanged += ThemeResolutionService_ApplicationThemeChanged;
            this.Disposed += RichTextBoxViewerPanel_Disposed;
        }

        private void RichTextBoxViewerPanel_Disposed(object? sender, EventArgs e)
        {
            ThemeResolutionService.ApplicationThemeChanged -= ThemeResolutionService_ApplicationThemeChanged;
        }

        private void ThemeResolutionService_ApplicationThemeChanged(object sender, ThemeChangedEventArgs args)
        {
            this.richTextBox1.BackColor = ForeColorUtility.GetTextBoxBackColor(args.ThemeName);
            this.richTextBox1.ForeColor = ForeColorUtility.GetTextBoxForeColor(args.ThemeName);
        }
    }
}
