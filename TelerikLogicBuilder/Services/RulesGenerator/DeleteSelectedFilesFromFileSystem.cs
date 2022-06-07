using ABIS.LogicBuilder.FlowBuilder.Configuration;
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
        private readonly IFileSystemFileDeleter _fileSystemFileDeleter;

        public DeleteSelectedFilesFromFileSystem(IFileSystemFileDeleter fileSystemFileDeleter)
        {
            _fileSystemFileDeleter = fileSystemFileDeleter;
        }

        public async Task<IList<ResultMessage>> Delete(IList<RulesResourcesPair> sourceFiles, Application application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
        {
            List<ResultMessage> result = new();
            int deletedFiles = 0;

            foreach (RulesResourcesPair pair in sourceFiles)
            {
                result.AddRange
                (
                    await _fileSystemFileDeleter.Delete
                    (
                        pair.RulesFile,
                        application.RulesDeploymentPath,
                        cancellationTokenSource
                    )
                );

                result.AddRange
                (
                    await _fileSystemFileDeleter.Delete
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
                        (int)((float)++deletedFiles / (float)sourceFiles.Count * 100),
                        string.Format(CultureInfo.CurrentCulture, Strings.deletingRulesFormat, application.Nickname)
                    )
                );
            }

            return result;
        }
    }
}
