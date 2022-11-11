using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories
{
    internal class XmlTreeViewSynchronizerFactory : IXmlTreeViewSynchronizerFactory
    {
        private readonly Func<IConfigureGenericArguments, IConfigureGenericArgumentsXmlTreeViewSynchronizer> _getConfigureGenericArgumentsXmlTreeViewSynchronizer;
        private readonly Func<IConfigureProjectProperties, IProjectPropertiesXmlTreeViewSynchronizer> _getProjectPropertiesXmlTreeViewSynchronizer;

        public XmlTreeViewSynchronizerFactory(
            Func<IConfigureGenericArguments, IConfigureGenericArgumentsXmlTreeViewSynchronizer> getConfigureGenericArgumentsXmlTreeViewSynchronizer,
            Func<IConfigureProjectProperties, IProjectPropertiesXmlTreeViewSynchronizer> getProjectPropertiesXmlTreeViewSynchronizer)
        {
            _getConfigureGenericArgumentsXmlTreeViewSynchronizer = getConfigureGenericArgumentsXmlTreeViewSynchronizer;
            _getProjectPropertiesXmlTreeViewSynchronizer = getProjectPropertiesXmlTreeViewSynchronizer;
        }

        public IConfigureGenericArgumentsXmlTreeViewSynchronizer GetConfigureGenericArgumentsXmlTreeViewSynchronizer(IConfigureGenericArguments configureGenericArguments)
            => _getConfigureGenericArgumentsXmlTreeViewSynchronizer(configureGenericArguments);

        public IProjectPropertiesXmlTreeViewSynchronizer GetProjectPropertiesXmlTreeViewSynchronizer(IConfigureProjectProperties configureProjectProperties)
            => _getProjectPropertiesXmlTreeViewSynchronizer(configureProjectProperties);
    }
}
