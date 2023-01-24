using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories
{
    internal class ConfigurationFormChildNodesRenamerFactory : IConfigurationFormChildNodesRenamerFactory
    {
        private readonly Func<IConfigureConstructorsForm, IConfigureConstructorsChildNodesRenamer> _getConfigureConstructorsChildNodesRenamer;
        private readonly Func<IConfigureFragmentsForm, IConfigureFragmentsChildNodesRenamer> _getConfigureFragmentsChildNodesRenamer;
        private readonly Func<IConfigureFunctionsForm, IConfigureFunctionsChildNodesRenamer> _getConfigureFunctionsChildNodesRenamer;
        private readonly Func<IConfigureVariablesForm, IConfigureVariablesChildNodesRenamer> _getConfigureVariablesChildNodesRenamer;

        public ConfigurationFormChildNodesRenamerFactory(
            Func<IConfigureConstructorsForm, IConfigureConstructorsChildNodesRenamer> getConfigureConstructorsChildNodesRenamer,
            Func<IConfigureFragmentsForm, IConfigureFragmentsChildNodesRenamer> getConfigureFragmentsChildNodesRenamer,
            Func<IConfigureFunctionsForm, IConfigureFunctionsChildNodesRenamer> getConfigureFunctionsChildNodesRenamer,
            Func<IConfigureVariablesForm, IConfigureVariablesChildNodesRenamer> getConfigureVariablesChildNodesRenamer)
        {
            _getConfigureConstructorsChildNodesRenamer = getConfigureConstructorsChildNodesRenamer;
            _getConfigureFragmentsChildNodesRenamer = getConfigureFragmentsChildNodesRenamer;
            _getConfigureFunctionsChildNodesRenamer = getConfigureFunctionsChildNodesRenamer;
            _getConfigureVariablesChildNodesRenamer = getConfigureVariablesChildNodesRenamer;
        }

        public IConfigureConstructorsChildNodesRenamer GetConfigureConstructorsChildNodesRenamer(IConfigureConstructorsForm configureConstructorsForm)
            => _getConfigureConstructorsChildNodesRenamer(configureConstructorsForm);

        public IConfigureFragmentsChildNodesRenamer GetConfigureFragmentsChildNodesRenamer(IConfigureFragmentsForm configureFragmentsForm)
            => _getConfigureFragmentsChildNodesRenamer(configureFragmentsForm);

        public IConfigureFunctionsChildNodesRenamer GetConfigureFunctionsChildNodesRenamer(IConfigureFunctionsForm configureFunctionsForm)
            => _getConfigureFunctionsChildNodesRenamer(configureFunctionsForm);

        public IConfigureVariablesChildNodesRenamer GetConfigureVariablesChildNodesRenamer(IConfigureVariablesForm configureVariablesForm)
            => _getConfigureVariablesChildNodesRenamer(configureVariablesForm);
    }
}
