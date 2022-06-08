﻿using ABIS.LogicBuilder.FlowBuilder.Configuration;
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
    internal class DeploySelectedFilesToApi : IDeploySelectedFilesToApi
    {
        private readonly IApiFileListDeployer _apiFileListDeployer;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IPathHelper _pathHelper;

        public DeploySelectedFilesToApi(IApiFileListDeployer apiFileListDeployer, IFileIOHelper fileIOHelper, IPathHelper pathHelper)
        {
            _apiFileListDeployer = apiFileListDeployer;
            _fileIOHelper = fileIOHelper;
            _pathHelper = pathHelper;
        }

        public Task<IList<ResultMessage>> Deploy(IList<RulesResourcesPair> sourceFiles, Application application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource) 
            => _apiFileListDeployer.Deploy
            (
                sourceFiles.Select
                (
                    pair => new ModuleData
                    {
                        Application = application.Name,
                        RulesStream = _fileIOHelper.GetFileBytes(pair.RulesFile),
                        ResourcesStream = _fileIOHelper.GetFileBytes(pair.ResourcesFile),
                        ModuleName = _pathHelper.GetModuleName(pair.RulesFile),
                        UploadedTime = DateTime.Now
                    }
                ).ToArray(),
                application,
                progress,
                cancellationTokenSource
            );
    }
}