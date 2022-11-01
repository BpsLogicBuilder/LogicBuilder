using System;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls
{
    internal interface IXmlElementControl
    {
        void SetControlValues(RadTreeNode treeNode);
        void UpdateXmlDocument(RadTreeNode treeNode);
    }
}
