using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Contoso.BSL.AutoMapperProfiles;
using Contoso.Contexts;
using Contoso.Data.Entities;
using Contoso.Domain.Entities;
using Contoso.Repositories;
using Contoso.Stores;
using Contoso.Test.Business.Responses;
using LogicBuilder.App.Utils.Interfaces;
using LogicBuilder.App.Utils.Rules;
using LogicBuilder.EntityFrameworkCore.Mapping;
using LogicBuilder.EntityFrameworkCore.Repositories;
using LogicBuilder.Expressions.Utils.ExpressionBuilder;
using LogicBuilder.Forms.Parameters.Expressions;
using LogicBuilder.RulesDirector;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Contoso.Test.Flow.Test.LmbdaExpressions
{
    [Collection("DatabaseCollection")]
    public class CourseFilterTest
    {
        static CourseFilterTest()
        {
            InitializeMapperConfiguration();
        }

        public CourseFilterTest(DatabaseFixture databaseFixture, ITestOutputHelper output)
        {
            this.databaseFixture = databaseFixture;
            this.output = output;
            Initialize();
        }

        [Theory]
        [InlineData("filter-course-against-value-source-member", "f => (f.CourseID == Contoso.Domain.Entities.CourseModel.CourseID)")]
        [InlineData("filter-course-against-value", "f => (f.CourseID == 1050)")]
        public async Task SetFilter(string filterName, string filterString)
        {
            //arrange
            IFlowManager flowManager = serviceProvider.GetRequiredService<IFlowManager>();
            IMappingOperations mappingOperations = serviceProvider.GetRequiredService<IMappingOperations>();
            ISchoolRepository schoolRepository = serviceProvider!.GetRequiredService<ISchoolRepository>();
            var entity = new CourseModel
            {
                CourseID = 1050,
                Title = "Mathematics",
                Credits = 5
            };
            flowManager.FlowDataCache.Items[typeof(CourseModel).FullName!] = entity;
            flowManager.FlowDataCache.Response = new SaveEntityResponse { Entity = entity, Success = true };

            //act
            System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();
            flowManager.Start(filterName);
            stopWatch.Stop();
            this.output.WriteLine("Get course filter = {0}", stopWatch.Elapsed.TotalMilliseconds);

            //assert
            Assert.True(flowManager.FlowDataCache.Response.Success);
            Assert.NotNull(flowManager.FlowDataCache.Items[filterName]);
            Assert.IsType<FilterLambdaOperatorParameters>(flowManager.FlowDataCache.Items[filterName]);

            IExpressionPart filterLambdaOperator = mappingOperations.MapToOperator((FilterLambdaOperatorParameters)(flowManager.FlowDataCache.Items[filterName]));
            Expression<Func<CourseModel, bool>> filter = (Expression<Func<CourseModel, bool>>)filterLambdaOperator.Build();
            var course = (await schoolRepository.GetAsync<CourseModel, Course>
            (
                 filter
            )).Single();

            Assert.Equal(1050, course.CourseID);
            AssertFilterStringIsCorrect(filter, filterString);
        }

        #region Fields
        private IServiceProvider serviceProvider;
        private readonly DatabaseFixture databaseFixture;
        private readonly ITestOutputHelper output;
        private static MapperConfiguration MapperConfiguration;
        #endregion Fields

        #region Helpers
        private static void AssertFilterStringIsCorrect(Expression expression, string expected)
        {
            AssertStringIsCorrect(ExpressionStringBuilder.ToString(expression));

            void AssertStringIsCorrect(string resultExpression)
                => Assert.True
                (
                    expected == resultExpression,
                    $"Expected expression '{expected}' but the deserializer produced '{resultExpression}'"
                );
        }

        [MemberNotNull(nameof(MapperConfiguration))]
        private static void InitializeMapperConfiguration()
        {
            MapperConfiguration ??= ConfigurationHelper.GetMapperConfiguration(cfg =>
            {
                cfg.AddExpressionMapping();

                cfg.AddProfile<ExpressionOperatorsMappingProfile>();
                cfg.AddProfile<ExpressionParameterToDescriptorMappingProfile>();
                cfg.AddProfile<ExpansionParameterToDescriptorMappingProfile>();
                cfg.AddProfile<ExpansionDescriptorToOperatorMappingProfile>();
                cfg.AddProfile<SchoolProfile>();
            });
            MapperConfiguration.AssertConfigurationIsValid();
        }

        [MemberNotNull(nameof(serviceProvider))]
        private void Initialize()
        {
            serviceProvider = new ServiceCollection()
                .AddDbContext<SchoolContext>
                (
                    options => options.UseSqlServer
                    (
                        databaseFixture.GetConnectionString($"{GetType().Name}_{Guid.NewGuid():N}"),
                        options => options.EnableRetryOnFailure()
                    ),
                    ServiceLifetime.Transient
                )
                .AddTransient<ISchoolStore, SchoolStore>()
                .AddTransient<IContextRepository, SchoolRepository>()
                .AddTransient<ISchoolRepository, SchoolRepository>()
                .AddLogging()
                .AddBslUtilsServices()
                .AddServiceRegistrations()
                .AddAppUtilsServices()
                .AddRulesCacheService
                (
                    new RulesLoaderRequest
                    (
                        "Contoso.Test.Flow.Rulesets",
                        typeof(FlowActivity),
                        [
                            typeof(Business.Requests.BaseRequest).Assembly,
                            typeof(LogicBuilder.App.Spa.Forms.Parameters.CommandButtonParameters).Assembly,
                            typeof(LogicBuilder.App.Utils.Interfaces.ITypeHelper).Assembly,
                            typeof(LogicBuilder.Forms.Parameters.Expansions.SelectExpandDefinitionParameters).Assembly,
                            typeof(Contoso.Domain.Entities.StudentModel).Assembly,
                            typeof(Contoso.Data.Entities.Course).Assembly,
                            typeof(DirectorBase).Assembly,
                            typeof(string).Assembly
                        ]
                    )
                )
                .AddSingleton<AutoMapper.IConfigurationProvider>
                (
                    MapperConfiguration
                )
                .AddTransient<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService))
                .BuildServiceProvider();

            ReCreateDataBase(serviceProvider.GetRequiredService<SchoolContext>()).GetAwaiter().GetResult();
            DatabaseSeeder.Seed_Database(serviceProvider.GetRequiredService<ISchoolRepository>()).GetAwaiter().GetResult();
        }

        private static async Task ReCreateDataBase(SchoolContext context)
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }
        #endregion Helpers
    }
}
