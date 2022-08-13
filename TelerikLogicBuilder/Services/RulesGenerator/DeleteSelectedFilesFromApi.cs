using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Enums;
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
        private readonly IDisplayResultMessages _displayResultMessages;
        private readonly IPathHelper _pathHelper;

        public DeleteSelectedFilesFromApi(
            IApiFileListDeleter apiFileListDeleter,
            IDisplayResultMessages displayResultMessages,
            IPathHelper pathHelper)
        {
            _apiFileListDeleter = apiFileListDeleter;
            _displayResultMessages = displayResultMessages;
            _pathHelper = pathHelper;
        }

        public async Task<IList<ResultMessage>> Delete(IList<RulesResourcesPair> sourceFiles, Application application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
        {
            _displayResultMessages.Clear(MessageTab.Rules);

            IList<ResultMessage> result = await _apiFileListDeleter.Delete
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

            if (result.Count == 0)
                result.Add(new ResultMessage(Strings.operationComplete));

            foreach (ResultMessage resultMessage in result)
                _displayResultMessages.AppendMessage(resultMessage, MessageTab.Rules);

            return result;
        }
    }
}
