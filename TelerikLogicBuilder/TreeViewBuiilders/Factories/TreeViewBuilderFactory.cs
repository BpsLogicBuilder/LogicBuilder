using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories
{
    internal class TreeViewBuilderFactory : ITreeViewBuilderFactory
    {
        private readonly Func<IConfigureConstructorsForm, IConfigureConstructorsTreeViewBuilder> _getConfigureConstructorsTreeViewBuilder;
        private readonly Func<IConfigureFragmentsForm, IConfigureFragmentsTreeViewBuilder> _getConfigureFragmentsTreeViewBuilder;
        private readonly Func<IConfigureFunctionsForm, IConfigureFunctionsTreeViewBuilder> _getConfigureFunctionsTreeViewBuilder;
        private readonly Func<IConfigureVariablesForm, IConfigureVariablesTreeViewBuilder> _getConfigureVariablesTreeViewBuilder;
        private readonly Func<IDictionary<string, string>, DocumentExplorerErrorsList, IDictionary<string, string>, IDocumentsExplorerTreeViewBuilder> _getDocumentsExplorerTreeViewBuilder;
        private readonly Func<ISelectConstructorControl, ISelectConstructorTreeViewBuilder> _getSelectConstructorTreeViewBuilder;
        private readonly Func<ISelectFunctionControl, ISelectFunctionTreeViewBuilder> _getSelectFunctionTreeViewBuilder;
        private readonly Func<ISelectVariableControl, ISelectVariableTreeViewBuilder> _getSelectVariableTreeViewBuilder;
        private readonly Func<IDictionary<string, string>, IRulesExplorerTreeViewBuilder> _getRulesExplorerTreeViewBuilder;

        public TreeViewBuilderFactory(
            Func<IConfigureConstructorsForm, IConfigureConstructorsTreeViewBuilder> getConfigureConstructorsTreeViewBuilder,
            Func<IConfigureFragmentsForm, IConfigureFragmentsTreeViewBuilder> getConfigureFragmentsTreeViewBuilder,
            Func<IConfigureFunctionsForm, IConfigureFunctionsTreeViewBuilder> getConfigureFunctionsTreeViewBuilder,
            Func<IConfigureVariablesForm, IConfigureVariablesTreeViewBuilder> getConfigureVariablesTreeViewBuilder,
            Func<IDictionary<string, string>, DocumentExplorerErrorsList, IDictionary<string, string>, IDocumentsExplorerTreeViewBuilder> getDocumentsExplorerTreeViewBuilder,
            Func<ISelectConstructorControl, ISelectConstructorTreeViewBuilder> getSelectConstructorTreeViewBuilder,
            Func<ISelectFunctionControl, ISelectFunctionTreeViewBuilder> getSelectFunctionTreeViewBuilder,
            Func<ISelectVariableControl, ISelectVariableTreeViewBuilder> getSelectVariableTreeViewBuilder,
            Func<IDictionary<string, string>, IRulesExplorerTreeViewBuilder> getRulesExplorerTreeViewBuilder)
        {
            _getConfigureConstructorsTreeViewBuilder = getConfigureConstructorsTreeViewBuilder;
            _getConfigureFragmentsTreeViewBuilder = getConfigureFragmentsTreeViewBuilder;
            _getConfigureFunctionsTreeViewBuilder = getConfigureFunctionsTreeViewBuilder;
            _getConfigureVariablesTreeViewBuilder = getConfigureVariablesTreeViewBuilder;
            _getDocumentsExplorerTreeViewBuilder = getDocumentsExplorerTreeViewBuilder;
            _getSelectConstructorTreeViewBuilder = getSelectConstructorTreeViewBuilder;
            _getSelectFunctionTreeViewBuilder = getSelectFunctionTreeViewBuilder;
            _getSelectVariableTreeViewBuilder = getSelectVariableTreeViewBuilder;
            _getRulesExplorerTreeViewBuilder = getRulesExplorerTreeViewBuilder;
        }

        public IConfigureConstructorsTreeViewBuilder GetConfigureConstructorsTreeViewBuilder(IConfigureConstructorsForm configureConstructorsForm)
            => _getConfigureConstructorsTreeViewBuilder(configureConstructorsForm);

        public IConfigureFragmentsTreeViewBuilder GetConfigureFragmentsTreeViewBuilder(IConfigureFragmentsForm configureFragmentsForm)
            => _getConfigureFragmentsTreeViewBuilder(configureFragmentsForm);

        public IConfigureFunctionsTreeViewBuilder GetConfigureFunctionsTreeViewBuilder(IConfigureFunctionsForm configureFunctionsForm)
            => _getConfigureFunctionsTreeViewBuilder(configureFunctionsForm);

        public IConfigureVariablesTreeViewBuilder GetConfigureVariablesTreeViewBuilder(IConfigureVariablesForm configureVariablesForm)
            => _getConfigureVariablesTreeViewBuilder(configureVariablesForm);

        public IDocumentsExplorerTreeViewBuilder GetDocumentsExplorerTreeViewBuilder(IDictionary<string, string> documentNames, DocumentExplorerErrorsList documentProfileErrors, IDictionary<string, string> expandedNodes)
            => _getDocumentsExplorerTreeViewBuilder(documentNames, documentProfileErrors, expandedNodes);

        public ISelectConstructorTreeViewBuilder GetSelectConstructorTreeViewBuilder(ISelectConstructorControl selectConstructorControl)
            => _getSelectConstructorTreeViewBuilder(selectConstructorControl);

        public ISelectFunctionTreeViewBuilder GetSelectFunctionTreeViewBuilder(ISelectFunctionControl selectFunctionControl)
            => _getSelectFunctionTreeViewBuilder(selectFunctionControl);

        public ISelectVariableTreeViewBuilder GetSelectVariableTreeViewBuilder(ISelectVariableControl selectVariableControl)
            => _getSelectVariableTreeViewBuilder(selectVariableControl);

        public IRulesExplorerTreeViewBuilder GetRulesExplorerTreeViewBuilder(IDictionary<string, string> expandedNodes)
            => _getRulesExplorerTreeViewBuilder(expandedNodes);
    }
}
