using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator
{
    internal interface ITableValidator
    {
        Task<IList<ResultMessage>> Validate(string sourceFile, DataSet dataSet, ApplicationTypeInfo application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource);
    }
}
