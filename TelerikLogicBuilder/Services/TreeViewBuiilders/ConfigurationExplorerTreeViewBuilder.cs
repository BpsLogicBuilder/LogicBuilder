using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Globalization;
using System.IO;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.TreeViewBuiilders
{
    internal class ConfigurationExplorerTreeViewBuilder : IConfigurationExplorerTreeViewBuilder
    {
        private readonly IConfigurationService _configurationService;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IImageListService _imageListService;
        private readonly ITreeViewService _treeViewService;
        private readonly UiNotificationService _uiNotificationService;

        public ConfigurationExplorerTreeViewBuilder(
            IConfigurationService configurationService,
            IFileIOHelper fileIOHelper,
            IImageListService imageListService,
            ITreeViewService treeViewService,
            UiNotificationService uiNotificationService)
        {
            _configurationService = configurationService;
            _fileIOHelper = fileIOHelper;
            _imageListService = imageListService;
            _treeViewService = treeViewService;
            _uiNotificationService = uiNotificationService;
        }

        public void Build(RadTreeView treeView)
        {
            treeView.BeginUpdate();

            treeView.ShowRootLines = false;
            treeView.ImageList = _imageListService.ImageList;
            treeView.Nodes.Clear();
            string documentPath = _configurationService.ProjectProperties.ProjectPath;

            try
            {
                if (!Directory.Exists(documentPath))
                    _fileIOHelper.CreateDirectory(documentPath);

                StateImageRadTreeNode rootNode = new()
                {
                    ImageIndex = ImageIndexes.PROJECTFOLDERIMAGEINDEX,
                    Text = _configurationService.ProjectProperties.ProjectName,
                    Name = documentPath
                };
                treeView.Nodes.Add(rootNode);

                AddFileNodes(rootNode, documentPath);
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
                return;
            }

            treeView.EndUpdate();
        }

        private void AddFileNodes(StateImageRadTreeNode treeNode, string directoryPath)
        {
            DirectoryInfo directoryInfo = new(directoryPath);
            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                if (
                        string.Compare(fileInfo.Name, $"{_configurationService.ProjectProperties.ProjectName}{FileExtensions.PROJECTFILEEXTENSION}", true, CultureInfo.InvariantCulture) == 0
                        || string.Compare(fileInfo.Name, ConfigurationFiles.Variables, true, CultureInfo.InvariantCulture) == 0
                        || string.Compare(fileInfo.Name, ConfigurationFiles.Functions, true, CultureInfo.InvariantCulture) == 0
                        || string.Compare(fileInfo.Name, ConfigurationFiles.Constructors, true, CultureInfo.InvariantCulture) == 0
                        || string.Compare(fileInfo.Name, ConfigurationFiles.Fragments, true, CultureInfo.InvariantCulture) == 0
                    )
                {
                    StateImageRadTreeNode childNode = new()
                    {
                        ImageIndex = ImageIndexes.FILEIMAGEINDEX,
                        Name = fileInfo.FullName,
                        Text = fileInfo.Name
                    };
                    treeNode.Nodes.Add(childNode);
                    _treeViewService.MakeVisible(childNode);
                }
            }
        }
    }
}
