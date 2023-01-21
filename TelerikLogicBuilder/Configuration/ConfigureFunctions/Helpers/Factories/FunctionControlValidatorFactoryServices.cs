using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunction;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Helpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class FunctionControlValidatorFactoryServices
    {
        internal static IServiceCollection AddFunctionControlValidatorFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureFunctionControl, IFunctionControlsValidator>>
                (
                    provider =>
                    functionControl => new FunctionControlsValidator
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFunctionValidationHelper>(),
                        provider.GetRequiredService<IStringHelper>(),
                        functionControl
                    )
                )
                .AddTransient<IFunctionControlValidatorFactory, FunctionControlValidatorFactory>();
        }
    }
}
