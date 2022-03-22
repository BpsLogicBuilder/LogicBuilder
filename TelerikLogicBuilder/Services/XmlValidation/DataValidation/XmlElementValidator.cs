using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class XmlElementValidator : IXmlElementValidator
    {
        public XmlElementValidator(IContextProvider contextProvider,
                                   IConstructorDataParser constructorDataParser,
                                   IConstructorGenericsConfigrationValidator constructorGenericsConfigrationValidator,
                                   IConstructorTypeHelper constructorTypeHelper,
                                   IGenericContructorHelper genericContructorHelper,
                                   IFunctionGenericsConfigrationValidator functionGenericsConfigrationValidator,
                                   ITypeLoadHelper typeLoadHelper,
                                   IVariableDataParser variableDataParser)
        {
            ContextProvider = contextProvider;//Must be assigned first because it is required in some of the following validator constructors.
            ConstructorDataParser = constructorDataParser;
            ConstructorGenericsConfigrationValidator = constructorGenericsConfigrationValidator;
            ConstructorTypeHelper = constructorTypeHelper;
            FunctionGenericsConfigrationValidator = functionGenericsConfigrationValidator;
            GenericContructorHelper = genericContructorHelper;
            TypeLoadHelper = typeLoadHelper;
            VariableDataParser = variableDataParser;

            AssertFunctionElementValidator = new AssertFunctionElementValidator(this);
            ConditionsElementValidator = new ConditionsElementValidator(this);
            ConnectorElementValidator = new ConnectorElementValidator(this);
            ConstructorElementValidator = new ConstructorElementValidator(this);
            DecisionElementValidator = new DecisionElementValidator(this);
            DecisionsElementValidator = new DecisionsElementValidator(this);
            FunctionElementValidator = new FunctionElementValidator(this);
            FunctionsElementValidator = new FunctionsElementValidator(this);
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

        public IConstructorDataParser ConstructorDataParser { get; }
        public IConstructorGenericsConfigrationValidator ConstructorGenericsConfigrationValidator { get; }
        public IConstructorTypeHelper ConstructorTypeHelper { get; }
        public IContextProvider ContextProvider { get; }
        public IFunctionGenericsConfigrationValidator FunctionGenericsConfigrationValidator { get; }
        public IGenericContructorHelper GenericContructorHelper { get; }
        public ITypeLoadHelper TypeLoadHelper { get; }
        public IVariableDataParser VariableDataParser { get; }

        public IAssertFunctionElementValidator AssertFunctionElementValidator { get; }

        public IConditionsElementValidator ConditionsElementValidator { get; }

        public IConnectorElementValidator ConnectorElementValidator { get; }

        public IConstructorElementValidator ConstructorElementValidator { get; }

        public IDecisionElementValidator DecisionElementValidator { get; }

        public IDecisionsElementValidator DecisionsElementValidator { get; }

        public IFunctionElementValidator FunctionElementValidator { get; }

        public IFunctionsElementValidator FunctionsElementValidator { get; }

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
