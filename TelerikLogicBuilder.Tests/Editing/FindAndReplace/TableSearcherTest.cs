using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Application = ABIS.LogicBuilder.FlowBuilder.Configuration.Application;

namespace TelerikLogicBuilder.Tests.Editing.FindAndReplace
{
    public class TableSearcherTest : IClassFixture<TableSearcherFixture>
    {
        private readonly TableSearcherFixture _fixture;

        public TableSearcherTest(TableSearcherFixture fixture)
        {
            _fixture = fixture;
        }

        const string FileNameNoExtention = "Module";

        [Fact]
        public void CreateTableSearcherThrows()
        {
            //assert
            Assert.Throws<InvalidOperationException>(() => _fixture.ServiceProvider.GetRequiredService<ITableSearcher>());
        }

        [Theory]
        [InlineData("Strong", true, true, true)]
        [InlineData("Stron", true, true, false)]
        [InlineData("Stron", true, false, true)]
        [InlineData("STRON", true, false, false)]
        [InlineData("STRONG", false, true, true)]
        [InlineData("STRONGD", false, true, false)]
        public async Task TextSearchReturnsTheExpectedResults(string searchString, bool matchCase, bool matchWholeWord, bool found)
        {
            //arrange
            ISearcherFactory factory = _fixture.ServiceProvider.GetRequiredService<ISearcherFactory>();
            ISearchFunctions searchFunctions = _fixture.ServiceProvider.GetRequiredService<ISearchFunctions>();
            string sourceFile = GetFullSourceFilePath(FileNameNoExtention);
            DataSet dataSet = GetDataSet(sourceFile);
            var progress = new Progress<ProgressMessage>(percent =>
            {
            });
            var cancellationToken = new CancellationTokenSource();

            //act
            var result = await factory.GetTableSearcher
            (
                sourceFile,
                dataSet,
                searchString,
                matchCase,
                matchWholeWord,
                searchFunctions.FindTextMatches,
                progress,
                cancellationToken
            ).Search();

            dataSet.Dispose();

            //assert
            Assert.Equal
            (
                found,
                result.CellCount > 0
            );
        }

        [Theory]
        [InlineData("CommandButtonParameters", true, true, true)]
        [InlineData("CommandButtonParameter", true, true, false)]
        [InlineData("CommandButtonParamet", true, false, true)]
        [InlineData("COMMANDBUTTONPARAMET", true, false, false)]
        [InlineData("COMMANDBUTTONPARAMETERS", false, true, true)]
        [InlineData("COMMANDBUTTONPARAMETERSs", false, true, false)]
        public async Task ConstructorSearchReturnsTheExpectedResults(string searchString, bool matchCase, bool matchWholeWord, bool found)
        {
            //arrange
            ISearcherFactory factory = _fixture.ServiceProvider.GetRequiredService<ISearcherFactory>();
            ISearchFunctions searchFunctions = _fixture.ServiceProvider.GetRequiredService<ISearchFunctions>();
            string sourceFile = GetFullSourceFilePath(FileNameNoExtention);
            DataSet dataSet = GetDataSet(sourceFile);
            var progress = new Progress<ProgressMessage>(percent =>
            {
            });
            var cancellationToken = new CancellationTokenSource();

            //act
            var result = await factory.GetTableSearcher
            (
                sourceFile,
                dataSet,
                searchString,
                matchCase,
                matchWholeWord,
                searchFunctions.FindConstructorMatches,
                progress,
                cancellationToken
            ).Search();

            dataSet.Dispose();

            //assert
            Assert.Equal
            (
                found,
                result.CellCount > 0
            );
        }

        [Theory]
        [InlineData("Set Variable", true, true, true)]
        [InlineData("Set Variabl", true, true, false)]
        [InlineData("Set Variabl", true, false, true)]
        [InlineData("SET VARIABL", true, false, false)]
        [InlineData("SET VARIABLE", false, true, true)]
        [InlineData("SET VARIABLEs", false, true, false)]
        public async Task FunctionSearchReturnsTheExpectedResults(string searchString, bool matchCase, bool matchWholeWord, bool found)
        {
            //arrange
            ISearcherFactory factory = _fixture.ServiceProvider.GetRequiredService<ISearcherFactory>();
            ISearchFunctions searchFunctions = _fixture.ServiceProvider.GetRequiredService<ISearchFunctions>();
            string sourceFile = GetFullSourceFilePath(FileNameNoExtention);
            DataSet dataSet = GetDataSet(sourceFile);
            var progress = new Progress<ProgressMessage>(percent =>
            {
            });
            var cancellationToken = new CancellationTokenSource();

            //act
            var result = await factory.GetTableSearcher
            (
                sourceFile,
                dataSet,
                searchString,
                matchCase,
                matchWholeWord,
                searchFunctions.FindFunctionMatches,
                progress,
                cancellationToken
            ).Search();

            dataSet.Dispose();

            //assert
            Assert.Equal
            (
                found,
                result.CellCount > 0
            );
        }

        [Theory]
        [InlineData("SomeObject", true, true, true)]
        [InlineData("SomeObjec", true, true, false)]
        [InlineData("SomeObjec", true, false, true)]
        [InlineData("SOMEOBJEC", true, false, false)]
        [InlineData("SOMEOBJECT", false, true, true)]
        [InlineData("SOMEOBJECTs", false, true, false)]
        public async Task VariableSearchReturnsTheExpectedResults(string searchString, bool matchCase, bool matchWholeWord, bool found)
        {
            //arrange
            ISearcherFactory factory = _fixture.ServiceProvider.GetRequiredService<ISearcherFactory>();
            ISearchFunctions searchFunctions = _fixture.ServiceProvider.GetRequiredService<ISearchFunctions>();
            string sourceFile = GetFullSourceFilePath(FileNameNoExtention);
            DataSet dataSet = GetDataSet(sourceFile);
            var progress = new Progress<ProgressMessage>(percent =>
            {
            });
            var cancellationToken = new CancellationTokenSource();

            //act
            var result = await factory.GetTableSearcher
            (
                sourceFile,
                dataSet,
                searchString,
                matchCase,
                matchWholeWord,
                searchFunctions.FindVariableMatches,
                progress,
                cancellationToken
            ).Search();

            dataSet.Dispose();

            //assert
            Assert.Equal
            (
                found,
                result.CellCount > 0
            );
        }

        private static DataSet GetDataSet(string sourceFullPath)
        {
            DataSet dataSet = new()
            {
                Locale = CultureInfo.InvariantCulture
            };

            using (StringReader stringReader = new(Schemas.GetSchema(Schemas.TableSchema)))
            {
                dataSet.ReadXmlSchema(stringReader);
                stringReader.Close();
            }

            using FileStream fileStream = new(sourceFullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            dataSet.ReadXml(fileStream);
            return dataSet;
        }

        private static string GetFullSourceFilePath(string fileNameNoExtension)
            => System.IO.Path.Combine(Directory.GetCurrentDirectory(), @$"Diagrams\{nameof(TableSearcherTest)}\{fileNameNoExtension}.tbl");
    }

    public class TableSearcherFixture : IDisposable
    {
        internal IServiceProvider ServiceProvider;
        internal IProjectPropertiesItemFactory ProjectPropertiesItemFactory;
		internal IWebApiDeploymentItemFactory WebApiDeploymentItemFactory;
        internal IConfigurationService ConfigurationService;
        internal IFunctionFactory FunctionFactory;
        internal IParameterFactory ParameterFactory;
        internal IReturnTypeFactory ReturnTypeFactory;

        public TableSearcherFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ProjectPropertiesItemFactory = ServiceProvider.GetRequiredService<IProjectPropertiesItemFactory>();
			WebApiDeploymentItemFactory = ServiceProvider.GetRequiredService<IWebApiDeploymentItemFactory>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            FunctionFactory = ServiceProvider.GetRequiredService<IFunctionFactory>();
            ParameterFactory = ServiceProvider.GetRequiredService<IParameterFactory>();
            ReturnTypeFactory = ServiceProvider.GetRequiredService<IReturnTypeFactory>();
            ConfigurationService.ProjectProperties = ProjectPropertiesItemFactory.GetProjectProperties
            (
                "Contoso",
                @"C:\ProjectPath",
                new Dictionary<string, Application>
                {
                    ["app01"] = ProjectPropertiesItemFactory.GetApplication
                    (
                        "App01",
                        "App01",
                        "Contoso.Test.Flow.dll",
                        $@"NotImportant",
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
                        $@"NotImportant",
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

            ConfigurationService.FunctionList = new FunctionList
            (
                new Dictionary<string, Function>
                {
                    ["ClearErrorMessages"] = FunctionFactory.GetFunction
                    (
                        "ClearErrorMessages",
                        "Clear",
                        FunctionCategories.Standard,
                        "",
                        "flowManager.FlowDataCache.Response.ErrorMessages",
                        "Field.Property.Property.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>()
                        {
                        },
                        new List<string> { },
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Void),
                        ""
                    ),
                    ["DisplayString"] = FunctionFactory.GetFunction
                    (
                        "DisplayString",
                        "DisplayString",
                        FunctionCategories.DialogForm,
                        "",
                        "flowManager.CustomDialogs",
                        "Field.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>()
                        {
                            ParameterFactory.GetLiteralParameter
                            (
                                "setting",
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
                        new List<string> { },
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Boolean),
                        ""
                    ),
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
                    ["WriteToLog"] = FunctionFactory.GetFunction
                    (
                        "WriteToLog",
                        "WriteToLog",
                        FunctionCategories.Standard,
                        "",
                        "flowManager.CustomActions",
                        "Field.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>()
                        {
                            ParameterFactory.GetLiteralParameter
                            (
                                "message",
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
                        new List<string> { },
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Boolean),
                        ""
                    )
                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>())
            );

        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
