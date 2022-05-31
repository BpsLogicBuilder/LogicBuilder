using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Application = ABIS.LogicBuilder.FlowBuilder.Configuration.Application;

namespace TelerikLogicBuilder.IntegrationTests.RulesGenerator
{
    public class TableValidatorTest : IClassFixture<TableValidatorFixture>
    {
        private readonly TableValidatorFixture _fixture;

        public TableValidatorTest(TableValidatorFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateTableValidator()
        {
            //arrange
            ITableValidator validator = _fixture.ServiceProvider.GetRequiredService<ITableValidator>();

            //assert
            Assert.NotNull(validator);
        }

        [Fact]
        public async Task TableValidationSucceeds()
        {
            //arrange
            ITableValidator validator = _fixture.ServiceProvider.GetRequiredService<ITableValidator>();
            string sourceFile = GetFullSourceFilePath(nameof(TableValidationSucceeds));
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            DataSet dataSet = GetDataSet(sourceFile);
            var progress = new Progress<ProgressMessage>(percent =>
            {
            });
            var cancellationToken = new CancellationTokenSource();

            //act
            IList<ResultMessage> errors = await validator.Validate
            (
                sourceFile,
                dataSet,
                applicationTypeInfo,
                progress,
                cancellationToken
            );

            dataSet.Dispose();

            //assert
            Assert.Empty(errors);
        }

        [Fact]
        public async Task FailsValidationForDialogFunctionInActionCell()
        {
            //arrange
            ITableValidator validator = _fixture.ServiceProvider.GetRequiredService<ITableValidator>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForDialogFunctionInActionCell));
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            DataSet dataSet = GetDataSet(sourceFile);
            var progress = new Progress<ProgressMessage>(percent =>
            {
            });
            var cancellationToken = new CancellationTokenSource();

            //act
            IList<ResultMessage> errors = await validator.Validate
            (
                sourceFile,
                dataSet,
                applicationTypeInfo,
                progress,
                cancellationToken
            );

            dataSet.Dispose();

            //assert
            Assert.Equal
            (
                Strings.actionCellDialogFunctionsInvalid, 
                errors.First().Message
            );
        }

        [Fact]
        public async Task FailsValidationForNoDataInActionCell()
        {
            //arrange
            ITableValidator validator = _fixture.ServiceProvider.GetRequiredService<ITableValidator>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForNoDataInActionCell));
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            DataSet dataSet = GetDataSet(sourceFile);
            var progress = new Progress<ProgressMessage>(percent =>
            {
            });
            var cancellationToken = new CancellationTokenSource();

            //act
            IList<ResultMessage> errors = await validator.Validate
            (
                sourceFile,
                dataSet,
                applicationTypeInfo,
                progress,
                cancellationToken
            );

            dataSet.Dispose();

            //assert
            Assert.Equal
            (
                Strings.cellDataRequired,
                errors.First().Message
            );
        }

        [Fact]
        public async Task FailsValidationForNoDataInPriorityCell()
        {
            //arrange
            ITableValidator validator = _fixture.ServiceProvider.GetRequiredService<ITableValidator>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForNoDataInPriorityCell));
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            DataSet dataSet = GetDataSet(sourceFile);
            var progress = new Progress<ProgressMessage>(percent =>
            {
            });
            var cancellationToken = new CancellationTokenSource();

            //act
            IList<ResultMessage> errors = await validator.Validate
            (
                sourceFile,
                dataSet,
                applicationTypeInfo,
                progress,
                cancellationToken
            );

            dataSet.Dispose();

            //assert
            Assert.Equal
            (
                Strings.invalidPriorityCellData,
                errors.First().Message
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
            => System.IO.Path.Combine(Directory.GetCurrentDirectory(), @$"Tables\{nameof(TableValidatorTest)}\{fileNameNoExtension}.tbl");
    }

    public class TableValidatorFixture : IDisposable
    {
        public TableValidatorFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ContextProvider = ServiceProvider.GetRequiredService<IContextProvider>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            LoadContextSponsor = ServiceProvider.GetRequiredService<ILoadContextSponsor>();
            ApplicationTypeInfoManager = ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>();
            AssemblyLoadContextService = ServiceProvider.GetRequiredService<IAssemblyLoadContextManager>();
            ConfigurationService.ProjectProperties = new ProjectProperties
            (
                "Contoso",
                @"C:\ProjectPath",
                new Dictionary<string, Application>
                {
                    ["app01"] = new Application
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
                        new WebApiDeployment("", "", "", "", ContextProvider),
                        ContextProvider
                    ),
                    ["app02"] = new Application
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
                        new WebApiDeployment("", "", "", "", ContextProvider),
                        ContextProvider
                    )
                },
                new HashSet<string>(),
                ContextProvider
            );

            ConfigurationService.FunctionList = new FunctionList
            (
                new Dictionary<string, Function>
                {
                    ["ClearErrorMessages"] = new Function
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
                        new LiteralReturnType(LiteralFunctionReturnType.Void, ContextProvider),
                        "",
                        ContextProvider
                    ),
                    ["DisplayString"] = new Function
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
                            new LiteralParameter
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
                                new List<string>(),
                                ContextProvider
                             )
                        },
                        new List<string> { },
                        new LiteralReturnType(LiteralFunctionReturnType.Boolean, ContextProvider),
                        "",
                        ContextProvider
                    ),
                    ["Equals"] = new Function
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
                            new LiteralParameter
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
                                new List<string>(),
                                ContextProvider
                            ),
                            new LiteralParameter
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
                                new List<string>(),
                                ContextProvider
                            )
                        },
                        new List<string>(),
                        new LiteralReturnType(LiteralFunctionReturnType.Boolean, ContextProvider),
                        "",
                        ContextProvider
                    ),
                    ["WriteToLog"] = new Function
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
                            new LiteralParameter
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
                                new List<string>(),
                                ContextProvider
                             )
                        },
                        new List<string> { },
                        new LiteralReturnType(LiteralFunctionReturnType.Boolean, ContextProvider),
                        "",
                        ContextProvider
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

            LoadContextSponsor.LoadAssembiesIfNeeded();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            LoadContextSponsor.UnloadAssembliesOnCloseProject();
            Assert.Empty(AssemblyLoadContextService.GetAssemblyLoadContext().Assemblies);
        }

        internal IServiceProvider ServiceProvider;
        internal IConfigurationService ConfigurationService;
        internal IContextProvider ContextProvider;
        internal ILoadContextSponsor LoadContextSponsor;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
    }
}
