using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructor;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructor.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructorsFolder;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Helpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.StateImageSetters;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureConstructorsControlFactoryServices
    {
        internal static IServiceCollection AddConfigureConstructorsControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureConstructorsForm, IConfigureConstructorControl>>
                (
                    provider =>
                    configureConstructorsForm => new ConfigureConstructorControl
                    (
                        provider.GetRequiredService<IConfigureConstructorControlCommandFactory>(),
                        provider.GetRequiredService<IConfigureConstructorsStateImageSetter>(),
                        provider.GetRequiredService<IConstructorControlValidatorFactory>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureConstructorsForm
                    )
                )
                .AddTransient<IConfigureConstructorsControlFactory, ConfigureConstructorsControlFactory>()
                .AddTransient<Func<IConfigureConstructorsForm, IConfigureConstructorsFolderControl>>
                (
                    provider =>
                    configureConstructorsForm => new ConfigureConstructorsFolderControl
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureConstructorsForm
                    )
                );
        }
    }
}
