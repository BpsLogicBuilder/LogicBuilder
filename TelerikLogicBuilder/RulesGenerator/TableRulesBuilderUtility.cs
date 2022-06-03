using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator
{
    internal class TableRulesBuilderUtility
    {
        private readonly ITableRowRuleBuilder _tableRowRuleBuilder;
        private readonly ITableValidator _tableValidator;

        public TableRulesBuilderUtility(
            string sourceFile,
            DataSet dataSet,
            ApplicationTypeInfo application,
            IProgress<ProgressMessage> progress,
            CancellationTokenSource cancellationTokenSource,
            IContextProvider contextProvider,
            ITableRowRuleBuilder tableRowRuleBuilder,
            ITableValidator tableValidator)
        {
            SourceFile = sourceFile;
            FileName = contextProvider.PathHelper.GetFileName(SourceFile);
            ModuleName = contextProvider.PathHelper.GetModuleName(FileName);
            DataSet = dataSet;
            Application = application;
            Progress = progress;
            CancellationTokenSource = cancellationTokenSource;
            _tableRowRuleBuilder = tableRowRuleBuilder;
            _tableValidator = tableValidator;
        }

        #region Properties
        private string SourceFile { get; }
        private string FileName { get; }
        private string ModuleName { get; }
        private DataSet DataSet { get; }
        private ApplicationTypeInfo Application { get; }
        private IList<ResultMessage> BuildErrors { get; } = new List<ResultMessage>();
        private IDictionary<string, string> ResourceStrings { get; } = new Dictionary<string, string>();
        private List<RuleBag> Rules { get; } = new List<RuleBag>();
        private IProgress<ProgressMessage> Progress { get; }
        private CancellationTokenSource CancellationTokenSource { get; }
        #endregion Properties

        internal async Task<BuildRulesResult> BuildRules()
        {
            var validationErrors = await _tableValidator.Validate(SourceFile, DataSet, Application, Progress, CancellationTokenSource);
            if (validationErrors.Count > 0)
                return new BuildRulesResult(validationErrors, Rules, ResourceStrings);

            Progress.Report
            (
                new ProgressMessage
                (
                    0,
                    string.Format(CultureInfo.CurrentCulture, Strings.progressFormTaskBuildingFormat, FileName)
                )
            );

            await Task.Run
            (
                Build, CancellationTokenSource.Token
            );

            return new BuildRulesResult(BuildErrors, Rules, ResourceStrings);
        }


        private void Build()
        {
            for (int i = 0; i < DataSet.Tables[TableName.RULESTABLE]!.Rows.Count; i++)
            {
                GenerateRule(DataSet.Tables[TableName.RULESTABLE]!.Rows[i]);

                Progress.Report
                (
                    new ProgressMessage
                    (
                        (int)((float)(i + 1) / (float)DataSet.Tables[TableName.RULESTABLE]!.Rows.Count * 100),
                        string.Format(CultureInfo.CurrentCulture, Strings.progressFormTaskBuildingFormat, FileName)
                    )
                );
            }
        }

        private void GenerateRule(DataRow dataRow)
        {
            Rules.AddRange
            (
                _tableRowRuleBuilder.GenerateRules
                (
                    dataRow, 
                    ModuleName, 
                    Rules.Count, 
                    Application, 
                    ResourceStrings
                )
                .Select(r => new RuleBag(r))
            );
        }
    }
}
