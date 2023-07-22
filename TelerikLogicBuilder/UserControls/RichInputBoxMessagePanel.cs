using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Components.Factories;
using System.ComponentModel;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal partial class RichInputBoxMessagePanel : UserControl, IRichInputBoxMessagePanel
    {
        private readonly IRichInputBox _richInputBox;
        public RichInputBoxMessagePanel(IComponentFactory componentFactory)
        {
            _richInputBox = componentFactory.GetRichInputBox();
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

            this.radPanel1.Controls.Add((Control)_richInputBox);
            ((ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        public IRichInputBox RichInputBox => _richInputBox;
    }
}
