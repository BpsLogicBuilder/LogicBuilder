using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Factories
{
    internal class ServiceFactory : IServiceFactory
    {
        private readonly Func<RadTreeView, SchemaName, ITreeViewXmlDocumentHelper> _getTreeViewXmlDocumentHelper;

        public ServiceFactory(Func<RadTreeView, SchemaName, ITreeViewXmlDocumentHelper> getTreeViewXmlDocumentHelper)
        {
            _getTreeViewXmlDocumentHelper = getTreeViewXmlDocumentHelper;
        }

        public ITreeViewXmlDocumentHelper GetTreeViewXmlDocumentHelper(RadTreeView treeView, SchemaName schema)
            => _getTreeViewXmlDocumentHelper(treeView, schema);
    }
}
