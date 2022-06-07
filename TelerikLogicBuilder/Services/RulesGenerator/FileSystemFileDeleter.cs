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
    internal class FileSystemFileDeleter : IFileSystemFileDeleter
    {
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IPathHelper _pathHelper;

        public FileSystemFileDeleter(IFileIOHelper fileIOHelper, IPathHelper pathHelper)
        {
            _fileIOHelper = fileIOHelper;
            _pathHelper = pathHelper;
        }

        public Task<IList<ResultMessage>> Delete(string sourceFile, string deploymentPath, CancellationTokenSource cancellationTokenSource)
            => Task.Run
            (
                () => Delete(sourceFile, deploymentPath),
                cancellationTokenSource.Token
            );

        private IList<ResultMessage> Delete(string sourceFile, string deploymentPath)
        {
            List<ResultMessage> result = new();
            try
            {
                string deploymentFullName = _pathHelper.CombinePaths(deploymentPath, _pathHelper.GetFileName(sourceFile));
                _fileIOHelper.SetWritable(deploymentFullName, true);
                File.Delete(deploymentFullName);
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
