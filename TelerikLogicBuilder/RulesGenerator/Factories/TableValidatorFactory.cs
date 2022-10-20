using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Data;
using System.Threading;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories
{
    internal class TableValidatorFactory : ITableValidatorFactory
    {
        private readonly Func<string, DataSet, ApplicationTypeInfo, IProgress<ProgressMessage>, CancellationTokenSource, ITableValidator> _getTableValidator;

        public TableValidatorFactory(
            Func<string, DataSet, ApplicationTypeInfo, IProgress<ProgressMessage>, CancellationTokenSource, ITableValidator> getTableValidator)
        {
            _getTableValidator = getTableValidator;
        }

        public ITableValidator GetTableValidator(string sourceFile, DataSet dataSet, ApplicationTypeInfo application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
            => _getTableValidator(sourceFile, dataSet, application, progress, cancellationTokenSource);
    }
}
