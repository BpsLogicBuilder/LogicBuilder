using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace TelerikLogicBuilder
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
