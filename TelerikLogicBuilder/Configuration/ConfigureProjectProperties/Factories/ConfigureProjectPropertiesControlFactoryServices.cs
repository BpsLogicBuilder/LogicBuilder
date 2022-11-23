using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories
{
    internal static class ConfigureProjectPropertiesControlFactoryServices
    {
        internal static IServiceCollection AddConfigureProjectPropertiesControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureProjectPropertiesForm, IApplicationControl>>
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
                .AddTransient<IConfigureProjectPropertiesControlFactory, ConfigureProjectPropertiesControlFactory>();
        }
    }
}
