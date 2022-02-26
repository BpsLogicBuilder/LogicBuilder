using ABIS.LogicBuilder.FlowBuilder.Configuration;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization
{
    internal interface IVariableTreeFolderBuilder
    {
        TreeFolder GetTreeFolder(XmlDocument xmlDocument);
    }
}
