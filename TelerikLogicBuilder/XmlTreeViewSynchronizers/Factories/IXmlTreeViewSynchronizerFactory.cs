using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;

namespace ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories
{
    internal interface IXmlTreeViewSynchronizerFactory
    {
        IConfigureGenericArgumentsXmlTreeViewSynchronizer GetConfigureGenericArgumentsXmlTreeViewSynchronizer(IConfigureGenericArgumentsForm configureGenericArgumentsForm);
        IProjectPropertiesXmlTreeViewSynchronizer GetProjectPropertiesXmlTreeViewSynchronizer(IConfigureProjectPropertiesForm configureProjectProperties);
    }
}
