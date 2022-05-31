using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.RuleBuilders
{
    internal class DecisionsRuleBuilder : IDecisionsRuleBuilder
    {
        private readonly IContextProvider _contextProvider;
        private readonly IAnyParametersHelper _anyParametersHelper;
        private readonly IAssertFunctionDataParser _assertFunctionDataParser;
        private readonly IConfigurationService _configurationService;
        private readonly IConnectorDataParser _connectorDataParser;
        private readonly IConstructorDataParser _constructorDataParser;
        private readonly IDecisionDataParser _decisionDataParser;
        private readonly IDecisionsDataParser _decisionsDataParser;
        private readonly IDiagramResourcesManager _diagramResourcesManager;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IFunctionHelper _functionHelper;
        private readonly IFunctionsDataParser _functionsDataParser;
        private readonly IGetValidConfigurationFromData _getValidConfigurationFromData;
        private readonly ILiteralListDataParser _literalListDataParser;
        private readonly ILiteralListParameterDataParser _literalListParameterDataParser;
        private readonly ILiteralListVariableDataParser _literalListVariableDataParser;
        private readonly IMetaObjectDataParser _metaObjectDataParser;
        private readonly IModuleDataParser _moduleDataParser;
        private readonly IObjectDataParser _objectDataParser;
        private readonly IObjectListDataParser _objectListDataParser;
        private readonly IObjectListParameterDataParser _objectListParameterDataParser;
        private readonly IObjectListVariableDataParser _objectListVariableDataParser;
        private readonly IObjectParameterDataParser _objectParameterDataParser;
        private readonly IObjectVariableDataParser _objectVariableDataParser;
        private readonly IParameterHelper _parameterHelper;
        private readonly IRetractFunctionDataParser _retractFunctionDataParser;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IVariableDataParser _variableDataParser;
        private readonly IVariableHelper _variableHelper;
        private readonly IVariableValueDataParser _variableValueDataParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public DecisionsRuleBuilder(
            IContextProvider contextProvider,
            IAnyParametersHelper anyParametersHelper,
            IAssertFunctionDataParser assertFunctionDataParser,
            IConfigurationService configurationService,
            IConnectorDataParser connectorDataParser,
            IConstructorDataParser constructorDataParser,
            IDecisionDataParser decisionDataParser,
            IDecisionsDataParser decisionsDataParser,
            IDiagramResourcesManager diagramResourcesManager,
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IFunctionDataParser functionDataParser,
            IFunctionHelper functionHelper,
            IFunctionsDataParser functionsDataParser,
            IGetValidConfigurationFromData getValidConfigurationFromData,
            ILiteralListDataParser literalListDataParser,
            ILiteralListParameterDataParser literalListParameterDataParser,
            ILiteralListVariableDataParser literalListVariableDataParser,
            IMetaObjectDataParser metaObjectDataParser,
            IModuleDataParser moduleDataParser,
            IObjectDataParser objectDataParser,
            IObjectListDataParser objectListDataParser,
            IObjectListParameterDataParser objectListParameterDataParser,
            IObjectListVariableDataParser objectListVariableDataParser,
            IObjectParameterDataParser objectParameterDataParser,
            IObjectVariableDataParser objectVariableDataParser,
            IParameterHelper parameterHelper,
            IRetractFunctionDataParser retractFunctionDataParser,
            IShapeXmlHelper shapeXmlHelper,
            ITypeLoadHelper typeLoadHelper,
            IVariableDataParser variableDataParser,
            IVariableHelper variableHelper,
            IVariableValueDataParser variableValueDataParser,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _contextProvider = contextProvider;
            _anyParametersHelper = anyParametersHelper;
            _assertFunctionDataParser = assertFunctionDataParser;
            _configurationService = configurationService;
            _connectorDataParser = connectorDataParser;
            _constructorDataParser = constructorDataParser;
            _decisionDataParser = decisionDataParser;
            _decisionsDataParser = decisionsDataParser;
            _diagramResourcesManager = diagramResourcesManager;
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _functionDataParser = functionDataParser;
            _functionHelper = functionHelper;
            _functionsDataParser = functionsDataParser;
            _getValidConfigurationFromData = getValidConfigurationFromData;
            _literalListDataParser = literalListDataParser;
            _literalListParameterDataParser = literalListParameterDataParser;
            _literalListVariableDataParser = literalListVariableDataParser;
            _metaObjectDataParser = metaObjectDataParser;
            _moduleDataParser = moduleDataParser;
            _objectDataParser = objectDataParser;
            _objectListDataParser = objectListDataParser;
            _objectListParameterDataParser = objectListParameterDataParser;
            _objectListVariableDataParser = objectListVariableDataParser;
            _objectParameterDataParser = objectParameterDataParser;
            _objectVariableDataParser = objectVariableDataParser;
            _parameterHelper = parameterHelper;
            _retractFunctionDataParser = retractFunctionDataParser;
            _shapeXmlHelper = shapeXmlHelper;
            _typeLoadHelper = typeLoadHelper;
            _variableDataParser = variableDataParser;
            _variableHelper = variableHelper;
            _variableValueDataParser = variableValueDataParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public IList<RuleBag> GenerateRules(IList<ShapeBag> ruleShapes, IList<Shape> ruleConnectors, string moduleName, int ruleCount, ApplicationTypeInfo application, IDictionary<string, string> resourceStrings)
        {
            var ruleBuilder = new DecisionsRuleBuilderUtility
            (
                ruleShapes,
                ruleConnectors,
                moduleName,
                ruleCount,
                application,
                resourceStrings,
                _contextProvider,
                _anyParametersHelper,
                _assertFunctionDataParser,
                _configurationService,
                _connectorDataParser,
                _constructorDataParser,
                _decisionDataParser,
                _decisionsDataParser,
                _diagramResourcesManager,
                _enumHelper,
                _exceptionHelper,
                _functionDataParser,
                _functionHelper,
                _functionsDataParser,
                _getValidConfigurationFromData,
                _literalListDataParser,
                _literalListParameterDataParser,
                _literalListVariableDataParser,
                _metaObjectDataParser,
                _moduleDataParser,
                _objectDataParser,
                _objectListDataParser,
                _objectListParameterDataParser,
                _objectListVariableDataParser,
                _objectParameterDataParser,
                _objectVariableDataParser,
                _parameterHelper,
                _retractFunctionDataParser,
                _shapeXmlHelper,
                _xmlDocumentHelpers,
                _typeLoadHelper,
                _variableDataParser,
                _variableHelper,
                _variableValueDataParser
            );

            ruleBuilder.GenerateRules();
            return ruleBuilder.Rules;
        }
    }
}
