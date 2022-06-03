using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator
{
    internal class TableRulesBuilder : ITableRulesBuilder
    {
        private readonly IContextProvider _contextProvider;
        private readonly ITableRowRuleBuilder _tableRowRuleBuilder;
        private readonly ITableValidator _tableValidator;

        public TableRulesBuilder(
            IContextProvider contextProvider,
            ITableRowRuleBuilder tableRowRuleBuilder,
            ITableValidator tableValidator)
        {
            _contextProvider = contextProvider;
            _tableRowRuleBuilder = tableRowRuleBuilder;
            _tableValidator = tableValidator;
        }

        public Task<BuildRulesResult> BuildRules(string sourceFile, DataSet dataSet, ApplicationTypeInfo application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource) 
            => new TableRulesBuilderUtility
            (
                sourceFile,
                dataSet,
                application,
                progress,
                cancellationTokenSource,
                _contextProvider,
                _tableRowRuleBuilder,
                _tableValidator
            ).BuildRules();
    }
}
