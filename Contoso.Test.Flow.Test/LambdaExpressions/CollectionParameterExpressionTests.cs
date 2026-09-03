using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using LogicBuilder.App.Utils.Interfaces;
using LogicBuilder.App.Utils.Rules;
using LogicBuilder.EntityFrameworkCore.Mapping;
using LogicBuilder.Expressions.Utils.ExpressionBuilder;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.Lambda;
using LogicBuilder.Expressions.Utils.ExpressionDescriptors;
using LogicBuilder.Forms.Parameters.Expressions;
using LogicBuilder.RulesDirector;
using LogicBuilder.Workflow.Activities.Rules;
using Microsoft.Extensions.DependencyInjection;
using Shop.Bsl.Flow;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Contoso.Test.Flow.Test.LambdaExpressions
{
    public class CollectionParameterExpressionTests
    {
        static CollectionParameterExpressionTests()
        {
            InitializeMapperConfiguration();
        }

        public CollectionParameterExpressionTests()
        {
            Initialize();
            InitializeLogicBuilderMetadata();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        private static readonly string parameterName = "$it";
        private const string mduleName = "collection-parameter-expression-tests";

        private IApplicationTypeInfoManager _applicationTypeInfoManager;
        private IConfigurationService _configurationService;
        private IFragmentListInitializer _fragmentListInitializer;
        private IFunctionListInitializer _functionListInitializer;
        private IVariableListInitializer _variableListInitializer;
        private ILoadProjectProperties _loadProjectProperties;
        private IConstructorListInitializer _constructorListInitializer;
        private IXmlDocumentHelpers _xmlDocumentHelpers;
        private IRulesGeneratorFactory _rulesGeneratorFactory;
        private IConstructorDataParser _constructorDataParser;
        #endregion Fields

        [Fact]
        public async Task ConcatOperatorParametersWorks()
        {
            //act
            var descriptor = new ConcatOperatorParameters
            (
                new MemberSelectorOperatorParameters("AlternateAddresses", new ParameterOperatorParameters(parameterName)),
                new ConstantOperatorParameters(new Address[] { new() { City = "Seattle" }, new() { City = "Portland" } })
            );
            var expression = GetExpression<Product, IEnumerable<Address>>(descriptor);
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(ConcatOperatorParametersWorks)}";
            Expression<Func<Product, IEnumerable<Address>>> newSelector = (Expression<Func<Product, IEnumerable<Address>>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Product), null);
            var result = RunExpression
            (
                newSelector,
                new Product { AlternateAddresses = [new Address { City = "Redmond" }, new Address { City = "Seattle" }] }
            );

            AssertExpressionStringIsCorrect(expression, "$it => $it.AlternateAddresses.Concat(LogicBuilder.EntityFrameworkCore.Tests.Data.Address[])");
            Assert.Equal(4, result.Count());
        }

        [Fact]
        public async Task ExceptOperatorParametersWorks()
        {
            //act
            var descriptor = new ExceptOperatorParameters
            (
                new MemberSelectorOperatorParameters("AlternateAddresses", new ParameterOperatorParameters(parameterName)),
                new ConstantOperatorParameters(new Address[] { new() { City = "Seattle" }, new() { City = "Portland" } })
            );
            var expression = GetExpression<Product, IEnumerable<Address>>(descriptor);
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(ExceptOperatorParametersWorks)}";
            Expression<Func<Product, IEnumerable<Address>>> newSelector = (Expression<Func<Product, IEnumerable<Address>>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Product), null);
            var result = RunExpression
            (
                newSelector,
                new Product { AlternateAddresses = [new Address { City = "Redmond" }, new Address { City = "Seattle" }] }
            );

            AssertExpressionStringIsCorrect(expression, "$it => $it.AlternateAddresses.Except(LogicBuilder.EntityFrameworkCore.Tests.Data.Address[])");
            var item = Assert.Single(result);
            Assert.Equal(new Address { City = "Redmond" }, item);
        }

        [Fact]
        public async Task UnionOperatorParametersWorks()
        {
            //act
            var descriptor = new UnionOperatorParameters
            (
                new MemberSelectorOperatorParameters("AlternateAddresses", new ParameterOperatorParameters(parameterName)),
                new ConstantOperatorParameters(new Address[] { new() { City = "Seattle" }, new() { City = "Portland" } })
            );
            var expression = GetExpression<Product, IEnumerable<Address>>(descriptor);
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(UnionOperatorParametersWorks)}";
            Expression<Func<Product, IEnumerable<Address>>> newSelector = (Expression<Func<Product, IEnumerable<Address>>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Product), null);
            var result = RunExpression
            (
                newSelector,
                new Product { AlternateAddresses = [new Address { City = "Redmond" }, new Address { City = "Seattle" }] }
            );

            AssertExpressionStringIsCorrect(expression, "$it => $it.AlternateAddresses.Union(LogicBuilder.EntityFrameworkCore.Tests.Data.Address[])");
            Assert.Equal(3, result.Count());
        }

        private RuleEngine CreateRuleEngine(string formattedXml, string ruleName)
        {
            string selectedApplication = _configurationService.GetSelectedApplication().Name;
            ApplicationTypeInfo applicationTypeInfo = _applicationTypeInfoManager.GetApplicationTypeInfo(selectedApplication);
            ICodeExpressionBuilder codeExpressionBuilder = _rulesGeneratorFactory.GetCodeExpressionBuilder(applicationTypeInfo, new Dictionary<string, string>(), mduleName);
            ConstructorData constructorData = _constructorDataParser.Parse(_xmlDocumentHelpers.ToXmlElement(formattedXml));
            var codeExpression = codeExpressionBuilder.BuildConstructor(constructorData);

            CodeBinaryOperatorExpression alwaysTrueCondition = new()
            {
                Left = new CodePrimitiveExpression(1),
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = new CodePrimitiveExpression(1)
            };

            CodeFieldReferenceExpression flowManagerReference = new(new CodeThisReferenceExpression(), "_flowManager");
            CodePropertyReferenceExpression flowdataCacheReference = new(flowManagerReference, "FlowDataCache");
            CodePropertyReferenceExpression itemsReference = new(flowdataCacheReference, "Items");
            CodeIndexerExpression itemsIndexer = new(itemsReference, new CodePrimitiveExpression(ruleName));

            CodeAssignStatement setOperatorObject = new
            (
                itemsIndexer,
                codeExpression
            );

            Rule rule = new("RuleName")
            {
                Condition = new RuleExpressionCondition(alwaysTrueCondition)
            };
            rule.ThenActions.Add(new RuleStatementAction(setOperatorObject));

            RuleSet ruleSet = new() { Name = "MyRuleSet", ChainingBehavior = RuleChainingBehavior.Full };
            ruleSet.Rules.Add(rule);
            RuleValidation ruleValidation = RuleValidationHelper.GetValidation(ruleSet, typeof(Shop.Bsl.Flow.FlowActivity));

            return new(ruleSet, ruleValidation);
        }

        [MemberNotNull(nameof(MapperConfiguration))]
        private static void InitializeMapperConfiguration()
        {
            MapperConfiguration ??= ConfigurationHelper.GetMapperConfiguration(cfg =>
            {
                cfg.AddExpressionMapping();
                cfg.AddProfile<ExpressionOperatorsMappingProfile>();
                cfg.AddProfile<ExpressionParameterToDescriptorMappingProfile>();
            });
        }

        static MapperConfiguration MapperConfiguration;

        [MemberNotNull(nameof(serviceProvider))]
        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection
                .AddSingleton<AutoMapper.IConfigurationProvider>
                (
                    MapperConfiguration
                )
                .AddTransient<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService))
                .AddAppUtilsServices()
                .AddLogging()
                .AddFlowFactories()
                .AddTransient<Shop.Bsl.Flow.Interfaces.IFlowManager, Shop.Bsl.Flow.FlowManager>()
                .AddTransient<Shop.Bsl.Flow.Factories.IFlowFactory, Shop.Bsl.Flow.Factories.FlowFactory>()
                .AddTransient<LogicBuilder.App.Bsl.Flow.Interfaces.ICustomActions, LogicBuilder.App.Bsl.Flow.CustomActions>()
                .AddSingleton<LogicBuilder.App.Bsl.Flow.Interfaces.IFlowDataCache, LogicBuilder.App.Bsl.Flow.FlowDataCache>()
                .AddSingleton<Progress, Progress>()
                .AddSingleton<IRulesCache>(sp => new RulesCache([], []))
                .BuildServiceProvider();
        }

        [MemberNotNull(nameof(_configurationService))]
        [MemberNotNull(nameof(_fragmentListInitializer))]
        [MemberNotNull(nameof(_functionListInitializer))]
        [MemberNotNull(nameof(_variableListInitializer))]
        [MemberNotNull(nameof(_loadProjectProperties))]
        [MemberNotNull(nameof(_constructorListInitializer))]
        [MemberNotNull(nameof(_xmlDocumentHelpers))]
        [MemberNotNull(nameof(_applicationTypeInfoManager))]
        [MemberNotNull(nameof(_rulesGeneratorFactory))]
        [MemberNotNull(nameof(_constructorDataParser))]
        private void InitializeLogicBuilderMetadata()
        {
            _configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            _fragmentListInitializer = serviceProvider.GetRequiredService<IFragmentListInitializer>();
            _functionListInitializer = serviceProvider.GetRequiredService<IFunctionListInitializer>();
            _variableListInitializer = serviceProvider.GetRequiredService<IVariableListInitializer>();
            _loadProjectProperties = serviceProvider.GetRequiredService<ILoadProjectProperties>();
            _constructorListInitializer = serviceProvider.GetRequiredService<IConstructorListInitializer>();
            _xmlDocumentHelpers = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            _applicationTypeInfoManager = serviceProvider.GetRequiredService<IApplicationTypeInfoManager>();
            _rulesGeneratorFactory = serviceProvider.GetRequiredService<IRulesGeneratorFactory>();
            _constructorDataParser = serviceProvider.GetRequiredService<IConstructorDataParser>();
            _configurationService.ProjectProperties = _loadProjectProperties.Load(Constants.ShopBslProjectFileFullPath);
            _configurationService.ConstructorList = _constructorListInitializer.InitializeList();
            _configurationService.FragmentList = _fragmentListInitializer.InitializeList();
            _configurationService.FunctionList = _functionListInitializer.InitializeList();
            _configurationService.VariableList = _variableListInitializer.InitializeList();
            _configurationService.UseLongStrings = true;
        }

        private static Dictionary<string, ParameterExpression> GetParameters()
            => [];

        private Expression<Func<T, TResult>> GetExpression<T, TResult>(IExpressionParameter filterBody, string defaultParameterName = "$it")
        {
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();
            DescriptorBase descriptorFilterBody = mapper.Map<DescriptorBase>(filterBody);
            return (Expression<Func<T, TResult>>)mapper.Map<SelectorLambdaOperator>
            (
                new SelectorLambdaDescriptor
                (
                    descriptorFilterBody,
                    typeof(T).AssemblyQualifiedName!,
                    defaultParameterName,
                    typeof(TResult).AssemblyQualifiedName
                ),
                opts => opts.Items["parameters"] = GetParameters()
            ).Build();
        }

        private async Task<LambdaExpression> RecreateSelectorFromSelectorLambdaOperatorParameters(LambdaExpression filter, string ruleName, Type entityType, object? entity)
        {
            string formattedXml = _xmlDocumentHelpers.GetXmlString(ConstructorXmlBuilder.ToContructorDefinitionXml(filter, serviceProvider));
            await File.WriteAllTextAsync
            (
                Path.Combine(ProjectDirectory.GetPath(), Constants.FilterResultsFolder, $"{ruleName}.xml"),
                formattedXml,
                CancellationToken.None
            );

            Shop.Bsl.Flow.Interfaces.IFlowManager flowManager = serviceProvider.GetRequiredService<Shop.Bsl.Flow.Interfaces.IFlowManager>();
            IMappingOperations mappingOperations = serviceProvider.GetRequiredService<IMappingOperations>();
            flowManager.FlowDataCache.Items[entityType.FullName!] = entity!;
            flowManager.FlowDataCache.Response = new LogicBuilder.App.Bsl.Business.Responses.SaveEntityResponse { Entity = null, Success = true };

            RuleEngine ruleEngine = CreateRuleEngine(formattedXml, ruleName);
            ruleEngine.Execute(new Shop.Bsl.Flow.FlowActivity(flowManager));

            IExpressionPart filterLambdaOperator = mappingOperations.MapToOperator((SelectorLambdaOperatorParameters)(flowManager.FlowDataCache.Items[ruleName]));

            return (LambdaExpression)filterLambdaOperator.Build();
        }

        private static TResult RunExpression<T, TResult>(Expression<Func<T, TResult>> filter, T instance)
            => filter.Compile().Invoke(instance);

        private static void AssertExpressionStringIsCorrect(Expression expression, string expected)
        {
            AssertStringIsCorrect(ExpressionStringBuilder.ToString(expression));

            void AssertStringIsCorrect(string resultExpression)
                => Assert.True
                (
                    expected == resultExpression,
                    $"Expected expression '{expected}' but the deserializer produced '{resultExpression}'"
                );
        }
    }
}
