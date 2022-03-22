using Contoso.Test.Flow.Cache;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Contoso.Test.Flow
{
    public class CustomActions : ICustomActions
    {
        private readonly ILogger<CustomActions> logger;
        private readonly FlowDataCache flowDataCache;

        public CustomActions(ILogger<CustomActions> logger, FlowDataCache flowDataCache)
        {
            this.logger = logger;
            this.flowDataCache = flowDataCache;
        }

        public void WriteToLog(string message) => this.logger.LogInformation(message);

        public Task SetValueAync(string key, object value)
            => Task.Run(() => this.flowDataCache.Items[key] = value);

        public string GetString()
        {
            throw new System.NotImplementedException();
        }
    }
}
