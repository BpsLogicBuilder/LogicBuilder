using Contoso.Domain.Entities;
using Contoso.Test.Business.Requests;
using Contoso.Test.Flow.Cache;
using LogicBuilder.App.Utils.Rules;
using LogicBuilder.RulesDirector;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Contoso.Test.Flow.Test
{
    public class SaveStudentTest
    {
        public SaveStudentTest(ITestOutputHelper output)
        {
            this.output = output;
            serviceProvider = GetServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        private readonly ITestOutputHelper output;
        #endregion Fields

        [Fact]
        public void SaveStudent()
        {
            //arrange
            IFlowManager flowManager = serviceProvider.GetRequiredService<IFlowManager>();
            flowManager.FlowDataCache.Request = new SaveEntityRequest
            {
                Entity = new StudentModel
                {
                    EntityState = LogicBuilder.Domain.EntityStateType.Modified,
                    FirstName = "John",
                    LastName = "Hopkins",
                    EnrollmentDate = new DateTime(2018, 3, 3)
                }
            };

            //act
            System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();
            flowManager.Start("savestudent");
            stopWatch.Stop();
            this.output.WriteLine("Saving valid student  = {0}", stopWatch.Elapsed.TotalMilliseconds);

            //assert
            Assert.True(flowManager.FlowDataCache.Response.Success);
        }

        [Fact]
        public void SaveInvalidStudent()
        {
            //arrange
            IFlowManager flowManager = serviceProvider.GetRequiredService<IFlowManager>();
            flowManager.FlowDataCache.Request = new SaveEntityRequest
            {
                Entity = new StudentModel
                {
                    EntityState = LogicBuilder.Domain.EntityStateType.Modified,
                    FirstName = "",
                    LastName = "",
                    EnrollmentDate = default
                }
            };

            //act
            System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();
            flowManager.Start("savestudent");
            stopWatch.Stop();
            this.output.WriteLine("Saving invalid student  = {0}", stopWatch.Elapsed.TotalMilliseconds);

            //assert
            Assert.False(flowManager.FlowDataCache.Response.Success);
            Assert.Equal(3, flowManager.FlowDataCache.Response.ErrorMessages.Count);
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
