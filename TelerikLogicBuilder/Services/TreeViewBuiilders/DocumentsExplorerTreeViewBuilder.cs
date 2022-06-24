using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.TreeViewBuiilders
{
    internal class DocumentsExplorerTreeViewBuilder : IDocumentsExplorerTreeViewBuilder
    {
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IPathHelper _pathHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly UiNotificationService _uiNotificationService;

        public DocumentsExplorerTreeViewBuilder(
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IFileIOHelper fileIOHelper,
            IPathHelper pathHelper,
            ITreeViewService treeViewService,
            UiNotificationService uiNotificationService)
        {
            _configurationService = configurationService;
            _exceptionHelper = exceptionHelper;
            _fileIOHelper = fileIOHelper;
            _pathHelper = pathHelper;
            _treeViewService = treeViewService;
            _uiNotificationService = uiNotificationService;
        }

        public void Build(RadTreeView treeView, DocumentExplorerErrorsList documentProfileErrors, IDictionary<string, string> documentNames, IDictionary<string, string> expandedNodes) 
            => new DocumentsExplorerTreeViewBuilderUtility
            (
                _configurationService,
                _exceptionHelper,
                _fileIOHelper,
                _pathHelper,
                _treeViewService,
                _uiNotificationService,
                documentNames,
                documentProfileErrors,
                expandedNodes
            ).Build(treeView);
    }
}
