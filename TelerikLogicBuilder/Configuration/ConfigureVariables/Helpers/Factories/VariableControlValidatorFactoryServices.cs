using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralListVariable;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralVariable;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureObjectListVariable;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureObjectVariable;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class VariableControlValidatorFactoryServices
    {
        internal static IServiceCollection AddVariableControlValidatorFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureLiteralListVariableControl, ILiteralListVariableControlsValidator>>
                (
                    provider =>
                    variableControl => new LiteralListVariableControlsValidator
                    (
                        provider.GetRequiredService<IVariableControlValidatorFactory>(),
                        variableControl
                    )
                )
                .AddTransient<Func<IConfigureLiteralVariableControl, ILiteralVariableControlsValidator>>
                (
                    provider =>
                    variableControl => new LiteralVariableControlsValidator
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<IVariableControlValidatorFactory>(),
                        variableControl
                    )
                )
                .AddTransient<Func<IConfigureObjectListVariableControl, IObjectListVariableControlsValidator>>
                (
                    provider =>
                    variableControl => new ObjectListVariableControlsValidator
                    (
                        provider.GetRequiredService<IVariableControlValidatorFactory>(),
                        variableControl
                    )
                )
                .AddTransient<Func<IConfigureObjectVariableControl, IObjectVariableControlsValidator>>
                (
                    provider =>
                    variableControl => new ObjectVariableControlsValidator
                    (
                        provider.GetRequiredService<IVariableControlValidatorFactory>(),
                        variableControl
                    )
                )
                .AddTransient<Func<IConfigureVariableControl, IVariableControlsValidator>>
                (
                    provider =>
                    variableControl => new VariableControlsValidator
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<IVariableValidationHelper>(),
                        variableControl
                    )
                )
                .AddTransient<IVariableControlValidatorFactory, VariableControlValidatorFactory>();
        }
    }
}
