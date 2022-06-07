using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator
{
    internal interface IFileSystemFileDeployer
    {
        Task<IList<ResultMessage>> Deploy(string sourceFile, string deploymentPath, CancellationTokenSource cancellationTokenSource);
    }
}
