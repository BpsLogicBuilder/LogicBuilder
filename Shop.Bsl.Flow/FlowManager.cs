using LogicBuilder.App.Bsl.Business.Responses;
using LogicBuilder.App.Bsl.Flow.Interfaces;
using LogicBuilder.RulesDirector;
using Microsoft.Extensions.Logging;
using Shop.Bsl.Flow.Factories;
using Shop.Bsl.Flow.Interfaces;
using System;
using System.Text.Json;

namespace Shop.Bsl.Flow
{
    public class FlowManager : IFlowManager
    {
        public FlowManager(
            ICustomActions customActions,
            IFlowDataCache flowDataCache,
            IFlowFactory flowFactory,
            ILogger<FlowManager> logger,
            Progress progress,
            IRulesCache rulesCache,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            CustomActions = customActions;
            FlowDataCache = flowDataCache;
            Progress = progress;
            RulesCache = rulesCache;
            ServiceProvider = serviceProvider;
            Director = flowFactory.GetDirector(this);
            FlowActivity = flowFactory.GetFlowActivity(this);
        }

        private readonly ILogger<FlowManager> _logger;

        public ICustomActions CustomActions { get; }

        public DirectorBase Director { get; }

        public IFlowActivity FlowActivity { get; }

        public IFlowDataCache FlowDataCache { get; }

        public Progress Progress { get; }

        public IRulesCache RulesCache { get; }

        public IServiceProvider ServiceProvider { get; }

        public void FlowComplete()
        {
            if (FlowDataCache.Response == null)
            {
                _logger.LogError("Response cannot be null.");
                throw new InvalidOperationException("Response cannot be null.");
            }
        }

        public void SetCurrentBusinessBackupData()
        {
        }

        public void Start(string module)
        {
            try
            {
                System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();
                this.Director.StartInitialFlow(module);
                stopWatch.Stop();
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("this.Director.StartInitialFlow: {Milliceonds}", stopWatch.Elapsed.TotalMilliseconds);
                }
            }
            catch (Exception ex)
            {
                FlowDataCache.Response = new ErrorResponse
                {
                    Success = false,
                    ErrorMessages = [ex.Message]
                };
                _logger.LogWarning(0, "Progress Start {Progress}", JsonSerializer.Serialize(this.Progress));
                _logger.LogError(ex, "{ExceptionType} : {ExceptionMessage}", ex.GetType().Name, ex.Message);
            }
        }

        public void Terminate()
        {
            throw new NotImplementedException();
        }
    }
}
