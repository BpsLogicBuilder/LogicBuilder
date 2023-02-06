using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

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
            this.richTextBox1.ScrollBars = RichTextBoxScrollBars.None;
            this.richTextBox1.Select(0, 0);
            this.richTextBox1.ContentsResized += RichTextBox1_ContentsResized;
            this.richTextBox1.MinimumSize = new Size(radPanel1.PanelContainer.ClientSize.Width, radPanel1.PanelContainer.ClientSize.Height);

            this.richTextBox1.BackColor = ForeColorUtility.GetTextBoxBackColor(ThemeResolutionService.ApplicationThemeName);
            this.richTextBox1.ForeColor = ForeColorUtility.GetTextBoxForeColor(ThemeResolutionService.ApplicationThemeName);

            radPanel1.HorizontalScrollBarState = ScrollState.AlwaysShow;
            radPanel1.VerticalScrollBarState = ScrollState.AlwaysShow;
            radPanel1.ClientSizeChanged += RadPanel1_ClientSizeChanged;

            ThemeResolutionService.ApplicationThemeChanged += ThemeResolutionService_ApplicationThemeChanged;
            this.Disposed += RichTextBoxViewerPanel_Disposed;
        }

        private void RadPanel1_ClientSizeChanged(object? sender, EventArgs e)
        {
            this.richTextBox1.MinimumSize = new Size(radPanel1.PanelContainer.ClientSize.Width, radPanel1.PanelContainer.ClientSize.Height);
        }

        private void RichTextBox1_ContentsResized(object? sender, ContentsResizedEventArgs e)
        {
            richTextBox1.Width = e.NewRectangle.Width;
            richTextBox1.Height = e.NewRectangle.Height;
        }

        private void RichTextBoxViewerPanel_Disposed(object? sender, EventArgs e)
        {
            ThemeResolutionService.ApplicationThemeChanged -= ThemeResolutionService_ApplicationThemeChanged;
        }

        private void ThemeResolutionService_ApplicationThemeChanged(object sender, ThemeChangedEventArgs args)
        {
            if (this.IsDisposed)
                return;

            this.richTextBox1.BackColor = ForeColorUtility.GetTextBoxBackColor(args.ThemeName);
            this.richTextBox1.ForeColor = ForeColorUtility.GetTextBoxForeColor(args.ThemeName);
        }
    }
}
