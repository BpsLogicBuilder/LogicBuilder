using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories
{
    internal class XmlTreeViewSynchronizerFactory : IXmlTreeViewSynchronizerFactory
    {
        private readonly Func<IConfigureGenericArgumentsForm, IConfigureGenericArgumentsXmlTreeViewSynchronizer> _getConfigureGenericArgumentsXmlTreeViewSynchronizer;
        private readonly Func<IConfigureProjectPropertiesForm, IProjectPropertiesXmlTreeViewSynchronizer> _getProjectPropertiesXmlTreeViewSynchronizer;

        public XmlTreeViewSynchronizerFactory(
            Func<IConfigureGenericArgumentsForm, IConfigureGenericArgumentsXmlTreeViewSynchronizer> getConfigureGenericArgumentsXmlTreeViewSynchronizer,
            Func<IConfigureProjectPropertiesForm, IProjectPropertiesXmlTreeViewSynchronizer> getProjectPropertiesXmlTreeViewSynchronizer)
        {
            _getConfigureGenericArgumentsXmlTreeViewSynchronizer = getConfigureGenericArgumentsXmlTreeViewSynchronizer;
            _getProjectPropertiesXmlTreeViewSynchronizer = getProjectPropertiesXmlTreeViewSynchronizer;
        }

        public IConfigureGenericArgumentsXmlTreeViewSynchronizer GetConfigureGenericArgumentsXmlTreeViewSynchronizer(IConfigureGenericArgumentsForm configureGenericArgumentsForm)
            => _getConfigureGenericArgumentsXmlTreeViewSynchronizer(configureGenericArgumentsForm);

        public IProjectPropertiesXmlTreeViewSynchronizer GetProjectPropertiesXmlTreeViewSynchronizer(IConfigureProjectPropertiesForm configureProjectProperties)
            => _getProjectPropertiesXmlTreeViewSynchronizer(configureProjectProperties);
    }
}
