using ABIS.LogicBuilder.FlowBuilder.Configuration;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization
{
    internal interface IEmptyTreeFolderRemover
    {
        void RemoveEmptyFolders(TreeFolder treeFolder);
    }
}
