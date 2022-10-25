using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    public partial class HelperButtonTextBox : UserControl
    {
        public HelperButtonTextBox()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            radButtonHelper.Image = Properties.Resources.more;
            radButtonHelper.Click += RadButtonHelper_Click;
        }

        public event EventHandler<EventArgs>? RaiseClickEvent;

        private void RadButtonHelper_Click(object? sender, EventArgs e)
        {
            RaiseClickEvent?.Invoke(this, e);
        }
    }
}
