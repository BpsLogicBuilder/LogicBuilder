using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using System.IO;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.TreeViewBuiilders
{
    internal class SelectDocunentsTreeViewBuilder : ISelectDocunentsTreeViewBuilder
    {
        private readonly IConfigurationService _configurationService;
        private readonly IEmptyFolderRemover _emptyFolderRemover;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IPathHelper _pathHelper;
        private readonly ITreeViewService _treeViewService;

        public SelectDocunentsTreeViewBuilder(IConfigurationService configurationService, IEmptyFolderRemover emptyFolderRemover, IExceptionHelper exceptionHelper, IPathHelper pathHelper, ITreeViewService treeViewService)
        {
            _configurationService = configurationService;
            _emptyFolderRemover = emptyFolderRemover;
            _exceptionHelper = exceptionHelper;
            _pathHelper = pathHelper;
            _treeViewService = treeViewService;
        }

        public void Build(RadTreeView treeView)
        {
            treeView.TriStateMode = true;
            treeView.ImageList = _treeViewService.ImageList;
            treeView.Nodes.Clear();

            string documentPath = _pathHelper.CombinePaths(_configurationService.ProjectProperties.ProjectPath, ProjectPropertiesConstants.SOURCEDOCUMENTFOLDER);
            if (!Directory.Exists(documentPath))
                Directory.CreateDirectory(documentPath);

            treeView.BeginUpdate();
            RadTreeNode rootNode = new()
            {
                ImageIndex = TreeNodeImageIndexes.PROJECTFOLDERIMAGEINDEX,
                Text = _configurationService.ProjectProperties.ProjectName,
                Name = documentPath
            };

            treeView.Nodes.Add(rootNode);
            AddDocumentNodes(rootNode, documentPath, true);
            _emptyFolderRemover.RemoveEmptyFolders(rootNode);
            treeView.Refresh();
            treeView.EndUpdate();
        }

        private void AddDocumentNodes(RadTreeNode treeNode, string directoryPath, bool root = false)
        {
            DirectoryInfo directoryInfo = new(directoryPath);

            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                string fileExtension = fileInfo.Extension.ToLowerInvariant();
                if (!FileExtensions.DocumentExtensions.Contains(fileExtension))
                    continue;

                RadTreeNode childNode = new()
                {
                    ImageIndex = GetImageIndex(),
                    Name = fileInfo.FullName,
                    Text = fileInfo.Name
                };

                treeNode.Nodes.Add(childNode);
                if (root)
                    _treeViewService.MakeVisible(childNode);

                int GetImageIndex() 
                    => fileExtension switch
                    {
                        FileExtensions.VSDXFILEEXTENSION => TreeNodeImageIndexes.VSDXFILEIMAGEINDEX,
                        FileExtensions.VISIOFILEEXTENSION => TreeNodeImageIndexes.VISIOFILEIMAGEINDEX,
                        FileExtensions.TABLEFILEEXTENSION => TreeNodeImageIndexes.TABLEFILEIMAGEINDEX,
                        _ => throw _exceptionHelper.CriticalException("{C4BCFFEF-9F8B-4638-B0FC-839265B926FD}"),
                    };
            }

            foreach (DirectoryInfo subDirectoryInfo in directoryInfo.GetDirectories())
            {
                RadTreeNode childNode = new()
                {
                    ImageIndex = TreeNodeImageIndexes.CLOSEDFOLDERIMAGEINDEX,
                    Name = subDirectoryInfo.FullName,
                    Text = subDirectoryInfo.Name
                };

                treeNode.Nodes.Add(childNode);
                if (root)
                    _treeViewService.MakeVisible(childNode);

                AddDocumentNodes(childNode, _pathHelper.CombinePaths(directoryPath, subDirectoryInfo.Name));
            }
        }
    }
}
