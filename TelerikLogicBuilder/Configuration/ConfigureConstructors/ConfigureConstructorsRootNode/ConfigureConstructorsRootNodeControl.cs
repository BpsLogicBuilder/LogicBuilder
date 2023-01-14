using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructorsRootNode
{
    internal partial class ConfigureConstructorsRootNodeControl : UserControl, IConfigureConstructorsRootNodeControl
    {
        public ConfigureConstructorsRootNodeControl()
        {
            InitializeComponent();
        }

        public void SetControlValues(RadTreeNode treeNode)
        {
        }

        public void UpdateXmlDocument(RadTreeNode treeNode)
        {
        }
    }
}
