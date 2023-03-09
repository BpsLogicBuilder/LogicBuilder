using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System.Collections.Generic;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Helpers
{
    internal interface IBinaryFunctionTableLayoutPanelHelper
    {
        void SetUp(TableLayoutPanel tableLayoutPanel, RadPanel radPanelTableParent, IList<ParameterBase> parameters, bool hasGenericArguments);
    }
}
