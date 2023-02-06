using ABIS.LogicBuilder.FlowBuilder.Components;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal partial class RichInputBoxMessagePanel : UserControl
    {
        private readonly RichInputBox _richInputBox;
        public RichInputBoxMessagePanel(RichInputBox richInputBox)
        {
            _richInputBox = richInputBox;
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            ((ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            this.SuspendLayout();
            _richInputBox.Dock = DockStyle.Top;
            _richInputBox.BorderStyle = BorderStyle.None;
            _richInputBox.ScrollBars = RichTextBoxScrollBars.None;
            _richInputBox.WordWrap = false;
            _richInputBox.ReadOnly = true;
            _richInputBox.DetectUrls = false;
            _richInputBox.HideSelection = false;
            _richInputBox.ContentsResized += RichTextBox1_ContentsResized;
            _richInputBox.MinimumSize = new Size(radPanel1.PanelContainer.ClientSize.Width, radPanel1.PanelContainer.ClientSize.Height);

            this.radPanel1.Controls.Add(_richInputBox);
            ((ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

            radPanel1.ClientSizeChanged += RadPanel1_ClientSizeChanged;
        }

        public RichInputBox RichInputBox => _richInputBox;

        private void RadPanel1_ClientSizeChanged(object? sender, EventArgs e)
        {
            _richInputBox.MinimumSize = new Size(radPanel1.PanelContainer.ClientSize.Width, radPanel1.PanelContainer.ClientSize.Height);
        }

        private void RichTextBox1_ContentsResized(object? sender, ContentsResizedEventArgs e)
        {
            _richInputBox.Width = e.NewRectangle.Width;
            _richInputBox.Height = e.NewRectangle.Height;
        }
    }
}
