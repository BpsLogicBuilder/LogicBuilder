using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator
{
    internal interface IValidateSelectedDocuments
    {
        Task<IList<ResultMessage>> Validate(IList<string> sourceFiles, Application application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource);
    }
}
