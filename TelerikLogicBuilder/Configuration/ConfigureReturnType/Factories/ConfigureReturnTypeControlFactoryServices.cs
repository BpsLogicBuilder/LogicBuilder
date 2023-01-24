using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.ConfigureGenericListReturnType;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.ConfigureGenericReturnType;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.ConfigureLiteralListReturnType;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.ConfigureLiteralReturnType;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.ConfigureObjectListReturnType;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.ConfigureObjectReturnType;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureReturnTypeControlFactoryServices
    {
        internal static IServiceCollection AddConfigureReturnTypeControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureReturnTypeForm, IConfigureGenericListReturnTypeControl>>
                (
                    provider =>
                    configureReturnTypeForm => new ConfigureGenericListReturnTypeControl
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IReturnTypeFactory>(),
                        configureReturnTypeForm
                    )
                )
                .AddTransient<Func<IConfigureReturnTypeForm, IConfigureGenericReturnTypeControl>>
                (
                    provider =>
                    configureReturnTypeForm => new ConfigureGenericReturnTypeControl
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IReturnTypeFactory>(),
                        configureReturnTypeForm
                    )
                )
                .AddTransient<Func<IConfigureReturnTypeForm, IConfigureLiteralListReturnTypeControl>>
                (
                    provider =>
                    configureReturnTypeForm => new ConfigureLiteralListReturnTypeControl
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IReturnTypeFactory>(),
                        configureReturnTypeForm
                    )
                )
                .AddTransient<Func<IConfigureReturnTypeForm, IConfigureLiteralReturnTypeControl>>
                (
                    provider =>
                    configureReturnTypeForm => new ConfigureLiteralReturnTypeControl
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IReturnTypeFactory>(),
                        configureReturnTypeForm
                    )
                )
                .AddTransient<Func<IConfigureReturnTypeForm, IConfigureObjectListReturnTypeControl>>
                (
                    provider =>
                    configureReturnTypeForm => new ConfigureObjectListReturnTypeControl
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IReturnTypeFactory>(),
                        configureReturnTypeForm
                    )
                )
                .AddTransient<Func<IConfigureReturnTypeForm, IConfigureObjectReturnTypeControl>>
                (
                    provider =>
                    configureReturnTypeForm => new ConfigureObjectReturnTypeControl
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IReturnTypeFactory>(),
                        configureReturnTypeForm
                    )
                )
                .AddTransient<IConfigureReturnTypeControlFactory, ConfigureReturnTypeControlFactory>(); 
        }
    }
}
