using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Helpers
{
    internal interface ISetValueFunctionTableLayoutPanelHelper
    {
        void SetUp(TableLayoutPanel tableLayoutPanel, RadPanel radPanelTableParent, bool isMultiLine);
    }
}
