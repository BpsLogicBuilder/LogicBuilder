using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator
{
    internal class FileSystemFileDeployer : IFileSystemFileDeployer
    {
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IPathHelper _pathHelper;

        public FileSystemFileDeployer(IFileIOHelper fileIOHelper, IPathHelper pathHelper)
        {
            _fileIOHelper = fileIOHelper;
            _pathHelper = pathHelper;
        }

        public Task<IList<ResultMessage>> Deploy(string sourceFile, string deploymentPath, CancellationTokenSource cancellationTokenSource) 
            => Task.Run
            (
                () => Deploy(sourceFile, deploymentPath), 
                cancellationTokenSource.Token
            );

        private IList<ResultMessage> Deploy(string sourceFile, string deploymentPath)
        {
            List<ResultMessage> result = new();
            try
            {
                FileInfo fileInfo = new(sourceFile);
                string deploymentFullName = _pathHelper.CombinePaths(deploymentPath, fileInfo.Name);
                _fileIOHelper.SetWritable(deploymentFullName, true);
                fileInfo.CopyTo(deploymentFullName, true);
            }
            catch (LogicBuilderException ex)
            {
                result.Add(new ResultMessage(ex.Message));
            }
            catch (ArgumentException ex)
            {
                result.Add(new ResultMessage(ex.Message));
            }
            catch (SecurityException ex)
            {
                result.Add(new ResultMessage(ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                result.Add(new ResultMessage(ex.Message));
            }
            catch (PathTooLongException ex)
            {
                result.Add(new ResultMessage(ex.Message));
            }
            catch (IOException ex)
            {
                result.Add(new ResultMessage(ex.Message));
            }
            catch (NotSupportedException ex)
            {
                result.Add(new ResultMessage(ex.Message));
            }

            return result;
        }
    }
}
