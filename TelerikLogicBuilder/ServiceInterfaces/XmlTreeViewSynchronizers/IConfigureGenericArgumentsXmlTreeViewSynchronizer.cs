using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers
{
    internal interface IConfigureGenericArgumentsXmlTreeViewSynchronizer
    {
        void ReplaceArgumentNode(RadTreeNode existingTreeNode, XmlElement newXmlGenArgParameterNode);
    }
}
