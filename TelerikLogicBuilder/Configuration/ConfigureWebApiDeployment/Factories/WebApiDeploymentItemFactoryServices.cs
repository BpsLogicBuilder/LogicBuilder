using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class WebApiDeploymentItemFactoryServices
    {
        internal static IServiceCollection AddWebApiDeploymentItemFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<string, string, string, string, WebApiDeployment>>
                (
                    provider =>
                    (postFileDataUrl, postVariablesMetaUrl, deleteRulesUrl, deleteAllRulesUrl) => new WebApiDeployment
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        postFileDataUrl,
                        postVariablesMetaUrl,
                        deleteRulesUrl,
                        deleteAllRulesUrl
                    )
                )
                .AddTransient<IWebApiDeploymentItemFactory, WebApiDeploymentItemFactory>();
        }
    }
}
