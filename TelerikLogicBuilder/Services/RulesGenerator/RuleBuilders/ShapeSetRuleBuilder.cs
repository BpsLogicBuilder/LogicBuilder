using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.RuleBuilders
{
    internal class ShapeSetRuleBuilder : IShapeSetRuleBuilder
    {
        private readonly IApplicationTypeInfoManager _applicationTypeInfoManager;
        private readonly IBeginFlowRuleBuilder _beginFlowRuleBuilder;
        private readonly IConditionsRuleBuilder _conditionsRuleBuilder;
        private readonly IDecisionsRuleBuilder _decisionsRuleBuilder;
        private readonly IDialogWithExitsRuleBuilder _dialogWithExitsRuleBuilder;
        private readonly IDialogWithoutExitsRuleBuilder _dialogWithoutExitsRuleBuilder;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMergeRuleBuilder _mergeRuleBuilder;
        private readonly IModuleBeginRuleBuilder _moduleBeginRuleBuilder;
        private readonly IModuleRuleBuilder _moduleRuleBuilder;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IWaitConditionsRuleBuilder _waitConditionsRuleBuilder;
        private readonly IWaitDecisionsRuleBuilder _waitDecisionsRuleBuilder;

        public ShapeSetRuleBuilder(
            IApplicationTypeInfoManager applicationTypeInfoManager,
            IBeginFlowRuleBuilder beginFlowRuleBuilder,
            IConditionsRuleBuilder conditionsRuleBuilder,
            IDecisionsRuleBuilder decisionsRuleBuilder,
            IDialogWithExitsRuleBuilder dialogWithExitsRuleBuilder,
            IDialogWithoutExitsRuleBuilder dialogWithoutExitsRuleBuilder,
            IExceptionHelper exceptionHelper,
            IMergeRuleBuilder mergeRuleBuilder,
            IModuleBeginRuleBuilder moduleBeginRuleBuilder,
            IModuleRuleBuilder moduleRuleBuilder,
            IShapeHelper shapeHelper,
            IShapeXmlHelper shapeXmlHelper,
            IWaitConditionsRuleBuilder waitConditionsRuleBuilder,
            IWaitDecisionsRuleBuilder waitDecisionsRuleBuilder)
        {
            _applicationTypeInfoManager = applicationTypeInfoManager;
            _beginFlowRuleBuilder = beginFlowRuleBuilder;
            _conditionsRuleBuilder = conditionsRuleBuilder;
            _decisionsRuleBuilder = decisionsRuleBuilder;
            _dialogWithExitsRuleBuilder = dialogWithExitsRuleBuilder;
            _dialogWithoutExitsRuleBuilder = dialogWithoutExitsRuleBuilder;
            _exceptionHelper = exceptionHelper;
            _mergeRuleBuilder = mergeRuleBuilder;
            _moduleBeginRuleBuilder = moduleBeginRuleBuilder;
            _moduleRuleBuilder = moduleRuleBuilder;
            _shapeHelper = shapeHelper;
            _shapeXmlHelper = shapeXmlHelper;
            _waitConditionsRuleBuilder = waitConditionsRuleBuilder;
            _waitDecisionsRuleBuilder = waitDecisionsRuleBuilder;
        }

        public IList<RuleBag> GenerateRules(string masterNameU, IList<ShapeBag> ruleShapes, IList<Shape> ruleConnectors, string moduleName, int ruleCount, ApplicationTypeInfo application, IDictionary<string, string> resourceStrings)
        {
            return masterNameU switch
            {
                UniversalMasterName.BEGINFLOW => _beginFlowRuleBuilder.GenerateRules(ruleShapes, ruleConnectors, moduleName, ruleCount, application, resourceStrings),
                UniversalMasterName.CONDITIONOBJECT => _conditionsRuleBuilder.GenerateRules(ruleShapes, ruleConnectors, moduleName, ruleCount, application, resourceStrings),
                UniversalMasterName.DECISIONOBJECT => _decisionsRuleBuilder.GenerateRules(ruleShapes, ruleConnectors, moduleName, ruleCount, application, resourceStrings),
                UniversalMasterName.DIALOG => string.IsNullOrEmpty(_shapeXmlHelper.GetXmlString(ruleConnectors[0]))
                                        ? _dialogWithoutExitsRuleBuilder.GenerateRules(ruleShapes, ruleConnectors, moduleName, ruleCount, application, resourceStrings)
                                        : _dialogWithExitsRuleBuilder.GenerateRules(ruleShapes, ruleConnectors, moduleName, ruleCount, application, resourceStrings),
                UniversalMasterName.MERGEOBJECT => _mergeRuleBuilder.GenerateRules(ruleShapes, ruleConnectors, moduleName, ruleCount, GetApplication(), resourceStrings),
                UniversalMasterName.MODULEBEGIN => _moduleBeginRuleBuilder.GenerateRules(ruleShapes, ruleConnectors, moduleName, ruleCount, application, resourceStrings),
                UniversalMasterName.MODULE => _moduleRuleBuilder.GenerateRules(ruleShapes, ruleConnectors, moduleName, ruleCount, GetApplication(), resourceStrings),
                UniversalMasterName.WAITCONDITIONOBJECT => _waitConditionsRuleBuilder.GenerateRules(ruleShapes, ruleConnectors, moduleName, ruleCount, GetApplication(), resourceStrings),
                UniversalMasterName.WAITDECISIONOBJECT => _waitDecisionsRuleBuilder.GenerateRules(ruleShapes, ruleConnectors, moduleName, ruleCount, GetApplication(), resourceStrings),
                _ => throw _exceptionHelper.CriticalException("{C6B7E60C-42CE-4E69-9BF5-37428EDF3EC6}"),
            };

            ApplicationTypeInfo GetApplication()
            {
                if (ruleConnectors[0].Master.NameU == UniversalMasterName.CONNECTOBJECT)
                    return application;

                return _applicationTypeInfoManager.GetApplicationTypeInfo
                (
                    _shapeHelper
                        .GetApplicationList(ruleConnectors[0], ruleShapes[0])
                        .OrderBy(n => n)
                        .First()
                );
            }
        }
    }
}
