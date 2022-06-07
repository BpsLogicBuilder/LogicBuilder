using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using LogicBuilder.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator
{
    internal class DeleteSelectedFilesFromApi : IDeleteSelectedFilesFromApi
    {
        private readonly IApiFileListDeleter _apiFileListDeleter;
        private readonly IPathHelper _pathHelper;

        public DeleteSelectedFilesFromApi(IApiFileListDeleter apiFileListDeleter, IPathHelper pathHelper)
        {
            _apiFileListDeleter = apiFileListDeleter;
            _pathHelper = pathHelper;
        }

        public Task<IList<ResultMessage>> Delete(IList<RulesResourcesPair> sourceFiles, Application application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource) 
            => _apiFileListDeleter.Delete
            (
                new DeleteRulesData
                {
                    Application = application.Name,
                    Files = sourceFiles.Select(pair => _pathHelper.GetModuleName(pair.RulesFile)).ToArray(),
                    DeleteTime = DateTime.Now
                },
                application,
                progress,
                cancellationTokenSource
            );
    }
}
