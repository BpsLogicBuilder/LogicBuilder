using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using LogicBuilder.Workflow.Activities.Rules;
using Microsoft.Office.Interop.Visio;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.RuleBuilders
{
    internal class ShapeSetRuleBuilderHelper : IShapeSetRuleBuilderHelper
    {
        private readonly IAssertFunctionDataParser _assertFunctionDataParser;
        private readonly ICodeExpressionBuilder _codeExpressionBuilder;
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IFunctionsDataParser _functionsDataParser;
        private readonly IGetValidConfigurationFromData _getValidConfigurationFromData;
        private readonly IModuleDataParser _moduleDataParser;
        private readonly IRetractFunctionDataParser _retractFunctionDataParser;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IVariableDataParser _variableDataParser;
        private readonly IVariableValueDataParser _variableValueDataParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ShapeSetRuleBuilderHelper(
            IAssertFunctionDataParser assertFunctionDataParser,
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IFunctionDataParser functionDataParser,
            IFunctionsDataParser functionsDataParser,
            IGetValidConfigurationFromData getValidConfigurationFromData,
            IModuleDataParser moduleDataParser,
            IRetractFunctionDataParser retractFunctionDataParser,
            IRulesGeneratorFactory rulesGeneratorFactory,
            IShapeXmlHelper shapeXmlHelper,
            IVariableDataParser variableDataParser,
            IVariableValueDataParser variableValueDataParser,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IList<ShapeBag> ruleShapes,
            IList<Shape> ruleConnectors,
            string moduleName,
            int ruleCount,
            ApplicationTypeInfo application,
            IDictionary<string, string> resourceStrings)
        {
            _assertFunctionDataParser = assertFunctionDataParser;
            _codeExpressionBuilder = rulesGeneratorFactory.GetCodeExpressionBuilder
            (
                application,
                resourceStrings,
                moduleName
            );
            _configurationService = configurationService;
            _exceptionHelper = exceptionHelper;
            _functionDataParser = functionDataParser;
            _functionsDataParser = functionsDataParser;
            _getValidConfigurationFromData = getValidConfigurationFromData;
            _moduleDataParser = moduleDataParser;
            _retractFunctionDataParser = retractFunctionDataParser;
            _shapeXmlHelper = shapeXmlHelper;
            _variableDataParser = variableDataParser;
            _variableValueDataParser = variableValueDataParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;

            this.RuleShapes = ruleShapes;
            this.RuleConnectors = ruleConnectors;
            this.ModuleName = moduleName;
            this.RuleCount = ruleCount;
            this.Application = application;
            this.ResourceStrings = resourceStrings;
            FirstConnector = this.RuleConnectors[0];
            Page = FirstConnector.ContainingPage;
        }

        public ApplicationTypeInfo Application { get; }

        public IList<IfCondition> Conditions { get; } = new List<IfCondition>();

        public CodePropertyReferenceExpression DirectorReference
            => new(ThisReference, RuleFunctionConstants.DIRECTORPROPERTYNAME);

        public CodePropertyReferenceExpression DriverReference
            => new(DirectorReference, DirectorProperties.DRIVERPROPERTY);

        public Shape FirstConnector { get; }

        public CodePropertyReferenceExpression ModuleBeginReference
            => new(DirectorReference, DirectorProperties.MODULEBEGINPROPERTY);

        public CodePropertyReferenceExpression ModuleEndReference
            => new(DirectorReference, DirectorProperties.MODULEENDPROPERTY);

        public string ModuleName { get; }

        public Page Page { get; }

        public IDictionary<string, string> ResourceStrings { get; }

        public int RuleCount { get; set; }

        public IList<Shape> RuleConnectors { get; }

        public IList<ShapeBag> RuleShapes { get; }

        public CodePropertyReferenceExpression SelectionReference
            => new(DirectorReference, DirectorProperties.SELECTIONPROPERTY);

        public IList<RuleAction> ThenActions { get; } = new List<RuleAction>();

        public CodeThisReferenceExpression ThisReference
            => new();

        public void GenerateRightHandSide()
        {
            if (RuleShapes.Count > 0 && (RuleShapes[RuleShapes.Count - 1].Shape.Master.NameU == UniversalMasterName.ENDFLOW
                                        || RuleShapes[RuleShapes.Count - 1].Shape.Master.NameU == UniversalMasterName.TERMINATE))
                ThenActions.Add(new RuleStatementAction(new CodeMethodInvokeExpression(DirectorReference, RuleFunctionConstants.VIRTUALFUNCTIONSETBUSINESSBACKUPDATA, Array.Empty<CodeExpression>())));

            for (int i = 1; i < RuleShapes.Count; i++)
            {
                int shapeIndex = RuleShapes[i].Shape.Index;
                short pageIndex = RuleShapes[i].Shape.ContainingPage.Index;
                switch (RuleShapes[i].Shape.Master.NameU)
                {
                    case UniversalMasterName.WAITCONDITIONOBJECT:
                    case UniversalMasterName.WAITDECISIONOBJECT:
                    case UniversalMasterName.CONDITIONOBJECT:
                    case UniversalMasterName.DECISIONOBJECT:
                    case UniversalMasterName.MERGEOBJECT:
                        ResetSelection(i);
                        AddAssignDriverAction(shapeIndex, pageIndex);
                        break;
                    case UniversalMasterName.MODULE:
                        //Reset Selection Property before Driver Property in case a new selection menu contains the current selection
                        ResetSelection(i);
                        //must set Driver before moduleBeginName because ModuleBeginName property records calling module driver in set_ModuleBeginName{}.
                        AddAssignDriverAction(shapeIndex, pageIndex);
                        string moduleShapeXml = _shapeXmlHelper.GetXmlString(RuleShapes[i].Shape);
                        if (string.IsNullOrEmpty(moduleShapeXml))
                            throw _exceptionHelper.CriticalException("{2EF158A2-3BE9-4911-9E0E-7CE6BE213D19}");

                        string externalModule = _moduleDataParser.Parse
                        (
                            _xmlDocumentHelpers.ToXmlElement(moduleShapeXml)
                        );

                        ThenActions.Add(new RuleStatementAction(new CodeAssignStatement(ModuleBeginReference, new CodePrimitiveExpression(externalModule))));
                        ThenActions.Add(new RuleHaltAction());
                        break;
                    case UniversalMasterName.DIALOG:
                        ResetSelection(i);
                        AddAssignDriverAction(shapeIndex, pageIndex);
                        break;
                    case UniversalMasterName.ACTION:
                        BuildActions(i);
                        break;
                    case UniversalMasterName.ENDFLOW:
                        ThenActions.Add(new RuleStatementAction(new CodeMethodInvokeExpression(ThisReference, RuleFunctionConstants.VIRTUALFUNCTIONFLOWCOMPLETE, Array.Empty<CodeExpression>())));
                        ResetSelection(i);
                        //In this case, the driver condition reports progress only
                        //and has no further functional purpose
                        AddAssignDriverAction(shapeIndex, pageIndex);
                        break;
                    case UniversalMasterName.MODULEEND:
                        //Reset Selection Property before ModuleEnd Property in case a new selection menu contains the current selection
                        ResetSelection(i);
                        //In this case, the driver condition reports progress only
                        //and has no further functional purpose
                        AddAssignDriverAction(shapeIndex, pageIndex);
                        ThenActions.Add(new RuleStatementAction(new CodeAssignStatement(ModuleEndReference, new CodePrimitiveExpression(this.ModuleName))));
                        ThenActions.Add(new RuleHaltAction());
                        break;
                    case UniversalMasterName.TERMINATE:
                        ThenActions.Add(new RuleStatementAction(new CodeMethodInvokeExpression(ThisReference, RuleFunctionConstants.VIRTUALFUNCTIONTERMINATE, Array.Empty<CodeExpression>())));
                        ResetSelection(i);
                        //In this case, the driver condition reports progress only
                        //and has no further functional purpose
                        AddAssignDriverAction(shapeIndex, pageIndex);
                        break;
                    default:
                        throw _exceptionHelper.CriticalException("{DBAFF218-FA8A-4FCE-8B5C-3AA6751C0800}");
                }
            }
        }

        public IList<RuleBag> GetRules()
        {
            Rule rule = new(string.Format(CultureInfo.InvariantCulture, RuleDefinitionConstants.RULENAMEFORMAT, ModuleName, RuleCount, FirstConnector.Index, Page.Index))
            {
                Condition = new RuleExpressionCondition(_codeExpressionBuilder.AggregateConditions(this.Conditions, CodeBinaryOperatorType.BooleanAnd))
            };

            foreach (RuleAction thenAction in ThenActions)
                rule.ThenActions.Add(thenAction);

            return new RuleBag[] { new RuleBag(rule) };
        }

        private void AddAssignDriverAction(int shapeIndex, short pageIndex)
        {
            ThenActions.Add
            (
                new RuleStatementAction
                (
                    new CodeAssignStatement
                    (
                        DriverReference,
                        new CodePrimitiveExpression
                        (
                            string.Format(CultureInfo.InvariantCulture, RuleDefinitionConstants.DRIVERFORMAT, shapeIndex, pageIndex)
                        )
                    )
                )
            );
        }

        private void BuildActions(int ruleShapesIndex)
        {
            string functionsXml = _shapeXmlHelper.GetXmlString(RuleShapes[ruleShapesIndex].Shape);
            if (string.IsNullOrEmpty(functionsXml))
                throw _exceptionHelper.CriticalException("{6AF0AE29-E3D6-4D0B-A44F-3C2983EA6209}");

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
                            throw _exceptionHelper.CriticalException("{4FE99B53-051C-4E14-968B-CFDA7D9D61E5}");

                        AddAssertActions
                        (
                            _variableDataParser.Parse(assertFunctionData.VariableElement),
                            _variableValueDataParser.Parse(assertFunctionData.VariableValueElement)
                        );
                        break;
                    case XmlDataConstants.RETRACTFUNCTIONELEMENT:
                        RetractFunctionData retractFunctionData = _retractFunctionDataParser.Parse(functionElement);
                        if (!_configurationService.FunctionList.Functions.TryGetValue(retractFunctionData.Name, out _))
                            throw _exceptionHelper.CriticalException("{F866D4E1-3C0D-48FD-B7CE-93818F78AE81}");

                        AddRetractActions(_variableDataParser.Parse(retractFunctionData.VariableElement));
                        break;
                    case XmlDataConstants.FUNCTIONELEMENT:
                        FunctionData functionData = _functionDataParser.Parse(functionElement);
                        if (!_getValidConfigurationFromData.TryGetFunction(functionData, this.Application, out Function? function))
                            throw _exceptionHelper.CriticalException("{267246D2-C364-4FFE-8FC0-C729928E14C5}");

                        if (function.FunctionCategory == FunctionCategories.DialogForm
                            || function.FunctionCategory == FunctionCategories.RuleChainingUpdate)
                            throw _exceptionHelper.CriticalException("{CE221607-210A-4F54-997A-91DA11506569}");

                        ThenActions.Add(new RuleStatementAction(_codeExpressionBuilder.BuildFunction(functionData, null)));

                        break;
                    default:
                        throw _exceptionHelper.CriticalException("{9CC8C750-410C-49FA-A15C-27C658EBB3C2}");


                }

                void AddAssertActions(VariableData variableData, VariableValueData variableValueData)
                {
                    if (!_getValidConfigurationFromData.TryGetVariable(variableData, this.Application, out VariableBase? variable))
                        throw _exceptionHelper.CriticalException("{585576D9-F860-4331-8948-7028582FFCC2}");

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

        private void ResetSelection(int shapeIndex)
        {
            if (shapeIndex != RuleShapes.Count - 1
                || RuleShapes[0].Shape.Master.NameU != UniversalMasterName.DIALOG)
                return;

            ThenActions.Add(new RuleStatementAction(new CodeAssignStatement(SelectionReference, new CodePrimitiveExpression(string.Empty))));
        }
    }
}
