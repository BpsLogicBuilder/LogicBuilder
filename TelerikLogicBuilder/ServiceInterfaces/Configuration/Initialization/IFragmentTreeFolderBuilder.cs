using ABIS.LogicBuilder.FlowBuilder.Configuration;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization
{
    internal interface IFragmentTreeFolderBuilder
    {
        TreeFolder GetTreeFolder(XmlDocument xmlDocument);
    }
}
