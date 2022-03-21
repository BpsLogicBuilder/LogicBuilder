﻿using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation
{
    internal interface IXmlElementValidator
    {
        IAssertFunctionElementValidator AssertFunctionElementValidator { get; }
        IConditionsElementValidator ConditionsElementValidator { get; }
        IConnectorElementValidator ConnectorElementValidator { get; }
        IConstructorDataParser ConstructorDataParser { get; }
        IConstructorElementValidator ConstructorElementValidator { get; }
        IConstructorGenericsConfigrationValidator ConstructorGenericsConfigrationValidator { get; }
        IConstructorParametersDataValidator ConstructorParametersDataValidator { get; }
        IConstructorTypeHelper ConstructorTypeHelper { get; }
        IContextProvider ContextProvider { get; }
        IDecisionElementValidator DecisionElementValidator { get; }
        IDecisionsElementValidator DecisionsElementValidator { get; }
        IFunctionElementValidator FunctionElementValidator { get; }
        IFunctionGenericsConfigrationValidator FunctionGenericsConfigrationValidator { get; }
        IFunctionParametersDataValidator FunctionParametersDataValidator { get; }
        IFunctionsElementValidator FunctionsElementValidator { get; }
        IGenericContructorHelper GenericContructorHelper { get; }
        ILiteralElementValidator LiteralElementValidator { get; }
        ILiteralListElementValidator LiteralListElementValidator { get; }
        ILiteralListParameterElementValidator LiteralListParameterElementValidator { get; }
        ILiteralListVariableElementValidator LiteralListVariableElementValidator { get; }
        ILiteralParameterElementValidator LiteralParameterElementValidator { get; }
        ILiteralVariableElementValidator LiteralVariableElementValidator { get; }
        IMetaObjectElementValidator MetaObjectElementValidator { get; }
        IObjectElementValidator ObjectElementValidator { get; }
        IObjectListElementValidator ObjectListElementValidator { get; }
        IObjectListParameterElementValidator ObjectListParameterElementValidator { get; }
        IObjectListVariableElementValidator ObjectListVariableElementValidator { get; }
        IObjectParameterElementValidator ObjectParameterElementValidator { get; }
        IObjectVariableElementValidator ObjectVariableElementValidator { get; }
        IParameterElementValidator ParameterElementValidator { get; }
        IRetractFunctionElementValidator RetractFunctionElementValidator { get; }
        ITypeLoadHelper TypeLoadHelper { get; }
        IVariableElementValidator VariableElementValidator { get; }
    }
}