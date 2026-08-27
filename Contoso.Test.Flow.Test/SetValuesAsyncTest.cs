using Contoso.Test.Flow.Cache;
using LogicBuilder.App.Utils.Rules;
using LogicBuilder.RulesDirector;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Contoso.Test.Flow.Test
{
    public class SetValuesAsyncTest
    {
        public SetValuesAsyncTest(ITestOutputHelper output)
        {
            this.output = output;
            serviceProvider = GetServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        private readonly ITestOutputHelper output;
        #endregion Fields

        [Fact]
        public void SetValues()
        {
            //arrange
            IFlowManager flowManager = serviceProvider.GetRequiredService<IFlowManager>();

            //act
            System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();
            flowManager.Start("setvaluesasync");
            stopWatch.Stop();
            this.output.WriteLine("Setting values async  = {0}", stopWatch.Elapsed.TotalMilliseconds);

            //assert
            Assert.Equal("A", flowManager.FlowDataCache.Items["A"].ToString());
            Assert.Equal("B", flowManager.FlowDataCache.Items["B"].ToString());
            Assert.Equal("C", flowManager.FlowDataCache.Items["C"].ToString());
            Assert.True(flowManager.FlowDataCache.Response.Success);
        }

        #region Helpers
        private IServiceProvider GetServiceProvider()
        {
            return new ServiceCollection()
                .AddLogging()
                .AddTransient<IFlowManager, FlowManager>()
                .AddTransient<FlowActivityFactory, FlowActivityFactory>()
                .AddTransient<DirectorFactory, DirectorFactory>()
                .AddTransient<ICustomActions, CustomActions>()
                .AddTransient<ICustomDialogs, CustomDialogs>()
                .AddSingleton<FlowDataCache, FlowDataCache>()
                .AddSingleton<Progress, Progress>()
                .AddAppUtilsServices()
                .AddRulesCacheService
                (
                    new RulesLoaderRequest
                    (
                        "Contoso.Test.Flow.Rulesets",
                        typeof(FlowActivity),
                        [
                            typeof(Business.Requests.BaseRequest).Assembly,
                            typeof(LogicBuilder.App.Utils.Interfaces.ITypeHelper).Assembly,
                            typeof(LogicBuilder.App.Spa.Forms.Parameters.CommandButtonParameters).Assembly,
                            typeof(LogicBuilder.Forms.Parameters.Expansions.SelectExpandDefinitionParameters).Assembly,
                            typeof(Contoso.Domain.Entities.StudentModel).Assembly,
                            typeof(Contoso.Data.Entities.Course).Assembly,
                            typeof(DirectorBase).Assembly,
                            typeof(string).Assembly
                        ]
                    )
                )
                .BuildServiceProvider();
        }
        #endregion Helpers
    }
}
