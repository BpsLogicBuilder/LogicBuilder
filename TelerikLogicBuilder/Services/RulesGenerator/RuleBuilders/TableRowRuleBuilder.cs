using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using LogicBuilder.Workflow.Activities.Rules;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
using WorkflowRules = LogicBuilder.Workflow.Activities.Rules;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.RuleBuilders
{
    internal class TableRowRuleBuilder : ITableRowRuleBuilder
    {
        private readonly IAssertFunctionDataParser _assertFunctionDataParser;
        private readonly ICellXmlHelper _cellXmlHelper;
        private readonly ICodeExpressionBuilder _codeExpressionBuilder;
        private readonly IConditionsDataParser _conditionsDataParser;
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IFunctionsDataParser _functionsDataParser;
        private readonly IGetValidConfigurationFromData _getValidConfigurationFromData;
        private readonly IPriorityDataParser _priorityDataParser;
        private readonly IRetractFunctionDataParser _retractFunctionDataParser;
        private readonly IVariableDataParser _variableDataParser;
        private readonly IVariableValueDataParser _variableValueDataParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public TableRowRuleBuilder(
            IAssertFunctionDataParser assertFunctionDataParser,
            ICellXmlHelper cellXmlHelper,
            IConditionsDataParser conditionsDataParser,
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IFunctionDataParser functionDataParser,
            IFunctionsDataParser functionsDataParser,
            IGetValidConfigurationFromData getValidConfigurationFromData,
            IPriorityDataParser priorityDataParser,
            IRetractFunctionDataParser retractFunctionDataParser,
            IRuleBuilderFactory ruleBuilderFactory,
            IVariableDataParser variableDataParser,
            IVariableValueDataParser variableValueDataParser,
            IXmlDocumentHelpers xmlDocumentHelpers,
            DataRow dataRow,
            string moduleName,
            int ruleCount,
            ApplicationTypeInfo application,
            IDictionary<string, string> resourceStrings)
        {
            _assertFunctionDataParser = assertFunctionDataParser;
            _cellXmlHelper = cellXmlHelper;
            _codeExpressionBuilder = ruleBuilderFactory.GetCodeExpressionBuilder
            (
                application,
                resourceStrings,
                $"{moduleName}{FileConstants.TABLESTRING}"
            );

            _conditionsDataParser = conditionsDataParser;
            _configurationService = configurationService;
            _exceptionHelper = exceptionHelper;
            _functionDataParser = functionDataParser;
            _functionsDataParser = functionsDataParser;
            _getValidConfigurationFromData = getValidConfigurationFromData;
            _priorityDataParser = priorityDataParser;
            _retractFunctionDataParser = retractFunctionDataParser;
            _variableDataParser = variableDataParser;
            _variableValueDataParser = variableValueDataParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;

            this.DataRow = dataRow;
            this.ModuleName = moduleName;
            this.RuleCount = ruleCount;
            this.Application = application;
        }

        private ApplicationTypeInfo Application { get; }
        private string ModuleName { get; }
        private int RuleCount { get; set; }
        private IList<RuleAction> ThenActions { get; } = new List<RuleAction>();
        private IList<IfCondition> Conditions { get; } = new List<IfCondition>();
        private DataRow DataRow { get; }

        public IList<WorkflowRules.Rule> GenerateRules()
        {
            string conditionsXml = _cellXmlHelper.GetXmlString((string)DataRow.ItemArray.GetValue(TableColumns.CONDITIONCOLUMNINDEX)!, TableColumns.CONDITIONCOLUMNINDEX);
            if (conditionsXml.Length == 0)
            {
                NoConditionsRule();
                return GetRules();
            }

            ConditionsData conditionsData = _conditionsDataParser.Parse
            (
                _xmlDocumentHelpers.ToXmlElement(conditionsXml)
            );

            switch (conditionsData.FirstChildElementName)
            {
                case XmlDataConstants.NOTELEMENT:
                case XmlDataConstants.FUNCTIONELEMENT:
                case XmlDataConstants.ANDELEMENT:
                    AndRule(conditionsData);
                    break;
                case XmlDataConstants.ORELEMENT:
                    OrRule(conditionsData);
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{6F9AE2B8-1C22-4372-A1C8-C74775C11DEB}");
            }

            return GetRules();
        }

        private int GetPriority()
        {
            string priorityXml = _cellXmlHelper.GetXmlString((string)DataRow.ItemArray.GetValue(TableColumns.PRIORITYCOLUMNINDEX)!, TableColumns.PRIORITYCOLUMNINDEX);

            if (priorityXml.Length == 0)
                throw _exceptionHelper.CriticalException("{88827A22-6FEB-4D61-9ECD-A165D454B24B}");

            int? priority = _priorityDataParser.Parse(_xmlDocumentHelpers.ToXmlElement(priorityXml));
            if (!priority.HasValue)
                throw _exceptionHelper.CriticalException("{B3305BB8-3EDC-42A6-837F-8DD61A473DBE}");

            return priority.Value;
        }

        private RuleReevaluationBehavior GetEvaluationBehavior()
        {
            if (!bool.TryParse(DataRow.ItemArray.GetValue(TableColumns.REEVALUATECOLUMNINDEX)!.ToString(), out bool reEvaluate))
                throw _exceptionHelper.CriticalException("{5EC7BC3E-4B74-42AD-8FA7-0FADEC8EE08A}");

            return reEvaluate ? RuleReevaluationBehavior.Always : RuleReevaluationBehavior.Never;
        }

        private bool GetActive()
        {
            if (!bool.TryParse(DataRow.ItemArray.GetValue(TableColumns.ACTIVECOLUMNINDEX)!.ToString(), out bool active))
                throw _exceptionHelper.CriticalException("{5EC7BC3E-4B74-42AD-8FA7-0FADEC8EE08A}");

            return active;
        }

        private IList<WorkflowRules.Rule> GetRules()
        {
            WorkflowRules.Rule rule = new($"{ModuleName}{FileConstants.TABLESTRING}{RuleCount}")
            {
                Priority = this.GetPriority(),
                ReevaluationBehavior = this.GetEvaluationBehavior(),
                Active = this.GetActive(),
                Condition = new RuleExpressionCondition(_codeExpressionBuilder.AggregateConditions(this.Conditions, CodeBinaryOperatorType.BooleanAnd))
            };

            foreach (RuleAction thenAction in ThenActions)
                rule.ThenActions.Add(thenAction);

            return new WorkflowRules.Rule[] { rule };
        }

        private void OrRule(ConditionsData conditionsData)
        {
            RuleCount++;
            Conditions.Add(GetAllConditions(conditionsData, true, CodeBinaryOperatorType.BooleanOr));
            GenerateRightHandSide();
        }

        private void AndRule(ConditionsData conditionsData)
        {
            RuleCount++;
            Conditions.Add(GetAllConditions(conditionsData, true, CodeBinaryOperatorType.BooleanAnd));
            GenerateRightHandSide();
        }

        private IfCondition GetAllConditions(ConditionsData conditionsData, bool isYesPath, CodeBinaryOperatorType binaryOperator)
            => new
            (
                _codeExpressionBuilder.AggregateConditions
                (
                    conditionsData
                        .FunctionElements
                        .Select(e => _functionDataParser.Parse(e))
                        .Select(fData => _codeExpressionBuilder.BuildIfCondition(fData)),
                    binaryOperator
                ),
                !isYesPath//if yes Path the Not is false
            );

        private void NoConditionsRule()
        {
            RuleCount++;
            Conditions.Add
            (
                new IfCondition
                (
                    _codeExpressionBuilder.BuildDirectorPropertyCondition
                    (
                        DirectorProperties.MODULEBEGINPROPERTY,
                        new CodePrimitiveExpression(ModuleName)
                    )
                )
            );
            GenerateRightHandSide();
        }

        private void GenerateRightHandSide()
        {
            BuildActions();
        }

        private void BuildActions()
        {
            string functionsXml = _cellXmlHelper.GetXmlString((string)DataRow.ItemArray.GetValue(TableColumns.ACTIONCOLUMNINDEX)!, TableColumns.ACTIONCOLUMNINDEX);
            if (string.IsNullOrEmpty(functionsXml))
                throw _exceptionHelper.CriticalException("{5D22D9E2-DC0C-4804-9F8B-EC060BA299CA}");

            FunctionsData functionsData = _functionsDataParser.Parse
            (
                _xmlDocumentHelpers.ToXmlElement(functionsXml)
            );

            foreach (XmlElement functionElement in functionsData.FunctionElements)
            {
                switch (functionElement.Name)
                {
                    case XmlDataConstants.ASSERTFUNCTIONELEMENT:
                        AssertFunctionData assertFunctionData = _assertFunctionDataParser.Parse(functionElement);
                        if (!_configurationService.FunctionList.Functions.TryGetValue(assertFunctionData.Name, out _))
                            throw _exceptionHelper.CriticalException("{6A953AA4-51D2-4A07-82F2-665E4A3E8888}");

                        AddAssertActions
                        (
                            _variableDataParser.Parse(assertFunctionData.VariableElement),
                            _variableValueDataParser.Parse(assertFunctionData.VariableValueElement)
                        );
                        break;
                    case XmlDataConstants.RETRACTFUNCTIONELEMENT:
                        RetractFunctionData retractFunctionData = _retractFunctionDataParser.Parse(functionElement);
                        if (!_configurationService.FunctionList.Functions.TryGetValue(retractFunctionData.Name, out _))
                            throw _exceptionHelper.CriticalException("{148D5BAC-6A2A-4120-9E3A-E47BE197C5D9}");

                        AddRetractActions(_variableDataParser.Parse(retractFunctionData.VariableElement));
                        break;
                    case XmlDataConstants.FUNCTIONELEMENT:
                        FunctionData functionData = _functionDataParser.Parse(functionElement);
                        if (!_getValidConfigurationFromData.TryGetFunction(functionData, this.Application, out Function? function))
                            throw _exceptionHelper.CriticalException("{F9B172E9-4F9A-4A12-A1F9-D2E28E7CE5F3}");

                        if (function.FunctionCategory == FunctionCategories.DialogForm)
                            throw _exceptionHelper.CriticalException("{2153353F-C89F-4CBE-9A0A-F42CBA534A33}");

                        if (function.FunctionCategory == FunctionCategories.RuleChainingUpdate)
                        {
                            if (functionData.ParameterElementsList.Count != 1)
                                throw _exceptionHelper.CriticalException("{6F06C11F-5937-4A99-95C7-E6BFB05C8A56}");

                            ThenActions.Add(new RuleUpdateAction(GetUpdatePath(functionData.ParameterElementsList[0])));
                        }
                        else
                        {
                            ThenActions.Add(new RuleStatementAction(_codeExpressionBuilder.BuildFunction(functionData, null)));
                        }
                        break;
                    default:
                        throw _exceptionHelper.CriticalException("{2BD8A3C1-EFCF-469F-8340-DF7FB5464756}");
                }

                void AddAssertActions(VariableData variableData, VariableValueData variableValueData)
                {
                    if (!_getValidConfigurationFromData.TryGetVariable(variableData, this.Application, out VariableBase? variable))
                        throw _exceptionHelper.CriticalException("{2B01ED7C-A674-44A9-9E5C-1299C7A79474}");

                    ThenActions.Add
                    (
                        new RuleStatementAction
                        (
                            _codeExpressionBuilder.BuildAssignmentStatement(variableValueData, variable)
                        )
                    );
                }

                void AddRetractActions(VariableData variableData)
                {
                    if (!_getValidConfigurationFromData.TryGetVariable(variableData, this.Application, out VariableBase? variable))
                        throw _exceptionHelper.CriticalException("{83A15F46-8162-4883-A2E5-9889819A8991}");

                    ThenActions.Add(new RuleStatementAction(_codeExpressionBuilder.BuildAssignToNullStatement(variable)));
                }
            }
        }

        private string GetUpdatePath(XmlElement textXmlNode)
        {
            if (textXmlNode.ChildNodes.Count == 1 && textXmlNode.ChildNodes[0]!.NodeType == XmlNodeType.Text)
                return ((XmlText)textXmlNode.ChildNodes[0]!).Value!.Trim();
            else
                throw _exceptionHelper.CriticalException("{0E230D61-25E2-48BC-9A1E-7C203C5E8E9B}");
        }
    }
}
