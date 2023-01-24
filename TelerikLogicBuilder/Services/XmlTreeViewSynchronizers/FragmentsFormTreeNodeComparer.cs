using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlTreeViewSynchronizers
{
    internal class FragmentsFormTreeNodeComparer : IFragmentsFormTreeNodeComparer
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;

        public FragmentsFormTreeNodeComparer(
            IExceptionHelper exceptionHelper,
            ITreeViewService treeViewService)
        {
            _exceptionHelper = exceptionHelper;
            _treeViewService = treeViewService;
        }

        public int Compare(RadTreeNode? treeNodeA, RadTreeNode? treeNodeB)
        {
            if (treeNodeA == null || treeNodeB == null)
                throw _exceptionHelper.CriticalException("{8C9ED591-8F48-4A00-BC26-1518B5185B86}");

            if ((_treeViewService.IsFileNode(treeNodeA) && _treeViewService.IsFileNode(treeNodeB)) 
                || (_treeViewService.IsFolderNode(treeNodeA) && _treeViewService.IsFolderNode(treeNodeB)))
                return string.Compare(treeNodeA.Text, treeNodeB.Text);
            else
                return _treeViewService.IsFileNode(treeNodeA) ? -1 : 1;
        }
    }
}
