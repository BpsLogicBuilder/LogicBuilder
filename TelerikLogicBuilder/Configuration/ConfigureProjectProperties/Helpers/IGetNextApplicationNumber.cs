using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Helpers
{
    internal interface IGetNextApplicationNumber
    {
        int Get(RadTreeNode projectPropertiesRootNode);
    }
}
