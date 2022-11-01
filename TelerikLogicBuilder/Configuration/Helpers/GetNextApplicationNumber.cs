using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Globalization;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Helpers
{
    internal class GetNextApplicationNumber : IGetNextApplicationNumber
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;

        public GetNextApplicationNumber(
            IExceptionHelper exception,
            ITreeViewService treeViewService)
        {
            _exceptionHelper = exception;
            _treeViewService = treeViewService;
        }

        public int Get(RadTreeNode projectPropertiesRootNode)
        {
            if (!_treeViewService.IsProjectRootNode(projectPropertiesRootNode))
                 throw _exceptionHelper.CriticalException("{FFDE6F0F-9F6C-483D-B87B-8BA0531097DF}");

            int appNumber = 1;
            foreach (RadTreeNode node in projectPropertiesRootNode.Nodes)
            {
                string newNodeText = string.Format(CultureInfo.CurrentCulture, Strings.applicationNameFormat, appNumber.ToString("00", CultureInfo.CurrentCulture));
                if (node.Text != newNodeText)
                    return node.Index + 1;
                appNumber++;
            }
            return appNumber;
        }
    }
}
