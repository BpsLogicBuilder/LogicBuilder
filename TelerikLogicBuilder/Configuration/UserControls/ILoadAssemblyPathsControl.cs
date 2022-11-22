using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls
{
    internal interface ILoadAssemblyPathsControl
    {
        IRadListBoxManager<AssemblyPath> RadListBoxManager { get; }
        HelperButtonTextBox TxtPath { get; }
        RadListControl ListBox { get; }

        IList<string> GetPaths();
        void SetPaths(IList<string> paths);
    }
}
