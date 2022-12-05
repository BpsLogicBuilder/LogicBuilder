using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories
{
    internal class TreeViewBuilderFactory : ITreeViewBuilderFactory
    {
        private readonly Func<IConfigureVariablesForm, IConfigureVariablesTreeViewBuilder> _getConfigureVariablesTreeViewBuilder;
        private readonly Func<IDictionary<string, string>, DocumentExplorerErrorsList, IDictionary<string, string>, IDocumentsExplorerTreeViewBuilder> _getDocumentsExplorerTreeViewBuilder;
        private readonly Func<IDictionary<string, string>, IRulesExplorerTreeViewBuilder> _getRulesExplorerTreeViewBuilder;

        public TreeViewBuilderFactory(
            Func<IConfigureVariablesForm, IConfigureVariablesTreeViewBuilder> getConfigureVariablesTreeViewBuilder,
            Func<IDictionary<string, string>, DocumentExplorerErrorsList, IDictionary<string, string>, IDocumentsExplorerTreeViewBuilder> getDocumentsExplorerTreeViewBuilder,
            Func<IDictionary<string, string>, IRulesExplorerTreeViewBuilder> getRulesExplorerTreeViewBuilder)
        {
            _getConfigureVariablesTreeViewBuilder = getConfigureVariablesTreeViewBuilder;
            _getDocumentsExplorerTreeViewBuilder = getDocumentsExplorerTreeViewBuilder;
            _getRulesExplorerTreeViewBuilder = getRulesExplorerTreeViewBuilder;
        }

        public IConfigureVariablesTreeViewBuilder GetConfigureVariablesTreeViewBuilder(IConfigureVariablesForm configureVariablesForm)
            => _getConfigureVariablesTreeViewBuilder(configureVariablesForm);

        public IDocumentsExplorerTreeViewBuilder GetDocumentsExplorerTreeViewBuilder(IDictionary<string, string> documentNames, DocumentExplorerErrorsList documentProfileErrors, IDictionary<string, string> expandedNodes)
            => _getDocumentsExplorerTreeViewBuilder(documentNames, documentProfileErrors, expandedNodes);

        public IRulesExplorerTreeViewBuilder GetRulesExplorerTreeViewBuilder(IDictionary<string, string> expandedNodes)
            => _getRulesExplorerTreeViewBuilder(expandedNodes);
    }
}
