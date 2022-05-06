using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.ComponentModel;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder
{
    internal partial class MDIParent : Telerik.WinControls.UI.RadForm, IMDIParent
    {
        private readonly IFormInitializer _formInitializer;
        private readonly IThemeManager _themeManager;
        public MDIParent(IFormInitializer formInitializer, IThemeManager themeManager)
        {
            _formInitializer = formInitializer;
            _themeManager = themeManager;
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                this.Icon = _formInitializer.GetLogicBuilderIcon();
            }

            commandBarStripElement1.OverflowButton.Visibility = ElementVisibility.Collapsed;
            _themeManager.CheckMenuItemForCurrentTheme(this.radMenuItemTheme.Items);
        }

        private void CommandBarButtonEdit_Click(object sender, EventArgs e)
        {

        }

        private void RadThemeMenuItem_Click(object sender, EventArgs e)
        {
            RadMenuItem clickedItem = (RadMenuItem)sender;
            _themeManager.SetTheme((string)clickedItem.Tag);
            _themeManager.CheckMenuItemForCurrentTheme(this.radMenuItemTheme.Items);
        }
    }
}
