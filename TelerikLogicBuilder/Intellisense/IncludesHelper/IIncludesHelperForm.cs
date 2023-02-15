using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.IncludesHelper
{
    internal interface IIncludesHelperForm : IApplicationForm
    {
        AutoCompleteRadDropDownList CmbClass { get; }
        string Includes { get; }
        VariableTreeNode ReferenceTreeNode { get; }
        RadTreeView TreeView { get; }

        void ClearTreeView();
        void ValidateOk();
    }
}
