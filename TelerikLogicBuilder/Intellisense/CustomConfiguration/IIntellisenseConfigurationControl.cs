using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.CustomConfiguration
{
    internal interface IIntellisenseConfigurationControl
    {
        void SetControlValues(RadTreeNode treeNode);
        void ValidateFields();
    }
}
