using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories
{
    internal class XmlTreeViewSynchronizerFactory : IXmlTreeViewSynchronizerFactory
    {
        private readonly Func<IConfigureProjectProperties, IProjectPropertiesXmlTreeViewSynchronizer> _getProjectPropertiesXmlTreeViewSynchronizer;

        public XmlTreeViewSynchronizerFactory(Func<IConfigureProjectProperties, IProjectPropertiesXmlTreeViewSynchronizer> getProjectPropertiesXmlTreeViewSynchronizer)
        {
            _getProjectPropertiesXmlTreeViewSynchronizer = getProjectPropertiesXmlTreeViewSynchronizer;
        }

        public IProjectPropertiesXmlTreeViewSynchronizer GetProjectPropertiesXmlTreeViewSynchronizer(IConfigureProjectProperties configureProjectProperties)
            => _getProjectPropertiesXmlTreeViewSynchronizer(configureProjectProperties);
    }
}
