using System;
using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder
{
    public partial class MDIParent : Telerik.WinControls.UI.RadForm
    {
        public MDIParent()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            commandBarStripElement1.OverflowButton.Visibility = ElementVisibility.Collapsed;
        }

        private void CommandBarButtonEdit_Click(object sender, EventArgs e)
        {

        }
    }
}
