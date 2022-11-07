using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Forms
{
    public partial class ConfigureWebApiDeployment : Telerik.WinControls.UI.RadForm
    {
        public ConfigureWebApiDeployment()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            ((BorderPrimitive)radPanelUrls.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;
            ((BorderPrimitive)radPanelTableParent.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;
        }
    }
}
