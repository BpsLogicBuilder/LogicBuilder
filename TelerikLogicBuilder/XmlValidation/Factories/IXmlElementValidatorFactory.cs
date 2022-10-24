using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;

namespace ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories
{
    internal interface IXmlElementValidatorFactory
    {
        IAssertFunctionElementValidator GetAssertFunctionElementValidator();
        IBinaryOperatorFunctionElementValidator GetBinaryOperatorFunctionElementValidator();
        ICallElementValidator GetCallElementValidator();
        IConditionsElementValidator GetConditionsElementValidator();
        IConnectorElementValidator GetConnectorElementValidator();
        IConstructorElementValidator GetConstructorElementValidator();
        IDecisionElementValidator GetDecisionElementValidator();
        IDecisionsElementValidator GetDecisionsElementValidator();
        IFunctionElementValidator GetFunctionElementValidator();
        IFunctionsElementValidator GetFunctionsElementValidator();
        ILiteralElementValidator GetLiteralElementValidator();
        ILiteralListElementValidator GetLiteralListElementValidator();
        ILiteralListParameterElementValidator GetLiteralListParameterElementValidator();
        ILiteralListVariableElementValidator GetLiteralListVariableElementValidator();
        ILiteralParameterElementValidator GetLiteralParameterElementValidator();
        ILiteralVariableElementValidator GetLiteralVariableElementValidator();
        IMetaObjectElementValidator GetMetaObjectElementValidator();
        IObjectElementValidator GetObjectElementValidator();
        IObjectListElementValidator GetObjectListElementValidator();
        IObjectListParameterElementValidator GetObjectListParameterElementValidator();
        IObjectListVariableElementValidator GetObjectListVariableElementValidator();
        IObjectParameterElementValidator GetObjectParameterElementValidator();
        IObjectVariableElementValidator GetObjectVariableElementValidator();
        IParameterElementValidator GetParameterElementValidator();
        IParametersElementValidator GetParametersElementValidator();
        IRetractFunctionElementValidator GetRetractFunctionElementValidator();
        IRuleChainingUpdateFunctionElementValidator GetRuleChainingUpdateFunctionElementValidator();
        IVariableElementValidator GetVariableElementValidator();
    }
}
