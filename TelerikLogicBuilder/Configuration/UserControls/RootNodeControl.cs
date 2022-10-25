using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls
{
    public partial class RootNodeControl : UserControl
    {
        public RootNodeControl()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            ((BorderPrimitive)radPanelFill.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;
        }
    }
}
