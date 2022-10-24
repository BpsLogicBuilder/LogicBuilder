using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories
{
    internal class XmlElementValidatorFactory : IXmlElementValidatorFactory
    {
        private readonly Func<IAssertFunctionElementValidator> _getAssertFunctionElementValidator;
        private readonly Func<IBinaryOperatorFunctionElementValidator> _getBinaryOperatorFunctionElementValidator;
        private readonly Func<ICallElementValidator> _getCallElementValidator;
        private readonly Func<IConditionsElementValidator> _getConditionsElementValidator;
        private readonly Func<IConnectorElementValidator> _getConnectorElementValidator;
        private readonly Func<IConstructorElementValidator> _getConstructorElementValidator;
        private readonly Func<IDecisionElementValidator> _getDecisionElementValidator;
        private readonly Func<IDecisionsElementValidator> _getDecisionsElementValidator;
        private readonly Func<IFunctionElementValidator> _getFunctionElementValidator;
        private readonly Func<IFunctionsElementValidator> _getFunctionsElementValidator;
        private readonly Func<ILiteralElementValidator> _getLiteralElementValidator;
        private readonly Func<ILiteralListElementValidator> _getLiteralListElementValidator;
        private readonly Func<ILiteralListParameterElementValidator> _getLiteralListParameterElementValidator;
        private readonly Func<ILiteralListVariableElementValidator> _getLiteralListVariableElementValidator;
        private readonly Func<ILiteralParameterElementValidator> _getLiteralParameterElementValidator;
        private readonly Func<ILiteralVariableElementValidator> _getLiteralVariableElementValidator;
        private readonly Func<IMetaObjectElementValidator> _getMetaObjectElementValidator;
        private readonly Func<IObjectElementValidator> _getObjectElementValidator;
        private readonly Func<IObjectListElementValidator> _getObjectListElementValidator;
        private readonly Func<IObjectListParameterElementValidator> _getObjectListParameterElementValidator;
        private readonly Func<IObjectListVariableElementValidator> _getObjectListVariableElementValidator;
        private readonly Func<IObjectParameterElementValidator> _getObjectParameterElementValidator;
        private readonly Func<IObjectVariableElementValidator> _getObjectVariableElementValidator;
        private readonly Func<IParameterElementValidator> _getParameterElementValidator;
        private readonly Func<IParametersElementValidator> _getParametersElementValidator;
        private readonly Func<IRetractFunctionElementValidator> _getRetractFunctionElementValidator;
        private readonly Func<IRuleChainingUpdateFunctionElementValidator> _getRuleChainingUpdateFunctionElementValidator;
        private readonly Func<IVariableElementValidator> _getVariableElementValidator;

        public XmlElementValidatorFactory(
            Func<IAssertFunctionElementValidator> getAssertFunctionElementValidator,
            Func<IBinaryOperatorFunctionElementValidator> getBinaryOperatorFunctionElementValidator,
            Func<ICallElementValidator> getCallElementValidator,
            Func<IConditionsElementValidator> getConditionsElementValidator,
            Func<IConnectorElementValidator> getConnectorElementValidator,
            Func<IConstructorElementValidator> getConstructorElementValidator,
            Func<IDecisionElementValidator> getDecisionElementValidator,
            Func<IDecisionsElementValidator> getDecisionsElementValidator,
            Func<IFunctionElementValidator> getFunctionElementValidator,
            Func<IFunctionsElementValidator> getFunctionsElementValidator,
            Func<ILiteralElementValidator> getLiteralElementValidator,
            Func<ILiteralListElementValidator> getLiteralListElementValidator,
            Func<ILiteralListParameterElementValidator> getLiteralListParameterElementValidator,
            Func<ILiteralListVariableElementValidator> getLiteralListVariableElementValidator,
            Func<ILiteralParameterElementValidator> getLiteralParameterElementValidator,
            Func<ILiteralVariableElementValidator> getLiteralVariableElementValidator,
            Func<IMetaObjectElementValidator> getMetaObjectElementValidator,
            Func<IObjectElementValidator> getObjectElementValidator,
            Func<IObjectListElementValidator> getObjectListElementValidator,
            Func<IObjectListParameterElementValidator> getObjectListParameterElementValidator,
            Func<IObjectListVariableElementValidator> getObjectListVariableElementValidator,
            Func<IObjectParameterElementValidator> getObjectParameterElementValidator,
            Func<IObjectVariableElementValidator> getObjectVariableElementValidator,
            Func<IParameterElementValidator> getParameterElementValidator,
            Func<IParametersElementValidator> getParametersElementValidator,
            Func<IRetractFunctionElementValidator> getRetractFunctionElementValidator,
            Func<IRuleChainingUpdateFunctionElementValidator> getRuleChainingUpdateFunctionElementValidator,
            Func<IVariableElementValidator> getVariableElementValidator)
        {
            _getAssertFunctionElementValidator = getAssertFunctionElementValidator;
            _getBinaryOperatorFunctionElementValidator = getBinaryOperatorFunctionElementValidator;
            _getCallElementValidator = getCallElementValidator;
            _getConditionsElementValidator = getConditionsElementValidator;
            _getConnectorElementValidator = getConnectorElementValidator;
            _getConstructorElementValidator = getConstructorElementValidator;
            _getDecisionElementValidator = getDecisionElementValidator;
            _getDecisionsElementValidator = getDecisionsElementValidator;
            _getFunctionElementValidator = getFunctionElementValidator;
            _getFunctionsElementValidator = getFunctionsElementValidator;
            _getLiteralElementValidator = getLiteralElementValidator;
            _getLiteralListElementValidator = getLiteralListElementValidator;
            _getLiteralListParameterElementValidator = getLiteralListParameterElementValidator;
            _getLiteralListVariableElementValidator = getLiteralListVariableElementValidator;
            _getLiteralParameterElementValidator = getLiteralParameterElementValidator;
            _getLiteralVariableElementValidator = getLiteralVariableElementValidator;
            _getMetaObjectElementValidator = getMetaObjectElementValidator;
            _getObjectElementValidator = getObjectElementValidator;
            _getObjectListElementValidator = getObjectListElementValidator;
            _getObjectListParameterElementValidator = getObjectListParameterElementValidator;
            _getObjectListVariableElementValidator = getObjectListVariableElementValidator;
            _getObjectParameterElementValidator = getObjectParameterElementValidator;
            _getObjectVariableElementValidator = getObjectVariableElementValidator;
            _getParameterElementValidator = getParameterElementValidator;
            _getParametersElementValidator = getParametersElementValidator;
            _getRetractFunctionElementValidator = getRetractFunctionElementValidator;
            _getRuleChainingUpdateFunctionElementValidator = getRuleChainingUpdateFunctionElementValidator;
            _getVariableElementValidator = getVariableElementValidator;
        }

        public IAssertFunctionElementValidator GetAssertFunctionElementValidator()
            => _getAssertFunctionElementValidator();

        public IBinaryOperatorFunctionElementValidator GetBinaryOperatorFunctionElementValidator()
            => _getBinaryOperatorFunctionElementValidator();

        public ICallElementValidator GetCallElementValidator()
            => _getCallElementValidator();

        public IConditionsElementValidator GetConditionsElementValidator()
            => _getConditionsElementValidator();

        public IConnectorElementValidator GetConnectorElementValidator()
            => _getConnectorElementValidator();

        public IConstructorElementValidator GetConstructorElementValidator()
            => _getConstructorElementValidator();

        public IDecisionElementValidator GetDecisionElementValidator()
            => _getDecisionElementValidator();

        public IDecisionsElementValidator GetDecisionsElementValidator()
            => _getDecisionsElementValidator();

        public IFunctionElementValidator GetFunctionElementValidator()
            => _getFunctionElementValidator();

        public IFunctionsElementValidator GetFunctionsElementValidator()
            => _getFunctionsElementValidator();

        public ILiteralElementValidator GetLiteralElementValidator()
            => _getLiteralElementValidator();

        public ILiteralListElementValidator GetLiteralListElementValidator()
            => _getLiteralListElementValidator();

        public ILiteralListParameterElementValidator GetLiteralListParameterElementValidator()
            => _getLiteralListParameterElementValidator();

        public ILiteralListVariableElementValidator GetLiteralListVariableElementValidator()
            => _getLiteralListVariableElementValidator();

        public ILiteralParameterElementValidator GetLiteralParameterElementValidator()
            => _getLiteralParameterElementValidator();

        public ILiteralVariableElementValidator GetLiteralVariableElementValidator()
            => _getLiteralVariableElementValidator();

        public IMetaObjectElementValidator GetMetaObjectElementValidator()
            => _getMetaObjectElementValidator();

        public IObjectElementValidator GetObjectElementValidator()
            => _getObjectElementValidator();

        public IObjectListElementValidator GetObjectListElementValidator()
            => _getObjectListElementValidator();

        public IObjectListParameterElementValidator GetObjectListParameterElementValidator()
            => _getObjectListParameterElementValidator();

        public IObjectListVariableElementValidator GetObjectListVariableElementValidator()
            => _getObjectListVariableElementValidator();

        public IObjectParameterElementValidator GetObjectParameterElementValidator()
            => _getObjectParameterElementValidator();

        public IObjectVariableElementValidator GetObjectVariableElementValidator()
            => _getObjectVariableElementValidator();

        public IParameterElementValidator GetParameterElementValidator()
            => _getParameterElementValidator();

        public IParametersElementValidator GetParametersElementValidator()
            => _getParametersElementValidator();

        public IRetractFunctionElementValidator GetRetractFunctionElementValidator()
            => _getRetractFunctionElementValidator();

        public IRuleChainingUpdateFunctionElementValidator GetRuleChainingUpdateFunctionElementValidator()
            => _getRuleChainingUpdateFunctionElementValidator();

        public IVariableElementValidator GetVariableElementValidator()
            => _getVariableElementValidator();
    }
}
