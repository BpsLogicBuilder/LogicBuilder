using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator
{
    internal interface IFileSystemFileDeleter
    {
        Task<IList<ResultMessage>> Delete(string sourceFile, string deploymentPath, CancellationTokenSource cancellationTokenSource);
    }
}
