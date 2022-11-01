using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Services;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System;
using Telerik.WinControls.UI;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class FactoryServices
    {
        internal static IServiceCollection AddFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<SchemaName, ITreeViewXmlDocumentHelper>>
                (
                    provider =>
                    schema => new TreeViewXmlDocumentHelper
                    (
                        provider.GetRequiredService<IXmlValidatorFactory>(),
                        schema
                    )
                )
                .AddTransient<IServiceFactory, ServiceFactory>();
        }
    }
}
