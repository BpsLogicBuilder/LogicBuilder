using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunctionsRootNode
{
    internal partial class ConfigureFunctionsRootNodeControl : UserControl, IConfigureFunctionsRootNodeControl
    {
        public ConfigureFunctionsRootNodeControl()
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
