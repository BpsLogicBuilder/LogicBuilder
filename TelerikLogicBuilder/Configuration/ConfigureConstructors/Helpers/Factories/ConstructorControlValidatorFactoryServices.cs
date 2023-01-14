using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructor;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Helpers.Factories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConstructorControlValidatorFactoryServices
    {
        internal static IServiceCollection AddConstructorControlValidatorFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureConstructorControl, IConstructorControlsValidator>>
                (
                    provider =>
                    constructorControl => new ConstructorControlsValidator
                    (
                        constructorControl
                    )
                )
                .AddTransient<IConstructorControlValidatorFactory, ConstructorControlValidatorFactory>();
        }
    }
}
