using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls
{
    internal interface ILoadAssemblyPathsControl
    {
        IRadListBoxManager<AssemblyPath> RadListBoxManager { get; }
        HelperButtonTextBox TxtPath { get; }
        RadListControl ListBox { get; }
    }
}
