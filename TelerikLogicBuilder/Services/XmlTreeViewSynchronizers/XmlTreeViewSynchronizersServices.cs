using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.Services.XmlTreeViewSynchronizers;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class XmlTreeViewSynchronizersServices
    {
        internal static IServiceCollection AddXmlTreeViewSynchronizers(this IServiceCollection services)
        {
            return services
                .AddSingleton<IConstructorsFormTreeNodeComparer, ConstructorsFormTreeNodeComparer>()
                .AddSingleton<IFragmentsFormTreeNodeComparer, FragmentsFormTreeNodeComparer>()
                .AddSingleton<IFunctionsFormTreeNodeComparer, FunctionsFormTreeNodeComparer>()
                .AddSingleton<IVariablesFormTreeNodeComparer, VariablesFormTreeNodeComparer>()
                .AddConfigurationFormChildNodesRenamerFactories()
                .AddXmlTreeViewSynchronizerFactories();
        }
    }
}
