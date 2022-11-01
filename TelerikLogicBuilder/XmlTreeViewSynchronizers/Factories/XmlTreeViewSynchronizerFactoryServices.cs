using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.Services.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class XmlTreeViewSynchronizerFactoryServices
    {
        internal static IServiceCollection AddXmlTreeViewSynchronizerFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureProjectProperties, IProjectPropertiesXmlTreeViewSynchronizer>>
                (
                    provider =>
                    configureProjectProperties => new ProjectPropertiesXmlTreeViewSynchronizer
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureProjectProperties
                    )
                )
                .AddSingleton<IXmlTreeViewSynchronizerFactory, XmlTreeViewSynchronizerFactory>();
        }
    }
}
