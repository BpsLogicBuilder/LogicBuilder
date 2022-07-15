using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders;
using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.TreeViewBuiilders
{
    internal class RulesExplorerTreeViewBuilder : IRulesExplorerTreeViewBuilder
    {
        private readonly IConfigurationService _configurationService;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IImageListService _imageListService;
        private readonly IPathHelper _pathHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly UiNotificationService _uiNotificationService;

        public RulesExplorerTreeViewBuilder(
            IConfigurationService configurationService,
            IFileIOHelper fileIOHelper,
            IImageListService imageListService,
            IPathHelper pathHelper,
            ITreeViewService treeViewService,
            UiNotificationService uiNotificationService)
        {
            _configurationService = configurationService;
            _fileIOHelper = fileIOHelper;
            _imageListService = imageListService;
            _pathHelper = pathHelper;
            _treeViewService = treeViewService;
            _uiNotificationService = uiNotificationService;
        }

        public void Build(RadTreeView treeView, IDictionary<string, string> expandedNodes) 
            => new RulesExplorerTreeViewBuilderUtility
            (
                _configurationService,
                _fileIOHelper,
                _imageListService,
                _pathHelper,
                _treeViewService,
                _uiNotificationService,
                expandedNodes
            ).Build(treeView);
    }
}
