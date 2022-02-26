using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Initialization;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using System;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration.Initialization
{
    internal class ConstructorTreeFolderBuilder : IConstructorTreeFolderBuilder
    {
        private readonly IContextProvider _contextProvider;

        public ConstructorTreeFolderBuilder(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public TreeFolder GetTreeFolder(XmlDocument xmlDocument) 
            => new ConstructorListFolderBuilderUtility(_contextProvider).GetTreeFolder(xmlDocument);
    }
}
