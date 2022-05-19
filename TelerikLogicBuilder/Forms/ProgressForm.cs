using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Threading;

namespace ABIS.LogicBuilder.FlowBuilder.Forms
{
    internal partial class ProgressForm : Telerik.WinControls.UI.RadForm
    {
        private readonly IFormInitializer _formInitializer;
        private readonly Progress<ProgressMessage> _progress;
        private readonly CancellationTokenSource _cancellationTokenSource;
        public ProgressForm(IFormInitializer formInitializer, Progress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
        {
            _formInitializer = formInitializer;
            _progress = progress;
            _progress.ProgressChanged += Progress_ProgressChanged;
            _cancellationTokenSource = cancellationTokenSource;
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            this.Text = ApplicationProperties.Name;
            _formInitializer.SetProgressFormDefaults(this, 288);
            radPanelLabel.Anchor = AnchorConstants.AnchorsLeftTopRight;
            radPanelProgressBar.Anchor = AnchorConstants.AnchorsLeftTopRight;
            radButtonCancel.Anchor = AnchorConstants.AnchorsLeftTopRight;
        }

        private void Progress_ProgressChanged(object? sender, ProgressMessage e)
        {
            radPanelLabel.Text = e.Message;
            radProgressBar.Value1 = e.Progress;
        }

        private void RadButtonCancel_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource.Cancel();
            this.Close();
        }
    }
}
