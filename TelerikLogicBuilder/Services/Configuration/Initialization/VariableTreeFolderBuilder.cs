using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Initialization;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration.Initialization
{
    internal class VariableTreeFolderBuilder : IVariableTreeFolderBuilder
    {
        private readonly IContextProvider _contextProvider;

        public VariableTreeFolderBuilder(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public TreeFolder GetTreeFolder(XmlDocument xmlDocument)
            => new VariableListFolderBuilderUtility(_contextProvider).GetTreeFolder(xmlDocument);
    }
}
