using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.CodeDom;
using System.Collections.Generic;
using Telerik.WinControls.UI;
using TelerikLogicBuilder.Tests.Constants;
using TelerikLogicBuilder.Tests.Mocks;
using Xunit;

namespace TelerikLogicBuilder.Tests.TreeViewBuiilders
{
    public  class SelectFunctionTreeViewBuilderTest : IClassFixture<SelectFunctionTreeViewBuilderFixture>
    {
        private readonly SelectFunctionTreeViewBuilderFixture _fixture;

        public SelectFunctionTreeViewBuilderTest(SelectFunctionTreeViewBuilderFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CreateSelectFunctionTreeViewBuilderThrows()
        {
            //assert
            Assert.Throws<InvalidOperationException>(_fixture.ServiceProvider.GetRequiredService<ISelectFunctionTreeViewBuilder>);
        }
        [Fact]
        public void CanBuildTreeView()
        {
            //arrange
            ITreeViewBuilderFactory factory = _fixture.ServiceProvider.GetRequiredService<ITreeViewBuilderFactory>();
            RadTreeView radTreeView = new();
            Dictionary<string, string> expandedNodes = new();

            //act
            factory.GetSelectFunctionTreeViewBuilder
            (
                new SelectFunctionControlMock
                (
                    expandedNodes,
                    new List<TreeFolder>
                    {
                        _fixture.ConfigurationService.FunctionList.BuiltInBooleanFunctionsTreeFolder,
                        _fixture.ConfigurationService.FunctionList.BooleanFunctionsTreeFolder
                    }
                )
            ).Build(radTreeView);

            //assert
            Assert.NotNull(radTreeView.Nodes[0]);
            Assert.NotNull(radTreeView.Nodes[1]);
            Assert.Equal(3, radTreeView.Nodes[0].Nodes.Count);
            Assert.Single(radTreeView.Nodes[1].Nodes);
        }

    }

    public class SelectFunctionTreeViewBuilderFixture : IDisposable
    {
        internal IServiceProvider ServiceProvider;
        internal IProjectPropertiesItemFactory ProjectPropertiesItemFactory;
        internal IWebApiDeploymentItemFactory WebApiDeploymentItemFactory;
        internal IConfigurationService ConfigurationService;
        internal IConstructorFactory ConstructorFactory;
        internal IFunctionFactory FunctionFactory;
        internal IReturnTypeFactory ReturnTypeFactory;
        internal IParameterFactory ParameterFactory;

        public SelectFunctionTreeViewBuilderFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ProjectPropertiesItemFactory = ServiceProvider.GetRequiredService<IProjectPropertiesItemFactory>();
            WebApiDeploymentItemFactory = ServiceProvider.GetRequiredService<IWebApiDeploymentItemFactory>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            ConstructorFactory = ServiceProvider.GetRequiredService<IConstructorFactory>();
            FunctionFactory = ServiceProvider.GetRequiredService<IFunctionFactory>();
            ParameterFactory = ServiceProvider.GetRequiredService<IParameterFactory>();
            ReturnTypeFactory = ServiceProvider.GetRequiredService<IReturnTypeFactory>();
            ConfigurationService.ProjectProperties = ProjectPropertiesItemFactory.GetProjectProperties
            (
                "Contoso",
                $@"{TestFolders.TestAssembliesFolder}\FlowProjects\Contoso.Test",
                new Dictionary<string, Application>
                {
                    ["app01"] = ProjectPropertiesItemFactory.GetApplication
                    (
                        "App01",
                        "App01",
                        "Contoso.Test.Flow.dll",
                        $@"{TestFolders.TestAssembliesFolder}\Contoso.Test.Flow\bin\Debug\netstandard2.0",
                        RuntimeType.NetCore,
                        new List<string>(),
                        "Contoso.Test.Flow.FlowActivity",
                        "",
                        "",
                        new List<string>(),
                        "",
                        "",
                        "",
                        "",
                        new List<string>(),
                        WebApiDeploymentItemFactory.GetWebApiDeployment("", "", "", "")
                    ),
                    ["app02"] = ProjectPropertiesItemFactory.GetApplication
                    (
                        "App02",
                        "App02",
                        "Contoso.Test.Flow.dll",
                        $@"{TestFolders.TestAssembliesFolder}\Contoso.Test.Flow\bin\Debug\netstandard2.0",
                        RuntimeType.NetCore,
                        new List<string>(),
                        "Contoso.Test.Flow.FlowActivity",
                        "",
                        "",
                        new List<string>(),
                        "",
                        "",
                        "",
                        "",
                        new List<string>(),
                        WebApiDeploymentItemFactory.GetWebApiDeployment("", "", "", "")
                    )
                },
                new HashSet<string>()
            );

            ConfigurationService.ConstructorList = new ConstructorList
            (
                new Dictionary<string, Constructor>
                {
                    ["TestResponseA"] = ConstructorFactory.GetConstructor
                    (
                        "TestResponseA",
                        "Contoso.Test.Business.Responses.TestResponseA",
                        new List<ParameterBase>
                        {
                            ParameterFactory.GetLiteralParameter
                            (
                                "stringProperty",
                                false,
                                "",
                                LiteralParameterType.String,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                true,
                                "",
                                "",
                                "",
                                new List<string>()
                            )
                        },
                        new List<string>(),
                        ""
                    ),
                    ["Object"] = ConstructorFactory.GetConstructor
                    (
                        "Object",
                        "System.Object",
                        new List<ParameterBase>
                        {
                        },
                        new List<string>(),
                        ""
                    ),
                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>())
            );

            foreach (string key in ConfigurationService.ConstructorList.Constructors.Keys)
                ConfigurationService.ConstructorList.ConstructorsTreeFolder.FileNames.Add(key);

            ConfigurationService.FunctionList = new FunctionList
            (
                new Dictionary<string, Function>
                {
                    ["Equals"] = FunctionFactory.GetFunction
                    (
                        "Equals",
                        Enum.GetName(typeof(CodeBinaryOperatorType), CodeBinaryOperatorType.ValueEquality)!,
                        FunctionCategories.BinaryOperator,
                        "",
                        "",
                        "",
                        "",
                        ReferenceCategories.None,
                        ParametersLayout.Binary,
                        new List<ParameterBase>()
                        {
                            ParameterFactory.GetLiteralParameter
                            (
                                "value1",
                                false,
                                "",
                                LiteralParameterType.Any,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                false,
                                "",
                                "",
                                "",
                                new List<string>()
                            ),
                            ParameterFactory.GetLiteralParameter
                            (
                                "value2",
                                false,
                                "",
                                LiteralParameterType.Any,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                false,
                                "",
                                "",
                                "",
                                new List<string>()
                            )
                        },
                        new List<string>(),
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Boolean),
                        ""
                    ),
                    ["Greater Than"] = FunctionFactory.GetFunction
                    (
                        "Greater Than",
                        Enum.GetName(typeof(CodeBinaryOperatorType), CodeBinaryOperatorType.GreaterThan)!,
                        FunctionCategories.BinaryOperator,
                        "",
                        "",
                        "",
                        "",
                        ReferenceCategories.None,
                        ParametersLayout.Binary,
                        new List<ParameterBase>()
                        {
                            ParameterFactory.GetLiteralParameter
                            (
                                "value1",
                                false,
                                "",
                                LiteralParameterType.Any,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                false,
                                "",
                                "",
                                "",
                                new List<string>()
                            ),
                            ParameterFactory.GetLiteralParameter
                            (
                                "value2",
                                false,
                                "",
                                LiteralParameterType.Any,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                false,
                                "",
                                "",
                                "",
                                new List<string>()
                            )
                        },
                        new List<string>(),
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Boolean),
                        ""
                    ),
                    ["Less Than"] = FunctionFactory.GetFunction
                    (
                        "Less Than",
                        Enum.GetName(typeof(CodeBinaryOperatorType), CodeBinaryOperatorType.LessThan)!,
                        FunctionCategories.BinaryOperator,
                        "",
                        "",
                        "",
                        "",
                        ReferenceCategories.None,
                        ParametersLayout.Binary,
                        new List<ParameterBase>()
                        {
                            ParameterFactory.GetLiteralParameter
                            (
                                "value1",
                                false,
                                "",
                                LiteralParameterType.Any,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                false,
                                "",
                                "",
                                "",
                                new List<string>()
                            ),
                            ParameterFactory.GetLiteralParameter
                            (
                                "value2",
                                false,
                                "",
                                LiteralParameterType.Any,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                false,
                                "",
                                "",
                                "",
                                new List<string>()
                            )
                        },
                        new List<string>(),
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Boolean),
                        ""
                    ),
                    ["StaticMethod"] = FunctionFactory.GetFunction
                    (
                        "StaticMethod",
                        "StaticMethod",
                        FunctionCategories.Standard,
                        "Contoso.Test.Business.StaticGenericClass`2",
                        "",
                        "",
                        "",
                        ReferenceCategories.Type,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>
                        {
                            ParameterFactory.GetGenericParameter
                            (
                                "aProperty",
                                false,
                                "",
                                "A"
                            ),
                            ParameterFactory.GetGenericParameter
                            (
                                "bProperty",
                                false,
                                "",
                                "B"
                            )
                        },
                        new List<string> { "A", "B" },
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Boolean),
                        ""
                    ),
                    ["Set To Null"] = FunctionFactory.GetFunction
                    (
                        "Set To Null",
                        "assert",
                        FunctionCategories.Retract,
                        "",
                        "",
                        "",
                        "",
                        ReferenceCategories.None,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>(),
                        new List<string>(),
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Void),
                        ""
                    ),
                    ["Set Variable"] = FunctionFactory.GetFunction
                    (
                        "Set Variable",
                        "assert",
                        FunctionCategories.Assert,
                        "",
                        "",
                        "",
                        "",
                        ReferenceCategories.None,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>(),
                        new List<string>(),
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Void),
                        ""
                    ),
                },
                new TreeFolder("Functions", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("Functions", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("Functions", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("Functions", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("Functions", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("Built In Functions", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("Built In Functions", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("Built In Functions", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("Built In Functions", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("Built In Functions", new List<string>(), new List<TreeFolder>())
            );

            ConfigurationService.FunctionList.BuiltInBooleanFunctionsTreeFolder.FileNames.Add("Equals");
            ConfigurationService.FunctionList.BuiltInBooleanFunctionsTreeFolder.FileNames.Add("Greater Than");
            ConfigurationService.FunctionList.BuiltInBooleanFunctionsTreeFolder.FileNames.Add("Less Than");
            ConfigurationService.FunctionList.BooleanFunctionsTreeFolder.FileNames.Add("StaticMethod");
            ConfigurationService.FunctionList.BuiltInVoidFunctionsTreeFolder.FileNames.Add("Set To Null");
            ConfigurationService.FunctionList.BuiltInVoidFunctionsTreeFolder.FileNames.Add("Set Variable");
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
