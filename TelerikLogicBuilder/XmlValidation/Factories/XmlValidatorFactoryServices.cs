using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class XmlValidatorFactoryServices
    {
        internal static IServiceCollection AddXmlValidatorFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<Func<IAssertFunctionElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IAssertFunctionElementValidator>()
                )
                .AddSingleton<Func<IBinaryOperatorFunctionElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IBinaryOperatorFunctionElementValidator>()
                )
                .AddSingleton<Func<ICallElementValidator>>
                (
                    provider => () => provider.GetRequiredService<ICallElementValidator>()
                )
                .AddSingleton<Func<IConditionsElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IConditionsElementValidator>()
                )
                .AddSingleton<Func<IConnectorElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IConnectorElementValidator>()
                )
                .AddSingleton<Func<IConstructorElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IConstructorElementValidator>()
                )
                .AddSingleton<Func<IDecisionElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IDecisionElementValidator>()
                )
                .AddSingleton<Func<IDecisionsElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IDecisionsElementValidator>()
                )
                .AddSingleton<Func<IFunctionElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IFunctionElementValidator>()
                )
                .AddSingleton<Func<IFunctionsElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IFunctionsElementValidator>()
                )
                .AddSingleton<Func<ILiteralElementValidator>>
                (
                    provider => () => provider.GetRequiredService<ILiteralElementValidator>()
                )
                .AddSingleton<Func<ILiteralListElementValidator>>
                (
                    provider => () => provider.GetRequiredService<ILiteralListElementValidator>()
                )
                .AddSingleton<Func<ILiteralListParameterElementValidator>>
                (
                    provider => () => provider.GetRequiredService<ILiteralListParameterElementValidator>()
                )
                .AddSingleton<Func<ILiteralListVariableElementValidator>>
                (
                    provider => () => provider.GetRequiredService<ILiteralListVariableElementValidator>()
                )
                .AddSingleton<Func<ILiteralParameterElementValidator>>
                (
                    provider => () => provider.GetRequiredService<ILiteralParameterElementValidator>()
                )
                .AddSingleton<Func<ILiteralVariableElementValidator>>
                (
                    provider => () => provider.GetRequiredService<ILiteralVariableElementValidator>()
                )
                .AddSingleton<Func<IMetaObjectElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IMetaObjectElementValidator>()
                )
                .AddSingleton<Func<IObjectElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IObjectElementValidator>()
                )
                .AddSingleton<Func<IObjectListElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IObjectListElementValidator>()
                )
                .AddSingleton<Func<IObjectListParameterElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IObjectListParameterElementValidator>()
                )
                .AddSingleton<Func<IObjectListVariableElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IObjectListVariableElementValidator>()
                )
                .AddSingleton<Func<IObjectParameterElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IObjectParameterElementValidator>()
                )
                .AddSingleton<Func<IObjectVariableElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IObjectVariableElementValidator>()
                )
                .AddSingleton<Func<IParameterElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IParameterElementValidator>()
                )
                .AddSingleton<Func<IParametersElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IParametersElementValidator>()
                )
                .AddSingleton<Func<IRetractFunctionElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IRetractFunctionElementValidator>()
                )
                .AddSingleton<Func<IRuleChainingUpdateFunctionElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IRuleChainingUpdateFunctionElementValidator>()
                )
                .AddSingleton<Func<IVariableElementValidator>>
                (
                    provider => () => provider.GetRequiredService<IVariableElementValidator>()
                )
                .AddSingleton<IXmlElementValidatorFactory, XmlElementValidatorFactory>()
                .AddSingleton<IXmlValidatorFactory, XmlValidatorFactory>();
        }
    }
}
