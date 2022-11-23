using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration
{
    internal interface IConfigurationXmlElementControl
    {
        void SetControlValues(RadTreeNode treeNode);
        void UpdateXmlDocument(RadTreeNode treeNode);
    }
}
