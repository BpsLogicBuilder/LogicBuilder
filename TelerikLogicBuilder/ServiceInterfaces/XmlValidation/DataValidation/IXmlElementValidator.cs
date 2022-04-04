using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation
{
    internal interface IXmlElementValidator
    {
        IAnyParametersHelper AnyParametersHelper { get; }
        IAssertFunctionDataParser AssertFunctionDataParser { get; }
        IAssertFunctionElementValidator AssertFunctionElementValidator { get; }
        IBinaryOperatorFunctionElementValidator BinaryOperatorFunctionElementValidator { get; }
        ICallElementValidator CallElementValidator { get; }
        IConditionsDataParser ConditionsDataParser { get; }
        IConditionsElementValidator ConditionsElementValidator { get; }
        IConnectorElementValidator ConnectorElementValidator { get; }
        IConstructorDataParser ConstructorDataParser { get; }
        IConstructorElementValidator ConstructorElementValidator { get; }
        IConstructorGenericsConfigrationValidator ConstructorGenericsConfigrationValidator { get; }
        IConstructorTypeHelper ConstructorTypeHelper { get; }
        IContextProvider ContextProvider { get; }
        IDecisionElementValidator DecisionElementValidator { get; }
        IDecisionsElementValidator DecisionsElementValidator { get; }
        IFunctionElementValidator FunctionElementValidator { get; }
        IFunctionGenericsConfigrationValidator FunctionGenericsConfigrationValidator { get; }
        IFunctionsElementValidator FunctionsElementValidator { get; }
        IFunctionDataParser FunctionDataParser { get; }
        IGenericConstructorHelper GenericConstructorHelper { get; }
        IGenericFunctionHelper GenericFunctionHelper { get; }
        ILiteralElementValidator LiteralElementValidator { get; }
        ILiteralListElementValidator LiteralListElementValidator { get; }
        ILiteralListParameterElementValidator LiteralListParameterElementValidator { get; }
        ILiteralListVariableElementValidator LiteralListVariableElementValidator { get; }
        ILiteralParameterElementValidator LiteralParameterElementValidator { get; }
        ILiteralVariableElementValidator LiteralVariableElementValidator { get; }
        IMetaObjectElementValidator MetaObjectElementValidator { get; }
        IMetaObjectDataParser MetaObjectDataParser { get; }
        IObjectElementValidator ObjectElementValidator { get; }
        IObjectListElementValidator ObjectListElementValidator { get; }
        IObjectListParameterElementValidator ObjectListParameterElementValidator { get; }
        IObjectListVariableElementValidator ObjectListVariableElementValidator { get; }
        IObjectParameterElementValidator ObjectParameterElementValidator { get; }
        IObjectVariableElementValidator ObjectVariableElementValidator { get; }
        IParameterElementValidator ParameterElementValidator { get; }
        IParametersElementValidator ParametersElementValidator { get; }
        IRetractFunctionElementValidator RetractFunctionElementValidator { get; }
        IRuleChainingUpdateFunctionElementValidator RuleChainingUpdateFunctionElementValidator { get; }
        ITypeLoadHelper TypeLoadHelper { get; }
        IVariableDataParser VariableDataParser { get; }
        IVariableValueDataParser VariableValueDataParser { get; }
        IVariableElementValidator VariableElementValidator { get; }
    }
}
