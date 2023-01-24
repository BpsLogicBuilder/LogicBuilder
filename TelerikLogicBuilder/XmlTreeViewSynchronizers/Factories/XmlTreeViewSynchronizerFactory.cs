using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using System;
using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories
{
    internal class XmlTreeViewSynchronizerFactory : IXmlTreeViewSynchronizerFactory
    {
        private readonly Func<IConfigurationForm, IComparer<RadTreeNode>, IConfigurationFormXmlTreeViewSynchronizer> _getConfigurationFormXmlTreeViewSynchronizer;
        private readonly Func<IConfigureConstructorsForm, IConfigureConstructorsXmlTreeViewSynchronizer> _getConfigureConstructorsXmlTreeViewSynchronizer;
        private readonly Func<IConfigureFragmentsForm, IConfigureFragmentsXmlTreeViewSynchronizer> _getConfigureFragmentsXmlTreeViewSynchronizer;
        private readonly Func<IConfigureFunctionsForm, IConfigureFunctionsXmlTreeViewSynchronizer> _getConfigureFunctionsXmlTreeViewSynchronizer;
        private readonly Func<IConfigureGenericArgumentsForm, IConfigureGenericArgumentsXmlTreeViewSynchronizer> _getConfigureGenericArgumentsXmlTreeViewSynchronizer;
        private readonly Func<IConfigureVariablesForm, IConfigureVariablesXmlTreeViewSynchronizer> _getConfigureVariablesXmlTreeViewSynchronizer;
        private readonly Func<IConfigureProjectPropertiesForm, IProjectPropertiesXmlTreeViewSynchronizer> _getProjectPropertiesXmlTreeViewSynchronizer;
        
        public XmlTreeViewSynchronizerFactory(
            Func<IConfigurationForm, IComparer<RadTreeNode>, IConfigurationFormXmlTreeViewSynchronizer> getConfigurationFormXmlTreeViewSynchronizer,
            Func<IConfigureConstructorsForm, IConfigureConstructorsXmlTreeViewSynchronizer> getConfigureConstructorsXmlTreeViewSynchronizer,
            Func<IConfigureFragmentsForm, IConfigureFragmentsXmlTreeViewSynchronizer> getConfigureFragmentsXmlTreeViewSynchronizer,
            Func<IConfigureFunctionsForm, IConfigureFunctionsXmlTreeViewSynchronizer> getConfigureFunctionsXmlTreeViewSynchronizer,
            Func<IConfigureGenericArgumentsForm, IConfigureGenericArgumentsXmlTreeViewSynchronizer> getConfigureGenericArgumentsXmlTreeViewSynchronizer,
            Func<IConfigureVariablesForm, IConfigureVariablesXmlTreeViewSynchronizer> getConfigureVariablesXmlTreeViewSynchronizer,
            Func<IConfigureProjectPropertiesForm, IProjectPropertiesXmlTreeViewSynchronizer> getProjectPropertiesXmlTreeViewSynchronizer)
        {
            _getConfigurationFormXmlTreeViewSynchronizer = getConfigurationFormXmlTreeViewSynchronizer;
            _getConfigureConstructorsXmlTreeViewSynchronizer = getConfigureConstructorsXmlTreeViewSynchronizer;
            _getConfigureFragmentsXmlTreeViewSynchronizer = getConfigureFragmentsXmlTreeViewSynchronizer;
            _getConfigureFunctionsXmlTreeViewSynchronizer = getConfigureFunctionsXmlTreeViewSynchronizer;
            _getConfigureGenericArgumentsXmlTreeViewSynchronizer = getConfigureGenericArgumentsXmlTreeViewSynchronizer;
            _getConfigureVariablesXmlTreeViewSynchronizer = getConfigureVariablesXmlTreeViewSynchronizer;
            _getProjectPropertiesXmlTreeViewSynchronizer = getProjectPropertiesXmlTreeViewSynchronizer;
        }

        public IConfigurationFormXmlTreeViewSynchronizer GetConfigurationFormXmlTreeViewSynchronizer(IConfigurationForm configurationForm, IComparer<RadTreeNode> treeNodeComparer)
            => _getConfigurationFormXmlTreeViewSynchronizer(configurationForm, treeNodeComparer);

        public IConfigureConstructorsXmlTreeViewSynchronizer GetConfigureConstructorsXmlTreeViewSynchronizer(IConfigureConstructorsForm configureConstructorsForm)
            => _getConfigureConstructorsXmlTreeViewSynchronizer(configureConstructorsForm);

        public IConfigureFragmentsXmlTreeViewSynchronizer GetConfigureFragmentsXmlTreeViewSynchronizer(IConfigureFragmentsForm configureFragmentsForm)
            => _getConfigureFragmentsXmlTreeViewSynchronizer(configureFragmentsForm);

        public IConfigureFunctionsXmlTreeViewSynchronizer GetConfigureFunctionsXmlTreeViewSynchronizer(IConfigureFunctionsForm configureFunctionsForm)
            => _getConfigureFunctionsXmlTreeViewSynchronizer(configureFunctionsForm);

        public IConfigureGenericArgumentsXmlTreeViewSynchronizer GetConfigureGenericArgumentsXmlTreeViewSynchronizer(IConfigureGenericArgumentsForm configureGenericArgumentsForm)
            => _getConfigureGenericArgumentsXmlTreeViewSynchronizer(configureGenericArgumentsForm);

        public IConfigureVariablesXmlTreeViewSynchronizer GetConfigureVariablesXmlTreeViewSynchronizer(IConfigureVariablesForm configureVariablesForm)
            => _getConfigureVariablesXmlTreeViewSynchronizer(configureVariablesForm);

        public IProjectPropertiesXmlTreeViewSynchronizer GetProjectPropertiesXmlTreeViewSynchronizer(IConfigureProjectPropertiesForm configureProjectProperties)
            => _getProjectPropertiesXmlTreeViewSynchronizer(configureProjectProperties);
    }
}
