using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers
{
    internal interface IProjectPropertiesXmlTreeViewSynchronizer
    {
        void AddApplicationNode(RadTreeNode destinationParentNode, XmlElement newXmlApplicationNode);
        void DeleteNode(RadTreeNode treeNode);
    }
}
