using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal interface IConfigurationExplorer
    {
        RadTreeView TreeView { get; }

        void RefreshTreeView();
    }
}
