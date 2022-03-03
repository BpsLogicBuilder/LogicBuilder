using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration
{
    internal class TreeFolder
    {
        internal TreeFolder(string name, List<string> fileNames, List<TreeFolder> folderNames)
        {
            this.Name = name;
            this.FileNames = fileNames;
            this.FolderNames = folderNames;
        }

        #region Properties
        internal string Name { get; }
        internal List<string> FileNames { get; }
        internal List<TreeFolder> FolderNames { get; }
        #endregion Properties
    }
}
