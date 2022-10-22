using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories
{
    internal class TreeViewBuiilderFactory : ITreeViewBuiilderFactory
    {
        private readonly Func<IDictionary<string, string>, DocumentExplorerErrorsList, IDictionary<string, string>, IDocumentsExplorerTreeViewBuilder> _getDocumentsExplorerTreeViewBuilder;
        private readonly Func<IDictionary<string, string>, IRulesExplorerTreeViewBuilder> _getRulesExplorerTreeViewBuilder;

        public TreeViewBuiilderFactory(
            Func<IDictionary<string, string>, DocumentExplorerErrorsList, IDictionary<string, string>, IDocumentsExplorerTreeViewBuilder> getDocumentsExplorerTreeViewBuilder,
            Func<IDictionary<string, string>, IRulesExplorerTreeViewBuilder> getRulesExplorerTreeViewBuilder)
        {
            _getDocumentsExplorerTreeViewBuilder = getDocumentsExplorerTreeViewBuilder;
            _getRulesExplorerTreeViewBuilder = getRulesExplorerTreeViewBuilder;
        }

        public IDocumentsExplorerTreeViewBuilder GetDocumentsExplorerTreeViewBuilder(IDictionary<string, string> documentNames, DocumentExplorerErrorsList documentProfileErrors, IDictionary<string, string> expandedNodes)
            => _getDocumentsExplorerTreeViewBuilder(documentNames, documentProfileErrors, expandedNodes);

        public IRulesExplorerTreeViewBuilder GetRulesExplorerTreeViewBuilder(IDictionary<string, string> expandedNodes)
            => _getRulesExplorerTreeViewBuilder(expandedNodes);
    }
}
