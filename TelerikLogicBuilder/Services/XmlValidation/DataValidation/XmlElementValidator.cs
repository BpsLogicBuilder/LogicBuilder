using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class XmlElementValidator : IXmlElementValidator
    {
        public XmlElementValidator(IConstructorGenericsConfigrationValidator constructorGenericsConfigrationValidator, IFunctionGenericsConfigrationValidator functionGenericsConfigrationValidator)
        {
            AssertFunctionElementValidator = new AssertFunctionElementValidator(this);
            ConditionsElementValidator = new ConditionsElementValidator(this);
            ConnectorElementValidator = new ConnectorElementValidator(this);
            ConstructorElementValidator = new ConstructorElementValidator(this);
            ConstructorGenericsConfigrationValidator = constructorGenericsConfigrationValidator;
            ConstructorParametersDataValidator = new ConstructorParametersDataValidator(this);
            DecisionElementValidator = new DecisionElementValidator(this);
            DecisionsElementValidator = new DecisionsElementValidator(this);
            FunctionElementValidator = new FunctionElementValidator(this);
            FunctionGenericsConfigrationValidator = functionGenericsConfigrationValidator;
            FunctionParametersDataValidator = new FunctionParametersDataValidator(this);
            FunctionsElementValidator = new FunctionsElementValidator(this);
            GenericsConfigrationValidator = new GenericsConfigrationValidator();
            LiteralElementValidator = new LiteralElementValidator(this);
            LiteralListElementValidator = new LiteralListElementValidator(this);
            LiteralListParameterElementValidator = new LiteralListParameterElementValidator(this);
            LiteralListVariableElementValidator = new LiteralListVariableElementValidator(this);
            LiteralParameterElementValidator = new LiteralParameterElementValidator(this);
            LiteralVariableElementValidator = new LiteralVariableElementValidator(this);
            MetaObjectElementValidator = new MetaObjectElementValidator(this);
            ObjectElementValidator = new ObjectElementValidator(this);
            ObjectListElementValidator = new ObjectListElementValidator(this);
            ObjectListParameterElementValidator = new ObjectListParameterElementValidator(this);
            ObjectListVariableElementValidator = new ObjectListVariableElementValidator(this);
            ObjectParameterElementValidator = new ObjectParameterElementValidator(this);
            ObjectVariableElementValidator = new ObjectVariableElementValidator(this);
            ParameterElementValidator = new ParameterElementValidator(this);
            RetractFunctionElementValidator = new RetractFunctionElementValidator(this);
            VariableElementValidator = new VariableElementValidator(this);
        }

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
