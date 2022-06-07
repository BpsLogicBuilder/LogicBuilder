using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator
{
    internal class DeploySelectedFilesToFileSystem : IDeploySelectedFilesToFileSystem
    {
        private readonly IFileSystemFileDeployer _fileSystemFileDeployer;

        public DeploySelectedFilesToFileSystem(IFileSystemFileDeployer fileSystemFileDeployer)
        {
            _fileSystemFileDeployer = fileSystemFileDeployer;
        }

        public async Task<IList<ResultMessage>> Deploy(IList<RulesResourcesPair> sourceFiles, Application application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
        {
            List<ResultMessage> result = new();
            int deployedFiles = 0;

            foreach (RulesResourcesPair pair in sourceFiles)
            {
                result.AddRange
                (
                    await _fileSystemFileDeployer.Deploy
                    (
                        pair.RulesFile,
                        application.RulesDeploymentPath,
                        cancellationTokenSource
                    )
                );

                result.AddRange
                (
                    await _fileSystemFileDeployer.Deploy
                    (
                        pair.ResourcesFile,
                        application.ResourceFileDeploymentPath,
                        cancellationTokenSource
                    )
                );

                progress.Report
                (
                    new ProgressMessage
                    (
                        (int)((float)++deployedFiles / (float)sourceFiles.Count * 100),
                        string.Format(CultureInfo.CurrentCulture, Strings.deployingRulesFormat, application.Nickname)
                    )
                );
            }

            return result;
        }
    }
}
