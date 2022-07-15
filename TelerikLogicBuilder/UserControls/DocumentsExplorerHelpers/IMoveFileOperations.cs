using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers
{
    internal interface IMoveFileOperations
    {
        void MoveFile(RadTreeNode treeNode, string newFileFullName);
        void MoveFolder(RadTreeNode treeNode, string newFolderFullName);
    }
}
