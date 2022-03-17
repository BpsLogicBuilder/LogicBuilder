namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation
{
    internal interface IXmlElementValidator
    {
        public IAssertFunctionElementValidator AssertFunctionElementValidator { get; }
        public IConditionsElementValidator ConditionsElementValidator { get; }
        public IConnectorElementValidator ConnectorElementValidator { get; }
        public IConstructorElementValidator ConstructorElementValidator { get; }
        public IConstructorGenericsConfigrationValidator ConstructorGenericsConfigrationValidator { get; }
        public IConstructorParametersDataValidator ConstructorParametersDataValidator { get; }
        public IDecisionElementValidator DecisionElementValidator { get; }
        public IDecisionsElementValidator DecisionsElementValidator { get; }
        public IFunctionElementValidator FunctionElementValidator { get; }
        public IFunctionGenericsConfigrationValidator FunctionGenericsConfigrationValidator { get; }
        public IFunctionParametersDataValidator FunctionParametersDataValidator { get; }
        public IFunctionsElementValidator FunctionsElementValidator { get; }
        public IGenericsConfigrationValidator GenericsConfigrationValidator { get; }
        public ILiteralElementValidator LiteralElementValidator { get; }
        public ILiteralListElementValidator LiteralListElementValidator { get; }
        public ILiteralListParameterElementValidator LiteralListParameterElementValidator { get; }
        public ILiteralListVariableElementValidator LiteralListVariableElementValidator { get; }
        public ILiteralParameterElementValidator LiteralParameterElementValidator { get; }
        public ILiteralVariableElementValidator LiteralVariableElementValidator { get; }
        public IMetaObjectElementValidator MetaObjectElementValidator { get; }
        public IObjectElementValidator ObjectElementValidator { get; }
        public IObjectListElementValidator ObjectListElementValidator { get; }
        public IObjectListParameterElementValidator ObjectListParameterElementValidator { get; }
        public IObjectListVariableElementValidator ObjectListVariableElementValidator { get; }
        public IObjectParameterElementValidator ObjectParameterElementValidator { get; }
        public IObjectVariableElementValidator ObjectVariableElementValidator { get; }
        public IParameterElementValidator ParameterElementValidator { get; }
        public IRetractFunctionElementValidator RetractFunctionElementValidator { get; }
        public IVariableElementValidator VariableElementValidator { get; }
    }
}
