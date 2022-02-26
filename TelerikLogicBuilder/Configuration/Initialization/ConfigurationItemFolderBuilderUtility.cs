using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Initialization
{
    abstract internal class ConfigurationItemFolderBuilderUtility
    {
        abstract internal TreeFolder GetTreeFolder(XmlDocument xmlDocument);

        protected void RemoveEmptyFolders(TreeFolder treeFolder)
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

        private bool HasFileDescendants(TreeFolder treeFolder)
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
