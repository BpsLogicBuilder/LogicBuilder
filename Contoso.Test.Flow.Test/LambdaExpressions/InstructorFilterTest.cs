using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
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

namespace Contoso.Test.Flow.Test.LambdaExpressions
{
    [Collection("DatabaseCollection")]
    public class InstructorFilterTest
    {
        static InstructorFilterTest()
        {
            InitializeMapperConfiguration();
        }

        public InstructorFilterTest(DatabaseFixture databaseFixture, ITestOutputHelper output)
        {
            this.databaseFixture = databaseFixture;
            this.output = output;
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        private readonly DatabaseFixture databaseFixture;
        private readonly ITestOutputHelper output;
        private static MapperConfiguration MapperConfiguration;
        #endregion Fields

        [Theory]
        [InlineData("filter-instructor-against-value-source-member", "$it => ($it.ID == Contoso.Domain.Entities.InstructorModel.ID)")]
        [InlineData("filter-instructor-against-value", "f => (f.ID == 1)")]
        public async Task SetFilter(string filterName, string filterString)
        {
            //arrange
            IConfigurationService _configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            IFragmentListInitializer _fragmentListInitializer = serviceProvider.GetRequiredService<IFragmentListInitializer>();
            IFunctionListInitializer _functionListInitializer = serviceProvider.GetRequiredService<IFunctionListInitializer>();
            IVariableListInitializer _variableListInitializer = serviceProvider.GetRequiredService<IVariableListInitializer>();
            ILoadProjectProperties _loadProjectProperties = serviceProvider.GetRequiredService<ILoadProjectProperties>();
            IConstructorListInitializer _constructorListInitializer = serviceProvider.GetRequiredService<IConstructorListInitializer>();
            _configurationService.ProjectProperties = _loadProjectProperties.Load(Constants.ProjectFileFullPath);
            _configurationService.ConstructorList = _constructorListInitializer.InitializeList();
            _configurationService.FragmentList = _fragmentListInitializer.InitializeList();
            _configurationService.FunctionList = _functionListInitializer.InitializeList();
            _configurationService.VariableList = _variableListInitializer.InitializeList();

            IFlowManager flowManager = serviceProvider.GetRequiredService<IFlowManager>();
            IMappingOperations mappingOperations = serviceProvider.GetRequiredService<IMappingOperations>();
            ISchoolRepository schoolRepository = serviceProvider!.GetRequiredService<ISchoolRepository>();
            var entity = new InstructorModel
            {
                ID = 1,
                FirstName = "Firstname",
                LastName = "LastName"
            };
            flowManager.FlowDataCache.Items[typeof(InstructorModel).FullName!] = entity;
            flowManager.FlowDataCache.Response = new SaveEntityResponse { Entity = entity, Success = true };

            //act
            System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();
            flowManager.Start(filterName);
            stopWatch.Stop();
            this.output.WriteLine("Get instructor filter = {0}", stopWatch.Elapsed.TotalMilliseconds);

            //assert
            Assert.True(flowManager.FlowDataCache.Response.Success);
            Assert.NotNull(flowManager.FlowDataCache.Items[filterName]);
            Assert.IsType<FilterLambdaOperatorParameters>(flowManager.FlowDataCache.Items[filterName]);

            IExpressionPart filterLambdaOperator = mappingOperations.MapToOperator((FilterLambdaOperatorParameters)(flowManager.FlowDataCache.Items[filterName]));
            Expression<Func<InstructorModel, bool>> filter = (Expression<Func<InstructorModel, bool>>)filterLambdaOperator.Build();
            var instructor = (await schoolRepository.GetAsync<InstructorModel, Instructor>
            (
                 filter
            )).Single();

            Assert.Equal(1, instructor.ID);
            AssertFilterStringIsCorrect(filter, filterString);
        }

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
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection
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
                            typeof(ITypeHelper).Assembly,
                            typeof(LogicBuilder.Forms.Parameters.Expansions.SelectExpandDefinitionParameters).Assembly,
                            typeof(StudentModel).Assembly,
                            typeof(Course).Assembly,
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
