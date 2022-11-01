using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Factories
{
    internal class ServiceFactory : IServiceFactory
    {
        private readonly Func<SchemaName, ITreeViewXmlDocumentHelper> _getTreeViewXmlDocumentHelper;

        public ServiceFactory(Func<SchemaName, ITreeViewXmlDocumentHelper> getTreeViewXmlDocumentHelper)
        {
            _getTreeViewXmlDocumentHelper = getTreeViewXmlDocumentHelper;
        }

        public ITreeViewXmlDocumentHelper GetTreeViewXmlDocumentHelper(SchemaName schema)
            => _getTreeViewXmlDocumentHelper(schema);
    }
}
