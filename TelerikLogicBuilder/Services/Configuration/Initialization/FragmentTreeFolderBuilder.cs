using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Initialization;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration.Initialization
{
    internal class FragmentTreeFolderBuilder : IFragmentTreeFolderBuilder
    {
        private readonly IContextProvider _contextProvider;

        public FragmentTreeFolderBuilder(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public TreeFolder GetTreeFolder(XmlDocument xmlDocument)
            => new FragmentListFolderBuilderUtility(_contextProvider).GetTreeFolder(xmlDocument);
    }
}
