using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using System.Collections.Generic;
using System.Data;
using WorkflowRules = LogicBuilder.Workflow.Activities.Rules;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.RuleBuilders
{
    internal class TableRowRuleBuilder : ITableRowRuleBuilder
    {
        private readonly IContextProvider _contextProvider;
        private readonly IAnyParametersHelper _anyParametersHelper;
        private readonly IAssertFunctionDataParser _assertFunctionDataParser;
        private readonly ICellXmlHelper _cellXmlHelper;
        private readonly IConditionsDataParser _conditionsDataParser;
        private readonly IConfigurationService _configurationService;
        private readonly IConstructorDataParser _constructorDataParser;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IFunctionsDataParser _functionsDataParser;
        private readonly IGetValidConfigurationFromData _getValidConfigurationFromData;
        private readonly ILiteralListDataParser _literalListDataParser;
        private readonly ILiteralListParameterDataParser _literalListParameterDataParser;
        private readonly ILiteralListVariableDataParser _literalListVariableDataParser;
        private readonly IMetaObjectDataParser _metaObjectDataParser;
        private readonly IObjectDataParser _objectDataParser;
        private readonly IObjectListDataParser _objectListDataParser;
        private readonly IObjectListParameterDataParser _objectListParameterDataParser;
        private readonly IObjectListVariableDataParser _objectListVariableDataParser;
        private readonly IObjectParameterDataParser _objectParameterDataParser;
        private readonly IObjectVariableDataParser _objectVariableDataParser;
        private readonly IParameterHelper _parameterHelper;
        private readonly IPriorityDataParser _priorityDataParser;
        private readonly IRetractFunctionDataParser _retractFunctionDataParser;
        private readonly ITableResourcesManager _tableResourcesManager;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IVariableDataParser _variableDataParser;
        private readonly IVariableValueDataParser _variableValueDataParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public TableRowRuleBuilder(IContextProvider contextProvider, IAnyParametersHelper anyParametersHelper, IAssertFunctionDataParser assertFunctionDataParser, ICellXmlHelper cellXmlHelper, IConditionsDataParser conditionsDataParser, IConfigurationService configurationService, IConstructorDataParser constructorDataParser, IExceptionHelper exceptionHelper, IFunctionDataParser functionDataParser, IFunctionsDataParser functionsDataParser, IGetValidConfigurationFromData getValidConfigurationFromData, ILiteralListDataParser literalListDataParser, ILiteralListParameterDataParser literalListParameterDataParser, ILiteralListVariableDataParser literalListVariableDataParser, IMetaObjectDataParser metaObjectDataParser, IObjectDataParser objectDataParser, IObjectListDataParser objectListDataParser, IObjectListParameterDataParser objectListParameterDataParser, IObjectListVariableDataParser objectListVariableDataParser, IObjectParameterDataParser objectParameterDataParser, IObjectVariableDataParser objectVariableDataParser, IParameterHelper parameterHelper, IPriorityDataParser priorityDataParser, IRetractFunctionDataParser retractFunctionDataParser, ITableResourcesManager tableResourcesManager, ITypeLoadHelper typeLoadHelper, IVariableDataParser variableDataParser, IVariableValueDataParser variableValueDataParser, IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _contextProvider = contextProvider;
            _anyParametersHelper = anyParametersHelper;
            _assertFunctionDataParser = assertFunctionDataParser;
            _cellXmlHelper = cellXmlHelper;
            _conditionsDataParser = conditionsDataParser;
            _configurationService = configurationService;
            _constructorDataParser = constructorDataParser;
            _exceptionHelper = exceptionHelper;
            _functionDataParser = functionDataParser;
            _functionsDataParser = functionsDataParser;
            _getValidConfigurationFromData = getValidConfigurationFromData;
            _literalListDataParser = literalListDataParser;
            _literalListParameterDataParser = literalListParameterDataParser;
            _literalListVariableDataParser = literalListVariableDataParser;
            _metaObjectDataParser = metaObjectDataParser;
            _objectDataParser = objectDataParser;
            _objectListDataParser = objectListDataParser;
            _objectListParameterDataParser = objectListParameterDataParser;
            _objectListVariableDataParser = objectListVariableDataParser;
            _objectParameterDataParser = objectParameterDataParser;
            _objectVariableDataParser = objectVariableDataParser;
            _parameterHelper = parameterHelper;
            _priorityDataParser = priorityDataParser;
            _retractFunctionDataParser = retractFunctionDataParser;
            _tableResourcesManager = tableResourcesManager;
            _typeLoadHelper = typeLoadHelper;
            _variableDataParser = variableDataParser;
            _variableValueDataParser = variableValueDataParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public IList<WorkflowRules.Rule> GenerateRules(DataRow dataRow, string moduleName, int ruleCount, ApplicationTypeInfo application, IDictionary<string, string> resourceStrings)
        {
            var ruleBuilder = new TableRowRuleBuilderUtility
            (
                dataRow,
                moduleName,
                ruleCount,
                application,
                resourceStrings,
                _contextProvider,
                _anyParametersHelper,
                _assertFunctionDataParser,
                _cellXmlHelper,
                _conditionsDataParser,
                _configurationService,
                _constructorDataParser,
                _exceptionHelper,
                _functionDataParser,
                _functionsDataParser,
                _getValidConfigurationFromData,
                _literalListDataParser,
                _literalListParameterDataParser,
                _literalListVariableDataParser,
                _metaObjectDataParser,
                _objectDataParser,
                _objectListDataParser,
                _objectListParameterDataParser,
                _objectListVariableDataParser,
                _objectParameterDataParser,
                _objectVariableDataParser,
                _parameterHelper,
                _priorityDataParser,
                _retractFunctionDataParser,
                _tableResourcesManager,
                _typeLoadHelper,
                _variableDataParser,
                _variableValueDataParser,
                _xmlDocumentHelpers
            );

            ruleBuilder.GenerateRules();
            return ruleBuilder.Rules;
        }
    }
}
