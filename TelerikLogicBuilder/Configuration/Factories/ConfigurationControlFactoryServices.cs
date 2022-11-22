using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigurationControlFactoryServices
    {
        internal static IServiceCollection AddConfigurationControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureProjectProperties, IApplicationControl>>
                (
                    provider =>
                    configureProjectProperties => new ApplicationControl
                    (
                        provider.GetRequiredService<IApplicationControlCommandFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureProjectProperties
                    )
                )
                .AddTransient<IConfigurationControlFactory, ConfigurationControlFactory>()
                .AddTransient<Func<IConfigureConnectorObjectsForm, IConfigureConnectorObjectsControl>>
                (
                    provider =>
                    configureConnectorObjectsForm => new ConfigureConnectorObjectsControl
                    (
                        provider.GetRequiredService<IConfigurationItemFactory>(),
                        provider.GetRequiredService<IConfigureConnectorObjectsCommandFactory>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        configureConnectorObjectsForm
                    )
                );
        }
    }
}
