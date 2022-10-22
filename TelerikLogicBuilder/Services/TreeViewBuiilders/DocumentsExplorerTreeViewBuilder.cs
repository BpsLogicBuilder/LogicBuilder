using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.TreeViewBuiilders
{
    internal class DocumentsExplorerTreeViewBuilder : IDocumentsExplorerTreeViewBuilder
    {
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IImageListService _imageListService;
        private readonly IPathHelper _pathHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IUiNotificationService _uiNotificationService;

        private readonly IDictionary<string, string> documentNames;
        private readonly DocumentExplorerErrorsList documentProfileErrors;
        private readonly IDictionary<string, string> expandedNodes;

        public DocumentsExplorerTreeViewBuilder(
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IFileIOHelper fileIOHelper,
            IImageListService imageListService,
            IPathHelper pathHelper,
            ITreeViewService treeViewService,
            IUiNotificationService uiNotificationService,
            IDictionary<string, string> documentNames,
            DocumentExplorerErrorsList documentProfileErrors,
            IDictionary<string, string> expandedNodes)
        {
            _configurationService = configurationService;
            _exceptionHelper = exceptionHelper;
            _fileIOHelper = fileIOHelper;
            _imageListService = imageListService;
            _pathHelper = pathHelper;
            _treeViewService = treeViewService;
            _uiNotificationService = uiNotificationService;

            this.documentNames = documentNames;
            this.documentProfileErrors = documentProfileErrors;
            this.expandedNodes = expandedNodes;
        }

        public void Build(RadTreeView treeView)
        {
            documentProfileErrors.Clear();
            documentNames.Clear();

            Point point = new(treeView.HScrollBar.Value, treeView.VScrollBar.Value);
            string? selectedNodeName = treeView.SelectedNode?.Name;

            treeView.BeginUpdate();
            Build();
            treeView.EndUpdate();

            /*ScrollToPreviousPosition does not work if executed before treeView.EndUpdate();.*/
            _treeViewService.SelectTreeNode(treeView, selectedNodeName);
            _treeViewService.ScrollToPreviousPosition(treeView, point);

            void Build()
            {
                treeView.ShowRootLines = false;
                treeView.ImageList = _imageListService.ImageList;
                treeView.TreeViewElement.ShowNodeToolTips = true;
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
                    return;
                }

                StateImageRadTreeNode rootNode = new()
                {
                    ImageIndex = ImageIndexes.PROJECTFOLDERIMAGEINDEX,
                    Text = _configurationService.ProjectProperties.ProjectName,
                    Name = documentPath
                };
                treeView.Nodes.Add(rootNode);
                AddDocumentNodes(rootNode, documentPath, true);
            }
        }

        #region Private Methods
        private void AddDocumentNodes(StateImageRadTreeNode treeNode, string directoryPath, bool root = false)
        {
            DirectoryInfo directoryInfo = new(directoryPath);
            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                string fileExtension = fileInfo.Extension.ToLowerInvariant();
                if (!FileExtensions.DocumentExtensions.Contains(fileExtension))
                    continue;

                StateImageRadTreeNode childNode = new()
                {
                    ImageIndex = GetImageIndex(),
                    Name = fileInfo.FullName,
                    Text = fileInfo.Name
                };

                treeNode.Nodes.Add(childNode);
                AddDocumentName(childNode, fileInfo.Name, childNode.FullPath);
                if (root)
                    _treeViewService.MakeVisible(childNode);

                int GetImageIndex()
                    => fileExtension switch
                    {
                        FileExtensions.VSDXFILEEXTENSION => ImageIndexes.VSDXFILEIMAGEINDEX,
                        FileExtensions.VISIOFILEEXTENSION => ImageIndexes.VISIOFILEIMAGEINDEX,
                        FileExtensions.TABLEFILEEXTENSION => ImageIndexes.TABLEFILEIMAGEINDEX,
                        _ => throw _exceptionHelper.CriticalException("{061F9EE2-AB56-4648-B109-BB3CFBF88706}"),
                    };
            }

            foreach (DirectoryInfo subDirectoryInfo in directoryInfo.GetDirectories())
            {
                StateImageRadTreeNode childNode = new()
                {
                    ImageIndex = ImageIndexes.CLOSEDFOLDERIMAGEINDEX,
                    Name = subDirectoryInfo.FullName,
                    Text = subDirectoryInfo.Name
                };

                treeNode.Nodes.Add(childNode);
                if (root)
                    _treeViewService.MakeVisible(childNode);

                AddDocumentNodes(childNode, _pathHelper.CombinePaths(directoryPath, subDirectoryInfo.Name));
                if (expandedNodes.ContainsKey(childNode.Name))
                    childNode.Expand();
            }
        }

        private void AddDocumentName(StateImageRadTreeNode treeNode, string fileName, string fileFullPath)
        {
            fileName = fileName.ToLowerInvariant().Trim();

            if (documentNames.TryGetValue(fileName, out string? existingFileFullPath))
            {
                string errorText = string.Format(CultureInfo.CurrentCulture, Strings.fileExistsExceptionMessage, existingFileFullPath);
                treeNode.StateImage = Properties.Resources.Error;
                treeNode.ToolTipText = errorText;
                documentProfileErrors.Add(errorText);
                _treeViewService.MakeVisible(treeNode);
            }
            else
            {
                documentNames.Add(fileName, fileFullPath);
            }
        }
        #endregion Private Methods
    }
}
