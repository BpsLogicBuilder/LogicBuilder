using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using LogicBuilder.DataContracts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator
{
    internal interface IApiFileListDeleter
    {
        Task<IList<ResultMessage>> Delete(DeleteRulesData deleteRulesData, Application application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource);
    }
}
