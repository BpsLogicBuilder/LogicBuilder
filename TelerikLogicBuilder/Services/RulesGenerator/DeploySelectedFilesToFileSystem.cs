using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
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
        private readonly IDisplayResultMessages _displayResultMessages;
        private readonly IFileSystemFileDeployer _fileSystemFileDeployer;
        private readonly IPathHelper _pathHelper;

        public DeploySelectedFilesToFileSystem(
            IDisplayResultMessages displayResultMessages,
            IPathHelper pathHelper,
            IFileSystemFileDeployer fileSystemFileDeployer)
        {
            _displayResultMessages = displayResultMessages;
            _fileSystemFileDeployer = fileSystemFileDeployer;
            _pathHelper = pathHelper;
        }

        public async Task<IList<ResultMessage>> Deploy(IList<RulesResourcesPair> sourceFiles, Application application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
        {
            List<ResultMessage> result = new();
            int deployedFiles = 0;
            _displayResultMessages.Clear(MessageTab.Rules);

            foreach (RulesResourcesPair pair in sourceFiles)
            {
                List<ResultMessage> pairResults = new();
                UpdateResults
                (
                    await _fileSystemFileDeployer.Deploy
                    (
                        pair.RulesFile,
                        application.RulesDeploymentPath,
                        cancellationTokenSource
                    ),
                    pair.RulesFile
                );

                UpdateResults
                (
                    await _fileSystemFileDeployer.Deploy
                    (
                        pair.ResourcesFile,
                        application.ResourceFileDeploymentPath,
                        cancellationTokenSource
                    ),
                    pair.ResourcesFile
                );

                pairResults.ForEach(resultMessage =>
                {
                    result.Add(resultMessage);
                    _displayResultMessages.AppendMessage(resultMessage, MessageTab.Rules);
                });

                progress.Report
                (
                    new ProgressMessage
                    (
                        (int)((float)++deployedFiles / (float)sourceFiles.Count * 100),
                        string.Format(CultureInfo.CurrentCulture, Strings.deployingRulesFormat, application.Nickname)
                    )
                );

                void UpdateResults(IList<ResultMessage> fileResult, string fileFullName)
                {
                    if (fileResult.Count == 0)
                    {
                        pairResults.Add
                        (
                            new ResultMessage
                            (
                                string.Format
                                (
                                    CultureInfo.CurrentCulture,
                                    Strings.fileDeployed,
                                    _pathHelper.GetFileName(fileFullName)
                                )
                            )
                        );
                        return;
                    }

                    pairResults.AddRange(fileResult);
                }
            }

            return result;
        }
    }
}
