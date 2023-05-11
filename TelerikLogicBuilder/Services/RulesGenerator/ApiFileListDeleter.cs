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
    internal class ApiFileListDeleter : IApiFileListDeleter
    {
        #region Constants
        private const string WEB_REQUEST_CONTENT_TYPE = "application/json";
        #endregion Constants

        public async Task<IList<ResultMessage>> Delete(DeleteRulesData deleteRulesData, Application application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
        {
            List<ResultMessage> result = new();

            try
            {
                progress.Report
                (
                    new ProgressMessage
                    (
                        50,
                        string.Format(CultureInfo.CurrentCulture, Strings.deletingRulesFormat, application.Nickname)
                    )
                );

                using HttpClient client = new();
                HttpResponseMessage resultMessage = await client.PostAsync
                (
                    application.WebApiDeployment.DeleteRulesUrl,
                    new StringContent
                    (
                        System.Text.Json.JsonSerializer.Serialize
                        (
                            deleteRulesData
                        ),
                        Encoding.Unicode, WEB_REQUEST_CONTENT_TYPE
                    )
                );
                resultMessage.EnsureSuccessStatusCode();
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
