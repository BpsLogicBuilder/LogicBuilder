using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class XmlDataValidationServices
    {
        internal static IServiceCollection AddXmlDataValidation(this IServiceCollection services) 
            => services
                .AddSingleton<IAssertFunctionElementValidator, AssertFunctionElementValidator>()
                .AddSingleton<IBinaryOperatorFunctionElementValidator, BinaryOperatorFunctionElementValidator>()
                .AddSingleton<ICallElementValidator, CallElementValidator>()
                .AddSingleton<IConditionsElementValidator, ConditionsElementValidator>()
                .AddSingleton<IConnectorElementValidator, ConnectorElementValidator>()
                .AddSingleton<IConstructorElementValidator, ConstructorElementValidator>()
                .AddSingleton<IConstructorGenericsConfigrationValidator, ConstructorGenericsConfigrationValidator>()
                .AddSingleton<IDecisionElementValidator, DecisionElementValidator>()
                .AddSingleton<IDecisionsElementValidator, DecisionsElementValidator>()
                .AddSingleton<IFunctionElementValidator, FunctionElementValidator>()
                .AddSingleton<IFunctionGenericsConfigrationValidator, FunctionGenericsConfigrationValidator>()
                .AddSingleton<IFunctionsElementValidator, FunctionsElementValidator>()
                .AddSingleton<IGenericsConfigrationValidator, GenericsConfigrationValidator>()
                .AddSingleton<ILiteralElementValidator, LiteralElementValidator>()
                .AddSingleton<ILiteralListElementValidator, LiteralListElementValidator>()
                .AddSingleton<ILiteralListParameterElementValidator, LiteralListParameterElementValidator>()
                .AddSingleton<ILiteralListVariableElementValidator, LiteralListVariableElementValidator>()
                .AddSingleton<ILiteralParameterElementValidator, LiteralParameterElementValidator>()
                .AddSingleton<ILiteralVariableElementValidator, LiteralVariableElementValidator>()
                .AddSingleton<IMetaObjectElementValidator, MetaObjectElementValidator>()
                .AddSingleton<IObjectElementValidator, ObjectElementValidator>()
                .AddSingleton<IObjectListElementValidator, ObjectListElementValidator>()
                .AddSingleton<IObjectListParameterElementValidator, ObjectListParameterElementValidator>()
                .AddSingleton<IObjectListVariableElementValidator, ObjectListVariableElementValidator>()
                .AddSingleton<IObjectParameterElementValidator, ObjectParameterElementValidator>()
                .AddSingleton<IObjectVariableElementValidator, ObjectVariableElementValidator>()
                .AddSingleton<IParameterElementValidator, ParameterElementValidator>()
                .AddSingleton<IParametersElementValidator, ParametersElementValidator>()
                .AddSingleton<IRetractFunctionElementValidator, RetractFunctionElementValidator>()
                .AddSingleton<IRuleChainingUpdateFunctionElementValidator, RuleChainingUpdateFunctionElementValidator>()
                .AddSingleton<IVariableElementValidator, VariableElementValidator>();
    }
}
