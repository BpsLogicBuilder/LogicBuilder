using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories
{
    internal interface ITreeViewBuilderFactory
    {
        IConfigureConstructorsTreeViewBuilder GetConfigureConstructorsTreeViewBuilder(IConfigureConstructorsForm configureConstructorsForm);

        IConfigureFunctionsTreeViewBuilder GetConfigureFunctionsTreeViewBuilder(IConfigureFunctionsForm configureFunctionsForm);

        IConfigureFragmentsTreeViewBuilder GetConfigureFragmentsTreeViewBuilder(IConfigureFragmentsForm configureFragmentsForm);

        IConfigureVariablesTreeViewBuilder GetConfigureVariablesTreeViewBuilder(IConfigureVariablesForm configureVariablesForm);

        IDocumentsExplorerTreeViewBuilder GetDocumentsExplorerTreeViewBuilder(IDictionary<string, string> documentNames,
            DocumentExplorerErrorsList documentProfileErrors,
            IDictionary<string, string> expandedNodes);

        IEditVariableTreeViewBuilder GetEditVariableTreeViewBuilder(IEditVariableControl editVariableControl);

        IRulesExplorerTreeViewBuilder GetRulesExplorerTreeViewBuilder(IDictionary<string, string> expandedNodes);
    }
}
