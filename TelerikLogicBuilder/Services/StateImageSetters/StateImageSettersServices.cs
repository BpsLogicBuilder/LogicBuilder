using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.StateImageSetters;
using ABIS.LogicBuilder.FlowBuilder.Services.StateImageSetters;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class StateImageSettersServices
    {
        internal static IServiceCollection AddStateImageSetters(this IServiceCollection services)
        {
            return services
                .AddSingleton<IConfigurationFolderStateImageSetter, ConfigurationFolderStateImageSetter>()
                .AddSingleton<IConfigureConstructorsStateImageSetter, ConfigureConstructorsStateImageSetter>()
                .AddSingleton<IConfigureFunctionsStateImageSetter, ConfigureFunctionsStateImageSetter>()
                .AddSingleton<IConfigureParametersStateImageSetter, ConfigureParametersStateImageSetter>()
                .AddSingleton<IConfigureVariablesStateImageSetter, ConfigureVariablesStateImageSetter>();
        }
    }
}
