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
using Microsoft.OData.Edm;
using Shop.Bsl.Flow;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Contoso.Test.Flow.Test.LambdaExpressions
{
    public class FilterParameterTests
    {
        static FilterParameterTests()
        {
            Initialize();
        }

        public FilterParameterTests()
        {
            InitializeLogicBuilderMetadata();
        }

        #region Fields
        private static IServiceProvider serviceProvider;
        private static readonly string parameterName = "$it";
        private static MapperConfiguration MapperConfiguration;
        private const string mduleName = "filter-parameter-tests";

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

        #region Inequalities
        [Theory]
        [InlineData(null, true)]
        [InlineData("", false)]
        [InlineData("Doritos", false)]
        public async Task EqualityOperatorWithNull(string? productName, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { ProductName = productName };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(EqualityOperatorWithNull)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>) await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.ProductName == null)");
            Assert.Equal(expected, result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters(null!)
                    )
                );
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("Doritos", true)]
        public async Task EqualityOperator(string? productName, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { ProductName = productName };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(EqualityOperator)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>) await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.ProductName == \"Doritos\")");
            Assert.Equal(expected, result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters("Doritos", typeof(string))
                    )
                );
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData("", true)]
        [InlineData("Doritos", false)]
        public async Task NotEqualOperatorParameters(string? productName, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { ProductName = productName };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(NotEqualOperatorParameters)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>) await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.ProductName != \"Doritos\")");
            Assert.Equal(expected, result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new NotEqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters("Doritos", typeof(string))
                    )
                );
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData(5.01, true)]
        [InlineData(4.99, false)]
        public async Task GreaterThanOperatorParameters(object? unitPrice, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { UnitPrice = ToNullable<decimal>(unitPrice) };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(GreaterThanOperatorParameters)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, string.Format(CultureInfo.InvariantCulture, "$it => ($it.UnitPrice > Convert({0:0.00}))", 5.0));
            Assert.Equal(expected, result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new GreaterThanBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("UnitPrice", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters(5.00m, typeof(decimal))
                    )
                );
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData(5.0, true)]
        [InlineData(4.99, false)]
        public async Task GreaterThanEqualOperatorParameters(object? unitPrice, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { UnitPrice = ToNullable<decimal>(unitPrice) };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(GreaterThanEqualOperatorParameters)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, string.Format(CultureInfo.InvariantCulture, "$it => ($it.UnitPrice >= Convert({0:0.00}))", 5.0));
            Assert.Equal(expected, result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new GreaterThanOrEqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("UnitPrice", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters(5.00m, typeof(decimal))
                    )
                );
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData(4.99, true)]
        [InlineData(5.01, false)]
        public async Task LessThanOperatorParameters(object? unitPrice, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { UnitPrice = ToNullable<decimal>(unitPrice) };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(LessThanOperatorParameters)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, string.Format(CultureInfo.InvariantCulture, "$it => ($it.UnitPrice < Convert({0:0.00}))", 5.0));
            Assert.Equal(expected, result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new LessThanBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("UnitPrice", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters(5.00m, typeof(decimal))
                    )
                );
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData(5.0, true)]
        [InlineData(5.01, false)]
        public async Task LessThanOrEqualOperatorParameters(object? unitPrice, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { UnitPrice = ToNullable<decimal>(unitPrice) };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(LessThanOrEqualOperatorParameters)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, string.Format(CultureInfo.InvariantCulture, "$it => ($it.UnitPrice <= Convert({0:0.00}))", 5.0));
            Assert.Equal(expected, result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new LessThanOrEqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("UnitPrice", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters(5.00m, typeof(decimal))
                    )
                );
        }

        [Fact]
        public async Task NegativeNumbers()
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { UnitPrice = ToNullable<decimal>(44m) };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(NegativeNumbers)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, string.Format(CultureInfo.InvariantCulture, "$it => ($it.UnitPrice <= Convert({0:0.00}))", -5.0));
            Assert.False(result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new LessThanOrEqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("UnitPrice", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters(-5.00m, typeof(decimal))
                    )
                );
        }

        public class DateTimeOffsetInequalitiesTheoryData(IExpressionParameter filterBody, string expectedExpression)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
        }

        public static TheoryData<DateTimeOffsetInequalitiesTheoryData> DateTimeOffsetInequalities_Data
            =>
            [
                new DateTimeOffsetInequalitiesTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("DateTimeOffsetProp", new ParameterOperatorParameters(parameterName)),
                        new MemberSelectorOperatorParameters("DateTimeOffsetProp", new ParameterOperatorParameters(parameterName))
                    ),
                    "$it => ($it.DateTimeOffsetProp == $it.DateTimeOffsetProp)"
                ),
                new DateTimeOffsetInequalitiesTheoryData
                (
                    new NotEqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("DateTimeOffsetProp", new ParameterOperatorParameters(parameterName)),
                        new MemberSelectorOperatorParameters("DateTimeOffsetProp", new ParameterOperatorParameters(parameterName))
                    ),
                    "$it => ($it.DateTimeOffsetProp != $it.DateTimeOffsetProp)"
                ),
                new DateTimeOffsetInequalitiesTheoryData
                (
                    new GreaterThanOrEqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("DateTimeOffsetProp", new ParameterOperatorParameters(parameterName)),
                        new MemberSelectorOperatorParameters("DateTimeOffsetProp", new ParameterOperatorParameters(parameterName))
                    ),
                    "$it => ($it.DateTimeOffsetProp >= $it.DateTimeOffsetProp)"
                ),
                new DateTimeOffsetInequalitiesTheoryData
                (
                    new LessThanOrEqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("DateTimeOffsetProp", new ParameterOperatorParameters(parameterName)),
                        new MemberSelectorOperatorParameters("DateTimeOffsetProp", new ParameterOperatorParameters(parameterName))
                    ),
                    "$it => ($it.DateTimeOffsetProp <= $it.DateTimeOffsetProp)"
                )
            ];

        [Theory]
        [MemberData(nameof(DateTimeOffsetInequalities_Data), MemberType = typeof(FilterParameterTests))]
        public async Task DateTimeOffsetInequalities(DateTimeOffsetInequalitiesTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<DataTypes>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(DateTimeOffsetInequalities)}";
            Expression<Func<DataTypes, bool>> newFilter = (Expression<Func<DataTypes, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(DataTypes), null);

            //assert
            AssertFilterStringIsCorrect(newFilter, theoryData.ExpectedExpression);

            Expression<Func<T, bool>> CreateFilter<T>()
            {
                return GetFilter<T>
                (
                    theoryData.FilterBody
                );
            }
        }

        public class DateInEqualitiesTheoryData(IExpressionParameter filterBody, string expectedExpression)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
        }

        public static TheoryData<DateInEqualitiesTheoryData> DateInEqualities_Data
            =>
            [
                new DateInEqualitiesTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("DateTimeProp", new ParameterOperatorParameters(parameterName)),
                        new MemberSelectorOperatorParameters("DateTimeProp", new ParameterOperatorParameters(parameterName))
                    ),
                    "$it => ($it.DateTimeProp == $it.DateTimeProp)"
                ),
                new DateInEqualitiesTheoryData
                (
                    new NotEqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("DateTimeProp", new ParameterOperatorParameters(parameterName)),
                        new MemberSelectorOperatorParameters("DateTimeProp", new ParameterOperatorParameters(parameterName))
                    ),
                    "$it => ($it.DateTimeProp != $it.DateTimeProp)"
                ),
                new DateInEqualitiesTheoryData
                (
                    new GreaterThanOrEqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("DateTimeProp", new ParameterOperatorParameters(parameterName)),
                        new MemberSelectorOperatorParameters("DateTimeProp", new ParameterOperatorParameters(parameterName))
                    ),
                    "$it => ($it.DateTimeProp >= $it.DateTimeProp)"
                ),
                new DateInEqualitiesTheoryData
                (
                    new LessThanOrEqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("DateTimeProp", new ParameterOperatorParameters(parameterName)),
                        new MemberSelectorOperatorParameters("DateTimeProp", new ParameterOperatorParameters(parameterName))
                    ),
                    "$it => ($it.DateTimeProp <= $it.DateTimeProp)"
                )
            ];

        [Theory]
        [MemberData(nameof(DateInEqualities_Data), MemberType = typeof(FilterParameterTests))]
        public async Task DateInEqualities(DateInEqualitiesTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<DataTypes>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(DateInEqualities)}";
            Expression<Func<DataTypes, bool>> newFilter = (Expression<Func<DataTypes, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(DataTypes), null);

            //assert
            AssertFilterStringIsCorrect(newFilter, theoryData.ExpectedExpression);

            Expression<Func<T, bool>> CreateFilter<T>()
            {
                return GetFilter<T>
                (
                    theoryData.FilterBody
                );
            }
        }
        #endregion Inequalities

        #region Logical Operators
        [Fact]
        public async Task BooleanOperatorNullableTypes()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(BooleanOperatorNullableTypes)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => (($it.UnitPrice == Convert(5.00)) OrElse ($it.CategoryID == 0))");

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new OrBinaryOperatorParameters
                    (
                        new EqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("UnitPrice", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters(5.00m, typeof(decimal))
                        ),
                        new EqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters(0, typeof(int))
                        )
                    )
                );
        }

        [Fact]
        public async Task BooleanComparisonOnNullableAndNonNullableType()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(BooleanComparisonOnNullableAndNonNullableType)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.Discontinued == Convert(True))");

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("Discontinued", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters(true, typeof(bool))
                    )
                );
        }

        [Fact]
        public async Task BooleanComparisonOnNullableType()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(BooleanComparisonOnNullableType)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.Discontinued == $it.Discontinued)");

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("Discontinued", new ParameterOperatorParameters(parameterName)),
                        new MemberSelectorOperatorParameters("Discontinued", new ParameterOperatorParameters(parameterName))
                    )
                );
        }

        [Theory]
        [InlineData(null, null, false)]
        [InlineData(5.0, 0, true)]
        [InlineData(null, 1, false)]
        public async Task OrOperatorParameters(object? unitPrice, object? unitsInStock, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { UnitPrice = ToNullable<decimal>(unitPrice), UnitsInStock = ToNullable<short>(unitsInStock) };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(OrOperatorParameters)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, string.Format(CultureInfo.InvariantCulture, "$it => (($it.UnitPrice == Convert({0:0.00})) OrElse (Convert($it.UnitsInStock) == Convert({1})))", 5.0, 0));
            Assert.Equal(expected, result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new OrBinaryOperatorParameters
                    (
                        new EqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("UnitPrice", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters(5.00m, typeof(decimal))
                        ),
                        new EqualsBinaryOperatorParameters
                        (
                            new ConvertOperatorParameters(new MemberSelectorOperatorParameters("UnitsInStock", new ParameterOperatorParameters(parameterName)), typeof(int?)),
                            new ConstantOperatorParameters(0, typeof(int))
                        )
                    )
                );
        }

        [Theory]
        [InlineData(null, null, false)]
        [InlineData(5.0, 10, true)]
        [InlineData(null, 1, false)]
        public async Task AndOperatorParameters(object? unitPrice, object? unitsInStock, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { UnitPrice = ToNullable<decimal>(unitPrice), UnitsInStock = ToNullable<short>(unitsInStock) };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(AndOperatorParameters)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, string.Format(CultureInfo.InvariantCulture, "$it => (($it.UnitPrice == Convert({0:0.00})) AndAlso (Convert($it.UnitsInStock) == Convert({1:0.00})))", 5.0, 10.0));
            Assert.Equal(expected, result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new AndBinaryOperatorParameters
                    (
                        new EqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("UnitPrice", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters(5.00m, typeof(decimal))
                        ),
                        new EqualsBinaryOperatorParameters
                        (
                            new ConvertOperatorParameters(new MemberSelectorOperatorParameters("UnitsInStock", new ParameterOperatorParameters(parameterName)), typeof(decimal?)),
                            new ConstantOperatorParameters(10.00m, typeof(decimal))
                        )
                    )
                );
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData(5.0, false)]
        [InlineData(5.5, true)]
        public async Task Negation(object? unitPrice, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { UnitPrice = ToNullable<decimal>(unitPrice) };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(Negation)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, string.Format(CultureInfo.InvariantCulture, "$it => Not(($it.UnitPrice == Convert({0:0.00})))", 5.0));
            Assert.Equal(expected, result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new NotOperatorParameters
                    (
                        new EqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("UnitPrice", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters(5.00m, typeof(decimal))
                        )
                    )
                );
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public async Task BoolNegation(bool discontinued, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { Discontinued = discontinued };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(BoolNegation)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => Convert(Not($it.Discontinued))");
            Assert.Equal(expected, result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new NotOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("Discontinued", new ParameterOperatorParameters(parameterName))
                    )
                );
        }

        [Fact]
        public async Task NestedNegation()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(NestedNegation)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => Convert(Not(Not(Not($it.Discontinued))))");

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new NotOperatorParameters
                    (
                        new NotOperatorParameters
                        (
                            new NotOperatorParameters
                            (
                                new MemberSelectorOperatorParameters("Discontinued", new ParameterOperatorParameters(parameterName))
                            )
                        )
                    )
                );
        }
        #endregion Logical Operators

        #region Arithmetic Operators
        [Theory]
        [InlineData(null, false)]
        [InlineData(5.0, true)]
        [InlineData(15.01, false)]
        public async Task Subtraction(object? unitPrice, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { UnitPrice = ToNullable<decimal>(unitPrice) };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(Subtraction)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, string.Format(CultureInfo.InvariantCulture, "$it => (($it.UnitPrice - Convert({0:0.00})) < Convert({1:0.00}))", 1.0, 5.0));
            Assert.Equal(expected, result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new LessThanBinaryOperatorParameters
                    (
                        new SubtractBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("UnitPrice", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters(1.00m, typeof(decimal))
                        ),
                        new ConstantOperatorParameters(5.00m, typeof(decimal))
                    )
                );
        }

        [Fact]
        public async Task Addition()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(Addition)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null);

            //assert
            AssertFilterStringIsCorrect(newFilter, string.Format(CultureInfo.InvariantCulture, "$it => (($it.UnitPrice + Convert({0:0.00})) < Convert({1:0.00}))", 1.0, 5.0));

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new LessThanBinaryOperatorParameters
                    (
                        new AddBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("UnitPrice", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters(1.00m, typeof(decimal))
                        ),
                        new ConstantOperatorParameters(5.00m, typeof(decimal))
                    )
                );
        }

        [Fact]
        public async Task Multiplication()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(Multiplication)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null);

            //assert
            AssertFilterStringIsCorrect(newFilter, string.Format(CultureInfo.InvariantCulture, "$it => (($it.UnitPrice * Convert({0:0.00})) < Convert({1:0.00}))", 1.0, 5.0));

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new LessThanBinaryOperatorParameters
                    (
                        new MultiplyBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("UnitPrice", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters(1.00m, typeof(decimal))
                        ),
                        new ConstantOperatorParameters(5.00m, typeof(decimal))
                    )
                );
        }

        [Fact]
        public async Task Division()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(Division)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null);

            //assert
            AssertFilterStringIsCorrect(newFilter, string.Format(CultureInfo.InvariantCulture, "$it => (($it.UnitPrice / Convert({0:0.00})) < Convert({1:0.00}))", 1.0, 5.0));

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new LessThanBinaryOperatorParameters
                    (
                        new DivideBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("UnitPrice", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters(1.00m, typeof(decimal))
                        ),
                        new ConstantOperatorParameters(5.00m, typeof(decimal))
                    )
                );
        }

        [Fact]
        public async Task Modulo()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(Modulo)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, string.Format(CultureInfo.InvariantCulture, "$it => (($it.UnitPrice % Convert({0:0.00})) < Convert({1:0.00}))", 1.0, 5.0));

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new LessThanBinaryOperatorParameters
                    (
                        new ModuloBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("UnitPrice", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters(1.00m, typeof(decimal))
                        ),
                        new ConstantOperatorParameters(5.00m, typeof(decimal))
                    )
                );
        }
        #endregion Arithmetic Operators

        #region NULL handling
        public class NullHandlingTheoryData(IExpressionParameter filterBody, object? unitsInStock, object? unitsOnOrder, bool expectedResult)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public object? UnitsInStock { get; } = unitsInStock;
            public object? UnitsOnOrder { get; } = unitsOnOrder;
            public bool ExpectedResult { get; } = expectedResult;
        }

        public static TheoryData<NullHandlingTheoryData> NullHandling_Data
            =>
            [
                new NullHandlingTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("UnitsInStock", new ParameterOperatorParameters(parameterName)),
                        new MemberSelectorOperatorParameters("UnitsOnOrder", new ParameterOperatorParameters(parameterName))
                    ),
                    null,
                    null,
                    true
                ),
                new NullHandlingTheoryData
                (
                    new NotEqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("UnitsInStock", new ParameterOperatorParameters(parameterName)),
                        new MemberSelectorOperatorParameters("UnitsOnOrder", new ParameterOperatorParameters(parameterName))
                    ),
                    null,
                    null,
                    false
                ),
                new NullHandlingTheoryData
                (
                    new GreaterThanBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("UnitsInStock", new ParameterOperatorParameters(parameterName)),
                        new MemberSelectorOperatorParameters("UnitsOnOrder", new ParameterOperatorParameters(parameterName))
                    ),
                    null,
                    null,
                    false
                ),
                new NullHandlingTheoryData
                (
                    new GreaterThanOrEqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("UnitsInStock", new ParameterOperatorParameters(parameterName)),
                        new MemberSelectorOperatorParameters("UnitsOnOrder", new ParameterOperatorParameters(parameterName))
                    ),
                    null,
                    null,
                    false
                ),
                new NullHandlingTheoryData
                (
                    new LessThanBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("UnitsInStock", new ParameterOperatorParameters(parameterName)),
                        new MemberSelectorOperatorParameters("UnitsOnOrder", new ParameterOperatorParameters(parameterName))
                    ),
                    null,
                    null,
                    false
                ),
                new NullHandlingTheoryData
                (
                    new LessThanOrEqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("UnitsInStock", new ParameterOperatorParameters(parameterName)),
                        new MemberSelectorOperatorParameters("UnitsOnOrder", new ParameterOperatorParameters(parameterName))
                    ),
                    null,
                    null,
                    false
                ),
                new NullHandlingTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new AddBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("UnitsInStock", new ParameterOperatorParameters(parameterName)),
                            new MemberSelectorOperatorParameters("UnitsOnOrder", new ParameterOperatorParameters(parameterName))
                        ),
                        new MemberSelectorOperatorParameters("UnitsInStock", new ParameterOperatorParameters(parameterName))
                    ),
                    null,
                    null,
                    true
                ),
                new NullHandlingTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new SubtractBinaryOperatorParameters

                        (
                            new MemberSelectorOperatorParameters("UnitsInStock", new ParameterOperatorParameters(parameterName)),
                            new MemberSelectorOperatorParameters("UnitsOnOrder", new ParameterOperatorParameters(parameterName))
                        ),
                        new MemberSelectorOperatorParameters("UnitsInStock", new ParameterOperatorParameters(parameterName))
                    ),
                    null,
                    null,
                    true
                ),
                new NullHandlingTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MultiplyBinaryOperatorParameters

                        (
                            new MemberSelectorOperatorParameters("UnitsInStock", new ParameterOperatorParameters(parameterName)),
                            new MemberSelectorOperatorParameters("UnitsOnOrder", new ParameterOperatorParameters(parameterName))
                        ),
                        new MemberSelectorOperatorParameters("UnitsInStock", new ParameterOperatorParameters(parameterName))
                    ),
                    null,
                    null,
                    true
                ),
                new NullHandlingTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new DivideBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("UnitsInStock", new ParameterOperatorParameters(parameterName)),
                            new MemberSelectorOperatorParameters("UnitsOnOrder", new ParameterOperatorParameters(parameterName))
                        ),
                        new MemberSelectorOperatorParameters("UnitsInStock", new ParameterOperatorParameters(parameterName))
                    ),
                    null,
                    null,
                    true
                ),
                new NullHandlingTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ModuloBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("UnitsInStock", new ParameterOperatorParameters(parameterName)),
                            new MemberSelectorOperatorParameters("UnitsOnOrder", new ParameterOperatorParameters(parameterName))
                        ),
                        new MemberSelectorOperatorParameters("UnitsInStock", new ParameterOperatorParameters(parameterName))
                    ),
                    null,
                    null,
                    true
                ),
                new NullHandlingTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("UnitsInStock", new ParameterOperatorParameters(parameterName)),
                        new MemberSelectorOperatorParameters("UnitsOnOrder", new ParameterOperatorParameters(parameterName))
                    ),
                    1,
                    null,
                    false
                ),
                new NullHandlingTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("UnitsInStock", new ParameterOperatorParameters(parameterName)),
                        new MemberSelectorOperatorParameters("UnitsOnOrder", new ParameterOperatorParameters(parameterName))
                    ),
                    1,
                    1,
                    true
                )
            ];

        [Theory]
        [MemberData(nameof(NullHandling_Data), MemberType = typeof(FilterParameterTests))]
        public async Task NullHandling(NullHandlingTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { UnitsInStock = ToNullable<short>(theoryData.UnitsInStock), UnitsOnOrder = ToNullable<short>(theoryData.UnitsOnOrder) };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(NullHandling)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            Assert.Equal(theoryData.ExpectedResult, result);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class NullHandling_LiteralNullTheoryData(IExpressionParameter filterBody, object? unitsInStock, bool expectedResult)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public object? UnitsInStock { get; } = unitsInStock;
            public bool ExpectedResult { get; } = expectedResult;
        }

        public static TheoryData<NullHandling_LiteralNullTheoryData> NullHandling_LiteralNull_Data
            =>
            [
                new NullHandling_LiteralNullTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("UnitsInStock", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters(null!)
                    ),
                    null,
                    true
                ),
                new NullHandling_LiteralNullTheoryData
                (
                    new NotEqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("UnitsInStock", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters(null!)
                    ),
                    null,
                    false
                )
            ];

        [Theory]
        [MemberData(nameof(NullHandling_LiteralNull_Data), MemberType = typeof(FilterParameterTests))]
        public async Task NullHandling_LiteralNull(NullHandling_LiteralNullTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { UnitsInStock = ToNullable<short>(theoryData.UnitsInStock) };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(NullHandling_LiteralNull)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            Assert.Equal(theoryData.ExpectedResult, result);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }
        #endregion NULL handling

        public class ComparisonsInvolvingCastsAndNullableValuesTheoryData(IExpressionParameter filterBody)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
        }

        public static TheoryData<ComparisonsInvolvingCastsAndNullableValuesTheoryData> ComparisonsInvolvingCastsAndNullableValues_Data
            =>
            [
                new ComparisonsInvolvingCastsAndNullableValuesTheoryData
                (
                    new GreaterThanBinaryOperatorParameters
                    (
                        new IndexOfOperatorParameters
                        (
                            new ConstantOperatorParameters("hello"),
                            new MemberSelectorOperatorParameters("StringProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConvertOperatorParameters

                        (
                            new MemberSelectorOperatorParameters("UIntProp", new ParameterOperatorParameters(parameterName)),
                            typeof(int?)
                        )
                    )
                ),
                new ComparisonsInvolvingCastsAndNullableValuesTheoryData
                (
                    new GreaterThanBinaryOperatorParameters
                    (
                        new IndexOfOperatorParameters
                        (
                            new ConstantOperatorParameters("hello"),
                            new MemberSelectorOperatorParameters("StringProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConvertOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ULongProp", new ParameterOperatorParameters(parameterName)),
                            typeof(int?)
                        )
                    )
                ),
                new ComparisonsInvolvingCastsAndNullableValuesTheoryData
                (
                    new GreaterThanBinaryOperatorParameters
                    (
                        new IndexOfOperatorParameters
                        (
                            new ConstantOperatorParameters("hello"),
                            new MemberSelectorOperatorParameters("StringProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConvertOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("UShortProp", new ParameterOperatorParameters(parameterName)),
                            typeof(int?)
                        )
                    )
                ),
                new ComparisonsInvolvingCastsAndNullableValuesTheoryData
                (
                    new GreaterThanBinaryOperatorParameters
                    (
                        new IndexOfOperatorParameters
                        (
                            new ConstantOperatorParameters("hello"),
                            new MemberSelectorOperatorParameters("StringProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConvertOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableUShortProp", new ParameterOperatorParameters(parameterName)),
                            typeof(int?)
                        )
                    )
                ),
                new ComparisonsInvolvingCastsAndNullableValuesTheoryData
                (
                    new GreaterThanBinaryOperatorParameters
                    (
                        new IndexOfOperatorParameters
                        (
                            new ConstantOperatorParameters("hello"),
                            new MemberSelectorOperatorParameters("StringProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConvertOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableUIntProp", new ParameterOperatorParameters(parameterName)),
                            typeof(int?)
                        )
                    )
                ),
                new ComparisonsInvolvingCastsAndNullableValuesTheoryData
                (
                    new GreaterThanBinaryOperatorParameters
                    (
                        new IndexOfOperatorParameters
                        (
                            new ConstantOperatorParameters("hello"),
                            new MemberSelectorOperatorParameters("StringProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConvertOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableULongProp", new ParameterOperatorParameters(parameterName)),
                            typeof(int?)
                        )
                    )
                )
            ];

        [Theory]
        [MemberData(nameof(ComparisonsInvolvingCastsAndNullableValues_Data), MemberType = typeof(FilterParameterTests))]
        public async Task ComparisonsInvolvingCastsAndNullableValues(ComparisonsInvolvingCastsAndNullableValuesTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<DataTypes>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(ComparisonsInvolvingCastsAndNullableValues)}";
            Expression<Func<DataTypes, bool>> newFilter = (Expression<Func<DataTypes, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(DataTypes), null);

            //assert
            Assert.Throws<ArgumentNullException>(() => RunFilter(newFilter, new DataTypes()));

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        [Theory]
        [InlineData(null, null, true)]
        [InlineData("not doritos", 0, true)]
        [InlineData("Doritos", 1, false)]
        public async Task Grouping(string? productName, object? unitsInStock, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { ProductName = productName, UnitsInStock = ToNullable<short>(unitsInStock) };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(Grouping)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, string.Format(CultureInfo.InvariantCulture, "$it => (($it.ProductName != \"Doritos\") OrElse ($it.UnitPrice < Convert({0:0.00})))", 5.0));
            Assert.Equal(expected, result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new OrBinaryOperatorParameters
                    (
                        new NotEqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters("Doritos")
                        ),
                        new LessThanBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("UnitPrice", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters(5.00m, typeof(decimal))
                        )
                    )
                );
        }

        [Fact]
        public async Task MemberExpressions()
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { Category = new Category { CategoryName = "Snacks" } };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(Grouping)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            Assert.Throws<NullReferenceException>(() => RunFilter(newFilter, new Product { }));
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.Category.CategoryName == \"Snacks\")");
            Assert.True(result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "CategoryName",
                            new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("Snacks")
                    )
                );
        }

        [Fact]
        public async Task MemberExpressionsRecursive()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(BooleanComparisonOnNullableType)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            Assert.Throws<NullReferenceException>(() => RunFilter(newFilter, new Product { }));
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.Category.Product.Category.CategoryName == \"Snacks\")");

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "CategoryName",
                            new MemberSelectorOperatorParameters
                            (
                                "Category",
                                new MemberSelectorOperatorParameters

                                (
                                    "Product",
                                    new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                                )
                            )
                        ),
                        new ConstantOperatorParameters("Snacks")
                    )
                );
        }

        [Fact]
        public async Task ComplexPropertyNavigation()
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { SupplierAddress = new Address { City = "Redmond" } };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(ComplexPropertyNavigation)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            Assert.Throws<NullReferenceException>(() => RunFilter(newFilter, new Product { }));
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.SupplierAddress.City == \"Redmond\")");
            Assert.True(result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "City",
                            new MemberSelectorOperatorParameters("SupplierAddress", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("Redmond")
                    )
                );
        }

        #region Any/All
        [Fact]
        public async Task AnyOnNavigationEnumerableCollections()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(AnyOnNavigationEnumerableCollections)}";
            var entity = new Product
            {
                Category = new Category
                {
                    EnumerableProducts =
                        [
                            new Product { ProductName = "Snacks" },
                            new Product { ProductName = "NonSnacks" }
                        ]
                }
            };
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);

            bool result1 = RunFilter
            (
                newFilter,
                entity
            );

            entity = new Product
            {
                Category = new Category
                {
                    EnumerableProducts =
                        [
                            new Product { ProductName = "NonSnacks" }
                        ]
                }
            };

            bool result2 = RunFilter
            (
                newFilter,
                entity
            );

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => $it.Category.EnumerableProducts.Any(P => (P.ProductName == \"Snacks\"))");
            Assert.True(result1);
            Assert.False(result2);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new AnyOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "EnumerableProducts",
                            new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                        ),
                        new EqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters("P")),
                            new ConstantOperatorParameters("Snacks")
                        ),
                        "P"
                    )
                );
        }

        [Fact]
        public async Task AnyOnNavigationQueryableCollections()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(AnyOnNavigationQueryableCollections)}";
            var entity = new Product
            {
                Category = new Category
                {
                    QueryableProducts = new Product[]
                        {
                            new() { ProductName = "Snacks" },
                            new() { ProductName = "NonSnacks" }
                        }.AsQueryable()
                }
            };
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);

            bool result1 = RunFilter
            (
                newFilter,
                entity
            );

            entity = new Product
            {
                Category = new Category
                {
                    QueryableProducts = new Product[]
                        {
                            new() { ProductName = "NonSnacks" }
                        }.AsQueryable()
                }
            };

            bool result2 = RunFilter
            (
                newFilter,
                entity
            );

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => $it.Category.QueryableProducts.Any(P => (P.ProductName == \"Snacks\"))");
            Assert.True(result1);
            Assert.False(result2);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new AnyOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "QueryableProducts",
                            new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                        ),
                        new EqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters("P")),
                            new ConstantOperatorParameters("Snacks")
                        ),
                        "P"
                    )
                );
        }

        public class AnyInOnNavigationTheoryData(IExpressionParameter filterBody, string expectedExpression)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
        }

        public static TheoryData<AnyInOnNavigationTheoryData> AnyInOnNavigation_Data
            =>
            [
                new AnyInOnNavigationTheoryData
                (
                    new AnyOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "QueryableProducts",
                            new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                        ),
                        new InOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ProductID", new ParameterOperatorParameters("P")),
                            new CollectionConstantOperatorParameters
                            (
                                [1],
                                typeof(int)
                            )
                        ),
                        "P"
                    ),
                    "$it => $it.Category.QueryableProducts.Any(P => System.Collections.Generic.List`1[System.Int32].Contains(P.ProductID))"
                ),
                new AnyInOnNavigationTheoryData
                (
                    new AnyOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "EnumerableProducts",
                            new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                        ),
                        new InOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ProductID", new ParameterOperatorParameters("P")),
                            new CollectionConstantOperatorParameters
                            (
                                [1],
                                typeof(int)
                            )
                        ),
                        "P"
                    ),
                    "$it => $it.Category.EnumerableProducts.Any(P => System.Collections.Generic.List`1[System.Int32].Contains(P.ProductID))"
                ),
                new AnyInOnNavigationTheoryData
                (
                    new AnyOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "QueryableProducts",
                            new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                        ),
                        new InOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("GuidProperty", new ParameterOperatorParameters("P")),
                            new CollectionConstantOperatorParameters
                            (
                                [new Guid("dc75698b-581d-488b-9638-3e28dd51d8f7")],
                                typeof(Guid)
                            )
                        ),
                        "P"
                    ),
                    "$it => $it.Category.QueryableProducts.Any(P => System.Collections.Generic.List`1[System.Guid].Contains(P.GuidProperty))"
                ),
                new AnyInOnNavigationTheoryData
                (
                    new AnyOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "EnumerableProducts",
                            new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                        ),
                        new InOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("GuidProperty", new ParameterOperatorParameters("P")),
                            new CollectionConstantOperatorParameters
                            (
                                [new Guid("dc75698b-581d-488b-9638-3e28dd51d8f7")],
                                typeof(Guid)
                            )
                        ),
                        "P"
                    ),
                    "$it => $it.Category.EnumerableProducts.Any(P => System.Collections.Generic.List`1[System.Guid].Contains(P.GuidProperty))"
                ),
                new AnyInOnNavigationTheoryData
                (
                    new AnyOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "QueryableProducts",
                            new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                        ),
                        new InOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableGuidProperty", new ParameterOperatorParameters("P")),
                            new CollectionConstantOperatorParameters
                            (
                                [new Guid("dc75698b-581d-488b-9638-3e28dd51d8f7")],
                                typeof(Guid?)
                            )
                        ),
                        "P"
                    ),
                    "$it => $it.Category.QueryableProducts.Any(P => System.Collections.Generic.List`1[System.Nullable`1[System.Guid]].Contains(P.NullableGuidProperty))"
                ),
                new AnyInOnNavigationTheoryData
                (
                    new AnyOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "EnumerableProducts",
                            new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                        ),
                        new InOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableGuidProperty", new ParameterOperatorParameters("P")),
                            new CollectionConstantOperatorParameters
                            (
                                [new Guid("dc75698b-581d-488b-9638-3e28dd51d8f7")],
                                typeof(Guid?)
                            )
                        ),
                        "P"
                    ),
                    "$it => $it.Category.EnumerableProducts.Any(P => System.Collections.Generic.List`1[System.Nullable`1[System.Guid]].Contains(P.NullableGuidProperty))"
                ),
                new AnyInOnNavigationTheoryData
                (
                    new AnyOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "QueryableProducts",
                            new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                        ),
                        new InOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("Discontinued", new ParameterOperatorParameters("P")),
                            new CollectionConstantOperatorParameters
                            (
                                [false, null!],
                                typeof(bool?)
                            )
                        ),
                        "P"
                    ),
                    "$it => $it.Category.QueryableProducts.Any(P => System.Collections.Generic.List`1[System.Nullable`1[System.Boolean]].Contains(P.Discontinued))"
                ),
                new AnyInOnNavigationTheoryData
                (
                    new AnyOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "EnumerableProducts",
                            new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                        ),
                        new InOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("Discontinued", new ParameterOperatorParameters("P")),
                            new CollectionConstantOperatorParameters
                            (
                                [false, null!],
                                typeof(bool?)
                            )
                        ),
                        "P"
                    ),
                    "$it => $it.Category.EnumerableProducts.Any(P => System.Collections.Generic.List`1[System.Nullable`1[System.Boolean]].Contains(P.Discontinued))"
                )
            ];

        [Theory]
        [MemberData(nameof(AnyInOnNavigation_Data), MemberType = typeof(FilterParameterTests))]
        public async Task AnyInOnNavigation(AnyInOnNavigationTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(AnyInOnNavigation)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, theoryData.ExpectedExpression);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class AnyOnNavigation_ContradictionTheoryData(IExpressionParameter filterBody, string expectedExpression)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
        }

        public static TheoryData<AnyOnNavigation_ContradictionTheoryData> AnyOnNavigation_Contradiction_Data
            =>
            [
                new AnyOnNavigation_ContradictionTheoryData
                (
                    new AnyOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "QueryableProducts",
                            new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(false),
                        "P"
                    ),
                    "$it => $it.Category.QueryableProducts.Any(P => False)"
                ),
                new AnyOnNavigation_ContradictionTheoryData
                (
                    new AnyOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "QueryableProducts",
                            new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                        ),
                        new AndBinaryOperatorParameters
                        (
                            new ConstantOperatorParameters(false),
                            new EqualsBinaryOperatorParameters
                            (
                                new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters("P")),
                                new ConstantOperatorParameters("Snacks")
                            )
                        ),
                        "P"
                    ),
                    "$it => $it.Category.QueryableProducts.Any(P => (False AndAlso (P.ProductName == \"Snacks\")))"
                ),
                new AnyOnNavigation_ContradictionTheoryData
                (
                    new AnyOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "QueryableProducts",
                            new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                        )
                    ),
                    "$it => $it.Category.QueryableProducts.Any()"
                )
            ];

        [Theory]
        [MemberData(nameof(AnyOnNavigation_Contradiction_Data), MemberType = typeof(FilterParameterTests))]
        public async Task AnyOnNavigation_Contradiction(AnyOnNavigation_ContradictionTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(AnyOnNavigation_Contradiction)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, theoryData.ExpectedExpression);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        [Fact]
        public async Task AnyOnNavigation_NullCollection()
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product
            {
                Category = new Category
                {
                    EnumerableProducts =
                        [
                            new Product { ProductName = "Snacks" }
                        ]
                }
            };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(AnyOnNavigation_NullCollection)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            Assert.Throws<ArgumentNullException>(() => RunFilter(newFilter, new Product { Category = new Category { } }));
            AssertFilterStringIsCorrect(newFilter, "$it => $it.Category.EnumerableProducts.Any(P => (P.ProductName == \"Snacks\"))");
            Assert.True(result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new AnyOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "EnumerableProducts",
                            new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                        ),
                        new EqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters("P")),
                            new ConstantOperatorParameters("Snacks")
                        ),
                        "P"
                    )
                );
        }


        [Fact]
        public async Task AllOnNavigation_NullCollection()
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product
            {
                Category = new Category
                {
                    EnumerableProducts =
                        [
                            new Product { ProductName = "Snacks" }
                        ]
                }
            };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(AllOnNavigation_NullCollection)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            Assert.Throws<ArgumentNullException>(() => RunFilter(newFilter, new Product { Category = new Category { } }));
            AssertFilterStringIsCorrect(newFilter, "$it => $it.Category.EnumerableProducts.All(P => (P.ProductName == \"Snacks\"))");
            Assert.True(result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new AllOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "EnumerableProducts",
                            new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                        ),
                        new EqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters("P")),
                            new ConstantOperatorParameters("Snacks")
                        ),
                        "P"
                    )
                );
        }

        [Fact]
        public async Task MultipleAnys_WithSameRangeVariableName()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(MultipleAnys_WithSameRangeVariableName)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.AlternateIDs.Any(n => (n == 42)) AndAlso $it.AlternateAddresses.Any(n => (n.City == \"Redmond\")))");

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new AndBinaryOperatorParameters
                    (
                        new AnyOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("AlternateIDs", new ParameterOperatorParameters(parameterName)),
                            new EqualsBinaryOperatorParameters
                            (
                                new ParameterOperatorParameters("n"),
                                new ConstantOperatorParameters(42)
                            ),
                            "n"
                        ),
                        new AnyOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("AlternateAddresses", new ParameterOperatorParameters(parameterName)),
                            new EqualsBinaryOperatorParameters
                            (
                                new MemberSelectorOperatorParameters("City", new ParameterOperatorParameters("n")),
                                new ConstantOperatorParameters("Redmond")
                            ),
                            "n"
                        )
                    )
                );
        }

        [Fact]
        public async Task MultipleAlls_WithSameRangeVariableName()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(MultipleAlls_WithSameRangeVariableName)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.AlternateIDs.All(n => (n == 42)) AndAlso $it.AlternateAddresses.All(n => (n.City == \"Redmond\")))");
            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new AndBinaryOperatorParameters
                    (
                        new AllOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("AlternateIDs", new ParameterOperatorParameters(parameterName)),
                            new EqualsBinaryOperatorParameters
                            (
                                new ParameterOperatorParameters("n"),
                                new ConstantOperatorParameters(42)
                            ),
                            "n"
                        ),
                        new AllOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("AlternateAddresses", new ParameterOperatorParameters(parameterName)),
                            new EqualsBinaryOperatorParameters
                            (
                                new MemberSelectorOperatorParameters("City", new ParameterOperatorParameters("n")),
                                new ConstantOperatorParameters("Redmond")
                            ),
                            "n"
                        )
                    )
                );
        }

        [Fact]
        public async Task AnyOnNavigationEnumerableCollections_EmptyFilter()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(AnyOnNavigationEnumerableCollections_EmptyFilter)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => $it.Category.EnumerableProducts.Any()");

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new AnyOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "EnumerableProducts",
                            new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                        )
                    )
                );
        }

        [Fact]
        public async Task AnyOnNavigationQueryableCollections_EmptyFilter()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(AnyOnNavigationQueryableCollections_EmptyFilter)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => $it.Category.QueryableProducts.Any()");

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new AnyOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "QueryableProducts",
                            new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                        )
                    )
                );
        }

        [Fact]
        public async Task AllOnNavigationEnumerableCollections()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(AllOnNavigationEnumerableCollections)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => $it.Category.EnumerableProducts.All(P => (P.ProductName == \"Snacks\"))");

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new AllOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "EnumerableProducts",
                            new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                        ),
                        new EqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters("P")),
                            new ConstantOperatorParameters("Snacks")
                        ),
                        "P"
                    )
                );
        }

        [Fact]
        public async Task AllOnNavigationQueryableCollections()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(AllOnNavigationQueryableCollections)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => $it.Category.QueryableProducts.All(P => (P.ProductName == \"Snacks\"))");

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new AllOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "QueryableProducts",
                            new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                        ),
                        new EqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters("P")),
                            new ConstantOperatorParameters("Snacks")
                        ),
                        "P"
                    )
                );
        }

        [Fact]
        public async Task AnyInSequenceNotNested()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(AnyInSequenceNotNested)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.Category.QueryableProducts.Any(P => (P.ProductName == \"Snacks\")) OrElse $it.Category.QueryableProducts.Any(P2 => (P2.ProductName == \"Snacks\")))");

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new OrBinaryOperatorParameters
                    (
                        new AnyOperatorParameters
                        (
                            new MemberSelectorOperatorParameters
                            (
                                "QueryableProducts",
                                new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                            ),
                            new EqualsBinaryOperatorParameters
                            (
                                new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters("P")),
                                new ConstantOperatorParameters("Snacks")
                            ),
                            "P"
                        ),
                        new AnyOperatorParameters
                        (
                            new MemberSelectorOperatorParameters
                            (
                                "QueryableProducts",
                                new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                            ),
                            new EqualsBinaryOperatorParameters
                            (
                                new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters("P2")),
                                new ConstantOperatorParameters("Snacks")
                            ),
                            "P2"
                        )
                    )
                );
        }

        [Fact]
        public async Task AllInSequenceNotNested()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(AllInSequenceNotNested)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.Category.QueryableProducts.All(P => (P.ProductName == \"Snacks\")) OrElse $it.Category.QueryableProducts.All(P2 => (P2.ProductName == \"Snacks\")))");

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new OrBinaryOperatorParameters
                    (
                        new AllOperatorParameters
                        (
                            new MemberSelectorOperatorParameters
                            (
                                "QueryableProducts",
                                new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                            ),
                            new EqualsBinaryOperatorParameters
                            (
                                new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters("P")),
                                new ConstantOperatorParameters("Snacks")
                            ),
                            "P"
                        ),
                        new AllOperatorParameters
                        (
                            new MemberSelectorOperatorParameters
                            (
                                "QueryableProducts",
                                new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                            ),
                            new EqualsBinaryOperatorParameters
                            (
                                new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters("P2")),
                                new ConstantOperatorParameters("Snacks")
                            ),
                            "P2"
                        )
                    )
                );
        }

        [Fact]
        public async Task AnyOnPrimitiveCollection()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(AnyOnPrimitiveCollection)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            bool result1 = RunFilter
            (
                newFilter,
                new Product { AlternateIDs = [1, 2, 42] }
            );

            bool result2 = RunFilter
            (
                newFilter,
                new Product { AlternateIDs = [1, 2] }
            );

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => $it.AlternateIDs.Any(id => (id == 42))");
            Assert.True(result1);
            Assert.False(result2);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new AnyOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("AlternateIDs", new ParameterOperatorParameters(parameterName)),
                        new EqualsBinaryOperatorParameters
                        (
                            new ParameterOperatorParameters("id"),
                            new ConstantOperatorParameters(42)
                        ),
                        "id"
                    )
                );
        }

        [Fact]
        public async Task AllOnPrimitiveCollection()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(AllOnPrimitiveCollection)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => $it.AlternateIDs.All(id => (id == 42))");

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new AllOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("AlternateIDs", new ParameterOperatorParameters(parameterName)),
                        new EqualsBinaryOperatorParameters
                        (
                            new ParameterOperatorParameters("id"),
                            new ConstantOperatorParameters(42)
                        ),
                        "id"
                    )
                );
        }

        [Fact]
        public async Task AnyOnComplexCollection()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(AnyOnComplexCollection)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            bool result = RunFilter
            (
                newFilter,
                new Product { AlternateAddresses = [new Address { City = "Redmond" }] }
            );

            //assert
            Assert.Throws<ArgumentNullException>(() => RunFilter(newFilter, new Product { }));
            AssertFilterStringIsCorrect(newFilter, "$it => $it.AlternateAddresses.Any(address => (address.City == \"Redmond\"))");
            Assert.True(result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new AnyOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("AlternateAddresses", new ParameterOperatorParameters(parameterName)),
                        new EqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("City", new ParameterOperatorParameters("address")),
                            new ConstantOperatorParameters("Redmond")
                        ),
                        "address"
                    )
                );
        }

        [Fact]
        public async Task AllOnComplexCollection()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(AllOnComplexCollection)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => $it.AlternateAddresses.All(address => (address.City == \"Redmond\"))");

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new AllOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("AlternateAddresses", new ParameterOperatorParameters(parameterName)),
                        new EqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("City", new ParameterOperatorParameters("address")),
                            new ConstantOperatorParameters("Redmond")
                        ),
                        "address"
                    )
                );
        }

        [Fact]
        public async Task RecursiveAllAny()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(RecursiveAllAny)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => $it.Category.QueryableProducts.All(P => P.Category.EnumerableProducts.Any(PP => (PP.ProductName == \"Snacks\")))");

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new AllOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "QueryableProducts",
                            new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                        ),
                        new AnyOperatorParameters
                        (
                            new MemberSelectorOperatorParameters
                            (
                                "EnumerableProducts",
                                new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters("P"))
                            ),
                            new EqualsBinaryOperatorParameters
                            (
                                new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters("PP")),
                                new ConstantOperatorParameters("Snacks")
                            ),
                            "PP"
                        ),
                        "P"
                    )
                );
        }
        #endregion Any/All

        #region String Functions
        [Theory]
        [InlineData("Abcd", 0, "Abcd", true)]
        [InlineData("Abcd", 1, "bcd", true)]
        [InlineData("Abcd", 3, "d", true)]
        [InlineData("Abcd", 4, "", true)]
        public async Task StringSubstringStart(string productName, int startIndex, string compareString, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { ProductName = productName };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(StringSubstringStart)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            Assert.Equal(expected, result);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new SubstringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters(startIndex)
                        ),
                        new ConstantOperatorParameters(compareString)
                    )
                );
        }

        [Theory]
        [InlineData("Abcd", -1, "Abcd")]
        [InlineData("Abcd", 5, "")]
        public async Task StringSubstringStartOutOfRange(string productName, int startIndex, string compareString)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { ProductName = productName };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(StringSubstringStartOutOfRange)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);

            //assert
            Assert.Throws<ArgumentOutOfRangeException>(() => RunFilter(newFilter, entity));

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new SubstringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters(startIndex)
                        ),
                        new ConstantOperatorParameters(compareString)
                    )
                );
        }

        [Theory]
        [InlineData("Abcd", 0, 1, "A", true)]
        [InlineData("Abcd", 0, 4, "Abcd", true)]
        [InlineData("Abcd", 0, 3, "Abc", true)]
        [InlineData("Abcd", 1, 3, "bcd", true)]
        [InlineData("Abcd", 2, 1, "c", true)]
        [InlineData("Abcd", 3, 1, "d", true)]
        public async Task StringSubstringStartAndLength(string productName, int startIndex, int length, string compareString, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { ProductName = productName };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(StringSubstringStartAndLength)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            Assert.Equal(expected, result);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new SubstringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters(startIndex),
                            new ConstantOperatorParameters(length)
                        ),
                        new ConstantOperatorParameters(compareString)
                    )
                );
        }

        [Theory]
        [InlineData("Abcd", -1, 4, "Abcd")]
        [InlineData("Abcd", -1, 3, "Abc")]
        [InlineData("Abcd", 0, 5, "Abcd")]
        [InlineData("Abcd", 1, 5, "bcd")]
        [InlineData("Abcd", 4, 1, "")]
        [InlineData("Abcd", 0, -1, "")]
        [InlineData("Abcd", 5, -1, "")]
        public async Task StringSubstringStartAndLengthOutOfRange(string productName, int startIndex, int length, string compareString)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { ProductName = productName };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(StringSubstringStartAndLengthOutOfRange)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);

            //assert
            Assert.Throws<ArgumentOutOfRangeException>(() => RunFilter(newFilter, entity));

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new SubstringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters(startIndex),
                            new ConstantOperatorParameters(length)
                        ),
                        new ConstantOperatorParameters(compareString)
                    )
                );
        }

        [Theory]
        [InlineData("Abcd", true)]
        [InlineData("Abd", false)]
        public async Task StringContains(string productName, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { ProductName = productName };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(StringContains)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => $it.ProductName.Contains(\"Abc\")");
            Assert.Equal(expected, result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new ContainsOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters("Abc")
                    )
                );
        }

        [Fact]
        public async Task StringContainsNullReferenceException()
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(StringContainsNullReferenceException)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => $it.ProductName.Contains(\"Abc\")");
            Assert.Throws<NullReferenceException>(() => RunFilter(newFilter, new Product { }));

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new ContainsOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters("Abc")
                    )
                );
        }

        [Theory]
        [InlineData("Abcd", true)]
        [InlineData("Abd", false)]
        public async Task StringStartsWith(string productName, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { ProductName = productName };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(StringStartsWith)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => $it.ProductName.StartsWith(\"Abc\")");
            Assert.Equal(expected, result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new StartsWithOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters("Abc")
                    )
                );
        }

        [Fact]
        public async Task StringStartsWithNullReferenceException()
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(StringStartsWithNullReferenceException)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => $it.ProductName.StartsWith(\"Abc\")");
            Assert.Throws<NullReferenceException>(() => RunFilter(newFilter, new Product { }));

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new StartsWithOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters("Abc")
                    )
                );
        }

        [Theory]
        [InlineData("AAbc", true)]
        [InlineData("Abcd", false)]
        public async Task StringEndsWith(string productName, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { ProductName = productName };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(StringEndsWith)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => $it.ProductName.EndsWith(\"Abc\")");
            Assert.Equal(expected, result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EndsWithOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters("Abc")
                    )
                );
        }

        [Fact]
        public async Task StringEndsWithNullReferenceException()
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(StringEndsWithNullReferenceException)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => $it.ProductName.EndsWith(\"Abc\")");
            Assert.Throws<NullReferenceException>(() => RunFilter(newFilter, entity));

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EndsWithOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters("Abc")
                    )
                );
        }

        [Theory]
        [InlineData("AAbc", true)]
        [InlineData("", false)]
        public async Task StringLength(string productName, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { ProductName = productName };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(StringLength)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.ProductName.Length > 0)");
            Assert.Equal(expected, result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new GreaterThanBinaryOperatorParameters
                    (
                        new LengthOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(0)
                    )
                );
        }

        [Fact]
        public async Task StringLengthNullReferenceException()
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(StringLengthNullReferenceException)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.ProductName.Length > 0)");
            Assert.Throws<NullReferenceException>(() => RunFilter(newFilter, new Product { }));

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new GreaterThanBinaryOperatorParameters
                    (
                        new LengthOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(0)
                    )
                );
        }

        [Theory]
        [InlineData("12345Abc", true)]
        [InlineData("1234Abc", false)]
        public async Task StringIndexOf(string productName, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { ProductName = productName };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(StringIndexOf)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.ProductName.IndexOf(\"Abc\") == 5)");
            Assert.Equal(expected, result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new IndexOfOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters("Abc")
                        ),
                        new ConstantOperatorParameters(5)
                    )
                );
        }

        [Fact]
        public async Task StringIndexOfNullReferenceException()
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(StringIndexOfNullReferenceException)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.ProductName.IndexOf(\"Abc\") == 5)");
            Assert.Throws<NullReferenceException>(() => RunFilter(newFilter, entity));

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new IndexOfOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters("Abc")
                        ),
                        new ConstantOperatorParameters(5)
                    )
                );
        }

        [Theory]
        [InlineData("123uctName", true)]
        [InlineData("1234Abc", false)]
        public async Task StringSubstring(string productName, bool expected)
        {
            //act
            var filter1 = CreateFilter1<Product>();
            var filter2 = CreateFilter2<Product>();
            var entity = new Product { ProductName = productName };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(StringSubstring)}";
            Expression<Func<Product, bool>> newFilter1 = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter1, ruleName, typeof(Product), entity);
            Expression<Func<Product, bool>> newFilter2 = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter2, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter1, new Product { ProductName = productName });

            //assert
            AssertFilterStringIsCorrect(newFilter1, "$it => ($it.ProductName.Substring(3) == \"uctName\")");
            AssertFilterStringIsCorrect(newFilter2, "$it => ($it.ProductName.Substring(3, 4) == \"uctN\")");
            Assert.Equal(expected, result);

            Expression<Func<T, bool>> CreateFilter1<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new SubstringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters(3)
                        ),
                        new ConstantOperatorParameters("uctName")
                    )
                );

            Expression<Func<T, bool>> CreateFilter2<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new SubstringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters(3),
                            new ConstantOperatorParameters(4)
                        ),
                        new ConstantOperatorParameters("uctN")
                    )
                );
        }

        [Fact]
        public async Task StringSubstringNullReferenceException()
        {
            //act
            var filter1 = CreateFilter1<Product>();
            var filter2 = CreateFilter2<Product>();
            var entity = new Product { };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(StringSubstringNullReferenceException)}";
            Expression<Func<Product, bool>> newFilter1 = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter1, ruleName, typeof(Product), entity);
            Expression<Func<Product, bool>> newFilter2 = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter2, ruleName, typeof(Product), entity);

            //assert
            AssertFilterStringIsCorrect(newFilter1, "$it => ($it.ProductName.Substring(3) == \"uctName\")");
            AssertFilterStringIsCorrect(newFilter2, "$it => ($it.ProductName.Substring(3, 4) == \"uctN\")");
            Assert.Throws<NullReferenceException>(() => RunFilter(newFilter1, new Product { }));

            Expression<Func<T, bool>> CreateFilter1<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new SubstringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters(3)
                        ),
                        new ConstantOperatorParameters("uctName")
                    )
                );

            Expression<Func<T, bool>> CreateFilter2<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new SubstringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters(3),
                            new ConstantOperatorParameters(4)
                        ),
                        new ConstantOperatorParameters("uctN")
                    )
                );
        }

        [Theory]
        [InlineData("Tasty Treats", true)]
        [InlineData("Tasty Treatss", false)]
        public async Task StringToLower(string productName, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { ProductName = productName };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(StringToLower)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.ProductName.ToLower() == \"tasty treats\")");
            Assert.Equal(expected, result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ToLowerOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("tasty treats")
                    )
                );
        }

        [Fact]
        public async Task StringToLowerNullReferenceException()
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(StringToLowerNullReferenceException)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.ProductName.ToLower() == \"tasty treats\")");
            Assert.Throws<NullReferenceException>(() => RunFilter(newFilter, entity));

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ToLowerOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("tasty treats")
                    )
                );
        }

        [Theory]
        [InlineData("Tasty Treats", true)]
        [InlineData("Tasty Treatss", false)]
        public async Task StringToUpper(string productName, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { ProductName = productName };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(StringToUpper)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.ProductName.ToUpper() == \"TASTY TREATS\")");
            Assert.Equal(expected, result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ToUpperOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("TASTY TREATS")
                    )
                );
        }

        [Fact]
        public async Task StringToUpperNullReferenceException()
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(StringToUpperNullReferenceException)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.ProductName.ToUpper() == \"TASTY TREATS\")");
            Assert.Throws<NullReferenceException>(() => RunFilter(newFilter, entity));

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ToUpperOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("TASTY TREATS")
                    )
                );
        }

        [Theory]
        [InlineData(" Tasty Treats  ", true)]
        [InlineData(" Tasty Treatss  ", false)]
        public async Task StringTrim(string productName, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { ProductName = productName };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(StringTrim)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.ProductName.Trim() == \"Tasty Treats\")");
            Assert.Equal(expected, result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new TrimOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("Tasty Treats")
                    )
                );
        }

        [Fact]
        public async Task StringTrimNullReferenceException()
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(StringTrimNullReferenceException)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.ProductName.Trim() == \"Tasty Treats\")");
            Assert.Throws<NullReferenceException>(() => RunFilter(newFilter, entity));

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new TrimOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("Tasty Treats")
                    )
                );
        }

        [Fact]
        public async Task StringConcat()
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(StringConcat)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => (\"Food\".Concat(\"Bar\") == \"FoodBar\")");
            Assert.True(result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConcatOperatorParameters
                        (
                            new ConstantOperatorParameters("Food"),
                            new ConstantOperatorParameters("Bar")
                        ),
                        new ConstantOperatorParameters("FoodBar")
                    )
                );
        }
        #endregion String Functions

        #region Date Functions
        [Fact]
        public async Task DateDay()
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { DiscontinuedDate = new DateTime(2000, 10, 8, 0, 0, 0, DateTimeKind.Unspecified) };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(DateDay)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.DiscontinuedDate.Value.Day == 8)");
            Assert.Throws<InvalidOperationException>(() => RunFilter(newFilter, new Product { }));
            Assert.True(result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new DayOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(8)
                    )
                );
        }

        [Fact]
        public async Task DateDayNonNullable()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(DateDayNonNullable)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.NonNullableDiscontinuedDate.Day == 8)");

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new DayOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NonNullableDiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(8)
                    )
                );
        }

        [Fact]
        public async Task DateMonth()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(DateMonth)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.DiscontinuedDate.Value.Month == 8)");

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MonthOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(8)
                    )
                );
        }

        [Fact]
        public async Task DateYear()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(DateYear)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.DiscontinuedDate.Value.Year == 1974)");

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new YearOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(1974)
                    )
                );
        }

        [Fact]
        public async Task DateHour()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(DateHour)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.DiscontinuedDate.Value.Hour == 8)");

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new HourOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(8)
                    )
                );
        }

        [Fact]
        public async Task DateMinute()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(DateMinute)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.DiscontinuedDate.Value.Minute == 12)");

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MinuteOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(12)
                    )
                );
        }

        [Fact]
        public async Task DateSecond()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(DateSecond)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.DiscontinuedDate.Value.Second == 33)");

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new SecondOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(33)
                    )
                );
        }

        public class DateTimeOffsetFunctionsTheoryData(IExpressionParameter filterBody, string expectedExpression)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
        }

        public static TheoryData<DateTimeOffsetFunctionsTheoryData> DateTimeOffsetFunctions_Data
            =>
            [
                new DateTimeOffsetFunctionsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new YearOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DiscontinuedOffset", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(100)
                    ),
                    "$it => ($it.DiscontinuedOffset.Year == 100)"
                ),
                new DateTimeOffsetFunctionsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MonthOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DiscontinuedOffset", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(100)
                    ),
                    "$it => ($it.DiscontinuedOffset.Month == 100)"
                ),
                new DateTimeOffsetFunctionsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new DayOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DiscontinuedOffset", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(100)
                    ),
                    "$it => ($it.DiscontinuedOffset.Day == 100)"
                ),
                new DateTimeOffsetFunctionsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new HourOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DiscontinuedOffset", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(100)
                    ),
                    "$it => ($it.DiscontinuedOffset.Hour == 100)"
                ),
                new DateTimeOffsetFunctionsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MinuteOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DiscontinuedOffset", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(100)
                    ),
                    "$it => ($it.DiscontinuedOffset.Minute == 100)"
                ),
                new DateTimeOffsetFunctionsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new SecondOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DiscontinuedOffset", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(100)
                    ),
                    "$it => ($it.DiscontinuedOffset.Second == 100)"
                ),
                new DateTimeOffsetFunctionsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new NowDateTimeOperatorParameters(),
                        new ConstantOperatorParameters(new DateTimeOffset(new DateTime(2016, 11, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0)))
                    ),
                    "$it => (DateTimeOffset.UtcNow == 11/08/2016 00:00:00 +00:00)"
                ),
            ];

        [Theory]
        [MemberData(nameof(DateTimeOffsetFunctions_Data), MemberType = typeof(FilterParameterTests))]
        public async Task DateTimeOffsetFunctions(DateTimeOffsetFunctionsTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(DateTimeOffsetFunctions)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, theoryData.ExpectedExpression);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class DateTimeFunctionsTheoryData(IExpressionParameter filterBody, string expectedExpression)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
        }

        public static TheoryData<DateTimeFunctionsTheoryData> DateTimeFunctions_Data
            =>
            [
                new DateTimeFunctionsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new YearOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("Birthday", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(100)
                    ),
                    "$it => ({0}.Year == 100)"
                ),
                new DateTimeFunctionsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MonthOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("Birthday", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(100)
                    ),
                    "$it => ({0}.Month == 100)"
                ),
                new DateTimeFunctionsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new DayOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("Birthday", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(100)
                    ),
                    "$it => ({0}.Day == 100)"
                ),
                new DateTimeFunctionsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new HourOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("Birthday", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(100)
                    ),
                    "$it => ({0}.Hour == 100)"
                ),
                new DateTimeFunctionsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MinuteOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("Birthday", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(100)
                    ),
                    "$it => ({0}.Minute == 100)"
                ),
                new DateTimeFunctionsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new SecondOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("Birthday", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(100)
                    ),
                    "$it => ({0}.Second == 100)"
                ),
            ];

        [Theory]
        [MemberData(nameof(DateTimeFunctions_Data), MemberType = typeof(FilterParameterTests))]
        public async Task DateTimeFunctions(DateTimeFunctionsTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(DateTimeFunctions)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, String.Format(theoryData.ExpectedExpression, "$it.Birthday"));

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class DateFunctions_NullableTheoryData(IExpressionParameter filterBody, string expectedExpression)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
        }

        public static TheoryData<DateFunctions_NullableTheoryData> DateFunctions_Nullable_Data
            =>
            [
                new DateFunctions_NullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new YearOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableDateProperty", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(2015)
                    ),
                    "$it => ($it.NullableDateProperty.Value.Year == 2015)"
                ),
                new DateFunctions_NullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MonthOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableDateProperty", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(12)
                    ),
                    "$it => ($it.NullableDateProperty.Value.Month == 12)"
                ),
                new DateFunctions_NullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new DayOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableDateProperty", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(23)
                    ),
                    "$it => ($it.NullableDateProperty.Value.Day == 23)"
                ),
            ];

        [Theory]
        [MemberData(nameof(DateFunctions_Nullable_Data), MemberType = typeof(FilterParameterTests))]
        public async Task DateFunctions_Nullable(DateFunctions_NullableTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(DateFunctions_Nullable)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, theoryData.ExpectedExpression);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class DateOnlyFunctions_NullableTheoryData(IExpressionParameter filterBody, string expectedExpression)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
        }

        public static TheoryData<DateOnlyFunctions_NullableTheoryData> DateOnlyFunctions_Nullable_Data
            =>
            [
                new DateOnlyFunctions_NullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new YearOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableDateOnlyProperty", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(2015)
                    ),
                    "$it => ($it.NullableDateOnlyProperty.Value.Year == 2015)"
                ),
                new DateOnlyFunctions_NullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MonthOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableDateOnlyProperty", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(12)
                    ),
                    "$it => ($it.NullableDateOnlyProperty.Value.Month == 12)"
                ),
                new DateOnlyFunctions_NullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new DayOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableDateOnlyProperty", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(23)
                    ),
                    "$it => ($it.NullableDateOnlyProperty.Value.Day == 23)"
                ),
            ];

        [Theory]
        [MemberData(nameof(DateOnlyFunctions_Nullable_Data), MemberType = typeof(FilterParameterTests))]
        public async Task DateOnlyFunctions_Nullable(DateOnlyFunctions_NullableTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(DateOnlyFunctions_Nullable)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, theoryData.ExpectedExpression);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class DateFunctions_NonNullableTheoryData(IExpressionParameter filterBody, string expectedExpression)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
        }

        public static TheoryData<DateFunctions_NonNullableTheoryData> DateFunctions_NonNullable_Data
            =>
            [
                new DateFunctions_NonNullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new YearOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DateProperty", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(2015)
                    ),
                    "$it => ($it.DateProperty.Year == 2015)"
                ),
                new DateFunctions_NonNullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MonthOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DateProperty", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(12)
                    ),
                    "$it => ($it.DateProperty.Month == 12)"
                ),
                new DateFunctions_NonNullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new DayOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DateProperty", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(23)
                    ),
                    "$it => ($it.DateProperty.Day == 23)"
                ),
            ];

        [Theory]
        [MemberData(nameof(DateFunctions_NonNullable_Data), MemberType = typeof(FilterParameterTests))]
        public async Task DateFunctions_NonNullable(DateFunctions_NonNullableTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(DateFunctions_NonNullable)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, theoryData.ExpectedExpression);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class DateOnlyFunctions_NonNullableTheoryData(IExpressionParameter filterBody, string expectedExpression)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
        }

        public static TheoryData<DateOnlyFunctions_NonNullableTheoryData> DateOnlyFunctions_NonNullable_Data
            =>
            [
                new DateOnlyFunctions_NonNullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new YearOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DateOnlyProperty", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(2015)
                    ),
                    "$it => ($it.DateOnlyProperty.Year == 2015)"
                ),
                new DateOnlyFunctions_NonNullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MonthOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DateOnlyProperty", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(12)
                    ),
                    "$it => ($it.DateOnlyProperty.Month == 12)"
                ),
                new DateOnlyFunctions_NonNullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new DayOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DateOnlyProperty", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(23)
                    ),
                    "$it => ($it.DateOnlyProperty.Day == 23)"
                ),
            ];

        [Theory]
        [MemberData(nameof(DateOnlyFunctions_NonNullable_Data), MemberType = typeof(FilterParameterTests))]
        public async Task DateOnlyFunctions_NonNullable(DateOnlyFunctions_NonNullableTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(DateOnlyFunctions_NonNullable)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, theoryData.ExpectedExpression);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class TimeOfDayFunctions_NullableTheoryData(IExpressionParameter filterBody, string expectedExpression)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
        }

        public static TheoryData<TimeOfDayFunctions_NullableTheoryData> TimeOfDayFunctions_Nullable_Data
            =>
            [
                new TimeOfDayFunctions_NullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new HourOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableTimeOfDayProperty", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(10)
                    ),
                    "$it => ($it.NullableTimeOfDayProperty.Value.Hours == 10)"
                ),
                new TimeOfDayFunctions_NullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MinuteOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableTimeOfDayProperty", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(20)
                    ),
                    "$it => ($it.NullableTimeOfDayProperty.Value.Minutes == 20)"
                ),
                new TimeOfDayFunctions_NullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new SecondOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableTimeOfDayProperty", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(30)
                    ),
                    "$it => ($it.NullableTimeOfDayProperty.Value.Seconds == 30)"
                ),
            ];

        [Theory]
        [MemberData(nameof(TimeOfDayFunctions_Nullable_Data), MemberType = typeof(FilterParameterTests))]
        public async Task TimeOfDayFunctions_Nullable(TimeOfDayFunctions_NullableTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(TimeOfDayFunctions_Nullable)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, theoryData.ExpectedExpression);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class TimeOnlyFunctions_NullableTheoryData(IExpressionParameter filterBody, string expectedExpression)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
        }

        public static TheoryData<TimeOnlyFunctions_NullableTheoryData> TimeOnlyFunctions_Nullable_Data
            =>
            [
                new TimeOnlyFunctions_NullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new HourOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableTimeOnlyProperty", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(10)
                    ),
                    "$it => ($it.NullableTimeOnlyProperty.Value.Hour == 10)"
                ),
                new TimeOnlyFunctions_NullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MinuteOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableTimeOnlyProperty", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(20)
                    ),
                    "$it => ($it.NullableTimeOnlyProperty.Value.Minute == 20)"
                ),
                new TimeOnlyFunctions_NullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new SecondOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableTimeOnlyProperty", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(30)
                    ),
                    "$it => ($it.NullableTimeOnlyProperty.Value.Second == 30)"
                ),
            ];

        [Theory]
        [MemberData(nameof(TimeOnlyFunctions_Nullable_Data), MemberType = typeof(FilterParameterTests))]
        public async Task TimeOnlyFunctions_Nullable(TimeOnlyFunctions_NullableTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(TimeOnlyFunctions_Nullable)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, theoryData.ExpectedExpression);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class TimeOfDayFunctions_NonNullableTheoryData(IExpressionParameter filterBody, string expectedExpression)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
        }

        public static TheoryData<TimeOfDayFunctions_NonNullableTheoryData> TimeOfDayFunctions_NonNullable_Data
            =>
            [
                new TimeOfDayFunctions_NonNullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new HourOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("TimeOfDayProperty", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(10)
                    ),
                    "$it => ($it.TimeOfDayProperty.Hours == 10)"
                ),
                new TimeOfDayFunctions_NonNullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MinuteOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("TimeOfDayProperty", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(20)
                    ),
                    "$it => ($it.TimeOfDayProperty.Minutes == 20)"
                ),
                new TimeOfDayFunctions_NonNullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new SecondOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("TimeOfDayProperty", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(30)
                    ),
                    "$it => ($it.TimeOfDayProperty.Seconds == 30)"
                ),
            ];

        [Theory]
        [MemberData(nameof(TimeOfDayFunctions_NonNullable_Data), MemberType = typeof(FilterParameterTests))]
        public async Task TimeOfDayFunctions_NonNullable(TimeOfDayFunctions_NonNullableTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(TimeOfDayFunctions_NonNullable)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, theoryData.ExpectedExpression);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class TimeOnlyFunctions_NonNullableTheoryData(IExpressionParameter filterBody, string expectedExpression)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
        }

        public static TheoryData<TimeOnlyFunctions_NonNullableTheoryData> TimeOnlyFunctions_NonNullable_Data
            =>
            [
                new TimeOnlyFunctions_NonNullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new HourOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("TimeOnlyProperty", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(10)
                    ),
                    "$it => ($it.TimeOnlyProperty.Hour == 10)"
                ),
                new TimeOnlyFunctions_NonNullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MinuteOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("TimeOnlyProperty", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(20)
                    ),
                    "$it => ($it.TimeOnlyProperty.Minute == 20)"
                ),
                new TimeOnlyFunctions_NonNullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new SecondOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("TimeOnlyProperty", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(30)
                    ),
                    "$it => ($it.TimeOnlyProperty.Second == 30)"
                ),
            ];

        [Theory]
        [MemberData(nameof(TimeOnlyFunctions_NonNullable_Data), MemberType = typeof(FilterParameterTests))]
        public async Task TimeOnlyFunctions_NonNullable(TimeOnlyFunctions_NonNullableTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(TimeOnlyFunctions_NonNullable)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, theoryData.ExpectedExpression);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class FractionalsecondsFunction_NullableTheoryData(IExpressionParameter filterBody, string expectedExpression)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
        }

        public static TheoryData<FractionalsecondsFunction_NullableTheoryData> FractionalsecondsFunction_Nullable_Data
            =>
            [
                new FractionalsecondsFunction_NullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new FractionalSecondsOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(0.2m)
                    ),
                    "$it => ((Convert($it.DiscontinuedDate.Value.Millisecond) / 1000) == 0.2)"
                ),
                new FractionalsecondsFunction_NullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new FractionalSecondsOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableTimeOfDayProperty", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(0.2m)
                    ),
                    "$it => ((Convert($it.NullableTimeOfDayProperty.Value.Milliseconds) / 1000) == 0.2)"
                ),
                new FractionalsecondsFunction_NullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new FractionalSecondsOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableTimeOnlyProperty", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(0.2m)
                    ),
                    "$it => ((Convert($it.NullableTimeOnlyProperty.Value.Millisecond) / 1000) == 0.2)"
                ),
            ];

        [Theory]
        [MemberData(nameof(FractionalsecondsFunction_Nullable_Data), MemberType = typeof(FilterParameterTests))]
        public async Task FractionalsecondsFunction_Nullable(FractionalsecondsFunction_NullableTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(FractionalsecondsFunction_Nullable)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, theoryData.ExpectedExpression);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class FractionalsecondsFunction_NonNullableTheoryData(IExpressionParameter filterBody, string expectedExpression)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
        }

        public static TheoryData<FractionalsecondsFunction_NonNullableTheoryData> FractionalsecondsFunction_NonNullable_Data
            =>
            [
                new FractionalsecondsFunction_NonNullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new FractionalSecondsOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NonNullableDiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(0.2m)
                    ),
                    "$it => ((Convert($it.NonNullableDiscontinuedDate.Millisecond) / 1000) == 0.2)"
                ),
                new FractionalsecondsFunction_NonNullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new FractionalSecondsOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("TimeOfDayProperty", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(0.2m)
                    ),
                    "$it => ((Convert($it.TimeOfDayProperty.Milliseconds) / 1000) == 0.2)"
                ),
                new FractionalsecondsFunction_NonNullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new FractionalSecondsOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("TimeOnlyProperty", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(0.2m)
                    ),
                    "$it => ((Convert($it.TimeOnlyProperty.Millisecond) / 1000) == 0.2)"
                ),
            ];

        [Theory]
        [MemberData(nameof(FractionalsecondsFunction_NonNullable_Data), MemberType = typeof(FilterParameterTests))]
        public async Task FractionalsecondsFunction_NonNullable(FractionalsecondsFunction_NonNullableTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(FractionalsecondsFunction_NonNullable)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, theoryData.ExpectedExpression);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class DateFunction_NullableTheoryData(IExpressionParameter filterBody, string expectedExpression)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
        }

        public static TheoryData<DateFunction_NullableTheoryData> DateFunction_Nullable_Data
            =>
            [
                new DateFunction_NullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToNumericDateOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConvertToNumericDateOperatorParameters
                        (
                            new ConstantOperatorParameters(new Date(2015, 2, 26))
                        )
                    ),
                    "$it => (((($it.DiscontinuedDate.Value.Year * 10000) + ($it.DiscontinuedDate.Value.Month * 100)) + $it.DiscontinuedDate.Value.Day) == (((2015-02-26.Year * 10000) + (2015-02-26.Month * 100)) + 2015-02-26.Day))"
                ),
                new DateFunction_NullableTheoryData
                (
                    new LessThanBinaryOperatorParameters
                    (
                        new ConvertToNumericDateOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConvertToNumericDateOperatorParameters
                        (
                            new ConstantOperatorParameters(new Date(2016, 2, 26))
                        )
                    ),
                    "$it => (((($it.DiscontinuedDate.Value.Year * 10000) + ($it.DiscontinuedDate.Value.Month * 100)) + $it.DiscontinuedDate.Value.Day) < (((2016-02-26.Year * 10000) + (2016-02-26.Month * 100)) + 2016-02-26.Day))"
                ),
                new DateFunction_NullableTheoryData
                (
                    new GreaterThanOrEqualsBinaryOperatorParameters
                    (
                        new ConvertToNumericDateOperatorParameters
                        (
                            new ConstantOperatorParameters(new Date(2015, 2, 26))
                        ),
                        new ConvertToNumericDateOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        )
                    ),
                    "$it => ((((2015-02-26.Year * 10000) + (2015-02-26.Month * 100)) + 2015-02-26.Day) >= ((($it.DiscontinuedDate.Value.Year * 10000) + ($it.DiscontinuedDate.Value.Month * 100)) + $it.DiscontinuedDate.Value.Day))"
                ),
                new DateFunction_NullableTheoryData
                (
                    new NotEqualsBinaryOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        new MemberSelectorOperatorParameters("DiscontinuedDate", new ParameterOperatorParameters(parameterName))
                    ),
                    "$it => (null != $it.DiscontinuedDate)"
                ),
                new DateFunction_NullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("DiscontinuedDate", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters(null!)
                    ),
                    "$it => ($it.DiscontinuedDate == null)"
                ),
            ];

        [Theory]
        [MemberData(nameof(DateFunction_Nullable_Data), MemberType = typeof(FilterParameterTests))]
        public async Task DateFunction_Nullable(DateFunction_NullableTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(DateFunction_Nullable)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, theoryData.ExpectedExpression);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class DateOnlyFunction_NullableTheoryData(IExpressionParameter filterBody, string expectedExpression)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
        }

        public static TheoryData<DateOnlyFunction_NullableTheoryData> DateOnlyFunction_Nullable_Data
            =>
            [
                new DateOnlyFunction_NullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToNumericDateOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConvertToNumericDateOperatorParameters
                        (
                            new ConstantOperatorParameters(new DateOnly(2015, 2, 26))
                        )
                    ),
                    "$it => (((($it.DiscontinuedDate.Value.Year * 10000) + ($it.DiscontinuedDate.Value.Month * 100)) + $it.DiscontinuedDate.Value.Day) == (((2015-02-26.Year * 10000) + (2015-02-26.Month * 100)) + 2015-02-26.Day))"
                ),
                new DateOnlyFunction_NullableTheoryData
                (
                    new LessThanBinaryOperatorParameters
                    (
                        new ConvertToNumericDateOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConvertToNumericDateOperatorParameters
                        (
                            new ConstantOperatorParameters(new DateOnly(2016, 2, 26))
                        )
                    ),
                    "$it => (((($it.DiscontinuedDate.Value.Year * 10000) + ($it.DiscontinuedDate.Value.Month * 100)) + $it.DiscontinuedDate.Value.Day) < (((2016-02-26.Year * 10000) + (2016-02-26.Month * 100)) + 2016-02-26.Day))"
                ),
                new DateOnlyFunction_NullableTheoryData
                (
                    new GreaterThanOrEqualsBinaryOperatorParameters
                    (
                        new ConvertToNumericDateOperatorParameters
                        (
                            new ConstantOperatorParameters(new DateOnly(2015, 2, 26))
                        ),
                        new ConvertToNumericDateOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        )
                    ),
                    "$it => ((((2015-02-26.Year * 10000) + (2015-02-26.Month * 100)) + 2015-02-26.Day) >= ((($it.DiscontinuedDate.Value.Year * 10000) + ($it.DiscontinuedDate.Value.Month * 100)) + $it.DiscontinuedDate.Value.Day))"
                ),
                new DateOnlyFunction_NullableTheoryData
                (
                    new NotEqualsBinaryOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        new MemberSelectorOperatorParameters("DiscontinuedDate", new ParameterOperatorParameters(parameterName))
                    ),
                    "$it => (null != $it.DiscontinuedDate)"
                ),
                new DateOnlyFunction_NullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("DiscontinuedDate", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters(null!)
                    ),
                    "$it => ($it.DiscontinuedDate == null)"
                ),
            ];

        [Theory]
        [MemberData(nameof(DateOnlyFunction_Nullable_Data), MemberType = typeof(FilterParameterTests))]
        public async Task DateOnlyFunction_Nullable(DateOnlyFunction_NullableTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(DateOnlyFunction_Nullable)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, theoryData.ExpectedExpression);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class DateFunction_NonNullableTheoryData(IExpressionParameter filterBody, string expectedExpression)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
        }

        public static TheoryData<DateFunction_NonNullableTheoryData> DateFunction_NonNullable_Data
            =>
            [
                new DateFunction_NonNullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToNumericDateOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NonNullableDiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConvertToNumericDateOperatorParameters
                        (
                            new ConstantOperatorParameters(new Date(2015, 2, 26))
                        )
                    ),
                    "$it => (((($it.NonNullableDiscontinuedDate.Year * 10000) + ($it.NonNullableDiscontinuedDate.Month * 100)) + $it.NonNullableDiscontinuedDate.Day) == (((2015-02-26.Year * 10000) + (2015-02-26.Month * 100)) + 2015-02-26.Day))"
                ),
                new DateFunction_NonNullableTheoryData
                (
                    new LessThanBinaryOperatorParameters
                    (
                        new ConvertToNumericDateOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NonNullableDiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConvertToNumericDateOperatorParameters
                        (
                            new ConstantOperatorParameters(new Date(2016, 2, 26))
                        )
                    ),
                    "$it => (((($it.NonNullableDiscontinuedDate.Year * 10000) + ($it.NonNullableDiscontinuedDate.Month * 100)) + $it.NonNullableDiscontinuedDate.Day) < (((2016-02-26.Year * 10000) + (2016-02-26.Month * 100)) + 2016-02-26.Day))"
                ),
                new DateFunction_NonNullableTheoryData
                (
                    new GreaterThanOrEqualsBinaryOperatorParameters
                    (
                        new ConvertToNumericDateOperatorParameters
                        (
                            new ConstantOperatorParameters(new Date(2015, 2, 26))
                        ),
                        new ConvertToNumericDateOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NonNullableDiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        )
                    ),
                    "$it => ((((2015-02-26.Year * 10000) + (2015-02-26.Month * 100)) + 2015-02-26.Day) >= ((($it.NonNullableDiscontinuedDate.Year * 10000) + ($it.NonNullableDiscontinuedDate.Month * 100)) + $it.NonNullableDiscontinuedDate.Day))"
                )
            ];

        [Theory]
        [MemberData(nameof(DateFunction_NonNullable_Data), MemberType = typeof(FilterParameterTests))]
        public async Task DateFunction_NonNullable(DateFunction_NonNullableTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(DateFunction_NonNullable)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, theoryData.ExpectedExpression);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class DateOnlyFunction_NonNullableTheoryData(IExpressionParameter filterBody, string expectedExpression)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
        }

        public static TheoryData<DateOnlyFunction_NonNullableTheoryData> DateOnlyFunction_NonNullable_Data
            =>
            [
                new DateOnlyFunction_NonNullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToNumericDateOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NonNullableDiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConvertToNumericDateOperatorParameters
                        (
                            new ConstantOperatorParameters(new DateOnly(2015, 2, 26))
                        )
                    ),
                    "$it => (((($it.NonNullableDiscontinuedDate.Year * 10000) + ($it.NonNullableDiscontinuedDate.Month * 100)) + $it.NonNullableDiscontinuedDate.Day) == (((2015-02-26.Year * 10000) + (2015-02-26.Month * 100)) + 2015-02-26.Day))"
                ),
                new DateOnlyFunction_NonNullableTheoryData
                (
                    new LessThanBinaryOperatorParameters
                    (
                        new ConvertToNumericDateOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NonNullableDiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConvertToNumericDateOperatorParameters
                        (
                            new ConstantOperatorParameters(new DateOnly(2016, 2, 26))
                        )
                    ),
                    "$it => (((($it.NonNullableDiscontinuedDate.Year * 10000) + ($it.NonNullableDiscontinuedDate.Month * 100)) + $it.NonNullableDiscontinuedDate.Day) < (((2016-02-26.Year * 10000) + (2016-02-26.Month * 100)) + 2016-02-26.Day))"
                ),
                new DateOnlyFunction_NonNullableTheoryData
                (
                    new GreaterThanOrEqualsBinaryOperatorParameters
                    (
                        new ConvertToNumericDateOperatorParameters
                        (
                            new ConstantOperatorParameters(new DateOnly(2015, 2, 26))
                        ),
                        new ConvertToNumericDateOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NonNullableDiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        )
                    ),
                    "$it => ((((2015-02-26.Year * 10000) + (2015-02-26.Month * 100)) + 2015-02-26.Day) >= ((($it.NonNullableDiscontinuedDate.Year * 10000) + ($it.NonNullableDiscontinuedDate.Month * 100)) + $it.NonNullableDiscontinuedDate.Day))"
                )
            ];

        [Theory]
        [MemberData(nameof(DateOnlyFunction_NonNullable_Data), MemberType = typeof(FilterParameterTests))]
        public async Task DateOnlyFunction_NonNullable(DateOnlyFunction_NonNullableTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(DateOnlyFunction_NonNullable)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, theoryData.ExpectedExpression);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class TimeFunction_NullableTheoryData(IExpressionParameter filterBody, string expectedExpression)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
        }

        public static TheoryData<TimeFunction_NullableTheoryData> TimeFunction_Nullable_Data
            =>
            [
                new TimeFunction_NullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToNumericTimeOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConvertToNumericTimeOperatorParameters
                        (
                            new ConstantOperatorParameters(new TimeOfDay(1, 2, 3, 4))
                        )
                    ),
                    "$it => (((Convert($it.DiscontinuedDate.Value.Hour) * 36000000000) + ((Convert($it.DiscontinuedDate.Value.Minute) * 600000000) + ((Convert($it.DiscontinuedDate.Value.Second) * 10000000) + Convert($it.DiscontinuedDate.Value.Millisecond)))) == ((Convert(01:02:03.0040000.Hours) * 36000000000) + ((Convert(01:02:03.0040000.Minutes) * 600000000) + ((Convert(01:02:03.0040000.Seconds) * 10000000) + Convert(01:02:03.0040000.Milliseconds)))))"
                ),
                new TimeFunction_NullableTheoryData
                (
                    new GreaterThanOrEqualsBinaryOperatorParameters
                    (
                        new ConvertToNumericTimeOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConvertToNumericTimeOperatorParameters
                        (
                            new ConstantOperatorParameters(new TimeOfDay(1, 2, 3, 4))
                        )
                    ),
                    "$it => (((Convert($it.DiscontinuedDate.Value.Hour) * 36000000000) + ((Convert($it.DiscontinuedDate.Value.Minute) * 600000000) + ((Convert($it.DiscontinuedDate.Value.Second) * 10000000) + Convert($it.DiscontinuedDate.Value.Millisecond)))) >= ((Convert(01:02:03.0040000.Hours) * 36000000000) + ((Convert(01:02:03.0040000.Minutes) * 600000000) + ((Convert(01:02:03.0040000.Seconds) * 10000000) + Convert(01:02:03.0040000.Milliseconds)))))"
                ),
                new TimeFunction_NullableTheoryData
                (
                    new LessThanOrEqualsBinaryOperatorParameters
                    (
                        new ConvertToNumericTimeOperatorParameters
                        (
                            new ConstantOperatorParameters(new TimeOfDay(1, 2, 3, 4))
                        ),
                        new ConvertToNumericTimeOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        )
                    ),
                    "$it => (((Convert(01:02:03.0040000.Hours) * 36000000000) + ((Convert(01:02:03.0040000.Minutes) * 600000000) + ((Convert(01:02:03.0040000.Seconds) * 10000000) + Convert(01:02:03.0040000.Milliseconds)))) <= ((Convert($it.DiscontinuedDate.Value.Hour) * 36000000000) + ((Convert($it.DiscontinuedDate.Value.Minute) * 600000000) + ((Convert($it.DiscontinuedDate.Value.Second) * 10000000) + Convert($it.DiscontinuedDate.Value.Millisecond)))))"
                ),
                new TimeFunction_NullableTheoryData
                (
                    new NotEqualsBinaryOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        new MemberSelectorOperatorParameters("DiscontinuedDate", new ParameterOperatorParameters(parameterName))
                    ),
                    "$it => (null != $it.DiscontinuedDate)"
                ),
                new TimeFunction_NullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("DiscontinuedDate", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters(null!)
                    ),
                    "$it => ($it.DiscontinuedDate == null)"
                )
            ];

        [Theory]
        [MemberData(nameof(TimeFunction_Nullable_Data), MemberType = typeof(FilterParameterTests))]
        public async Task TimeFunction_Nullable(TimeFunction_NullableTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(TimeFunction_Nullable)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, theoryData.ExpectedExpression);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class TimeOnlyFunction_NullableTheoryData(IExpressionParameter filterBody, string expectedExpression)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
        }

        public static TheoryData<TimeOnlyFunction_NullableTheoryData> TimeOnlyFunction_Nullable_Data
            =>
            [
                new TimeOnlyFunction_NullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToNumericTimeOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConvertToNumericTimeOperatorParameters
                        (
                            new ConstantOperatorParameters(new TimeOnly(1, 2, 3, 4))
                        )
                    ),
                    "$it => (((Convert($it.DiscontinuedDate.Value.Hour) * 36000000000) + ((Convert($it.DiscontinuedDate.Value.Minute) * 600000000) + ((Convert($it.DiscontinuedDate.Value.Second) * 10000000) + Convert($it.DiscontinuedDate.Value.Millisecond)))) == ((Convert(01:02:03.0040000.Hour) * 36000000000) + ((Convert(01:02:03.0040000.Minute) * 600000000) + ((Convert(01:02:03.0040000.Second) * 10000000) + Convert(01:02:03.0040000.Millisecond)))))"
                ),
                new TimeOnlyFunction_NullableTheoryData
                (
                    new GreaterThanOrEqualsBinaryOperatorParameters
                    (
                        new ConvertToNumericTimeOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConvertToNumericTimeOperatorParameters
                        (
                            new ConstantOperatorParameters(new TimeOnly(1, 2, 3, 4))
                        )
                    ),
                    "$it => (((Convert($it.DiscontinuedDate.Value.Hour) * 36000000000) + ((Convert($it.DiscontinuedDate.Value.Minute) * 600000000) + ((Convert($it.DiscontinuedDate.Value.Second) * 10000000) + Convert($it.DiscontinuedDate.Value.Millisecond)))) >= ((Convert(01:02:03.0040000.Hour) * 36000000000) + ((Convert(01:02:03.0040000.Minute) * 600000000) + ((Convert(01:02:03.0040000.Second) * 10000000) + Convert(01:02:03.0040000.Millisecond)))))"
                ),
                new TimeOnlyFunction_NullableTheoryData
                (
                    new LessThanOrEqualsBinaryOperatorParameters
                    (
                        new ConvertToNumericTimeOperatorParameters
                        (
                            new ConstantOperatorParameters(new TimeOnly(1, 2, 3, 4))
                        ),
                        new ConvertToNumericTimeOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        )
                    ),
                    "$it => (((Convert(01:02:03.0040000.Hour) * 36000000000) + ((Convert(01:02:03.0040000.Minute) * 600000000) + ((Convert(01:02:03.0040000.Second) * 10000000) + Convert(01:02:03.0040000.Millisecond)))) <= ((Convert($it.DiscontinuedDate.Value.Hour) * 36000000000) + ((Convert($it.DiscontinuedDate.Value.Minute) * 600000000) + ((Convert($it.DiscontinuedDate.Value.Second) * 10000000) + Convert($it.DiscontinuedDate.Value.Millisecond)))))"
                ),
                new TimeOnlyFunction_NullableTheoryData
                (
                    new NotEqualsBinaryOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        new MemberSelectorOperatorParameters("DiscontinuedDate", new ParameterOperatorParameters(parameterName))
                    ),
                    "$it => (null != $it.DiscontinuedDate)"
                ),
                new TimeOnlyFunction_NullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("DiscontinuedDate", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters(null!)
                    ),
                    "$it => ($it.DiscontinuedDate == null)"
                )
            ];

        [Theory]
        [MemberData(nameof(TimeOnlyFunction_Nullable_Data), MemberType = typeof(FilterParameterTests))]
        public async Task TimeOnlyFunction_Nullable(TimeOnlyFunction_NullableTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(TimeOnlyFunction_Nullable)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, theoryData.ExpectedExpression);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class TimeFunction_NonNullableTheoryData(IExpressionParameter filterBody, string expectedExpression)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
        }

        public static TheoryData<TimeFunction_NonNullableTheoryData> TimeFunction_NonNullable_Data
            =>
            [
                new TimeFunction_NonNullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToNumericTimeOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NonNullableDiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConvertToNumericTimeOperatorParameters
                        (
                            new ConstantOperatorParameters(new TimeOfDay(1, 2, 3, 4))
                        )
                    ),
                    "$it => (((Convert($it.NonNullableDiscontinuedDate.Hour) * 36000000000) + ((Convert($it.NonNullableDiscontinuedDate.Minute) * 600000000) + ((Convert($it.NonNullableDiscontinuedDate.Second) * 10000000) + Convert($it.NonNullableDiscontinuedDate.Millisecond)))) == ((Convert(01:02:03.0040000.Hours) * 36000000000) + ((Convert(01:02:03.0040000.Minutes) * 600000000) + ((Convert(01:02:03.0040000.Seconds) * 10000000) + Convert(01:02:03.0040000.Milliseconds)))))"
                ),
                new TimeFunction_NonNullableTheoryData
                (
                    new GreaterThanOrEqualsBinaryOperatorParameters
                    (
                        new ConvertToNumericTimeOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NonNullableDiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConvertToNumericTimeOperatorParameters
                        (
                            new ConstantOperatorParameters(new TimeOfDay(1, 2, 3, 4))
                        )
                    ),
                    "$it => (((Convert($it.NonNullableDiscontinuedDate.Hour) * 36000000000) + ((Convert($it.NonNullableDiscontinuedDate.Minute) * 600000000) + ((Convert($it.NonNullableDiscontinuedDate.Second) * 10000000) + Convert($it.NonNullableDiscontinuedDate.Millisecond)))) >= ((Convert(01:02:03.0040000.Hours) * 36000000000) + ((Convert(01:02:03.0040000.Minutes) * 600000000) + ((Convert(01:02:03.0040000.Seconds) * 10000000) + Convert(01:02:03.0040000.Milliseconds)))))"
                ),
                new TimeFunction_NonNullableTheoryData
                (
                    new LessThanOrEqualsBinaryOperatorParameters
                    (
                        new ConvertToNumericTimeOperatorParameters
                        (
                            new ConstantOperatorParameters(new TimeOfDay(1, 2, 3, 4))
                        ),
                        new ConvertToNumericTimeOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NonNullableDiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        )
                    ),
                    "$it => (((Convert(01:02:03.0040000.Hours) * 36000000000) + ((Convert(01:02:03.0040000.Minutes) * 600000000) + ((Convert(01:02:03.0040000.Seconds) * 10000000) + Convert(01:02:03.0040000.Milliseconds)))) <= ((Convert($it.NonNullableDiscontinuedDate.Hour) * 36000000000) + ((Convert($it.NonNullableDiscontinuedDate.Minute) * 600000000) + ((Convert($it.NonNullableDiscontinuedDate.Second) * 10000000) + Convert($it.NonNullableDiscontinuedDate.Millisecond)))))"
                )
            ];

        [Theory]
        [MemberData(nameof(TimeFunction_NonNullable_Data), MemberType = typeof(FilterParameterTests))]
        public async Task TimeFunction_NonNullable(TimeFunction_NonNullableTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(TimeFunction_NonNullable)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, theoryData.ExpectedExpression);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class TimeOnlyFunction_NonNullableTheoryData(IExpressionParameter filterBody, string expectedExpression)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
        }

        public static TheoryData<TimeOnlyFunction_NonNullableTheoryData> TimeOnlyFunction_NonNullable_Data
            =>
            [
                new TimeOnlyFunction_NonNullableTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToNumericTimeOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NonNullableDiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConvertToNumericTimeOperatorParameters
                        (
                            new ConstantOperatorParameters(new TimeOnly(1, 2, 3, 4))
                        )
                    ),
                    "$it => (((Convert($it.NonNullableDiscontinuedDate.Hour) * 36000000000) + ((Convert($it.NonNullableDiscontinuedDate.Minute) * 600000000) + ((Convert($it.NonNullableDiscontinuedDate.Second) * 10000000) + Convert($it.NonNullableDiscontinuedDate.Millisecond)))) == ((Convert(01:02:03.0040000.Hour) * 36000000000) + ((Convert(01:02:03.0040000.Minute) * 600000000) + ((Convert(01:02:03.0040000.Second) * 10000000) + Convert(01:02:03.0040000.Millisecond)))))"
                ),
                new TimeOnlyFunction_NonNullableTheoryData
                (
                    new GreaterThanOrEqualsBinaryOperatorParameters
                    (
                        new ConvertToNumericTimeOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NonNullableDiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConvertToNumericTimeOperatorParameters
                        (
                            new ConstantOperatorParameters(new TimeOnly(1, 2, 3, 4))
                        )
                    ),
                    "$it => (((Convert($it.NonNullableDiscontinuedDate.Hour) * 36000000000) + ((Convert($it.NonNullableDiscontinuedDate.Minute) * 600000000) + ((Convert($it.NonNullableDiscontinuedDate.Second) * 10000000) + Convert($it.NonNullableDiscontinuedDate.Millisecond)))) >= ((Convert(01:02:03.0040000.Hour) * 36000000000) + ((Convert(01:02:03.0040000.Minute) * 600000000) + ((Convert(01:02:03.0040000.Second) * 10000000) + Convert(01:02:03.0040000.Millisecond)))))"
                ),
                new TimeOnlyFunction_NonNullableTheoryData
                (
                    new LessThanOrEqualsBinaryOperatorParameters
                    (
                        new ConvertToNumericTimeOperatorParameters
                        (
                            new ConstantOperatorParameters(new TimeOnly(1, 2, 3, 4))
                        ),
                        new ConvertToNumericTimeOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NonNullableDiscontinuedDate", new ParameterOperatorParameters(parameterName))
                        )
                    ),
                    "$it => (((Convert(01:02:03.0040000.Hour) * 36000000000) + ((Convert(01:02:03.0040000.Minute) * 600000000) + ((Convert(01:02:03.0040000.Second) * 10000000) + Convert(01:02:03.0040000.Millisecond)))) <= ((Convert($it.NonNullableDiscontinuedDate.Hour) * 36000000000) + ((Convert($it.NonNullableDiscontinuedDate.Minute) * 600000000) + ((Convert($it.NonNullableDiscontinuedDate.Second) * 10000000) + Convert($it.NonNullableDiscontinuedDate.Millisecond)))))"
                )
            ];

        [Theory]
        [MemberData(nameof(TimeOnlyFunction_NonNullable_Data), MemberType = typeof(FilterParameterTests))]
        public async Task TimeOnlyFunction_NonNullable(TimeOnlyFunction_NonNullableTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(TimeOnlyFunction_NonNullable)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, theoryData.ExpectedExpression);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }
        #endregion Date Functions

        #region Math Functions
        [Fact]
        public async Task RecursiveMethodCall()
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { UnitPrice = 123.3m };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(RecursiveMethodCall)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.UnitPrice.Value.Floor().Floor() == 123)");
            Assert.True(result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new FloorOperatorParameters
                        (
                            new FloorOperatorParameters
                            (
                                new MemberSelectorOperatorParameters("UnitPrice", new ParameterOperatorParameters(parameterName))
                            )
                        ),
                        new ConstantOperatorParameters(123m)
                    )
                );
        }

        [Fact]
        public async Task RecursiveMethodCallInvalidOperationException()
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(RecursiveMethodCallInvalidOperationException)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.UnitPrice.Value.Floor().Floor() == 123)");
            Assert.Throws<InvalidOperationException>(() => RunFilter(newFilter, entity));

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new FloorOperatorParameters
                        (
                            new FloorOperatorParameters
                            (
                                new MemberSelectorOperatorParameters("UnitPrice", new ParameterOperatorParameters(parameterName))
                            )
                        ),
                        new ConstantOperatorParameters(123m)
                    )
                );
        }

        [Fact]
        public async Task MathRoundDecimalInvalidOperationException()
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(MathRoundDecimalInvalidOperationException)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, string.Format(CultureInfo.InvariantCulture, "$it => ($it.UnitPrice.Value.Round() > {0:0.00})", 5.0));
            Assert.Throws<InvalidOperationException>(() => RunFilter(newFilter, new Product { }));

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new GreaterThanBinaryOperatorParameters
                    (
                        new RoundOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("UnitPrice", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(5.00m)
                    )
                );
        }

        public class MathRoundDecimalTheoryData(decimal? unitPrice, bool expected)
        {
            public decimal? UnitPrice { get; } = unitPrice;
            public bool Expected { get; } = expected;
        }

        public static TheoryData<MathRoundDecimalTheoryData> MathRoundDecimal_DataSet
            =>
                [
                    new MathRoundDecimalTheoryData(5.9m, true),
                    new MathRoundDecimalTheoryData(5.4m, false)
                ];

        [Theory]
        [MemberData(nameof(MathRoundDecimal_DataSet), MemberType = typeof(FilterParameterTests))]
        public async Task MathRoundDecimal(MathRoundDecimalTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { UnitPrice = ToNullable<decimal>(theoryData.UnitPrice) };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(MathRoundDecimal)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, string.Format(CultureInfo.InvariantCulture, "$it => ($it.UnitPrice.Value.Round() > {0:0.00})", 5.0));
            Assert.Equal(theoryData.Expected, result);
            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new GreaterThanBinaryOperatorParameters
                    (
                        new RoundOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("UnitPrice", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(5.00m)
                    )
                );
        }

        [Fact]
        public async Task MathRoundDoubleInvalidOperationException()
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(MathRoundDoubleInvalidOperationException)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, string.Format(CultureInfo.InvariantCulture, "$it => ($it.Weight.Value.Round() > {0})", 5));
            Assert.Throws<InvalidOperationException>(() => RunFilter(newFilter, entity));

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new GreaterThanBinaryOperatorParameters
                    (
                        new RoundOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("Weight", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(5d)
                    )
                );
        }

        [Theory]
        [InlineData(5.9d, true)]
        [InlineData(5.4d, false)]
        public async Task MathRoundDouble(double? weight, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { Weight = ToNullable<double>(weight) };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(MathRoundDouble)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, string.Format(CultureInfo.InvariantCulture, "$it => ($it.Weight.Value.Round() > {0})", 5));
            Assert.Equal(expected, result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new GreaterThanBinaryOperatorParameters
                    (
                        new RoundOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("Weight", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(5d)
                    )
                );
        }

        [Fact]
        public async Task MathRoundFloatInvalidOperationException()
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(MathRoundFloatInvalidOperationException)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, string.Format(CultureInfo.InvariantCulture, "$it => (Convert($it.Width).Value.Round() > {0})", 5));
            Assert.Throws<InvalidOperationException>(() => RunFilter(newFilter, new Product { }));

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new GreaterThanBinaryOperatorParameters
                    (
                        new RoundOperatorParameters
                        (
                            new ConvertOperatorParameters(new MemberSelectorOperatorParameters("Width", new ParameterOperatorParameters(parameterName)), typeof(double?))
                        ),
                        new ConstantOperatorParameters(5d)
                    )
                );
        }

        [Theory]
        [InlineData(5.9f, true)]
        [InlineData(5.4f, false)]
        public async Task MathRoundFloat(float? width, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { Width = ToNullable<float>(width) };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(MathRoundFloat)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, string.Format(CultureInfo.InvariantCulture, "$it => (Convert($it.Width).Value.Round() > {0})", 5));
            Assert.Equal(expected, result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new GreaterThanBinaryOperatorParameters
                    (
                        new RoundOperatorParameters
                        (
                            new ConvertOperatorParameters(new MemberSelectorOperatorParameters("Width", new ParameterOperatorParameters(parameterName)), typeof(double?))
                        ),
                        new ConstantOperatorParameters(5d)
                    )
                );
        }

        [Fact]
        public async Task MathFloorDecimalInvalidOperationException()
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(MathFloorDecimalInvalidOperationException)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.UnitPrice.Value.Floor() == 5)");
            Assert.Throws<InvalidOperationException>(() => RunFilter(newFilter, entity));

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new FloorOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("UnitPrice", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(5m)
                    )
                );
        }

        public class MathFloorDecimalTheoryData(decimal? unitPrice, bool expected)
        {
            public decimal? UnitPrice { get; } = unitPrice;
            public bool Expected { get; } = expected;
        }

        public static TheoryData<MathFloorDecimalTheoryData> MathFloorDecimal_DataSet
            =>
                [
                    new MathFloorDecimalTheoryData(5.4m, true),
                    new MathFloorDecimalTheoryData(4.4m, false)
                ];

        [Theory, MemberData(nameof(MathFloorDecimal_DataSet), MemberType = typeof(FilterParameterTests))]
        public async Task MathFloorDecimal(MathFloorDecimalTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { UnitPrice = ToNullable<decimal>(theoryData.UnitPrice) };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(MathFloorDecimal)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.UnitPrice.Value.Floor() == 5)");
            Assert.Equal(theoryData.Expected, result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new FloorOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("UnitPrice", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(5m)
                    )
                );
        }

        [Fact]
        public async Task MathFloorDoubleInvalidOperationException()
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(MathFloorDoubleInvalidOperationException)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.Weight.Value.Floor() == 5)");
            Assert.Throws<InvalidOperationException>(() => RunFilter(newFilter, entity));

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new FloorOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("Weight", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(5d)
                    )
                );
        }

        [Theory]
        [InlineData(5.4d, true)]
        [InlineData(4.4d, false)]
        public async Task MathFloorDouble(double? weight, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { Weight = ToNullable<double>(weight) };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(MathFloorDouble)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.Weight.Value.Floor() == 5)");
            Assert.Equal(expected, result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new FloorOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("Weight", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(5d)
                    )
                );
        }

        [Fact]
        public async Task MathFloorFloatInvalidOperationException()
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(MathFloorFloatInvalidOperationException)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => (Convert($it.Width).Value.Floor() == 5)");
            Assert.Throws<InvalidOperationException>(() => RunFilter(newFilter, entity));

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new FloorOperatorParameters
                        (
                            new ConvertOperatorParameters(new MemberSelectorOperatorParameters("Width", new ParameterOperatorParameters(parameterName)), typeof(double?))
                        ),
                        new ConstantOperatorParameters(5d)
                    )
                );
        }

        [Theory]
        [InlineData(5.4f, true)]
        [InlineData(4.4f, false)]
        public async Task MathFloorFloat(float? width, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { Width = ToNullable<float>(width) };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(MathFloorFloat)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => (Convert($it.Width).Value.Floor() == 5)");
            Assert.Equal(expected, result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new FloorOperatorParameters
                        (
                            new ConvertOperatorParameters(new MemberSelectorOperatorParameters("Width", new ParameterOperatorParameters(parameterName)), typeof(double?))
                        ),
                        new ConstantOperatorParameters(5d)
                    )
                );
        }

        [Fact]
        public async Task MathCeilingDecimalInvalidOperationException()
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(MathCeilingDecimalInvalidOperationException)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.UnitPrice.Value.Ceiling() == 5)");
            Assert.Throws<InvalidOperationException>(() => RunFilter(newFilter, entity));

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new CeilingOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("UnitPrice", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(5m)
                    )
                );
        }

        public class MathCeilingDecimalTheoryData(decimal? unitPrice, bool expected)
        {
            public decimal? UnitPrice { get; } = unitPrice;
            public bool Expected { get; } = expected;
        }

        public static TheoryData<MathCeilingDecimalTheoryData> MathCeilingDecimal_DataSet
            =>
                [
                    new MathCeilingDecimalTheoryData(4.1m, true),
                    new MathCeilingDecimalTheoryData(5.9m, false)
                ];

        [Theory, MemberData(nameof(MathCeilingDecimal_DataSet), MemberType = typeof(FilterParameterTests))]
        public async Task MathCeilingDecimal(MathCeilingDecimalTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { UnitPrice = ToNullable<decimal>(theoryData.UnitPrice) };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(MathCeilingDecimal)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.UnitPrice.Value.Ceiling() == 5)");
            Assert.Equal(theoryData.Expected, result);
            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new CeilingOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("UnitPrice", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(5m)
                    )
                );
        }

        [Fact]
        public async Task MathCeilingDoubleInvalidOperationException()
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(MathCeilingDoubleInvalidOperationException)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.Weight.Value.Ceiling() == 5)");
            Assert.Throws<InvalidOperationException>(() => RunFilter(newFilter, entity));

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new CeilingOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("Weight", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(5d)
                    )
                );
        }

        [Theory]
        [InlineData(4.1d, true)]
        [InlineData(5.9d, false)]
        public async Task MathCeilingDouble(double? weight, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { Weight = ToNullable<double>(weight) };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(MathCeilingDouble)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.Weight.Value.Ceiling() == 5)");
            Assert.Equal(expected, result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new CeilingOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("Weight", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters(5d)
                    )
                );
        }

        [Fact]
        public async Task MathCeilingFloatInvalidOperationException()
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(MathCeilingFloatInvalidOperationException)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => (Convert($it.Width).Value.Ceiling() == 5)");
            Assert.Throws<InvalidOperationException>(() => RunFilter(newFilter, entity));

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new CeilingOperatorParameters
                        (
                            new ConvertOperatorParameters(new MemberSelectorOperatorParameters("Width", new ParameterOperatorParameters(parameterName)), typeof(double?))
                        ),
                        new ConstantOperatorParameters(5d)
                    )
                );
        }

        [Theory]
        [InlineData(4.1f, true)]
        [InlineData(5.9f, false)]
        public async Task MathCeilingFloat(float? width, bool expected)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { Width = ToNullable<float>(width) };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(MathCeilingFloat)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => (Convert($it.Width).Value.Ceiling() == 5)");
            Assert.Equal(expected, result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new CeilingOperatorParameters
                        (
                            new ConvertOperatorParameters(new MemberSelectorOperatorParameters("Width", new ParameterOperatorParameters(parameterName)), typeof(double?))
                        ),
                        new ConstantOperatorParameters(5d)
                    )
                );
        }

        public class MathFunctions_VariousTypesTheoryData(IExpressionParameter filterBody)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
        }

        public static TheoryData<MathFunctions_VariousTypesTheoryData> MathFunctions_VariousTypes_Data
            =>
            [
                new MathFunctions_VariousTypesTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new FloorOperatorParameters
                        (
                            new ConvertOperatorParameters(new MemberSelectorOperatorParameters("FloatProp", new ParameterOperatorParameters(parameterName)), typeof(double))
                        ),
                        new FloorOperatorParameters
                        (
                            new ConvertOperatorParameters(new MemberSelectorOperatorParameters("FloatProp", new ParameterOperatorParameters(parameterName)), typeof(double))
                        )
                    )
                ),
                new MathFunctions_VariousTypesTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new RoundOperatorParameters
                        (
                            new ConvertOperatorParameters(new MemberSelectorOperatorParameters("FloatProp", new ParameterOperatorParameters(parameterName)), typeof(double))
                        ),
                        new RoundOperatorParameters
                        (
                            new ConvertOperatorParameters(new MemberSelectorOperatorParameters("FloatProp", new ParameterOperatorParameters(parameterName)), typeof(double))
                        )
                    )
                ),
                new MathFunctions_VariousTypesTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new CeilingOperatorParameters
                        (
                            new ConvertOperatorParameters(new MemberSelectorOperatorParameters("FloatProp", new ParameterOperatorParameters(parameterName)), typeof(double))
                        ),
                        new CeilingOperatorParameters
                        (
                            new ConvertOperatorParameters(new MemberSelectorOperatorParameters("FloatProp", new ParameterOperatorParameters(parameterName)), typeof(double))
                        )
                    )
                ),
                new MathFunctions_VariousTypesTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new FloorOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DoubleProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new FloorOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DoubleProp", new ParameterOperatorParameters(parameterName))
                        )
                    )
                ),
                new MathFunctions_VariousTypesTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new RoundOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DoubleProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new RoundOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DoubleProp", new ParameterOperatorParameters(parameterName))
                        )
                    )
                ),
                new MathFunctions_VariousTypesTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new CeilingOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DoubleProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new CeilingOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DoubleProp", new ParameterOperatorParameters(parameterName))
                        )
                    )
                ),
                new MathFunctions_VariousTypesTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new FloorOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DecimalProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new FloorOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DecimalProp", new ParameterOperatorParameters(parameterName))
                        )
                    )
                ),
                new MathFunctions_VariousTypesTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new RoundOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DecimalProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new RoundOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DecimalProp", new ParameterOperatorParameters(parameterName))
                        )
                    )
                ),
                new MathFunctions_VariousTypesTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new CeilingOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DecimalProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new CeilingOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DecimalProp", new ParameterOperatorParameters(parameterName))
                        )
                    )
                ),
            ];

        [Theory]
        [MemberData(nameof(MathFunctions_VariousTypes_Data), MemberType = typeof(FilterParameterTests))]
        public async Task MathFunctions_VariousTypes(MathFunctions_VariousTypesTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<DataTypes>();
            var entity = new DataTypes { };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(MathFunctions_VariousTypes)}";
            Expression<Func<DataTypes, bool>> newFilter = (Expression<Func<DataTypes, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(DataTypes), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            Assert.True(result);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }
        #endregion Math Functions

        #region Custom Functions
        [Fact]
        public async Task CustomMethod_InstanceMethodOfDeclaringType()
        {
            //arrange
            const string productName = "Abcd";
            const int totalWidth = 5;
            const string expectedProductName = "Abcd ";

            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { ProductName = productName };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(CustomMethod_InstanceMethodOfDeclaringType)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            Assert.True(result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new CustomMethodOperatorParameters
                        (
                            typeof(string),
                            "PadRight",
                            [typeof(int).AssemblyQualifiedName!],
                            new IExpressionParameter[]
                            {
                                new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                                new ConstantOperatorParameters(totalWidth)
                            }
                        ),
                        new ConstantOperatorParameters(expectedProductName)
                    )
                );
        }

        [Fact]
        public async Task CustomMethod_StaticExtensionMethod()
        {
            //arrange
            const string productName = "Abcd";
            const int totalWidth = 5;
            const string expectedProductName = "Abcd ";

            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { ProductName = productName };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(CustomMethod_StaticExtensionMethod)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            Assert.True(result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new CustomMethodOperatorParameters
                        (
                            typeof(StringExtender),
                            "PadRightExStatic",
                            [typeof(string).AssemblyQualifiedName!, typeof(int).AssemblyQualifiedName!],
                            new IExpressionParameter[]
                            {
                                new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                                new ConstantOperatorParameters(totalWidth)
                            }
                        ),
                        new ConstantOperatorParameters(expectedProductName)
                    )
                );
        }

        [Fact]
        public async Task CustomMethod_StaticMethoOfDeclaringType()
        {
            //arrange
            const string productName = "Abcd";
            const int totalWidth = 5;
            const string expectedProductName = "Abcd ";

            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { ProductName = productName };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(CustomMethod_StaticMethoOfDeclaringType)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            Assert.True(result);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new CustomMethodOperatorParameters
                        (
                            typeof(FilterParameterTests),
                            nameof(PadRightStatic),
                            [typeof(string).AssemblyQualifiedName!, typeof(int).AssemblyQualifiedName!],
                            new IExpressionParameter[]
                            {
                                new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                                new ConstantOperatorParameters(totalWidth)
                            }
                        ),
                        new ConstantOperatorParameters(expectedProductName)
                    )
                );
        }
        #endregion Custom Functions

        #region Data Types
        [Fact]
        public async Task GuidExpression()
        {
            //act
            var filter1 = CreateFilter1<DataTypes>();
            var filter2 = CreateFilter2<DataTypes>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(GuidExpression)}";
            Expression<Func<DataTypes, bool>> newFilter1 = (Expression<Func<DataTypes, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter1, ruleName, typeof(DataTypes), null);
            Expression<Func<DataTypes, bool>> newFilter2 = (Expression<Func<DataTypes, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter2, ruleName, typeof(DataTypes), null);

            //assert
            AssertFilterStringIsCorrect(newFilter1, "$it => ($it.GuidProp == 0efdaecf-a9f0-42f3-a384-1295917af95e)");
            AssertFilterStringIsCorrect(newFilter2, "$it => ($it.GuidProp == 0efdaecf-a9f0-42f3-a384-1295917af95e)");

            Expression<Func<T, bool>> CreateFilter1<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("GuidProp", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters(new Guid("0EFDAECF-A9F0-42F3-A384-1295917AF95E"))
                    )
                );

            Expression<Func<T, bool>> CreateFilter2<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("GuidProp", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters(new Guid("0efdaecf-a9f0-42f3-a384-1295917af95e"))
                    )
                );
        }

        public class DateTimeExpressionTheoryData(IExpressionParameter filterBody, string expectedExpression)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
        }

        public static TheoryData<DateTimeExpressionTheoryData> DateTimeExpression_Data
            =>
            [
                new DateTimeExpressionTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("DateTimeProp", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters(new DateTimeOffset(new DateTime(2000, 12, 12, 12, 0, 0, DateTimeKind.Unspecified), TimeSpan.Zero))
                    ),
                    "$it => ($it.DateTimeProp == {0})"
                ),
                new DateTimeExpressionTheoryData
                (
                    new LessThanBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("DateTimeProp", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters(new DateTimeOffset(new DateTime(2000, 12, 12, 12, 0, 0, DateTimeKind.Unspecified), TimeSpan.Zero))
                    ),
                    "$it => ($it.DateTimeProp < {0})"
                )
            ];

        [Theory]
        [MemberData(nameof(DateTimeExpression_Data), MemberType = typeof(FilterParameterTests))]
        public async Task DateTimeExpression(DateTimeExpressionTheoryData theoryData)
        {
            //arrange
            var dateTime = new DateTimeOffset(new DateTime(2000, 12, 12, 12, 0, 0, DateTimeKind.Unspecified), TimeSpan.Zero);

            //act
            var filter = CreateFilter<DataTypes>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(DateTimeExpression)}";
            Expression<Func<DataTypes, bool>> newFilter = (Expression<Func<DataTypes, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(DataTypes), null);

            //assert
            AssertFilterStringIsCorrect(newFilter, string.Format(CultureInfo.InvariantCulture, theoryData.ExpectedExpression, dateTime));

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        [Fact]
        public async Task IntegerLiteralSuffix()
        {
            //act
            var filter1 = CreateFilter1<DataTypes>();
            var filter2 = CreateFilter2<DataTypes>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(IntegerLiteralSuffix)}";
            Expression<Func<DataTypes, bool>> newFilter1 = (Expression<Func<DataTypes, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter1, ruleName, typeof(DataTypes), null);
            Expression<Func<DataTypes, bool>> newFilter2 = (Expression<Func<DataTypes, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter2, ruleName, typeof(DataTypes), null);

            //assert
            AssertFilterStringIsCorrect(newFilter1, "$it => (($it.LongProp < 987654321) AndAlso ($it.LongProp > 123456789))");
            AssertFilterStringIsCorrect(newFilter2, "$it => (($it.LongProp < -987654321) AndAlso ($it.LongProp > -123456789))");

            Expression<Func<T, bool>> CreateFilter1<T>()
                => GetFilter<T>
                (
                    new AndBinaryOperatorParameters
                    (
                        new LessThanBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("LongProp", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters((long)987654321, typeof(long))
                        ),
                        new GreaterThanBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("LongProp", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters((long)123456789, typeof(long))
                        )
                    )
                );

            Expression<Func<T, bool>> CreateFilter2<T>()
                => GetFilter<T>
                (
                    new AndBinaryOperatorParameters
                    (
                        new LessThanBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("LongProp", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters((long)-987654321, typeof(long))
                        ),
                        new GreaterThanBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("LongProp", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters((long)-123456789, typeof(long))
                        )
                    )
                );
        }

        [Fact]
        public async Task EnumInExpression()
        {
            //act
            var filter = CreateFilter<DataTypes>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(EnumInExpression)}";
            Expression<Func<DataTypes, bool>> newFilter = (Expression<Func<DataTypes, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(DataTypes), null);
            var constant = (ConstantExpression)((MethodCallExpression)newFilter.Body).Arguments[0];
            var values = (IList<Position>)constant.Value!;

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => System.Collections.Generic.List`1[LogicBuilder.EntityFrameworkCore.Tests.Data.Position].Contains($it.SimpleEnumProp)");
            Assert.Equal([Position.First, Position.Second], values);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new InOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("SimpleEnumProp", new ParameterOperatorParameters(parameterName)),
                        new CollectionConstantOperatorParameters(new List<object?> { Position.First, Position.Second }, typeof(Position))
                    )
                );
        }

        [Fact]
        public async Task EnumInExpression_NullableEnum_WithNullable()
        {
            //act
            var filter = CreateFilter<DataTypes>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(EnumInExpression_NullableEnum_WithNullable)}";
            Expression<Func<DataTypes, bool>> newFilter = (Expression<Func<DataTypes, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(DataTypes), null);
            var constant = (ConstantExpression)((MethodCallExpression)newFilter.Body).Arguments[0];
            var values = (IList<Position?>)constant.Value!;

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => System.Collections.Generic.List`1[System.Nullable`1[LogicBuilder.EntityFrameworkCore.Tests.Data.Position]].Contains($it.NullableSimpleEnumProp)");
            Assert.Equal([Position.First, Position.Second], values);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new InOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("NullableSimpleEnumProp", new ParameterOperatorParameters(parameterName)),
                        new CollectionConstantOperatorParameters(new List<object?> { Position.First, Position.Second }, typeof(Position?))
                    )
                );
        }

        [Fact]
        public async Task EnumInExpression_NullableEnum_WithNullValue()
        {
            //act
            var filter = CreateFilter<DataTypes>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(EnumInExpression_NullableEnum_WithNullValue)}";
            Expression<Func<DataTypes, bool>> newFilter = (Expression<Func<DataTypes, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(DataTypes), null);
            var constant = (ConstantExpression)((MethodCallExpression)newFilter.Body).Arguments[0];
            var values = (IList<Position?>)constant.Value!;

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => System.Collections.Generic.List`1[System.Nullable`1[LogicBuilder.EntityFrameworkCore.Tests.Data.Position]].Contains($it.NullableSimpleEnumProp)");
            Assert.Equal([Position.First, null], values);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new InOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("NullableSimpleEnumProp", new ParameterOperatorParameters(parameterName)),
                        new CollectionConstantOperatorParameters(new List<object?> { Position.First, null! }, typeof(Position?))
                    )
                );
        }

        [Fact]
        public async Task RealLiteralSuffixes()
        {
            //act
            var filter1 = CreateFilter1<DataTypes>();
            var filter2 = CreateFilter2<DataTypes>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(RealLiteralSuffixes)}";
            Expression<Func<DataTypes, bool>> newFilter1 = (Expression<Func<DataTypes, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter1, ruleName, typeof(DataTypes), null);
            Expression<Func<DataTypes, bool>> newFilter2 = (Expression<Func<DataTypes, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter2, ruleName, typeof(DataTypes), null);

            //assert
            AssertFilterStringIsCorrect(newFilter1, string.Format(CultureInfo.InvariantCulture, "$it => (($it.FloatProp < {0:0.00}) AndAlso ($it.FloatProp > {1:0.00}))", 4321.56, 1234.56));
            AssertFilterStringIsCorrect(newFilter2, string.Format(CultureInfo.InvariantCulture, "$it => (($it.DecimalProp < {0:0.00}) AndAlso ($it.DecimalProp > {1:0.00}))", 4321.56, 1234.56));

            Expression<Func<T, bool>> CreateFilter1<T>()
                => GetFilter<T>
                (
                    new AndBinaryOperatorParameters
                    (
                        new LessThanBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("FloatProp", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters(4321.56F)
                        ),
                        new GreaterThanBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("FloatProp", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters(1234.56f)
                        )
                    )
                );

            Expression<Func<T, bool>> CreateFilter2<T>()
                => GetFilter<T>
                (
                    new AndBinaryOperatorParameters
                    (
                        new LessThanBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DecimalProp", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters(4321.56M)
                        ),
                        new GreaterThanBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DecimalProp", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters(1234.56m)
                        )
                    )
                );
        }

        [Theory]
        [InlineData("hello,world", "hello,world")]
        [InlineData("'hello,world", "'hello,world")]
        [InlineData("hello,world'", "hello,world'")]
        [InlineData("hello,'wor'ld", "hello,'wor'ld")]
        [InlineData("hello,''world", "hello,''world")]
        [InlineData("\"hello,world\"", "\"hello,world\"")]
        [InlineData("\"hello,world", "\"hello,world")]
        [InlineData("hello,world\"", "hello,world\"")]
        [InlineData("hello,\"world", "hello,\"world")]
        [InlineData("México D.F.", "México D.F.")]
        [InlineData("æææøøøååå", "æææøøøååå")]
        [InlineData("いくつかのテキスト", "いくつかのテキスト")]
        public async Task StringLiterals(string literal, string expected)
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(StringLiterals)}";
            Expression<Func<DataTypes, bool>> newFilter = (Expression<Func<DataTypes, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(DataTypes), null);

            //assert
            AssertFilterStringIsCorrect(newFilter, string.Format("$it => ($it.ProductName == \"{0}\")", expected));

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters(literal)
                    )
                );
        }

        [Theory]
        [InlineData('$')]
        [InlineData('&')]
        [InlineData('+')]
        [InlineData(',')]
        [InlineData('/')]
        [InlineData(':')]
        [InlineData(';')]
        [InlineData('=')]
        [InlineData('?')]
        [InlineData('@')]
        [InlineData(' ')]
        [InlineData('<')]
        [InlineData('>')]
        [InlineData('#')]
        [InlineData('%')]
        [InlineData('{')]
        [InlineData('}')]
        [InlineData('|')]
        [InlineData('\\')]
        [InlineData('^')]
        [InlineData('~')]
        [InlineData('[')]
        [InlineData(']')]
        [InlineData('`')]
        public async Task SpecialCharactersInStringLiteral(char c)
        {
            //act
            var filter = CreateFilter<Product>();
            var entity = new Product { ProductName = c.ToString() };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(SpecialCharactersInStringLiteral)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, new Product { ProductName = c.ToString() });

            //assert
            AssertFilterStringIsCorrect(newFilter, string.Format("$it => ($it.ProductName == \"{0}\")", c));
            Assert.True(result);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters(c.ToString())
                    )
                );
        }
        #endregion Data Types

        #region Casts
        [Fact]
        public async Task NSCast_OnEnumerableEntityCollection_GeneratesExpression_WithOfTypeOnEnumerable()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(NSCast_OnEnumerableEntityCollection_GeneratesExpression_WithOfTypeOnEnumerable)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => $it.Category.EnumerableProducts.Cast().Any(p => (p.ProductName == \"ProductName\"))");

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new AnyOperatorParameters
                    (
                        new CollectionCastOperatorParameters
                        (
                            new MemberSelectorOperatorParameters
                            (
                                "EnumerableProducts",
                                new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                            ),
                            typeof(DerivedProduct)
                        ),
                        new EqualsBinaryOperatorParameters
                        (
                             new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters("p")),
                             new ConstantOperatorParameters("ProductName")
                        ),
                        "p"
                    )
                );
        }

        [Fact]
        public async Task NSCast_OnQueryableEntityCollection_GeneratesExpression_WithOfTypeOnQueryable()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(NSCast_OnQueryableEntityCollection_GeneratesExpression_WithOfTypeOnQueryable)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => $it.Category.QueryableProducts.Cast().Any(p => (p.ProductName == \"ProductName\"))");

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new AnyOperatorParameters
                    (
                        new CollectionCastOperatorParameters
                        (
                            new MemberSelectorOperatorParameters
                            (
                                "QueryableProducts",
                                new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                            ),
                            typeof(DerivedProduct)
                        ),
                        new EqualsBinaryOperatorParameters
                        (
                             new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters("p")),
                             new ConstantOperatorParameters("ProductName")
                        ),
                        "p"
                    )
                );
        }

        [Fact]
        public async Task NSCast_OnEntityCollection_CanAccessDerivedInstanceProperty()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(NSCast_OnEntityCollection_CanAccessDerivedInstanceProperty)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);
            bool result1 = RunFilter(newFilter, new Product { Category = new Category { Products = [new DerivedProduct { DerivedProductName = "DerivedProductName" }] } });
            bool result2 = RunFilter(newFilter, new Product { Category = new Category { Products = [new DerivedProduct { DerivedProductName = "NotDerivedProductName" }] } });

            //assert
            Assert.True(result1);
            Assert.False(result2);

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new AnyOperatorParameters
                    (
                        new CollectionCastOperatorParameters
                        (
                            new MemberSelectorOperatorParameters
                            (
                                "Products",
                                new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                            ),
                            typeof(DerivedProduct)
                        ),
                        new EqualsBinaryOperatorParameters
                        (
                             new MemberSelectorOperatorParameters("DerivedProductName", new ParameterOperatorParameters("p")),
                             new ConstantOperatorParameters("DerivedProductName")
                        ),
                        "p"
                    )
                );
        }

        [Fact]
        public async Task NSCast_OnSingleEntity_GeneratesExpression_WithAsOperatorParameters()
        {
            //act
            var filter = CreateFilter<DerivedProduct>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(NSCast_OnSingleEntity_GeneratesExpression_WithAsOperatorParameters)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => (($it As Product).ProductName == \"ProductName\")");

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "ProductName",
                            new CastOperatorParameters
                            (
                                new ParameterOperatorParameters(parameterName),
                                typeof(Product)
                            )
                        ),
                        new ConstantOperatorParameters("ProductName")
                    )
                );
        }

        public class Inheritance_WithDerivedInstanceTheoryData(IExpressionParameter filterBody)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
        }

        public static TheoryData<Inheritance_WithDerivedInstanceTheoryData> Inheritance_WithDerivedInstance_Data
            =>
            [
                new Inheritance_WithDerivedInstanceTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "ProductName",
                            new CastOperatorParameters
                            (
                                new ParameterOperatorParameters(parameterName),
                                typeof(Product)
                            )
                        ),
                        new ConstantOperatorParameters("ProductName")
                    )
                ),
                new Inheritance_WithDerivedInstanceTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "DerivedProductName",
                            new CastOperatorParameters
                            (
                                new ParameterOperatorParameters(parameterName),
                                typeof(DerivedProduct)
                            )
                        ),
                        new ConstantOperatorParameters("DerivedProductName")
                    )
                ),
                new Inheritance_WithDerivedInstanceTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "CategoryID",
                            new MemberSelectorOperatorParameters
                            (
                                "Category",
                                new CastOperatorParameters
                                (
                                    new ParameterOperatorParameters(parameterName),
                                    typeof(DerivedProduct)
                                )
                            )
                        ),
                        new ConstantOperatorParameters(123)
                    )
                ),
                new Inheritance_WithDerivedInstanceTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "CategoryID",
                            new CastOperatorParameters
                            (
                                new MemberSelectorOperatorParameters
                                (
                                    "Category",
                                    new CastOperatorParameters
                                    (
                                        new ParameterOperatorParameters(parameterName),
                                        typeof(DerivedProduct)
                                    )
                                ),
                                typeof(DerivedCategory)
                            )
                        ),
                        new ConstantOperatorParameters(123)
                    )
                ),
            ];

        [Theory]
        [MemberData(nameof(Inheritance_WithDerivedInstance_Data), MemberType = typeof(FilterParameterTests))]
        public async Task Inheritance_WithDerivedInstance(Inheritance_WithDerivedInstanceTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<DerivedProduct>();
            var entity = new DerivedProduct { Category = new DerivedCategory { CategoryID = 123 }, ProductName = "ProductName", DerivedProductName = "DerivedProductName" };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(Inheritance_WithDerivedInstance)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, new DerivedProduct { Category = new DerivedCategory { CategoryID = 123 }, ProductName = "ProductName", DerivedProductName = "DerivedProductName" });

            //assert
            Assert.True(result);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class Inheritance_WithBaseInstanceTheoryData(IExpressionParameter filterBody)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
        }

        public static TheoryData<Inheritance_WithBaseInstanceTheoryData> Inheritance_WithBaseInstance_Data
            =>
            [
                new Inheritance_WithBaseInstanceTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "DerivedProductName",
                            new CastOperatorParameters
                            (
                                new ParameterOperatorParameters(parameterName),
                                typeof(DerivedProduct)
                            )
                        ),
                        new ConstantOperatorParameters("DerivedProductName")
                    )
                ),
                new Inheritance_WithBaseInstanceTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "CategoryID",
                            new MemberSelectorOperatorParameters
                            (
                                "Category",
                                new CastOperatorParameters
                                (
                                    new ParameterOperatorParameters(parameterName),
                                    typeof(DerivedProduct)
                                )
                            )
                        ),
                        new ConstantOperatorParameters(123)
                    )
                ),
                new Inheritance_WithBaseInstanceTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters
                        (
                            "CategoryID",
                            new CastOperatorParameters
                            (
                                new MemberSelectorOperatorParameters
                                (
                                    "Category",
                                    new CastOperatorParameters
                                    (
                                        new ParameterOperatorParameters(parameterName),
                                        typeof(DerivedProduct)
                                    )
                                ),
                                typeof(DerivedCategory)
                            )
                        ),
                        new ConstantOperatorParameters(123)
                    )
                ),
            ];

        [Theory]
        [MemberData(nameof(Inheritance_WithBaseInstance_Data), MemberType = typeof(FilterParameterTests))]
        public async Task Inheritance_WithBaseInstance(Inheritance_WithBaseInstanceTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(Inheritance_WithBaseInstance)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            Assert.Throws<NullReferenceException>(() => RunFilter(newFilter, new Product()));

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class CastMethod_SucceedsTheoryData(IExpressionParameter filterBody, string expectedResult)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedResult { get; } = expectedResult;
        }

        public static TheoryData<CastMethod_SucceedsTheoryData> CastMethod_Succeeds_Data
            =>
            [
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        new ConstantOperatorParameters(null!)
                    ),
                    "$it => (null == null)"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        new ConstantOperatorParameters(123)
                    ),
                    "$it => (null == Convert(123))"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new NotEqualsBinaryOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        new ConstantOperatorParameters(123)
                    ),
                    "$it => (null != Convert(123))"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new NotEqualsBinaryOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        new ConstantOperatorParameters(true)
                    ),
                    "$it => (null != Convert(True))"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new NotEqualsBinaryOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        new ConstantOperatorParameters(1)
                    ),
                    "$it => (null != Convert(1))"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        new ConstantOperatorParameters(Guid.Empty)
                    ),
                    "$it => (null == Convert(00000000-0000-0000-0000-000000000000))"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new NotEqualsBinaryOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        new ConstantOperatorParameters("123")
                    ),
                    "$it => (null != \"123\")"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        new ConstantOperatorParameters(new DateTimeOffset(new DateTime(2001, 1, 1, 12, 0, 0, DateTimeKind.Unspecified), new TimeSpan(8, 0, 0)))
                    ),
                    "$it => (null == Convert(01/01/2001 12:00:00 +08:00))"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        new ConstantOperatorParameters(new TimeSpan(7775999999000))
                    ),
                    "$it => (null == Convert(8.23:59:59.9999000))"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToStringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("IntProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("123")
                    ),
                    "$it => ($it.IntProp.ToString() == \"123\")"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToStringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("LongProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("123")
                    ),
                    "$it => ($it.LongProp.ToString() == \"123\")"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToStringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("SingleProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("123")
                    ),
                    "$it => ($it.SingleProp.ToString() == \"123\")"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToStringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DoubleProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("123")
                    ),
                    "$it => ($it.DoubleProp.ToString() == \"123\")"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToStringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DecimalProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("123")
                    ),
                    "$it => ($it.DecimalProp.ToString() == \"123\")"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToStringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("BoolProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("123")
                    ),
                    "$it => ($it.BoolProp.ToString() == \"123\")"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToStringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ByteProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("123")
                    ),
                    "$it => ($it.ByteProp.ToString() == \"123\")"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToStringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("GuidProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("123")
                    ),
                    "$it => ($it.GuidProp.ToString() == \"123\")"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("StringProp", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters("123")
                    ),
                    "$it => ($it.StringProp == \"123\")"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToStringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("DateTimeOffsetProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("123")
                    ),
                    "$it => ($it.DateTimeOffsetProp.ToString() == \"123\")"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToStringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("TimeSpanProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("123")
                    ),
                    "$it => ($it.TimeSpanProp.ToString() == \"123\")"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToStringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("SimpleEnumProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("123")
                    ),
                    "$it => (Convert($it.SimpleEnumProp).ToString() == \"123\")"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToStringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("FlagsEnumProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("123")
                    ),
                    "$it => (Convert($it.FlagsEnumProp).ToString() == \"123\")"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToStringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("LongEnumProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("123")
                    ),
                    "$it => (Convert($it.LongEnumProp).ToString() == \"123\")"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToStringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableIntProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("123")
                    ),
                    "$it => (IIF($it.NullableIntProp.HasValue, $it.NullableIntProp.Value.ToString(), null) == \"123\")"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToStringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableLongProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("123")
                    ),
                    "$it => (IIF($it.NullableLongProp.HasValue, $it.NullableLongProp.Value.ToString(), null) == \"123\")"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToStringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableSingleProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("123")
                    ),
                    "$it => (IIF($it.NullableSingleProp.HasValue, $it.NullableSingleProp.Value.ToString(), null) == \"123\")"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToStringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableDoubleProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("123")
                    ),
                    "$it => (IIF($it.NullableDoubleProp.HasValue, $it.NullableDoubleProp.Value.ToString(), null) == \"123\")"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToStringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableDecimalProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("123")
                    ),
                    "$it => (IIF($it.NullableDecimalProp.HasValue, $it.NullableDecimalProp.Value.ToString(), null) == \"123\")"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToStringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableBoolProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("123")
                    ),
                    "$it => (IIF($it.NullableBoolProp.HasValue, $it.NullableBoolProp.Value.ToString(), null) == \"123\")"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToStringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableByteProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("123")
                    ),
                    "$it => (IIF($it.NullableByteProp.HasValue, $it.NullableByteProp.Value.ToString(), null) == \"123\")"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToStringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableGuidProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("123")
                    ),
                    "$it => (IIF($it.NullableGuidProp.HasValue, $it.NullableGuidProp.Value.ToString(), null) == \"123\")"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToStringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableDateTimeOffsetProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("123")
                    ),
                    "$it => (IIF($it.NullableDateTimeOffsetProp.HasValue, $it.NullableDateTimeOffsetProp.Value.ToString(), null) == \"123\")"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToStringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableTimeSpanProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("123")
                    ),
                    "$it => (IIF($it.NullableTimeSpanProp.HasValue, $it.NullableTimeSpanProp.Value.ToString(), null) == \"123\")"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToStringOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableSimpleEnumProp", new ParameterOperatorParameters(parameterName))
                        ),
                        new ConstantOperatorParameters("123")
                    ),
                    "$it => (IIF($it.NullableSimpleEnumProp.HasValue, Convert($it.NullableSimpleEnumProp.Value).ToString(), null) == \"123\")"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("IntProp", new ParameterOperatorParameters(parameterName)),
                            typeof(long)
                        ),
                        new ConstantOperatorParameters((long)123)
                    ),
                    "$it => (Convert($it.IntProp) == 123)"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("NullableLongProp", new ParameterOperatorParameters(parameterName)),
                            typeof(double)
                        ),
                        new ConstantOperatorParameters(1.23d)
                    ),
                    "$it => (Convert($it.NullableLongProp) == 1.23)"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new NotEqualsBinaryOperatorParameters
                    (
                        new ConvertOperatorParameters
                        (
                            new ConstantOperatorParameters(2147483647),
                            typeof(short)
                        ),
                        new ConstantOperatorParameters(null!)
                    ),
                    "$it => (Convert(Convert(2147483647)) != null)"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToStringOperatorParameters
                        (
                            new ConstantOperatorParameters(Position.Second, typeof(Position))
                        ),
                        new ConstantOperatorParameters("1")
                    ),
                    "$it => (Convert(Second).ToString() == \"1\")"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToStringOperatorParameters
                        (
                            new ConvertOperatorParameters
                            (
                                new ConvertOperatorParameters
                                (
                                    new MemberSelectorOperatorParameters("IntProp", new ParameterOperatorParameters(parameterName)),
                                    typeof(long)
                                ),
                                typeof(short)
                            )
                        ),
                        new ConstantOperatorParameters("123")
                    ),
                    "$it => (Convert(Convert($it.IntProp)).ToString() == \"123\")"
                ),
                new CastMethod_SucceedsTheoryData
                (
                    new NotEqualsBinaryOperatorParameters
                    (
                        new ConvertToEnumOperatorParameters
                        (
                            "123",
                            typeof(Position)
                        ),
                        new ConstantOperatorParameters(null!)
                    ),
                    "$it => (Convert(123) != null)"
                )
            ];

        [Theory]
        [MemberData(nameof(CastMethod_Succeeds_Data), MemberType = typeof(FilterParameterTests))]
        public async Task CastMethod_Succeeds(CastMethod_SucceedsTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<DataTypes>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(CastMethod_Succeeds)}";
            Expression<Func<DataTypes, bool>> newFilter = (Expression<Func<DataTypes, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(DataTypes), null);

            //assert
            AssertFilterStringIsCorrect(newFilter, theoryData.ExpectedResult);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }
        #endregion Casts

        #region 'isof' in query option
        public class IsofMethod_SucceedsTheoryData(IExpressionParameter filterBody, string expectedExpression)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
        }

        public static TheoryData<IsofMethod_SucceedsTheoryData> IsofMethod_Succeeds_Data
            =>
            [
                new IsofMethod_SucceedsTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        typeof(short)
                    ),
                    "$it => IIF(($it Is System.Int16), True, False)"
                ),
                new IsofMethod_SucceedsTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        typeof(Product)
                    ),
                    "$it => IIF(($it Is LogicBuilder.EntityFrameworkCore.Tests.Data.Product), True, False)"
                ),
                new IsofMethod_SucceedsTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                        typeof(string)
                    ),
                    "$it => IIF(($it.ProductName Is System.String), True, False)"
                ),
                new IsofMethod_SucceedsTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName)),
                        typeof(Category)
                    ),
                    "$it => IIF(($it.Category Is LogicBuilder.EntityFrameworkCore.Tests.Data.Category), True, False)"
                ),
                new IsofMethod_SucceedsTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName)),
                        typeof(DerivedCategory)
                    ),
                    "$it => IIF(($it.Category Is LogicBuilder.EntityFrameworkCore.Tests.Data.DerivedCategory), True, False)"
                ),
                new IsofMethod_SucceedsTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("Ranking", new ParameterOperatorParameters(parameterName)),
                        typeof(Position)
                    ),
                    "$it => IIF(($it.Ranking Is LogicBuilder.EntityFrameworkCore.Tests.Data.Position), True, False)"
                ),
            ];

        [Theory]
        [MemberData(nameof(IsofMethod_Succeeds_Data), MemberType = typeof(FilterParameterTests))]
        public async Task IsofMethod_Succeeds(IsofMethod_SucceedsTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(IsofMethod_Succeeds)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, theoryData.ExpectedExpression);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class IsOfPrimitiveType_Succeeds_WithFalseTheoryData(IExpressionParameter filterBody)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
        }

        public static TheoryData<IsOfPrimitiveType_Succeeds_WithFalseTheoryData> IsOfPrimitiveType_Succeeds_WithFalse_Data
            =>
            [
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        typeof(byte[])
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        typeof(bool)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        typeof(byte)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        typeof(DateTimeOffset)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        typeof(Decimal)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        typeof(double)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        typeof(TimeSpan)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        typeof(Guid)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        typeof(Int16)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        typeof(Int32)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        typeof(Int64)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        typeof(sbyte)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        typeof(Single)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        typeof(System.IO.Stream)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        typeof(string)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        typeof(Position)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        typeof(Bits)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("ByteArrayProp", new ParameterOperatorParameters(parameterName)),
                        typeof(byte[])
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("IntProp", new ParameterOperatorParameters(parameterName)),
                        typeof(Position)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("NullableShortProp", new ParameterOperatorParameters(parameterName)),
                        typeof(short)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        typeof(byte[])
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        typeof(bool)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        typeof(byte)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        typeof(DateTimeOffset)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        typeof(Decimal)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        typeof(double)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        typeof(TimeSpan)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        typeof(Guid)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        typeof(Int16)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        typeof(Int32)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        typeof(Int64)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        typeof(sbyte)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        typeof(Single)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        typeof(System.IO.Stream)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        typeof(string)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        typeof(Position)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        typeof(Bits)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(23),
                        typeof(byte)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(23),
                        typeof(decimal)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(23),
                        typeof(double)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(23),
                        typeof(short)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(23),
                        typeof(long)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(23),
                        typeof(sbyte)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(23),
                        typeof(float)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters("hello"),
                        typeof(Stream)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(0),
                        typeof(Bits)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(0),
                        typeof(Position)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters("2001-01-01T12:00:00.000+08:00"),
                        typeof(DateTimeOffset)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters("00000000-0000-0000-0000-000000000000"),
                        typeof(Guid)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters("23"),
                        typeof(byte)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters("23"),
                        typeof(short)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters("23"),
                        typeof(int)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters("false"),
                        typeof(bool)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters("OData"),
                        typeof(byte[])
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters("PT12H'"),
                        typeof(TimeSpan)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(23),
                        typeof(string)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters("0"),
                        typeof(Bits)
                    )
                ),
                new IsOfPrimitiveType_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters("0"),
                        typeof(Position)
                    )
                )
            ];

        [Theory]
        [MemberData(nameof(IsOfPrimitiveType_Succeeds_WithFalse_Data), MemberType = typeof(FilterParameterTests))]
        public async Task IsOfPrimitiveType_Succeeds_WithFalse(IsOfPrimitiveType_Succeeds_WithFalseTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<DataTypes>();
            var entity = new DataTypes();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(IsOfPrimitiveType_Succeeds_WithFalse)}";
            Expression<Func<DataTypes, bool>> newFilter = (Expression<Func<DataTypes, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(DataTypes), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            Assert.False(result);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class IsOfQuotedNonPrimitiveTypeTheoryData(IExpressionParameter filterBody)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
        }

        public static TheoryData<IsOfQuotedNonPrimitiveTypeTheoryData> IsOfQuotedNonPrimitiveType
            =>
            [
                new IsOfQuotedNonPrimitiveTypeTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        typeof(DerivedProduct)
                    )
                ),
                new IsOfQuotedNonPrimitiveTypeTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("SupplierAddress", new ParameterOperatorParameters(parameterName)),
                        typeof(Address)
                    )
                ),
                new IsOfQuotedNonPrimitiveTypeTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName)),
                        typeof(DerivedCategory)
                    )
                )
            ];

        [Theory]
        [MemberData(nameof(IsOfQuotedNonPrimitiveType), MemberType = typeof(FilterParameterTests))]
        public async Task IsOfQuotedNonPrimitiveType_Succeeds(IsOfQuotedNonPrimitiveTypeTheoryData theoryData)
        {
            //arrange
            var entity = new DerivedProduct
            {
                SupplierAddress = new Address { City = "Redmond", },
                Category = new DerivedCategory { DerivedCategoryName = "DerivedCategory" }
            };

            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(IsOfQuotedNonPrimitiveType_Succeeds)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            Assert.True(result);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class IsOfQuotedNonPrimitiveTypeWithNull_Succeeds_WithFalseTheoryData(IExpressionParameter filterBody)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
        }

        public static TheoryData<IsOfQuotedNonPrimitiveTypeWithNull_Succeeds_WithFalseTheoryData> IsOfQuotedNonPrimitiveTypeWithNull_Succeeds_WithFalse_Data
            =>
            [
                new IsOfQuotedNonPrimitiveTypeWithNull_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        typeof(Address)
                    )
                ),
                new IsOfQuotedNonPrimitiveTypeWithNull_Succeeds_WithFalseTheoryData
                (
                    new IsOfOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        typeof(DerivedCategory)
                    )
                )
            ];

        [Theory]
        [MemberData(nameof(IsOfQuotedNonPrimitiveTypeWithNull_Succeeds_WithFalse_Data), MemberType = typeof(FilterParameterTests))]
        public async Task IsOfQuotedNonPrimitiveTypeWithNull_Succeeds_WithFalse(IsOfQuotedNonPrimitiveTypeWithNull_Succeeds_WithFalseTheoryData theoryData)
        {
            //arrange
            var entity = new DerivedProduct
            {
                SupplierAddress = new Address { City = "Redmond", },
                Category = new DerivedCategory { DerivedCategoryName = "DerivedCategory" }
            };

            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(IsOfQuotedNonPrimitiveTypeWithNull_Succeeds_WithFalse)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), entity);
            bool result = RunFilter(newFilter, entity);


            //assert
            Assert.False(result);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }
        #endregion 'isof' in query option

        #region
        public class ByteArrayComparisonsTheoryData(IExpressionParameter filterBody, string expectedExpression, bool expectedResult)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
            public bool ExpectedResult { get; } = expectedResult;
        }

        public static TheoryData<ByteArrayComparisonsTheoryData> ByteArrayComparisons_Data
            =>
            [
                new ByteArrayComparisonsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("ByteArrayProp", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters(Convert.FromBase64String("I6v/"))
                    ),
                    "$it => ($it.ByteArrayProp == System.Byte[])",
                    true
                ),
                new ByteArrayComparisonsTheoryData
                (
                    new NotEqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("ByteArrayProp", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters(Convert.FromBase64String("I6v/"))
                    ),
                    "$it => ($it.ByteArrayProp != System.Byte[])",
                    false
                ),
                new ByteArrayComparisonsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConstantOperatorParameters(Convert.FromBase64String("I6v/")),
                        new ConstantOperatorParameters(Convert.FromBase64String("I6v/"))
                    ),
                    "$it => (System.Byte[] == System.Byte[])",
                    true
                ),
                new ByteArrayComparisonsTheoryData
                (
                    new NotEqualsBinaryOperatorParameters
                    (
                        new ConstantOperatorParameters(Convert.FromBase64String("I6v/")),
                        new ConstantOperatorParameters(Convert.FromBase64String("I6v/"))
                    ),
                    "$it => (System.Byte[] != System.Byte[])",
                    false
                ),
                new ByteArrayComparisonsTheoryData
                (
                    new NotEqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("ByteArrayPropWithNullValue", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters(Convert.FromBase64String("I6v/"))
                    ),
                    "$it => ($it.ByteArrayPropWithNullValue != System.Byte[])",
                    true
                ),
                new ByteArrayComparisonsTheoryData
                (
                    new NotEqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("ByteArrayPropWithNullValue", new ParameterOperatorParameters(parameterName)),
                        new MemberSelectorOperatorParameters("ByteArrayPropWithNullValue", new ParameterOperatorParameters(parameterName))
                    ),
                    "$it => ($it.ByteArrayPropWithNullValue != $it.ByteArrayPropWithNullValue)",
                    false
                ),
                new ByteArrayComparisonsTheoryData
                (
                    new NotEqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("ByteArrayPropWithNullValue", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters(null!)
                    ),
                    "$it => ($it.ByteArrayPropWithNullValue != null)",
                    false
                ),
                new ByteArrayComparisonsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("ByteArrayPropWithNullValue", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters(null!)
                    ),
                    "$it => ($it.ByteArrayPropWithNullValue == null)",
                    true
                ),
                new ByteArrayComparisonsTheoryData
                (
                    new NotEqualsBinaryOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        new MemberSelectorOperatorParameters("ByteArrayPropWithNullValue", new ParameterOperatorParameters(parameterName))
                    ),
                    "$it => (null != $it.ByteArrayPropWithNullValue)",
                    false
                ),
                new ByteArrayComparisonsTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConstantOperatorParameters(null!),
                        new MemberSelectorOperatorParameters("ByteArrayPropWithNullValue", new ParameterOperatorParameters(parameterName))
                    ),
                    "$it => (null == $it.ByteArrayPropWithNullValue)",
                    true
                ),
            ];

        [Theory]
        [MemberData(nameof(ByteArrayComparisons_Data), MemberType = typeof(FilterParameterTests))]
        public async Task ByteArrayComparisons(ByteArrayComparisonsTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<DataTypes>();
            var entity = new DataTypes
            {
                ByteArrayProp = [35, 171, 255]
            };
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(ByteArrayComparisons)}";
            Expression<Func<DataTypes, bool>> newFilter = (Expression<Func<DataTypes, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(DataTypes), entity);
            bool result = RunFilter(newFilter, entity);

            //assert
            Assert.Equal(theoryData.ExpectedResult, result);
            AssertFilterStringIsCorrect(newFilter, theoryData.ExpectedExpression);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class DisAllowed_ByteArrayComparisonsTheoryData(IExpressionParameter filterBody)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
        }

        public static TheoryData<DisAllowed_ByteArrayComparisonsTheoryData> DisAllowed_ByteArrayComparisons_Data
            =>
            [
                new DisAllowed_ByteArrayComparisonsTheoryData
                (
                    new GreaterThanOrEqualsBinaryOperatorParameters
                    (
                        new ConstantOperatorParameters(Convert.FromBase64String("AP8Q")),
                        new ConstantOperatorParameters(Convert.FromBase64String("AP8Q"))
                    )
                ),
                new DisAllowed_ByteArrayComparisonsTheoryData
                (
                    new LessThanOrEqualsBinaryOperatorParameters
                    (
                        new ConstantOperatorParameters(Convert.FromBase64String("AP8Q")),
                        new ConstantOperatorParameters(Convert.FromBase64String("AP8Q"))
                    )
                ),
                new DisAllowed_ByteArrayComparisonsTheoryData
                (
                    new LessThanBinaryOperatorParameters
                    (
                        new ConstantOperatorParameters(Convert.FromBase64String("AP8Q")),
                        new ConstantOperatorParameters(Convert.FromBase64String("AP8Q"))
                    )
                ),
                new DisAllowed_ByteArrayComparisonsTheoryData
                (
                    new GreaterThanBinaryOperatorParameters
                    (
                        new ConstantOperatorParameters(Convert.FromBase64String("AP8Q")),
                        new ConstantOperatorParameters(Convert.FromBase64String("AP8Q"))
                    )
                ),
            ];

        [Theory]
        [MemberData(nameof(DisAllowed_ByteArrayComparisons_Data), MemberType = typeof(FilterParameterTests))]
        public async Task DisAllowed_ByteArrayComparisons(DisAllowed_ByteArrayComparisonsTheoryData theoryData)
        {
            //assert
            Assert.Throws<InvalidOperationException>(CreateFilter<DataTypes>);
            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class Nullable_NonstandardEdmPrimitivesTheoryData(IExpressionParameter filterBody, string expectedExpression)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
        }

        public static TheoryData<Nullable_NonstandardEdmPrimitivesTheoryData> Nullable_NonstandardEdmPrimitives_Data
            =>
            [
                new Nullable_NonstandardEdmPrimitivesTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertOperatorParameters
                        (
                            new ConvertToNullableUnderlyingValueOperatorParameters
                            (
                                new MemberSelectorOperatorParameters("NullableUShortProp", new ParameterOperatorParameters(parameterName))
                            ),
                            typeof(int?)
                        ),
                        new ConstantOperatorParameters(12)
                    ),
                    "$it => (Convert($it.NullableUShortProp.Value) == Convert(12))"
                ),
                new Nullable_NonstandardEdmPrimitivesTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertOperatorParameters
                        (
                            new ConvertToNullableUnderlyingValueOperatorParameters
                            (
                                new MemberSelectorOperatorParameters("NullableULongProp", new ParameterOperatorParameters(parameterName))
                            ),
                            typeof(long?)
                        ),
                        new ConstantOperatorParameters(12L)
                    ),
                    "$it => (Convert($it.NullableULongProp.Value) == Convert(12))"
                ),
                new Nullable_NonstandardEdmPrimitivesTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertOperatorParameters
                        (
                            new ConvertToNullableUnderlyingValueOperatorParameters
                            (
                                new MemberSelectorOperatorParameters("NullableUIntProp", new ParameterOperatorParameters(parameterName))
                            ),
                            typeof(int?)
                        ),
                        new ConstantOperatorParameters(12)
                    ),
                    "$it => (Convert($it.NullableUIntProp.Value) == Convert(12))"
                ),
                new Nullable_NonstandardEdmPrimitivesTheoryData
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new ConvertToStringOperatorParameters
                        (
                            new ConvertToNullableUnderlyingValueOperatorParameters
                            (
                                new MemberSelectorOperatorParameters("NullableCharProp", new ParameterOperatorParameters(parameterName))
                            )
                        ),
                        new ConstantOperatorParameters("a")
                    ),
                    "$it => ($it.NullableCharProp.Value.ToString() == \"a\")"
                ),
            ];

        [Theory]
        [MemberData(nameof(Nullable_NonstandardEdmPrimitives_Data), MemberType = typeof(FilterParameterTests))]
        public async Task Nullable_NonstandardEdmPrimitives(Nullable_NonstandardEdmPrimitivesTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<DataTypes>();
            var entity = new DataTypes();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(Nullable_NonstandardEdmPrimitives)}";
            Expression<Func<DataTypes, bool>> newFilter = (Expression<Func<DataTypes, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(DataTypes), entity);

            //assert
            AssertFilterStringIsCorrect(newFilter, theoryData.ExpectedExpression);
            Assert.Throws<InvalidOperationException>(() => RunFilter(newFilter, new DataTypes()));

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        public class InOnNavigationTheoryData(IExpressionParameter filterBody, string expectedExpression)
        {
            public IExpressionParameter FilterBody { get; } = filterBody;
            public string ExpectedExpression { get; } = expectedExpression;
        }

        public static TheoryData<InOnNavigationTheoryData> InOnNavigation_Data
            =>
                [
                    new InOnNavigationTheoryData
                    (
                        new InOperatorParameters
                        (
                            new MemberSelectorOperatorParameters
                            (
                                "ProductID",
                                new MemberSelectorOperatorParameters
                                (
                                    "Product",
                                    new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                                )
                            ),
                            new CollectionConstantOperatorParameters
                            (
                                [1],
                                typeof(int)
                            )
                        ),
                        "$it => System.Collections.Generic.List`1[System.Int32].Contains($it.Category.Product.ProductID)"
                    ),
                    new InOnNavigationTheoryData
                    (
                        new InOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("Category.Product.ProductID", new ParameterOperatorParameters(parameterName)),
                            new CollectionConstantOperatorParameters
                            (
                                [1],
                                typeof(int)
                            )
                        ),
                        "$it => System.Collections.Generic.List`1[System.Int32].Contains($it.Category.Product.ProductID)"
                    ),
                    new InOnNavigationTheoryData
                    (
                        new InOperatorParameters
                        (
                            new MemberSelectorOperatorParameters
                            (
                                "GuidProperty",
                                new MemberSelectorOperatorParameters
                                (
                                    "Product",
                                    new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                                )
                            ),
                            new CollectionConstantOperatorParameters
                            (
                                [new Guid("dc75698b-581d-488b-9638-3e28dd51d8f7")],
                                typeof(Guid)
                            )
                        ),
                        "$it => System.Collections.Generic.List`1[System.Guid].Contains($it.Category.Product.GuidProperty)"
                    ),
                    new InOnNavigationTheoryData
                    (
                        new InOperatorParameters
                        (
                            new MemberSelectorOperatorParameters
                            (
                                "NullableGuidProperty",
                                new MemberSelectorOperatorParameters
                                (
                                    "Product",
                                    new MemberSelectorOperatorParameters("Category", new ParameterOperatorParameters(parameterName))
                                )
                            ),
                            new CollectionConstantOperatorParameters
                            (
                                [new Guid("dc75698b-581d-488b-9638-3e28dd51d8f7")],
                                typeof(Guid?)
                            )
                        ),
                        "$it => System.Collections.Generic.List`1[System.Nullable`1[System.Guid]].Contains($it.Category.Product.NullableGuidProperty)"
                    )
                ];

        [Theory]
        [MemberData(nameof(InOnNavigation_Data), MemberType = typeof(FilterParameterTests))]
        public async Task InOnNavigation(InOnNavigationTheoryData theoryData)
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(InOnNavigation)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, theoryData.ExpectedExpression);

            Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    theoryData.FilterBody
                );
        }

        [Fact]
        public async Task MultipleConstants_Are_Parameterized()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(MultipleConstants_Are_Parameterized)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => (((($it.ProductName == \"1\") OrElse ($it.ProductName == \"2\")) OrElse ($it.ProductName == \"3\")) OrElse ($it.ProductName == \"4\"))");

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new OrBinaryOperatorParameters
                    (
                        new OrBinaryOperatorParameters
                        (
                            new OrBinaryOperatorParameters
                            (
                                new EqualsBinaryOperatorParameters
                                (
                                    new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                                    new ConstantOperatorParameters("1")
                                ),
                                new EqualsBinaryOperatorParameters
                                (
                                    new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                                    new ConstantOperatorParameters("2")
                                )
                            ),
                            new EqualsBinaryOperatorParameters
                            (
                                new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                                new ConstantOperatorParameters("3")
                            )
                        ),
                        new EqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                            new ConstantOperatorParameters("4")
                        )
                    )
                );
        }

        [Fact]
        public async Task Constants_Are_Not_Parameterized_IfDisabled()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(Constants_Are_Not_Parameterized_IfDisabled)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => ($it.ProductName == \"1\")");

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new EqualsBinaryOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                        new ConstantOperatorParameters("1")
                    )
                );
        }

        [Fact]
        public async Task CollectionConstants_Are_Parameterized()
        {
            //act
            var filter = CreateFilter<Product>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(CollectionConstants_Are_Parameterized)}";
            Expression<Func<Product, bool>> newFilter = (Expression<Func<Product, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(Product), null!);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => System.Collections.Generic.List`1[System.String].Contains($it.ProductName)");

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new InOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters(parameterName)),
                        new CollectionConstantOperatorParameters
                        (
                            new List<object?> { "Prod1", "Prod2" },
                            typeof(string)
                        )
                    )
                );
        }

        [Fact]
        public async Task CollectionConstants_OfEnums_Are_Not_Parameterized_If_Disabled()
        {
            //act
            var filter = CreateFilter<DataTypes>();
            string ruleName = $"{nameof(FilterParameterTests)}_{nameof(CollectionConstants_OfEnums_Are_Not_Parameterized_If_Disabled)}";
            Expression<Func<DataTypes, bool>> newFilter = (Expression<Func<DataTypes, bool>>)await RecreateFilterFromFilterLambdaOperatorParameters(filter, ruleName, typeof(DataTypes), null);

            //assert
            AssertFilterStringIsCorrect(newFilter, "$it => System.Collections.Generic.List`1[LogicBuilder.EntityFrameworkCore.Tests.Data.Position].Contains($it.SimpleEnumProp)");

            static Expression<Func<T, bool>> CreateFilter<T>()
                => GetFilter<T>
                (
                    new InOperatorParameters
                    (
                        new MemberSelectorOperatorParameters("SimpleEnumProp", new ParameterOperatorParameters(parameterName)),
                        new CollectionConstantOperatorParameters
                        (
                            new List<object?> { Position.First, Position.Second },
                            typeof(Position)
                        )
                    )
                );
        }
        #endregion

        #region Helpers
        private static void AssertFilterStringIsCorrect(Expression expression, string expected)
        {
            string resultExpression = ExpressionStringBuilder.ToString(expression);
            Assert.True(expected == resultExpression, string.Format("Expected expression '{0}' but the deserializer produced '{1}'", expected, resultExpression));
        }

        private RuleEngine CreateRuleEngine(string formattedXml, string ruleName)
        {
            string selectedApplication = _configurationService.GetSelectedApplication().Name;
            ApplicationTypeInfo applicationTypeInfo = _applicationTypeInfoManager.GetApplicationTypeInfo(selectedApplication);
            ICodeExpressionBuilder codeExpressionBuilder = _rulesGeneratorFactory.GetCodeExpressionBuilder(applicationTypeInfo, new Dictionary<string, string>(), mduleName);
            ConstructorData constructorData = _constructorDataParser.Parse(_xmlDocumentHelpers.ToXmlElement(formattedXml));
            var codeExpression = (CodeExpression)null!;//codeExpressionBuilder.BuildConstructor(constructorData);

            CodeBinaryOperatorExpression alwaysTrueCondition = new()
            {
                Left = new CodePrimitiveExpression(1),
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = new CodePrimitiveExpression(1)
            };

            CodeIndexerExpression itemsIndexer = new
            (
                new CodePropertyReferenceExpression
                (
                    new CodePropertyReferenceExpression
                    (
                        new CodeFieldReferenceExpression
                        (
                            new CodeThisReferenceExpression(), 
                            "_flowManager"
                        ), 
                        "FlowDataCache"
                    ), 
                    "Items"
                ), 
                new CodePrimitiveExpression(ruleName)
            );

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

        private static Expression<Func<T, bool>> GetFilter<T>(IExpressionParameter filterBody)
        {
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();
            IExpressionParameter completeLambda = new FilterLambdaOperatorParameters
            (
                filterBody,
                typeof(T),
                parameterName
            );//Create IExpressionParameter for lambda expression e.g. $it => $it.Any()

            return (Expression<Func<T, bool>>)mapper.Map<FilterLambdaOperator>//map the complete lambda from decriptor object to operator object
            (
                mapper.Map<DescriptorBase>(completeLambda),//map the complete lambda from parameter object to the FilterLambdaDescriptor object
                opts => opts.Items["parameters"] = new Dictionary<string, ParameterExpression>()
            ).Build();//create the lambda expression from the operator object
        }

        [MemberNotNull(nameof(MapperConfiguration))]
        [MemberNotNull(nameof(serviceProvider))]
        private static void Initialize()
        {
            MapperConfiguration ??= ConfigurationHelper.GetMapperConfiguration(cfg =>
            {
                cfg.AddProfile<ExpressionOperatorsMappingProfile>();
                cfg.AddProfile<ExpressionParameterToDescriptorMappingProfile>();
            });

            MapperConfiguration.AssertConfigurationIsValid();

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
                .AddScoped<LogicBuilder.App.Bsl.Flow.Interfaces.IFlowDataCache, LogicBuilder.App.Bsl.Flow.FlowDataCache>()
                .AddScoped<Progress, Progress>()
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
        }
        // Used by Custom Method binder tests - by reflection
        private static string PadRightStatic(string str, int number)
        {
            return str.PadRight(number);
        }

        private async Task<LambdaExpression> RecreateFilterFromFilterLambdaOperatorParameters(LambdaExpression filter, string ruleName, Type entityType, object? entity)
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

            IExpressionPart filterLambdaOperator = mappingOperations.MapToOperator((FilterLambdaOperatorParameters)(flowManager.FlowDataCache.Items[ruleName]));

            return (LambdaExpression)filterLambdaOperator.Build();
        }

        private static bool RunFilter<TModel>(Expression<Func<TModel, bool>> filter, TModel instance)
            => filter.Compile().Invoke(instance);

        private static T? ToNullable<T>(object? value) where T : struct =>
            value == null ? null : (T?)Convert.ChangeType(value, typeof(T));
        #endregion Helpers
    }

    public static class StringExtender
    {
        public static string PadRightExStatic(this string str, int width)
        {
            return str.PadRight(width);
        }
    }
}
