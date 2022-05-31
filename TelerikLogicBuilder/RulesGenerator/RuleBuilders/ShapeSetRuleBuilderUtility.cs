using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using LogicBuilder.Workflow.Activities.Rules;
using Microsoft.Office.Interop.Visio;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders
{
    internal abstract class ShapeSetRuleBuilderUtility
    {
        protected readonly IContextProvider _contextProvider;
        protected readonly IAnyParametersHelper _anyParametersHelper;
        protected readonly IAssertFunctionDataParser _assertFunctionDataParser;
        protected readonly IConfigurationService _configurationService;
        protected readonly IConstructorDataParser _constructorDataParser;
        protected readonly IDiagramResourcesManager _diagramResourcesManager;
        protected readonly IEnumHelper _enumHelper;
        protected readonly IExceptionHelper _exceptionHelper;
        protected readonly IFunctionDataParser _functionDataParser;
        protected readonly IFunctionHelper _functionHelper;
        protected readonly IFunctionsDataParser _functionsDataParser;
        protected readonly IGetValidConfigurationFromData _getValidConfigurationFromData;
        protected readonly ILiteralListDataParser _literalListDataParser;
        protected readonly ILiteralListParameterDataParser _literalListParameterDataParser;
        protected readonly ILiteralListVariableDataParser _literalListVariableDataParser;
        protected readonly IMetaObjectDataParser _metaObjectDataParser;
        protected readonly IModuleDataParser _moduleDataParser;
        protected readonly IObjectDataParser _objectDataParser;
        protected readonly IObjectListDataParser _objectListDataParser;
        protected readonly IObjectListParameterDataParser _objectListParameterDataParser;
        protected readonly IObjectListVariableDataParser _objectListVariableDataParser;
        protected readonly IObjectParameterDataParser _objectParameterDataParser;
        protected readonly IObjectVariableDataParser _objectVariableDataParser;
        protected readonly IParameterHelper _parameterHelper;
        protected readonly IRetractFunctionDataParser _retractFunctionDataParser;
        protected readonly IShapeXmlHelper _shapeXmlHelper;
        protected readonly ITypeLoadHelper _typeLoadHelper;
        protected readonly IVariableDataParser _variableDataParser;
        protected readonly IVariableHelper _variableHelper;
        protected readonly IVariableValueDataParser _variableValueDataParser;
        protected readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        protected CodeExpressionBuilderUtility codeExpressionBuilderUtility;

        protected ShapeSetRuleBuilderUtility(
            IList<ShapeBag> ruleShapes,
            IList<Shape> ruleConnectors,
            string moduleName,
            int ruleCount,
            ApplicationTypeInfo application,
            IDictionary<string, string> resourceStrings,
            IContextProvider contextProvider,
            IAnyParametersHelper anyParametersHelper,
            IAssertFunctionDataParser assertFunctionDataParser,
            IConfigurationService configurationService,
            IConstructorDataParser constructorDataParser,
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
            IXmlDocumentHelpers xmlDocumentHelpers,
            ITypeLoadHelper typeLoadHelper,
            IVariableDataParser variableDataParser,
            IVariableHelper variableHelper,
            IVariableValueDataParser variableValueDataParser)
        {
            _contextProvider = contextProvider;
            _anyParametersHelper = anyParametersHelper;
            _assertFunctionDataParser = assertFunctionDataParser;
            _configurationService = configurationService;
            _constructorDataParser = constructorDataParser;
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
            this.RuleShapes = ruleShapes;
            this.RuleConnectors = ruleConnectors;
            this.ModuleName = moduleName;
            this.RuleCount = ruleCount;
            this.Application = application;
            this.ResourceStrings = resourceStrings;
            FirstConnector = this.RuleConnectors[0];
            Page = FirstConnector.ContainingPage;

            codeExpressionBuilderUtility = new CodeExpressionBuilderUtility
            (
                this.ModuleName,
                this.Application,
                this.ResourceStrings,
                contextProvider,
                anyParametersHelper,
                constructorDataParser,
                functionDataParser,
                getValidConfigurationFromData,
                literalListDataParser,
                literalListParameterDataParser,
                literalListVariableDataParser,
                metaObjectDataParser,
                objectDataParser,
                objectListDataParser,
                objectListParameterDataParser,
                objectListVariableDataParser,
                objectParameterDataParser,
                objectVariableDataParser,
                parameterHelper,
                diagramResourcesManager,
                typeLoadHelper,
                variableDataParser
            );
        }

        protected static CodePropertyReferenceExpression DirectorReference
            => new(ThisReference, RuleFunctionConstants.DIRECTORPROPERTYNAME);

        protected static CodePropertyReferenceExpression DriverReference
            => new(DirectorReference, DirectorProperties.DRIVERPROPERTY);

        protected static CodePropertyReferenceExpression ModuleBeginReference 
            => new(DirectorReference, DirectorProperties.MODULEBEGINPROPERTY);

        protected static CodePropertyReferenceExpression ModuleEndReference
            => new(DirectorReference, DirectorProperties.MODULEENDPROPERTY);

        protected static CodePropertyReferenceExpression SelectionReference
            => new(DirectorReference, DirectorProperties.SELECTIONPROPERTY);

        protected static CodeThisReferenceExpression ThisReference
            => new();

        protected ApplicationTypeInfo Application { get; }
        protected IDictionary<string, string> ResourceStrings { get; }
        protected string ModuleName { get; }
        protected int RuleCount { get; set; }
        protected IList<RuleAction> ThenActions { get; } = new List<RuleAction>();
        protected IList<IfCondition> Conditions { get; } = new List<IfCondition>();
        protected IList<ShapeBag> RuleShapes { get; }
        protected IList<Shape> RuleConnectors { get; }
        protected Shape FirstConnector { get; }
        protected Page Page { get; }

        internal abstract IList<RuleBag> Rules { get; }
        internal abstract void GenerateRules();

        protected virtual IList<RuleBag> GetRules()
        {
            Rule rule = new(string.Format(CultureInfo.InvariantCulture, RuleDefinitionConstants.RULENAMEFORMAT, ModuleName, RuleCount, FirstConnector.Index, Page.Index))
            {
                Condition = new RuleExpressionCondition(CodeExpressionBuilderUtility.AggregateConditions(this.Conditions, CodeBinaryOperatorType.BooleanAnd))
            };

            foreach (RuleAction thenAction in ThenActions)
                rule.ThenActions.Add(thenAction);

            return new RuleBag[] { new RuleBag(rule) };
        }

        protected void GenerateRightHandSide()
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

                        ThenActions.Add(new RuleStatementAction(codeExpressionBuilderUtility.BuildFunction(functionData, null)));

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
                            codeExpressionBuilderUtility.BuildAssignmentStatement(variableValueData, variable)
                        )
                    );
                }

                void AddRetractActions(VariableData variableData)
                {
                    if (!_getValidConfigurationFromData.TryGetVariable(variableData, this.Application, out VariableBase? variable))
                        throw _exceptionHelper.CriticalException("{83A15F46-8162-4883-A2E5-9889819A8991}");

                    ThenActions.Add(new RuleStatementAction(codeExpressionBuilderUtility.BuildAssignToNullStatement(variable)));
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
