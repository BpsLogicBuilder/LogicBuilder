using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories
{
    internal interface IXmlTreeViewSynchronizerFactory
    {
        IConfigurationFormXmlTreeViewSynchronizer GetConfigurationFormXmlTreeViewSynchronizer(IConfigurationForm configurationForm, IComparer<RadTreeNode> treeNodeComparer);
        IConfigureConstructorsXmlTreeViewSynchronizer GetConfigureConstructorsXmlTreeViewSynchronizer(IConfigureConstructorsForm configureConstructorsForm);
        IConfigureFragmentsXmlTreeViewSynchronizer GetConfigureFragmentsXmlTreeViewSynchronizer(IConfigureFragmentsForm configureFragmentsForm);
        IConfigureFunctionsXmlTreeViewSynchronizer GetConfigureFunctionsXmlTreeViewSynchronizer(IConfigureFunctionsForm configureFunctionsForm);
        IConfigureGenericArgumentsXmlTreeViewSynchronizer GetConfigureGenericArgumentsXmlTreeViewSynchronizer(IConfigureGenericArgumentsForm configureGenericArgumentsForm);
        IProjectPropertiesXmlTreeViewSynchronizer GetProjectPropertiesXmlTreeViewSynchronizer(IConfigureProjectPropertiesForm configureProjectProperties);
        IConfigureVariablesXmlTreeViewSynchronizer GetConfigureVariablesXmlTreeViewSynchronizer(IConfigureVariablesForm configureVariablesForm);
    }
}
