using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class XmlElementValidator : IXmlElementValidator
    {
        public XmlElementValidator(IContextProvider contextProvider,
                                   IAnyParametersHelper anyParametersHelper,
                                   IAssertFunctionDataParser assertFunctionDataParser,
                                   IConditionsDataParser conditionsDataParser,
                                   IConnectorDataParser connectorDataParser,
                                   IConstructorDataParser constructorDataParser,
                                   IConstructorGenericsConfigrationValidator constructorGenericsConfigrationValidator,
                                   IConstructorTypeHelper constructorTypeHelper,
                                   IDecisionDataParser decisionDataParser,
                                   IDecisionsDataParser decisionsDataParser,
                                   IFunctionDataParser functionDataParser,
                                   IFunctionGenericsConfigrationValidator functionGenericsConfigrationValidator,
                                   IFunctionsDataParser functionsDataParser,
                                   IGenericConstructorHelper genericConstructorHelper,
                                   IGenericFunctionHelper genericFunctionHelper,
                                   IMetaObjectDataParser metaObjectDataParser,
                                   IRetractFunctionDataParser retractFunctionDataParser,
                                   ITypeLoadHelper typeLoadHelper,
                                   IVariableDataParser variableDataParser,
                                   IVariableValueDataParser variableValueDataParser)
        {
            #region Injected 
            //Must be assigned first because they may be required in some of the manually initialized constructors.
            ContextProvider = contextProvider;
            AssertFunctionDataParser = assertFunctionDataParser;
            AnyParametersHelper = anyParametersHelper;
            ConditionsDataParser = conditionsDataParser;
            ConnectorDataParser = connectorDataParser;
            ConstructorDataParser = constructorDataParser;
            ConstructorGenericsConfigrationValidator = constructorGenericsConfigrationValidator;
            ConstructorTypeHelper = constructorTypeHelper;
            DecisionDataParser = decisionDataParser;
            DecisionsDataParser = decisionsDataParser;
            FunctionDataParser = functionDataParser;
            FunctionGenericsConfigrationValidator = functionGenericsConfigrationValidator;
            FunctionsDataParser = functionsDataParser;
            GenericConstructorHelper = genericConstructorHelper;
            GenericFunctionHelper = genericFunctionHelper;
            MetaObjectDataParser = metaObjectDataParser;
            RetractFunctionDataParser = retractFunctionDataParser;
            TypeLoadHelper = typeLoadHelper;
            VariableDataParser = variableDataParser; 
            VariableValueDataParser = variableValueDataParser;
            #endregion Injected

            AssertFunctionElementValidator = new AssertFunctionElementValidator(this);
            BinaryOperatorFunctionElementValidator = new BinaryOperatorFunctionElementValidator(this);
            CallElementValidator = new CallElementValidator(this);
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
            ParametersElementValidator = new ParametersElementValidator(this);
            RetractFunctionElementValidator = new RetractFunctionElementValidator(this);
            RuleChainingUpdateFunctionElementValidator = new RuleChainingUpdateFunctionElementValidator(this);
            VariableElementValidator = new VariableElementValidator(this);
        }

        public IAnyParametersHelper AnyParametersHelper { get; }
        public IAssertFunctionDataParser AssertFunctionDataParser { get; }
        public IConditionsDataParser ConditionsDataParser { get; }
        public IConstructorDataParser ConstructorDataParser { get; }
        public IConstructorGenericsConfigrationValidator ConstructorGenericsConfigrationValidator { get; }
        public IConstructorTypeHelper ConstructorTypeHelper { get; }
        public IContextProvider ContextProvider { get; }
        public IFunctionDataParser FunctionDataParser { get; }
        public IFunctionGenericsConfigrationValidator FunctionGenericsConfigrationValidator { get; }
        public IFunctionsDataParser FunctionsDataParser { get; }
        public IGenericConstructorHelper GenericConstructorHelper { get; }
        public IGenericFunctionHelper GenericFunctionHelper { get; }
        public IRetractFunctionDataParser RetractFunctionDataParser { get; }
        public ITypeLoadHelper TypeLoadHelper { get; }
        public IVariableDataParser VariableDataParser { get; }

        public IAssertFunctionElementValidator AssertFunctionElementValidator { get; }

        public IBinaryOperatorFunctionElementValidator BinaryOperatorFunctionElementValidator { get; }

        public ICallElementValidator CallElementValidator { get; }

        public IConditionsElementValidator ConditionsElementValidator { get; }

        public IConnectorDataParser ConnectorDataParser { get; }

        public IConnectorElementValidator ConnectorElementValidator { get; }

        public IConstructorElementValidator ConstructorElementValidator { get; }

        public IDecisionDataParser DecisionDataParser { get; }

        public IDecisionElementValidator DecisionElementValidator { get; }

        public IDecisionsDataParser DecisionsDataParser { get; }

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

        public IMetaObjectDataParser MetaObjectDataParser { get; }

        public IObjectElementValidator ObjectElementValidator { get; }

        public IObjectListElementValidator ObjectListElementValidator { get; }

        public IObjectListParameterElementValidator ObjectListParameterElementValidator { get; }

        public IObjectListVariableElementValidator ObjectListVariableElementValidator { get; }

        public IObjectParameterElementValidator ObjectParameterElementValidator { get; }

        public IObjectVariableElementValidator ObjectVariableElementValidator { get; }

        public IParameterElementValidator ParameterElementValidator { get; }

        public IParametersElementValidator ParametersElementValidator { get; }

        public IRetractFunctionElementValidator RetractFunctionElementValidator { get; }

        public IRuleChainingUpdateFunctionElementValidator RuleChainingUpdateFunctionElementValidator { get; }

        public IVariableElementValidator VariableElementValidator { get; }

        public IVariableValueDataParser VariableValueDataParser { get; }
    }
}
