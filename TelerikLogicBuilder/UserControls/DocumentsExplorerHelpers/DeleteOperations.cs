using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers
{
    internal class DeleteOperations : IDeleteOperations
    {
        private readonly IConfigurationService _configurationService;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IMainWindow _mainWindow;

        public DeleteOperations(
            IConfigurationService configurationService,
            IFileIOHelper fileIOHelper,
            IMainWindow mainWindow)
        {
            _configurationService = configurationService;
            _fileIOHelper = fileIOHelper;
            _mainWindow = mainWindow;
        }

        public void DeleteFile(RadTreeNode treeNode)
        {
            DialogResult dialogResult = DisplayMessage.ShowQuestion
            (
                _mainWindow.Instance,
                string.Format(CultureInfo.CurrentCulture, Strings.deleteFileQuestionFormat, treeNode.Text),
                string.Empty,
                _mainWindow.RightToLeft,
                Telerik.WinControls.RadMessageIcon.Error
            );
            if (dialogResult != DialogResult.OK)
                return;

            IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;

            if (string.Compare(mdiParent.EditControl?.SourceFile, treeNode.Name, StringComparison.InvariantCultureIgnoreCase) == 0)
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.closeFileWarningFormat, treeNode.Text));

            _fileIOHelper.DeleteFile(treeNode.Name);
        }

        public void DeleteFolder(RadTreeNode treeNode)
        {
            DialogResult dialogResult = DisplayMessage.ShowQuestion
            (
                _mainWindow.Instance,
                string.Format(CultureInfo.CurrentCulture, Strings.deleteFolderQuestion, treeNode.Text),
                string.Empty,
                _mainWindow.RightToLeft,
                Telerik.WinControls.RadMessageIcon.Error
            );
            if (dialogResult != DialogResult.OK)
                return;

            IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;

            if (mdiParent.EditControl?.SourceFile.ToLowerInvariant().Trim().Contains(treeNode.Name.ToLowerInvariant().Trim()) == true)
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.closeFolderFilesWarningFormat, treeNode.Text));

            _fileIOHelper.DeleteFolder(treeNode.Name, true);
        }

        public void DeleteProject(RadTreeNode treeNode)
        {
            DialogResult dialogResult = DisplayMessage.ShowQuestion
            (
                _mainWindow.Instance,
                string.Format(CultureInfo.CurrentCulture, Strings.deleteProjectQuestionFormat, treeNode.Text), 
                string.Empty,
                _mainWindow.RightToLeft,
                Telerik.WinControls.RadMessageIcon.Error
            );

            if (dialogResult != DialogResult.OK)
                return;

            IMDIParent mdIParent = (IMDIParent)_mainWindow.Instance;
            if (mdIParent.EditControl != null)
                mdIParent.EditControl.Close();

            //Closing the project will set _configurationService.ProjectProperties to null
            string projectFileFullName = _configurationService.ProjectProperties.ProjectFileFullName;
            string projectPath = _configurationService.ProjectProperties.ProjectPath;

            mdIParent.CloseProject();//project must be closed first to stop the explorer file system watchers

            try
            {
                _fileIOHelper.DeleteFile(projectFileFullName);
                _fileIOHelper.DeleteFolder(projectPath, true);
            }
            catch (LogicBuilderException ex)
            {
                if (Directory.Exists(projectPath))
                    throw new LogicBuilderException(Strings.unsuccessfulProjectDeletion, ex);
                else
                    throw;
            }
        }
    }
}
