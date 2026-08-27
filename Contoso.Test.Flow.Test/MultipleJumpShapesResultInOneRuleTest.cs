using Contoso.Test.Flow.Cache;
using LogicBuilder.App.Utils.Rules;
using LogicBuilder.RulesDirector;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Contoso.Test.Flow.Test
{
    public class MultipleJumpShapesResultInOneRuleTest
    {
        public MultipleJumpShapesResultInOneRuleTest(ITestOutputHelper output)
        {
            this.output = output;
            serviceProvider = GetServiceProvider();
        }

        [Fact]
        public void TestMultipleJumpShapesResultInOneRule()
        {
            //arrange
            IFlowManager flowManager = serviceProvider.GetRequiredService<IFlowManager>();

            //act
            System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();
            flowManager.Start("multiplejumpshapesresultinonerule");
            stopWatch.Stop();
            this.output.WriteLine("Running multiplejumpshapesresultinonerule  = {0}", stopWatch.Elapsed.TotalMilliseconds);

            //assert
            Assert.Equal("MultipleJumpShapesResultInOneRule", (string)flowManager.FlowDataCache.Items["StringItem"]);
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        private readonly ITestOutputHelper output;
        #endregion Fields

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
                .AddRulesCacheService
                (
                    new RulesLoaderRequest
                    (
                        "Contoso.Test.Flow.Rulesets",
                        typeof(FlowActivity),
                        [
                            typeof(Business.Requests.BaseRequest).Assembly,
                            typeof(LogicBuilder.App.Utils.Interfaces.ITypeHelper).Assembly,
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
