using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using LogicBuilder.DataContracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator
{
    internal class ApiFileListDeployer : IApiFileListDeployer
    {
        #region Constants
        private const string WEB_REQUEST_CONTENT_TYPE = "application/json";
        #endregion Constants

        public async Task<IList<ResultMessage>> Deploy(IList<ModuleData> sourceFiles, Application application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
        {
            List<ResultMessage> result = new();

            try
            {
                int deployedFiles = 0;
                using HttpClient client = new();
                foreach (ModuleData moduleData in sourceFiles)
                {
                    HttpResponseMessage resultMessage = await client.PostAsync
                    (
                        application.WebApiDeployment.PostFileDataUrl,
                        new StringContent
                        (
                            System.Text.Json.JsonSerializer.Serialize(moduleData),
                            Encoding.Unicode,
                            WEB_REQUEST_CONTENT_TYPE
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
            }
            catch (LogicBuilderException ex)
            {
                result.Add(new ResultMessage(ex.Message));
            }
            catch (ArgumentException ex)
            {
                result.Add(new ResultMessage(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                result.Add(new ResultMessage(ex.Message));
            }
            catch (HttpRequestException ex)
            {
                result.Add(new ResultMessage(ex.Message));
            }
            catch (UriFormatException ex)
            {
                result.Add(new ResultMessage(ex.Message));
            }

            return result;
        }
    }
}
