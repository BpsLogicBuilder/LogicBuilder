using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration.Initialization
{
    internal class EmptyTreeFolderRemover : IEmptyTreeFolderRemover
    {
        public void RemoveEmptyFolders(TreeFolder treeFolder)
        {
            List<TreeFolder> foldersToRemove = new();

            foreach (TreeFolder folder in treeFolder.FolderNames)
            {
                if (!HasFileDescendants(folder))
                    foldersToRemove.Add(folder);
                else
                    RemoveEmptyFolders(folder);
            }

            foldersToRemove.ForEach
            (
                folder => treeFolder.FolderNames.Remove(folder)
            );
        }

        private static bool HasFileDescendants(TreeFolder treeFolder)
        {
            if (treeFolder.FileNames.Count > 0)
                return true;

            foreach (TreeFolder folder in treeFolder.FolderNames)
            {
                if (HasFileDescendants(folder))
                    return true;
            }

            return false;
        }
    }
}
