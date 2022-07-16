using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Globalization;
using System.IO;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers
{
    internal class MoveFileOperations : IMoveFileOperations
    {
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IMainWindow _mainWindow;

        public MoveFileOperations(IFileIOHelper fileIOHelper, IMainWindow mainWindow)
        {
            _fileIOHelper = fileIOHelper;
            _mainWindow = mainWindow;
        }

        public void MoveFile(RadTreeNode treeNode, string newFileFullName)
        {
            IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;
            if (string.Compare(mdiParent.EditControl?.SourceFile, treeNode.Name, StringComparison.InvariantCultureIgnoreCase) == 0)
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.closeFileWarningFormat, treeNode.Text));

            if (File.Exists(newFileFullName))
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.fileAlreadyExistsFormat, newFileFullName));

            _fileIOHelper.MoveFile(treeNode.Name, newFileFullName);
        }

        public void MoveFolder(RadTreeNode treeNode, string newFolderFullName)
        {
            IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;
            if (mdiParent.EditControl?.SourceFile.ToLowerInvariant().Trim().Contains(treeNode.Name.ToLowerInvariant().Trim()) == true)
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.closeFolderFilesWarningFormat, treeNode.Text));

            if (Directory.Exists(newFolderFullName))
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.folderAlreadyExistsFormat, newFolderFullName));

            _fileIOHelper.MoveFolder(treeNode.Name, newFolderFullName);
        }
    }
}
