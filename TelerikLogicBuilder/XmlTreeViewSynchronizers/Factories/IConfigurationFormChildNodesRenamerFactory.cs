using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;

namespace ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories
{
    internal interface IConfigurationFormChildNodesRenamerFactory
    {
        IConfigureConstructorsChildNodesRenamer GetConfigureConstructorsChildNodesRenamer(IConfigureConstructorsForm configureConstructorsForm);
        IConfigureFragmentsChildNodesRenamer GetConfigureFragmentsChildNodesRenamer(IConfigureFragmentsForm configureFragmentsForm);
        IConfigureFunctionsChildNodesRenamer GetConfigureFunctionsChildNodesRenamer(IConfigureFunctionsForm configureFunctionsForm);
        IConfigureVariablesChildNodesRenamer GetConfigureVariablesChildNodesRenamer(IConfigureVariablesForm configureVariablesForm);
    }
}
