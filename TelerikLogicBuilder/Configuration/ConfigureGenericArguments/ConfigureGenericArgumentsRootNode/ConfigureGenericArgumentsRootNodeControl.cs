using ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericArgumentsRootNode
{
    public partial class ConfigureGenericArgumentsRootNodeControl : UserControl, IXmlElementControl
    {
        public ConfigureGenericArgumentsRootNodeControl()
        {
            InitializeComponent();
            Initialize();
        }

        public void SetControlValues(RadTreeNode treeNode)
        {
        }

        public void UpdateXmlDocument(RadTreeNode treeNode)
        {
        }

        private void Initialize()
        {
            ((BorderPrimitive)radPanelFill.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;
        }
    }
}
