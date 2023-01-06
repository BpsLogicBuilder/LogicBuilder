using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.CustomConfiguration
{
    internal partial class FunctionConfigurationControl : UserControl, IFunctionConfigurationControl
    {
        public FunctionConfigurationControl()
        {
            InitializeComponent();
        }

        public void SetControlValues(RadTreeNode treeNode)
        {
        }

        public void ValidateFields()
        {
        }
    }
}
