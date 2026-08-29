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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Contoso.Test.Flow.Test.LambdaExpressions
{
    [Collection("DatabaseCollection")]
    public class SelectorTests
    {
        static SelectorTests()
        {
            InitializeMapperConfiguration();
        }

        public SelectorTests(DatabaseFixture databaseFixture, ITestOutputHelper output)
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

        [Fact]
        public async Task Selector_group_students_by_enrollment_date()
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
            string selectorName = "selector-group-students-by-enrollment-date";
            var entity = new StudentModel
            {
                ID = 1,
                FirstName = "Firstname",
                LastName = "LastName"
            };
            flowManager.FlowDataCache.Items[typeof(StudentModel).FullName!] = entity;
            flowManager.FlowDataCache.Response = new SaveEntityResponse { Entity = entity, Success = true };

            //act
            System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();
            flowManager.Start(selectorName);
            stopWatch.Stop();
            this.output.WriteLine($"{selectorName} = {stopWatch.Elapsed.TotalMilliseconds}");

            //assert
            IExpressionPart selectorLambdaOperator = mappingOperations.MapToOperator((SelectorLambdaOperatorParameters)flowManager.FlowDataCache.Items[selectorName]);
            Expression<Func<IQueryable<StudentModel>, IEnumerable<object>>> selector = (Expression<Func<IQueryable<StudentModel>, IEnumerable<object>>>)selectorLambdaOperator.Build();

            IEnumerable<object> result = await schoolRepository.QueryAsync<StudentModel, Student, IEnumerable<object>, IEnumerable<object>>(selector);
            List<object> resultList = [.. result];
            Assert.NotEmpty(resultList);
            AssertFilterStringIsCorrect(selector, "$it => Convert($it.GroupBy(item => item.EnrollmentDate).OrderByDescending(group => group.Key).Select(sel => new AnonymousType() {enrollmentDate = sel.Key, count = Convert(sel.AsQueryable().Count())}))");
        }

        [Fact]
        public async Task Selector_select_course_credits_for_grid_filter()
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
            string selectorName = "selector-select-course-credits-for-grid-filter";
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
            flowManager.Start(selectorName);
            stopWatch.Stop();
            this.output.WriteLine($"{selectorName} = {stopWatch.Elapsed.TotalMilliseconds}");

            //assert
            IExpressionPart selectorLambdaOperator = mappingOperations.MapToOperator((SelectorLambdaOperatorParameters)flowManager.FlowDataCache.Items[selectorName]);
            Expression<Func<IQueryable<LookUpsModel>, IEnumerable<object>>> selector = (Expression<Func<IQueryable<LookUpsModel>, IEnumerable<object>>>)selectorLambdaOperator.Build();

            IEnumerable<object> result = await schoolRepository.QueryAsync<LookUpsModel, LookUps, IEnumerable<object>, IEnumerable<object>>(selector);
            List<object> resultList = [.. result];
            Assert.NotEmpty(resultList);
            AssertFilterStringIsCorrect(selector, "$it => Convert($it.Where(w => (w.ListName == \"Credits\")).OrderBy(o => o.NumericValue).Select(s => new AnonymousType() {credits = s.NumericValue}))");
        }

        [Fact]
        public async Task Selector_select_course_credits_text_and_value()
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
            string selectorName = "selector-select-course-credits-text-and-value";
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
            flowManager.Start(selectorName);
            stopWatch.Stop();
            this.output.WriteLine($"{selectorName} = {stopWatch.Elapsed.TotalMilliseconds}");

            //assert
            IExpressionPart selectorLambdaOperator = mappingOperations.MapToOperator((SelectorLambdaOperatorParameters)flowManager.FlowDataCache.Items[selectorName]);
            Expression<Func<IQueryable<LookUpsModel>, IQueryable<LookUpsModel>>> selector = (Expression<Func<IQueryable<LookUpsModel>, IQueryable<LookUpsModel>>>)selectorLambdaOperator.Build();

            IQueryable<LookUpsModel> result = await schoolRepository.QueryAsync<LookUpsModel, LookUps, IQueryable<LookUpsModel>, IQueryable<LookUps>>(selector);
            List<LookUpsModel> resultList = [.. result];
            Assert.NotEmpty(resultList);
            AssertFilterStringIsCorrect(selector, "$it => $it.Where(l => (l.ListName == \"Credits\")).OrderByDescending(l => l.NumericValue).Select(s => new LookUpsModel() {NumericValue = s.NumericValue, Text = s.Text})");
        }

        [Fact]
        public async Task Selector_select_course_id_for_grid_column_filter()
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
            string selectorName = "selector-select-course-id-for-grid-column-filter";
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
            flowManager.Start(selectorName);
            stopWatch.Stop();
            this.output.WriteLine($"{selectorName} = {stopWatch.Elapsed.TotalMilliseconds}");

            //assert
            IExpressionPart selectorLambdaOperator = mappingOperations.MapToOperator((SelectorLambdaOperatorParameters)flowManager.FlowDataCache.Items[selectorName]);
            Expression<Func<IQueryable<CourseModel>, IEnumerable<object>>> selector = (Expression<Func<IQueryable<CourseModel>, IEnumerable<object>>>)selectorLambdaOperator.Build();

            IEnumerable<object> result = await schoolRepository.QueryAsync<CourseModel, Course, IEnumerable<object>, IEnumerable<object>>(selector);
            List<object> resultList = [.. result];
            Assert.NotEmpty(resultList);
            AssertFilterStringIsCorrect(selector, "$it => Convert($it.OrderBy(o => o.CourseID).Select(s => new AnonymousType() {courseID = s.CourseID}))");
        }

        [Fact]
        public async Task Selector_select_course_title_and_id()
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
            string selectorName = "selector-select-course-title-and-id";
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
            flowManager.Start(selectorName);
            stopWatch.Stop();
            this.output.WriteLine($"{selectorName} = {stopWatch.Elapsed.TotalMilliseconds}");

            //assert
            IExpressionPart selectorLambdaOperator = mappingOperations.MapToOperator((SelectorLambdaOperatorParameters)flowManager.FlowDataCache.Items[selectorName]);
            Expression<Func<IQueryable<CourseModel>, IQueryable<CourseAssignmentModel>>> selector = (Expression<Func<IQueryable<CourseModel>, IQueryable<CourseAssignmentModel>>>)selectorLambdaOperator.Build();

            IQueryable<CourseAssignmentModel> result = await schoolRepository.QueryAsync<CourseModel, Course, IQueryable<CourseAssignmentModel>, IQueryable<CourseAssignment>>(selector);
            List<CourseAssignmentModel> resultList = [.. result];
            Assert.NotEmpty(resultList);
            AssertFilterStringIsCorrect(selector, "$it => $it.OrderBy(d => d.Title).Select(s => new CourseAssignmentModel() {CourseID = s.CourseID, CourseTitle = s.Title})");
        }

        [Fact]
        public async Task Selector_select_department_name_and_id()
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
            string selectorName = "selector-select-department-name-and-id";
            var entity = new DepartmentModel
            {
                DepartmentID= 1,
                Name = "Mathematics",
                Budget = 50000
            };
            flowManager.FlowDataCache.Items[typeof(DepartmentModel).FullName!] = entity;
            flowManager.FlowDataCache.Response = new SaveEntityResponse { Entity = entity, Success = true };

            //act
            System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();
            flowManager.Start(selectorName);
            stopWatch.Stop();
            this.output.WriteLine($"{selectorName} = {stopWatch.Elapsed.TotalMilliseconds}");

            //assert
            IExpressionPart selectorLambdaOperator = mappingOperations.MapToOperator((SelectorLambdaOperatorParameters)flowManager.FlowDataCache.Items[selectorName]);
            Expression<Func<IQueryable<DepartmentModel>, IQueryable<DepartmentModel>>> selector = (Expression<Func<IQueryable<DepartmentModel>, IQueryable<DepartmentModel>>>)selectorLambdaOperator.Build();

            IQueryable<DepartmentModel> result = await schoolRepository.QueryAsync<DepartmentModel, Department, IQueryable<DepartmentModel>, IQueryable<Department>>(selector);
            List<DepartmentModel> resultList = [.. result];
            Assert.NotEmpty(resultList);
            AssertFilterStringIsCorrect(selector, "$it => $it.OrderBy(d => d.Name).Select(d => new DepartmentModel() {DepartmentID = d.DepartmentID, Name = d.Name})");
        }

        [Fact]
        public async Task Selector_select_instructor_full_name_and_id()
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
            string selectorName = "selector-select-instructor-full-name-and-id";
            var entity = new InstructorModel
            {
                ID = 1,
                FirstName = "John",
                LastName = "Smith"
            };
            flowManager.FlowDataCache.Items[typeof(DepartmentModel).FullName!] = entity;
            flowManager.FlowDataCache.Response = new SaveEntityResponse { Entity = entity, Success = true };

            //act
            System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();
            flowManager.Start(selectorName);
            stopWatch.Stop();
            this.output.WriteLine($"{selectorName} = {stopWatch.Elapsed.TotalMilliseconds}");

            //assert
            IExpressionPart selectorLambdaOperator = mappingOperations.MapToOperator((SelectorLambdaOperatorParameters)flowManager.FlowDataCache.Items[selectorName]);
            Expression<Func<IQueryable<InstructorModel>, IQueryable<InstructorModel>>> selector = (Expression<Func<IQueryable<InstructorModel>, IQueryable<InstructorModel>>>)selectorLambdaOperator.Build();

            IQueryable<InstructorModel> result = await schoolRepository.QueryAsync<InstructorModel, Instructor, IQueryable<InstructorModel>, IQueryable<Instructor>>(selector);
            List<InstructorModel> resultList = [.. result];
            Assert.NotEmpty(resultList);
            AssertFilterStringIsCorrect(selector, "$it => $it.OrderBy(d => d.FullName).Select(s => new InstructorModel() {ID = s.ID, FirstName = s.FirstName, LastName = s.LastName, FullName = s.FullName})");
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
