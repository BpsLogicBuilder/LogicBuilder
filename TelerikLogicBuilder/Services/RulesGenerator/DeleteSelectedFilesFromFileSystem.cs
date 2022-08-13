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
    internal class DeleteSelectedFilesFromFileSystem : IDeleteSelectedFilesFromFileSystem
    {
        private readonly IDisplayResultMessages _displayResultMessages;
        private readonly IFileSystemFileDeleter _fileSystemFileDeleter;
        private readonly IPathHelper _pathHelper;

        public DeleteSelectedFilesFromFileSystem(IDisplayResultMessages displayResultMessages, IFileSystemFileDeleter fileSystemFileDeleter, IPathHelper pathHelper)
        {
            _displayResultMessages = displayResultMessages;
            _fileSystemFileDeleter = fileSystemFileDeleter;
            _pathHelper = pathHelper;
        }

        public async Task<IList<ResultMessage>> Delete(IList<RulesResourcesPair> sourceFiles, Application application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
        {
            List<ResultMessage> result = new();
            int deletedFiles = 0;
            _displayResultMessages.Clear(MessageTab.Rules);

            foreach (RulesResourcesPair pair in sourceFiles)
            {
                List<ResultMessage> pairResults = new();

                UpdateResults
                (
                    await _fileSystemFileDeleter.Delete
                    (
                        pair.RulesFile,
                        application.RulesDeploymentPath,
                        cancellationTokenSource
                    ),
                    pair.RulesFile
                );

                UpdateResults
                (
                    await _fileSystemFileDeleter.Delete
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
                        (int)((float)++deletedFiles / (float)sourceFiles.Count * 100),
                        string.Format(CultureInfo.CurrentCulture, Strings.deletingRulesFormat, application.Nickname)
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
                                    Strings.fileDeleted,
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
