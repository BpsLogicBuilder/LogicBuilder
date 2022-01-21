using ABIS.LogicBuilder.FlowBuilder.Services;
using System;
using System.ComponentModel;
using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder
{
    internal partial class MDIParent : Telerik.WinControls.UI.RadForm
    {
        private readonly IFormInitializer formInitializer;
        public MDIParent(IFormInitializer formInitializer)
        {
            this.formInitializer = formInitializer;
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                this.Icon = formInitializer.GetLogicBuilderIcon();
            }

            commandBarStripElement1.OverflowButton.Visibility = ElementVisibility.Collapsed;
        }

        private void CommandBarButtonEdit_Click(object sender, EventArgs e)
        {

        }
    }
}
