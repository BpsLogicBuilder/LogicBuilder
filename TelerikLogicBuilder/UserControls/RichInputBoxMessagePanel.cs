using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System.ComponentModel;
using System.Windows.Forms;
using Telerik.WinControls;

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
            _richInputBox.Dock = DockStyle.Fill;
            _richInputBox.BorderStyle = BorderStyle.None;
            _richInputBox.WordWrap = false;
            _richInputBox.ReadOnly = true;
            _richInputBox.DetectUrls = false;
            _richInputBox.HideSelection = false;

            this.radPanel1.Controls.Add(_richInputBox);
            ((ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        public RichInputBox RichInputBox => _richInputBox;
    }
}
