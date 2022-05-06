using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.ComponentModel;
using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder
{
    internal partial class MDIParent : Telerik.WinControls.UI.RadForm, IMDIParent
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

        private void RadThemeMenuItem_Click(object sender, EventArgs e)
        {
            Telerik.WinControls.UI.RadMenuItem clickedItem = (Telerik.WinControls.UI.RadMenuItem)sender;
            foreach (Telerik.WinControls.UI.RadMenuItem menuItem in this.radMenuItemTheme.Items)
            {
                if (menuItem != clickedItem)
                    menuItem.IsChecked = false;
            }

            ThemeResolutionService.ApplicationThemeName = (string)clickedItem.Tag;
            Properties.Settings.Default.themeName = ThemeResolutionService.ApplicationThemeName;
            Properties.Settings.Default.Save();
        }
    }
}
