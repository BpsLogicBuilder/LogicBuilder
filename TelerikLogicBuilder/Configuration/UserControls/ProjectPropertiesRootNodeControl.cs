using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls
{
    public partial class ProjectPropertiesRootNodeControl : UserControl, IXmlElementControl
    {
        public ProjectPropertiesRootNodeControl()
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
