using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
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
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IImageListService _imageListService;
        private readonly IPathHelper _pathHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly UiNotificationService _uiNotificationService;

        public SelectDocunentsTreeViewBuilder(
            IConfigurationService configurationService,
            IEmptyFolderRemover emptyFolderRemover,
            IExceptionHelper exceptionHelper,
            IFileIOHelper fileIOHelper,
            IImageListService imageListService,
            IPathHelper pathHelper,
            ITreeViewService treeViewService,
            UiNotificationService uiNotificationService)
        {
            _configurationService = configurationService;
            _emptyFolderRemover = emptyFolderRemover;
            _exceptionHelper = exceptionHelper;
            _fileIOHelper = fileIOHelper;
            _imageListService = imageListService;
            _pathHelper = pathHelper;
            _treeViewService = treeViewService;
            _uiNotificationService = uiNotificationService;
        }

        public void Build(RadTreeView treeView)
        {
            treeView.TriStateMode = true;
            treeView.ImageList = _imageListService.ImageList;
            treeView.Nodes.Clear();

            string documentPath = _pathHelper.CombinePaths(_configurationService.ProjectProperties.ProjectPath, ProjectPropertiesConstants.SOURCEDOCUMENTFOLDER);
            try
            {
                if (!Directory.Exists(documentPath))
                    _fileIOHelper.CreateDirectory(documentPath);
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }

            treeView.BeginUpdate();
            RadTreeNode rootNode = new()
            {
                ImageIndex = ImageIndexes.PROJECTFOLDERIMAGEINDEX,
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
                        FileExtensions.VSDXFILEEXTENSION => ImageIndexes.VSDXFILEIMAGEINDEX,
                        FileExtensions.VISIOFILEEXTENSION => ImageIndexes.VISIOFILEIMAGEINDEX,
                        FileExtensions.TABLEFILEEXTENSION => ImageIndexes.TABLEFILEIMAGEINDEX,
                        _ => throw _exceptionHelper.CriticalException("{C4BCFFEF-9F8B-4638-B0FC-839265B926FD}"),
                    };
            }

            foreach (DirectoryInfo subDirectoryInfo in directoryInfo.GetDirectories())
            {
                RadTreeNode childNode = new()
                {
                    ImageIndex = ImageIndexes.CLOSEDFOLDERIMAGEINDEX,
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
