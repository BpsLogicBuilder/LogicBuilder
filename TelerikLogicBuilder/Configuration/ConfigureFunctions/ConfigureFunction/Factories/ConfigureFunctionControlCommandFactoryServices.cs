﻿using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunction;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunction.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunction.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureFunctionControlCommandFactoryServices
    {
        internal static IServiceCollection AddConfigureFunctionControlCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IConfigureFunctionControlCommandFactory, ConfigureFunctionControlCommandFactory>()
                .AddTransient<Func<IConfigureFunctionControl, ConfigureFunctionReturnTypeCommand>>
                (
                    provider =>
                    configureFunctionControl => new ConfigureFunctionReturnTypeCommand
                    (
                        provider.GetRequiredService<IReturnTypeXmlParser>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureFunctionControl
                    )
                )
                .AddTransient<Func<IConfigureFunctionControl, EditFunctionGenericArgumentsCommand>>
                (
                    provider =>
                    configureFunctionControl => new EditFunctionGenericArgumentsCommand
                    (
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureFunctionControl
                    )
                );
        }
    }
}
