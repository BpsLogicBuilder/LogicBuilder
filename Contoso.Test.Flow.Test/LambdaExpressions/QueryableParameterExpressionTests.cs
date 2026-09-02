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
using Contoso.Data.Entities;
using LogicBuilder.App.Utils.Interfaces;
using LogicBuilder.App.Utils.Rules;
using LogicBuilder.EntityFrameworkCore.Mapping;
using LogicBuilder.Expressions.Utils;
using LogicBuilder.Expressions.Utils.ExpressionBuilder;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.Lambda;
using LogicBuilder.Expressions.Utils.ExpressionDescriptors;
using LogicBuilder.Expressions.Utils.Strutures;
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
    public class QueryableParameterExpressionTests
    {
        static QueryableParameterExpressionTests()
        {
            Initialize();
        }

        public QueryableParameterExpressionTests()
        {
            InitializeLogicBuilderMetadata();
        }

        #region Tests
        [Fact]
        public async Task BuildWhere_OrderBy_ThenBy_Skip_Take_Average()
        {
            //act
            var descriptor = new AverageOperatorParameters
            (
                new TakeOperatorParameters
                (
                    new SkipOperatorParameters
                    (
                        new ThenByOperatorParameters
                        (
                            new OrderByOperatorParameters
                            (
                                new WhereOperatorParameters
                                (//q.Where(s => ((s.ID > 1) AndAlso (Compare(s.FirstName, s.LastName) > 0)))
                                    new ParameterOperatorParameters("q"),//q. the source operand
                                    new AndBinaryOperatorParameters//((s.ID > 1) AndAlso (Compare(s.FirstName, s.LastName) > 0)
                                    (
                                        new GreaterThanBinaryOperatorParameters
                                        (
                                            new MemberSelectorOperatorParameters("Id", new ParameterOperatorParameters("s")),
                                            new ConstantOperatorParameters(1, typeof(int))
                                        ),
                                        new GreaterThanBinaryOperatorParameters
                                        (
                                            new MemberSelectorOperatorParameters("FirstName", new ParameterOperatorParameters("s")),
                                            new MemberSelectorOperatorParameters("LastName", new ParameterOperatorParameters("s"))
                                        )
                                    ),
                                    "s"//s => (created in Where operator.  The parameter type is based on the source operand underlying type in this case Student.)
                                ),
                                new MemberSelectorOperatorParameters("LastName", new ParameterOperatorParameters("v")),
                                ListSortDirection.Ascending,
                                "v"
                            ),
                            new MemberSelectorOperatorParameters("FirstName", new ParameterOperatorParameters("v")),
                            ListSortDirection.Descending,
                            "v"
                        ),
                        2
                    ),
                    3
                ),
                new MemberSelectorOperatorParameters("Id", new ParameterOperatorParameters("j")),
                "j"
            );

            Expression<Func<IQueryable<Student>, double>> expression = GetExpression<IQueryable<Student>, double>(descriptor, "q");
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(BuildWhere_OrderBy_ThenBy_Skip_Take_Average)}";
            Expression<Func<IQueryable<Student>, double>> newSelector = (Expression<Func<IQueryable<Student>, double>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Student), null);

            //assert
            AssertFilterStringIsCorrect(newSelector, "q => q.Where(s => ((s.ID > 1) AndAlso (s.FirstName.Compare(s.LastName) > 0))).OrderBy(v => v.LastName).ThenByDescending(v => v.FirstName).Skip(2).Take(3).Average(j => j.ID)");
        }

        [Fact]
        public async Task BuildGroupBy_OrderBy_ThenBy_Skip_Take_Average()
        {
            //act
            var descriptor = new SelectOperatorParameters
            (
                new OrderByOperatorParameters
                (
                    new GroupByOperatorParameters
                    (
                        new ParameterOperatorParameters("q"),
                        new ConstantOperatorParameters(1, typeof(int)),
                        "a"
                    ),
                    new MemberSelectorOperatorParameters("Key", new ParameterOperatorParameters("b")),
                    ListSortDirection.Ascending,
                    "b"
                ),
                new MemberInitOperatorParameters
                (
                    [
                        new
                        (
                            "Sum_budget",
                            new ToListOperatorParameters
                            (
                                new WhereOperatorParameters
                                (
                                    new ParameterOperatorParameters("q"),
                                    new AndBinaryOperatorParameters
                                    (
                                        new EqualsBinaryOperatorParameters
                                        (
                                            new MemberSelectorOperatorParameters("DepartmentID", new ParameterOperatorParameters("d")),
                                            new CountOperatorParameters(new ParameterOperatorParameters("q"))
                                        ),
                                        new EqualsBinaryOperatorParameters
                                        (
                                            new MemberSelectorOperatorParameters("DepartmentID", new ParameterOperatorParameters("d")),
                                            new MemberSelectorOperatorParameters("Key", new ParameterOperatorParameters("c"))
                                        )
                                    ),
                                    "d"
                                )
                            )
                        )
                    ]
                ),
                "c"
            );

            Expression<Func<IQueryable<Department>, IQueryable<object>>> expression = GetExpression<IQueryable<Department>, IQueryable<object>>(descriptor, "q");
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(BuildGroupBy_OrderBy_ThenBy_Skip_Take_Average)}";
            Expression<Func<IQueryable<Department>, IQueryable<object>>> newSelector = (Expression<Func<IQueryable<Department>, IQueryable<object>>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Department), null);

            //assert
            AssertFilterStringIsCorrect(newSelector, "q => Convert(q.GroupBy(a => 1).OrderBy(b => b.Key).Select(c => new AnonymousType() {Sum_budget = q.Where(d => ((d.DepartmentID == q.Count()) AndAlso (d.DepartmentID == c.Key))).ToList()}))");
        }

        [Fact]
        public async Task BuildGroupBy_AsQueryable_OrderBy_Select_FirstOrDefault()
        {
            //act
            var descriptor = new FirstOrDefaultOperatorParameters
            (
                new SelectOperatorParameters
                (
                    new OrderByOperatorParameters
                    (
                        new AsQueryableOperatorParameters
                        (
                            new GroupByOperatorParameters
                            (
                                new ParameterOperatorParameters("q"),
                                new ConstantOperatorParameters(1, typeof(int)),
                                "item"
                            )
                        ),
                        new MemberSelectorOperatorParameters("Key", new ParameterOperatorParameters("group")),
                        ListSortDirection.Ascending,
                        "group"
                    ),
                    new MemberInitOperatorParameters
                    (
                        [
                            new
                            (
                                "Min_administratorName",
                                new MinOperatorParameters
                                (
                                    new WhereOperatorParameters
                                    (
                                        new ParameterOperatorParameters("q"),
                                        new EqualsBinaryOperatorParameters
                                        (
                                            new ConstantOperatorParameters(1, typeof(int)),
                                            new MemberSelectorOperatorParameters("Key", new ParameterOperatorParameters("sel"))
                                        ),
                                        "d"
                                    ),
                                    new ConcatOperatorParameters
                                    (
                                        new ConcatOperatorParameters
                                        (
                                            new MemberSelectorOperatorParameters("Administrator.LastName", new ParameterOperatorParameters("item")),
                                            new ConstantOperatorParameters(" ", typeof(string))
                                        ),
                                        new MemberSelectorOperatorParameters("Administrator.FirstName", new ParameterOperatorParameters("item"))
                                    ),
                                    "item"
                                )
                            ),
                            new
                            (
                                "Count_name",
                                new CountOperatorParameters
                                (
                                    new WhereOperatorParameters
                                    (
                                        new ParameterOperatorParameters("q"),
                                        new EqualsBinaryOperatorParameters
                                        (
                                            new ConstantOperatorParameters(1, typeof(int)),
                                            new MemberSelectorOperatorParameters("Key", new ParameterOperatorParameters("sel"))
                                        ),
                                        "d"
                                    )
                                )
                            ),
                            new
                            (
                                "Sum_budget",
                                new SumOperatorParameters
                                (
                                    new WhereOperatorParameters
                                    (
                                        new ParameterOperatorParameters("q"),
                                        new EqualsBinaryOperatorParameters
                                        (
                                            new ConstantOperatorParameters(1, typeof(int)),
                                            new MemberSelectorOperatorParameters("Key", new ParameterOperatorParameters("sel"))
                                        ),
                                        "d"
                                    ),
                                    new MemberSelectorOperatorParameters("Budget", new ParameterOperatorParameters("item")),
                                    "item"
                                )
                            ),
                            new
                            (
                                "Min_budget",
                                new MinOperatorParameters
                                (
                                    new WhereOperatorParameters
                                    (
                                        new ParameterOperatorParameters("q"),
                                        new EqualsBinaryOperatorParameters
                                        (
                                            new ConstantOperatorParameters(1, typeof(int)),
                                            new MemberSelectorOperatorParameters("Key", new ParameterOperatorParameters("sel"))
                                        ),
                                        "d"
                                    ),
                                    new MemberSelectorOperatorParameters("Budget", new ParameterOperatorParameters("item")),
                                    "item"
                            )
                            ),
                            new
                            (
                                "Min_startDate",
                                new MinOperatorParameters
                                (
                                    new WhereOperatorParameters
                                    (
                                        new ParameterOperatorParameters("q"),
                                        new EqualsBinaryOperatorParameters
                                        (
                                            new ConstantOperatorParameters(1, typeof(int)),
                                            new MemberSelectorOperatorParameters("Key", new ParameterOperatorParameters("sel"))
                                        ),
                                        "d"
                                    ),
                                    new MemberSelectorOperatorParameters("StartDate", new ParameterOperatorParameters("item")),
                                    "item"
                                )
                            )

                        ]
                    ),
                    "sel"
                )
            );

            Expression<Func<IQueryable<Department>, object>> expression = GetExpression<IQueryable<Department>, object>(descriptor, "q");
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(BuildGroupBy_AsQueryable_OrderBy_Select_FirstOrDefault)}";
            Expression<Func<IQueryable<Department>, object>> newSelector = (Expression<Func<IQueryable<Department>, object>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Department), null);

            //assert
            AssertFilterStringIsCorrect(newSelector, "q => Convert(q.GroupBy(item => 1).AsQueryable().OrderBy(group => group.Key).Select(sel => new AnonymousType() {Min_administratorName = q.Where(d => (1 == sel.Key)).Min(item => item.Administrator.LastName.Concat(\" \").Concat(item.Administrator.FirstName)), Count_name = q.Where(d => (1 == sel.Key)).Count(), Sum_budget = q.Where(d => (1 == sel.Key)).Sum(item => item.Budget), Min_budget = q.Where(d => (1 == sel.Key)).Min(item => item.Budget), Min_startDate = q.Where(d => (1 == sel.Key)).Min(item => item.StartDate)}).FirstOrDefault())");
        }

        [Fact]
        public async Task BuildGroupBy_AsQueryable_OrderBy_Select_IGroupableAsEnumerable_FirstOrDefault()
        {
            //act
            var descriptor = new FirstOrDefaultOperatorParameters
            (
                new SelectOperatorParameters
                (
                    new OrderByOperatorParameters
                    (
                        new AsQueryableOperatorParameters
                        (
                            new GroupByOperatorParameters
                            (
                                new ParameterOperatorParameters("q"),
                                new ConstantOperatorParameters(1, typeof(int)),
                                "item"
                            )
                        ),
                        new MemberSelectorOperatorParameters("Key", new ParameterOperatorParameters("group")),
                        ListSortDirection.Ascending,
                        "group"
                    ),
                    new MemberInitOperatorParameters
                    (
                        [
                            new MemberBindingItem
                            (
                                "NumericValue",
                                new CountOperatorParameters
                                (
                                    new AsEnumerableOperatorParameters(new ParameterOperatorParameters("sel"))
                                )
                            )
                        ]
                    ),
                    "sel"
                )
            );

            Expression<Func<IQueryable<Department>, object>> expression = GetExpression<IQueryable<Department>, object>(descriptor, "q");
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(BuildGroupBy_AsQueryable_OrderBy_Select_IGroupableAsEnumerable_FirstOrDefault)}";
            Expression<Func<IQueryable<Department>, object>> newSelector = (Expression<Func<IQueryable<Department>, object>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Department), null);

            //assert
            AssertFilterStringIsCorrect(newSelector, "q => Convert(q.GroupBy(item => 1).AsQueryable().OrderBy(group => group.Key).Select(sel => new AnonymousType() {NumericValue = sel.AsEnumerable().Count()}).FirstOrDefault())");
        }

        [Fact]
        public async Task All_Filter()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, bool>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(All_Filter)}";
            Expression<Func<IQueryable<Category>, bool>> newSelector = (Expression<Func<IQueryable<Category>, bool>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);

            var result = RunExpression(newSelector, GetCategories());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.All(a => ((a.CategoryName == \"CategoryOne\") OrElse (a.CategoryName == \"CategoryTwo\")))");
            Assert.True(result);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new AllOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        new OrBinaryOperatorParameters
                        (
                            new EqualsBinaryOperatorParameters
                            (
                                new MemberSelectorOperatorParameters("CategoryName", new ParameterOperatorParameters("a")),
                                new ConstantOperatorParameters("CategoryOne")
                            ),
                            new EqualsBinaryOperatorParameters
                            (
                                new MemberSelectorOperatorParameters("CategoryName", new ParameterOperatorParameters("a")),
                                new ConstantOperatorParameters("CategoryTwo")
                            )
                        ),
                        "a"
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task Any_Filter()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, bool>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(Any_Filter)}";
            Expression<Func<IQueryable<Category>, bool>> newSelector = (Expression<Func<IQueryable<Category>, bool>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.Any(a => (a.CategoryName == \"CategoryOne\"))");
            Assert.True(result);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new AnyOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        new EqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("CategoryName", new ParameterOperatorParameters("a")),
                            new ConstantOperatorParameters("CategoryOne")
                        ),
                        "a"
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task Any()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, bool>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(Any)}";
            Expression<Func<IQueryable<Category>, bool>> newSelector = (Expression<Func<IQueryable<Category>, bool>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.Any()");
            Assert.True(result);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new AnyOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName)
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task AsQueryable()
        {
            //act
            var expression = CreateExpression<IEnumerable<Category>, IQueryable<Category>>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(AsQueryable)}";
            Expression<Func<IEnumerable<Category>, IQueryable<Category>>> newSelector = (Expression<Func<IEnumerable<Category>, IQueryable<Category>>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, [new Category()]);

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.AsQueryable()");
            Assert.True(result.GetType().IsIQueryable());

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new AsQueryableOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName)
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task Average_Selector()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, double>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(Average_Selector)}";
            Expression<Func<IQueryable<Category>, double>> newSelector = (Expression<Func<IQueryable<Category>, double>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.Average(a => a.CategoryID)");
            Assert.Equal(1.5, result);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new AverageOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a")),
                        "a"
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task Average()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, double>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(Average)}";
            Expression<Func<IQueryable<Category>, double>> newSelector = (Expression<Func<IQueryable<Category>, double>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.Select(a => a.CategoryID).Average()");
            Assert.Equal(1.5, result);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new AverageOperatorParameters
                    (
                        new SelectOperatorParameters
                        (
                            new ParameterOperatorParameters(parameterName),
                            new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a")),
                            "a"
                        )
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task Count_Filter()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, int>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(Count_Filter)}";
            Expression<Func<IQueryable<Category>, int>> newSelector = (Expression<Func<IQueryable<Category>, int>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.Count(a => (a.CategoryID == 1))");
            Assert.Equal(1, result);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new CountOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        new EqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a")),
                            new ConstantOperatorParameters(1)
                        ),
                        "a"
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task Count()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, int>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(Count)}";
            Expression<Func<IQueryable<Category>, int>> newSelector = (Expression<Func<IQueryable<Category>, int>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.Count()");
            Assert.Equal(2, result);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new CountOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName)
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task Distinct()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, IQueryable<Category>>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(Distinct)}";
            Expression<Func<IQueryable<Category>, IQueryable<Category>>> newSelector = (Expression<Func<IQueryable<Category>, IQueryable<Category>>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.Distinct()");
            Assert.Equal(2, result.Count());

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new DistinctOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName)
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task First_Filter_Throws_Exception()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, Category>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(First_Filter_Throws_Exception)}";
            Expression<Func<IQueryable<Category>, Category>> newSelector = (Expression<Func<IQueryable<Category>, Category>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.First(a => (a.CategoryID == -1))");
            Assert.Throws<InvalidOperationException>(() => RunExpression(newSelector, GetCategories()));

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new FirstOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        new EqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a")),
                            new ConstantOperatorParameters(-1)
                        ),
                        "a"
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task First_Filter_Returns_match()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, Category>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(First_Filter_Returns_match)}";
            Expression<Func<IQueryable<Category>, Category>> newSelector = (Expression<Func<IQueryable<Category>, Category>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.First(a => (a.CategoryID == 1))");
            Assert.Equal(1, result.CategoryID);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new FirstOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        new EqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a")),
                            new ConstantOperatorParameters(1)
                        ),
                        "a"
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task First()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, Category>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(First)}";
            Expression<Func<IQueryable<Category>, Category>> newSelector = (Expression<Func<IQueryable<Category>, Category>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.First()");
            Assert.NotNull(result);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new FirstOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName)
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task FirstOrDefault_Filter_Returns_null()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, Category>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(FirstOrDefault_Filter_Returns_null)}";
            Expression<Func<IQueryable<Category>, Category>> newSelector = (Expression<Func<IQueryable<Category>, Category>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.FirstOrDefault(a => (a.CategoryID == -1))");
            Assert.Null(result);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new FirstOrDefaultOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        new EqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a")),
                            new ConstantOperatorParameters(-1)
                        ),
                        "a"
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task FirstOrDefault_Filter_Returns_match()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, Category>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(FirstOrDefault_Filter_Returns_match)}";
            Expression<Func<IQueryable<Category>, Category>> newSelector = (Expression<Func<IQueryable<Category>, Category>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.FirstOrDefault(a => (a.CategoryID == 1))");
            Assert.Equal(1, result.CategoryID);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new FirstOrDefaultOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        new EqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a")),
                            new ConstantOperatorParameters(1)
                        ),
                        "a"
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task FirstOrDefault()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, Category>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(FirstOrDefault)}";
            Expression<Func<IQueryable<Category>, Category>> newSelector = (Expression<Func<IQueryable<Category>, Category>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.FirstOrDefault()");
            Assert.NotNull(result);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new FirstOrDefaultOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName)
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task GroupBy()
        {
            //act
            var expression = CreateExpression<IQueryable<Product>, IQueryable<IGrouping<int, Product>>>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(GroupBy)}";
            Expression<Func<IQueryable<Product>, IQueryable<IGrouping<int, Product>>>> newSelector = (Expression<Func<IQueryable<Product>, IQueryable<IGrouping<int, Product>>>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetProducts());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.GroupBy(a => a.SupplierID)");
            Assert.Equal(1, result.Count());
            Assert.Equal(2, result.First().Count());
            Assert.Equal(3, result.First().First().SupplierID);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new GroupByOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        new MemberSelectorOperatorParameters("SupplierID", new ParameterOperatorParameters("a")),
                        "a"
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task Last_Filter_Throws_Exception()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, Category>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(Last_Filter_Throws_Exception)}";
            Expression<Func<IQueryable<Category>, Category>> newSelector = (Expression<Func<IQueryable<Category>, Category>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.Last(a => (a.CategoryID == -1))");
            Assert.Throws<InvalidOperationException>(() => RunExpression(newSelector, GetCategories()));

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new LastOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        new EqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a")),
                            new ConstantOperatorParameters(-1)
                        ),
                        "a"
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task Last_Filter_Returns_match()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, Category>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(Last_Filter_Returns_match)}";
            Expression<Func<IQueryable<Category>, Category>> newSelector = (Expression<Func<IQueryable<Category>, Category>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.Last(a => (a.CategoryID == 2))");
            Assert.Equal(2, result.CategoryID);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new LastOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        new EqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a")),
                            new ConstantOperatorParameters(2)
                        ),
                        "a"
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task Last()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, Category>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(Last)}";
            Expression<Func<IQueryable<Category>, Category>> newSelector = (Expression<Func<IQueryable<Category>, Category>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.Last()");
            Assert.NotNull(result);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new LastOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName)
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task LastOrDefault_Filter_Returns_null()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, Category>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(LastOrDefault_Filter_Returns_null)}";
            Expression<Func<IQueryable<Category>, Category>> newSelector = (Expression<Func<IQueryable<Category>, Category>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.LastOrDefault(a => (a.CategoryID == -1))");
            Assert.Null(result);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new LastOrDefaultOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        new EqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a")),
                            new ConstantOperatorParameters(-1)
                        ),
                        "a"
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task LastOrDefault_Filter_Returns_match()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, Category>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(LastOrDefault_Filter_Returns_match)}";
            Expression<Func<IQueryable<Category>, Category>> newSelector = (Expression<Func<IQueryable<Category>, Category>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.LastOrDefault(a => (a.CategoryID == 2))");
            Assert.Equal(2, result.CategoryID);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new LastOrDefaultOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        new EqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a")),
                            new ConstantOperatorParameters(2)
                        ),
                        "a"
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task LastOrDefault()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, Category>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(LastOrDefault)}";
            Expression<Func<IQueryable<Category>, Category>> newSelector = (Expression<Func<IQueryable<Category>, Category>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.LastOrDefault()");
            Assert.NotNull(result);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new LastOrDefaultOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName)
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task Max_Selector()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, int>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(Max_Selector)}";
            Expression<Func<IQueryable<Category>, int>> newSelector = (Expression<Func<IQueryable<Category>, int>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.Max(a => a.CategoryID)");
            Assert.Equal(2, result);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new MaxOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a")),
                        "a"
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task Max()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, int>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(Max)}";
            Expression<Func<IQueryable<Category>, int>> newSelector = (Expression<Func<IQueryable<Category>, int>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.Select(a => a.CategoryID).Max()");
            Assert.Equal(2, result);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new MaxOperatorParameters
                    (
                        new SelectOperatorParameters
                        (
                            new ParameterOperatorParameters(parameterName),
                            new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a")),
                            "a"
                        )
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task Min_Selector()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, int>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(Min_Selector)}";
            Expression<Func<IQueryable<Category>, int>> newSelector = (Expression<Func<IQueryable<Category>, int>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.Min(a => a.CategoryID)");
            Assert.Equal(1, result);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new MinOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a")),
                        "a"
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task Min()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, int>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(Min)}";
            Expression<Func<IQueryable<Category>, int>> newSelector = (Expression<Func<IQueryable<Category>, int>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.Select(a => a.CategoryID).Min()");
            Assert.Equal(1, result);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new MinOperatorParameters
                    (
                        new SelectOperatorParameters
                        (
                            new ParameterOperatorParameters(parameterName),
                            new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a")),
                            "a"
                        )
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task OrderBy()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, IOrderedQueryable<Category>>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(OrderBy)}";
            Expression<Func<IQueryable<Category>, IOrderedQueryable<Category>>> newSelector = (Expression<Func<IQueryable<Category>, IOrderedQueryable<Category>>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.OrderBy(a => a.CategoryID)");
            Assert.Equal(1, result.First().CategoryID);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new OrderByOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a")),
                        ListSortDirection.Ascending,
                        "a"
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task OrderByDescending()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, IOrderedQueryable<Category>>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(OrderByDescending)}";
            Expression<Func<IQueryable<Category>, IOrderedQueryable<Category>>> newSelector = (Expression<Func<IQueryable<Category>, IOrderedQueryable<Category>>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.OrderByDescending(a => a.CategoryID)");
            Assert.Equal(2, result.First().CategoryID);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new OrderByOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a")),
                        ListSortDirection.Descending,
                        "a"
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task OrderByThenBy()
        {
            //act
            var expression = CreateExpression<IQueryable<Product>, IOrderedQueryable<Product>>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(OrderByThenBy)}";
            Expression<Func<IQueryable<Product>, IOrderedQueryable<Product>>> newSelector = (Expression<Func<IQueryable<Product>, IOrderedQueryable<Product>>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Product), null);
            var result = RunExpression(newSelector, GetProducts());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.OrderBy(a => a.SupplierID).ThenBy(a => a.ProductID)");
            Assert.Equal(1, result.First().ProductID);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new ThenByOperatorParameters
                    (
                        new OrderByOperatorParameters
                        (
                            new ParameterOperatorParameters(parameterName),
                            new MemberSelectorOperatorParameters("SupplierID", new ParameterOperatorParameters("a")),
                            ListSortDirection.Ascending,
                            "a"
                        ),
                        new MemberSelectorOperatorParameters("ProductID", new ParameterOperatorParameters("a")),
                        ListSortDirection.Ascending,
                        "a"
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task OrderByThenByDescending()
        {
            //act
            var expression = CreateExpression<IQueryable<Product>, IOrderedQueryable<Product>>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(OrderByThenByDescending)}";
            Expression<Func<IQueryable<Product>, IOrderedQueryable<Product>>> newSelector = (Expression<Func<IQueryable<Product>, IOrderedQueryable<Product>>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Product), null);
            var result = RunExpression(newSelector, GetProducts());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.OrderBy(a => a.SupplierID).ThenByDescending(a => a.ProductID)");
            Assert.Equal(2, result.First().ProductID);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new ThenByOperatorParameters
                    (
                        new OrderByOperatorParameters
                        (
                            new ParameterOperatorParameters(parameterName),
                            new MemberSelectorOperatorParameters("SupplierID", new ParameterOperatorParameters("a")),
                            ListSortDirection.Ascending,
                            "a"
                        ),
                        new MemberSelectorOperatorParameters("ProductID", new ParameterOperatorParameters("a")),
                        ListSortDirection.Descending,
                        "a"
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task Paging()
        {
            //act
            var expression = CreateExpression<IQueryable<Product>, IQueryable<Address>>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(Paging)}";
            Expression<Func<IQueryable<Product>, IQueryable<Address>>> newSelector = (Expression<Func<IQueryable<Product>, IQueryable<Address>>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Product), null);
            var result = RunExpression(newSelector, GetProducts());

            //assert
            AssertFilterStringIsCorrect
            (
                newSelector,
                "$it => $it.SelectMany(a => a.AlternateAddresses).OrderBy(a => a.State).ThenBy(a => a.AddressID).Skip(1).Take(2)"
            );
            Assert.Equal(2, result.Count());
            Assert.Equal(4, result.First().AddressID);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new TakeOperatorParameters
                    (
                        new SkipOperatorParameters
                        (
                            new ThenByOperatorParameters
                            (
                                new OrderByOperatorParameters
                                (
                                    new SelectManyOperatorParameters
                                    (
                                        new ParameterOperatorParameters(parameterName),
                                        new MemberSelectorOperatorParameters("AlternateAddresses", new ParameterOperatorParameters("a")),
                                        "a"
                                    ),
                                    new MemberSelectorOperatorParameters("State", new ParameterOperatorParameters("a")),
                                    ListSortDirection.Ascending,
                                    "a"
                                ),
                                new MemberSelectorOperatorParameters("AddressID", new ParameterOperatorParameters("a")),
                                ListSortDirection.Ascending,
                                "a"
                            ),
                            1
                        ),
                        2
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task Select_New()
        {
            var expression = CreateExpression<IQueryable<Category>, IQueryable<dynamic>>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(Select_New)}";
            Expression<Func<IQueryable<Category>, IQueryable<dynamic>>> newSelector = (Expression<Func<IQueryable<Category>, IQueryable<dynamic>>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            Assert.Equal(2, result.First().CategoryID);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new SelectOperatorParameters
                    (
                        new OrderByOperatorParameters
                        (
                            new ParameterOperatorParameters(parameterName),
                            new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a")),
                            ListSortDirection.Descending,
                            "a"
                        ),
                        new MemberInitOperatorParameters
                        (
                            new MemberBindingItem[]
                            {
                                new ("CategoryID", new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a"))),
                                new ("CategoryName", new MemberSelectorOperatorParameters("CategoryName", new ParameterOperatorParameters("a"))),
                                new ("Products", new MemberSelectorOperatorParameters("Products", new ParameterOperatorParameters("a")))
                            }
                        ),
                        "a"
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task SelectMany()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, IQueryable<Product>>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(SelectMany)}";
            Expression<Func<IQueryable<Category>, IQueryable<Product>>> newSelector = (Expression<Func<IQueryable<Category>, IQueryable<Product>>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.SelectMany(a => a.Products)");
            Assert.Equal(3, result.Count());

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new SelectManyOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        new MemberSelectorOperatorParameters("Products", new ParameterOperatorParameters("a")),
                        "a"
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task Single_Filter_Throws_Exception()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, Category>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(Single_Filter_Throws_Exception)}";
            Expression<Func<IQueryable<Category>, Category>> newSelector = (Expression<Func<IQueryable<Category>, Category>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.Single(a => (a.CategoryID == -1))");
            Assert.Throws<InvalidOperationException>(() => RunExpression(newSelector, GetCategories()));

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new SingleOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        new EqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a")),
                            new ConstantOperatorParameters(-1)
                        ),
                        "a"
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task Single_Filter_Returns_match()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, Category>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(Single_Filter_Returns_match)}";
            Expression<Func<IQueryable<Category>, Category>> newSelector = (Expression<Func<IQueryable<Category>, Category>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.Single(a => (a.CategoryID == 1))");
            Assert.Equal(1, result.CategoryID);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new SingleOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        new EqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a")),
                            new ConstantOperatorParameters(1)
                        ),
                        "a"
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task Single_with_multiple_matches_Throws_Exception()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, Category>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(Single_with_multiple_matches_Throws_Exception)}";
            Expression<Func<IQueryable<Category>, Category>> newSelector = (Expression<Func<IQueryable<Category>, Category>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.Single()");
            Assert.Throws<InvalidOperationException>(() => RunExpression(newSelector, GetCategories()));

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new SingleOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName)
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task Sum_Selector()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, int>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(Sum_Selector)}";
            Expression<Func<IQueryable<Category>, int>> newSelector = (Expression<Func<IQueryable<Category>, int>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.Sum(a => a.CategoryID)");
            Assert.Equal(3, result);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new SumOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a")),
                        "a"
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task Sum()
        {
            //act
            var expression = CreateExpression<IQueryable<Category>, int>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(Sum)}";
            Expression<Func<IQueryable<Category>, int>> newSelector = (Expression<Func<IQueryable<Category>, int>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            //assert
            AssertFilterStringIsCorrect(newSelector, "$it => $it.Select(a => a.CategoryID).Sum()");
            Assert.Equal(3, result);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new SumOperatorParameters
                    (
                        new SelectOperatorParameters
                        (
                            new ParameterOperatorParameters(parameterName),
                            new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a")),
                            "a"
                        )
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task ToList()
        {
            var expression = CreateExpression<IQueryable<Category>, List<Category>>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(ToList)}";
            Expression<Func<IQueryable<Category>, List<Category>>> newSelector = (Expression<Func<IQueryable<Category>, List<Category>>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            Assert.Equal(2, result.Count);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new ToListOperatorParameters
                    (
                       new ParameterOperatorParameters(parameterName)
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task Where_with_matches()
        {
            var expression = CreateExpression<IQueryable<Category>, IQueryable<Category>>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(Where_with_matches)}";
            Expression<Func<IQueryable<Category>, IQueryable<Category>>> newSelector = (Expression<Func<IQueryable<Category>, IQueryable<Category>>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            Assert.Equal(2, result.First().CategoryID);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new WhereOperatorParameters
                    (
                        new OrderByOperatorParameters
                        (
                            new ParameterOperatorParameters(parameterName),
                            new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a")),
                            ListSortDirection.Descending,
                            "a"
                        ),
                        new NotEqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a")),
                            new ConstantOperatorParameters(1)
                        ),
                        "a"
                    ),
                    parameterName
                );
        }

        [Fact]
        public async Task Where_without_matches()
        {
            var expression = CreateExpression<IQueryable<Category>, IQueryable<Category>>();
            string ruleName = $"{nameof(QueryableParameterExpressionTests)}_{nameof(Where_without_matches)}";
            Expression<Func<IQueryable<Category>, IQueryable<Category>>> newSelector = (Expression<Func<IQueryable<Category>, IQueryable<Category>>>)await RecreateSelectorFromSelectorLambdaOperatorParameters(expression, ruleName, typeof(Category), null);
            var result = RunExpression(newSelector, GetCategories());

            Assert.Empty(result);

            static Expression<Func<T, TReturn>> CreateExpression<T, TReturn>()
                => GetExpression<T, TReturn>
                (
                    new WhereOperatorParameters
                    (
                        new ParameterOperatorParameters(parameterName),
                        new EqualsBinaryOperatorParameters
                        (
                            new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a")),
                            new ConstantOperatorParameters(-1)
                        ),
                        "a"
                    ),
                    parameterName
                );
        }
        #endregion Tests

        #region Fields
        private static MapperConfiguration MapperConfiguration;
        private static readonly string parameterName = "$it";
        private static IServiceProvider serviceProvider;
        private const string mduleName = "queryable-parameter-expression-tests";

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

        private static Expression<Func<T, TResult>> GetExpression<T, TResult>(IExpressionParameter filterBody, string defaultParameterName = "$it")
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
                opts => opts.Items["parameters"] = new Dictionary<string, ParameterExpression>()
            ).Build();
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
        #endregion Helpers

        private static IQueryable<Category> GetCategories()
         => new Category[]
            {
                new()
                {
                    CategoryID = 1,
                    CategoryName = "CategoryOne",
                    Products =
                    [
                        new Product
                        {
                            ProductID = 1,
                            ProductName = "ProductOne",
                            AlternateAddresses =
                            [
                                new Address { AddressID = 1, City = "CityOne" },
                                new Address { AddressID = 2, City = "CityTwo"  },
                            ]
                        },
                        new Product
                        {
                            ProductID = 2,
                            ProductName = "ProductTwo",
                            AlternateAddresses =
                            [
                                new Address { AddressID = 3, City = "CityThree" },
                                new Address { AddressID = 4, City = "CityFour"  },
                            ]
                        }
                    ]
                },
                new()
                {
                    CategoryID = 2,
                    CategoryName = "CategoryTwo",
                    Products =
                    [
                        new Product
                        {
                            AlternateAddresses = []
                        }
                    ]
                }
            }.AsQueryable();

        private static IQueryable<Product> GetProducts()
         => new Product[]
         {
             new()
             {
                 ProductID = 1,
                 ProductName = "ProductOne",
                 SupplierID = 3,
                 AlternateAddresses =
                 [
                     new Address { AddressID = 1, City = "CityOne", State = "OH" },
                     new Address { AddressID = 2, City = "CityTwo", State = "MI"   },
                 ]
             },
             new()
             {
                 ProductID = 2,
                 ProductName = "ProductTwo",
                 SupplierID = 3,
                 AlternateAddresses =
                 [
                     new Address { AddressID = 3, City = "CityThree", State = "OH"  },
                     new Address { AddressID = 4, City = "CityFour", State = "MI"   },
                 ]
             }
         }.AsQueryable();
    }
}
