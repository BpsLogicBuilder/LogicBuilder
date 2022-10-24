using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables.Factories;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Office.Interop.Visio;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using TelerikLogicBuilder.IntegrationTests.Constants;
using Xunit;
using Application = ABIS.LogicBuilder.FlowBuilder.Configuration.Application;

namespace TelerikLogicBuilder.IntegrationTests.RulesGenerator.ShapeValidators
{
    public class ConditionShapeValidatorTest : IClassFixture<ConditionShapeValidatorFixture>
    {
        private readonly ConditionShapeValidatorFixture _fixture;

        public ConditionShapeValidatorTest(ConditionShapeValidatorFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void ConditionShapeValidationSucceeds()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(ConditionShapeValidationSucceeds));
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape
            (
                visioDocument, 
                s => 
                { 
                    return s.Master.NameU == UniversalMasterName.CONDITIONOBJECT; 
                }
            );
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<ResultMessage> errors = new();

            //act
            rulesGeneratorFactory.GetShapeValidator
            (
                sourceFile,
                GetPage(visioDocument),
                new ShapeBag(shape),
                errors,
                applicationTypeInfo
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Empty(errors);
        }

        [Fact]
        public void FailsValidationForTooManyOutgoingConnectors()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForTooManyOutgoingConnectors));
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape
            (
                visioDocument,
                s =>
                {
                    return s.Master.NameU == UniversalMasterName.CONDITIONOBJECT;
                }
            );
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<ResultMessage> errors = new();

            //act
            rulesGeneratorFactory.GetShapeValidator
            (
                sourceFile,
                GetPage(visioDocument),
                new ShapeBag(shape),
                errors,
                applicationTypeInfo
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.conditionBoxOutgoingRequired, errors.First().Message);
        }

        [Fact]
        public void FailsValidationForMissingOutgoingConnector()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForMissingOutgoingConnector));
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape
            (
                visioDocument,
                s =>
                {
                    return s.Master.NameU == UniversalMasterName.CONDITIONOBJECT;
                }
            );
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<ResultMessage> errors = new();

            //act
            rulesGeneratorFactory.GetShapeValidator
            (
                sourceFile,
                GetPage(visioDocument),
                new ShapeBag(shape),
                errors,
                applicationTypeInfo
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.conditionBoxOutgoingRequired, errors.First().Message);
        }

        [Fact]
        public void FailsValidationForMissingIncomingConnector()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForMissingIncomingConnector));
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape
            (
                visioDocument,
                s =>
                {
                    return s.Master.NameU == UniversalMasterName.CONDITIONOBJECT;
                }
            );
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<ResultMessage> errors = new();

            //act
            rulesGeneratorFactory.GetShapeValidator
            (
                sourceFile,
                GetPage(visioDocument),
                new ShapeBag(shape),
                errors,
                applicationTypeInfo
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.conditionBoxIncomingRequired, errors.First().Message);
        }

        [Fact]
        public void FailsValidationForMissingShapeData()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForMissingShapeData));
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape
            (
                visioDocument,
                s =>
                {
                    return s.Master.NameU == UniversalMasterName.CONDITIONOBJECT;
                }
            );
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<ResultMessage> errors = new();

            //act
            rulesGeneratorFactory.GetShapeValidator
            (
                sourceFile,
                GetPage(visioDocument),
                new ShapeBag(shape),
                errors,
                applicationTypeInfo
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.conditionShapeDataRequired, errors.First().Message);
        }

        private static string GetFullSourceFilePath(string fileNameNoExtension)
            => System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\{nameof(ConditionShapeValidatorTest)}\{fileNameNoExtension}.vsdx");

        private static void CloseVisioDocument(Document visioDocument)
        {
            visioDocument.Saved = true;
            visioDocument.Close();
        }

        private static Shape GetOnlyShape(Document document, Func<Shape, bool> selector)
        {
            return document.Pages
                .OfType<Page>()
                .Single()
                .Shapes
                .OfType<Shape>()
                .Single(selector);
        }

        private static Page GetPage(Document document)
        {
            return document.Pages[1];
        }
    }

    public class ConditionShapeValidatorFixture : IDisposable
    {
        internal InvisibleApp VisioApplication;
        internal IServiceProvider ServiceProvider;
        internal IConfigurationItemFactory ConfigurationItemFactory;
        internal IConfigurationService ConfigurationService;
        internal IContextProvider ContextProvider;
        internal IEnumHelper EnumHelper;
        internal ITypeHelper TypeHelper;
        internal IFunctionFactory FunctionFactory;
        internal ILoadContextSponsor LoadContextSponsor;
        internal IParameterFactory ParameterFactory;
        internal IReturnTypeFactory ReturnTypeFactory;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
        internal IVariableFactory VariableFactory;

        public ConditionShapeValidatorFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ServiceProvider.GetRequiredService<IMainWindow>().Instance = new Mocks.MockMdiParent();
            ContextProvider = ServiceProvider.GetRequiredService<IContextProvider>();
            ConfigurationItemFactory = ServiceProvider.GetRequiredService<IConfigurationItemFactory>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            EnumHelper = ServiceProvider.GetRequiredService<IEnumHelper>();
            TypeHelper = ServiceProvider.GetRequiredService<ITypeHelper>();
            FunctionFactory = ServiceProvider.GetRequiredService<IFunctionFactory>();
            LoadContextSponsor = ServiceProvider.GetRequiredService<ILoadContextSponsor>();
            ParameterFactory = ServiceProvider.GetRequiredService<IParameterFactory>();
            ReturnTypeFactory = ServiceProvider.GetRequiredService<IReturnTypeFactory>();
            ApplicationTypeInfoManager = ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>();
            AssemblyLoadContextService = ServiceProvider.GetRequiredService<IAssemblyLoadContextManager>();
            VariableFactory = ServiceProvider.GetRequiredService<IVariableFactory>();
            ConfigurationService.ProjectProperties = ConfigurationItemFactory.GetProjectProperties
            (
                "Contoso",
                @"C:\ProjectPath",
                new Dictionary<string, Application>
                {
                    ["app01"] = ConfigurationItemFactory.GetApplication
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
                        ConfigurationItemFactory.GetWebApiDeployment("", "", "", "")
                    ),
                    ["app02"] = ConfigurationItemFactory.GetApplication
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
                        ConfigurationItemFactory.GetWebApiDeployment("", "", "", "")
                    )
                },
                new HashSet<string>()
            );

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

            ConfigurationService.VariableList = new VariableList
            (
                new Dictionary<string, VariableBase>
                {

                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>())
            );

            foreach (LiteralVariableType enumValue in Enum.GetValues<LiteralVariableType>())
            {
                string variableName = $"{Enum.GetName(typeof(LiteralVariableType), enumValue)}Item";
                ConfigurationService.VariableList.Variables.Add(variableName, GetLiteralVariable(variableName, enumValue));
            }

            VisioApplication = new InvisibleApp();
            LoadContextSponsor.LoadAssembiesIfNeeded();
        }

        LiteralVariable GetLiteralVariable(string name, LiteralVariableType literalVariableType)
            => VariableFactory.GetLiteralVariable
            (
                name,
                name,
                VariableCategory.StringKeyIndexer,
                TypeHelper.ToId(EnumHelper.GetSystemType(literalVariableType)),
                "",
                "flowManager.FlowDataCache.Items",
                "Field.Property.Property",
                "",
                ReferenceCategories.InstanceReference,
                "",
                literalVariableType,
                LiteralVariableInputStyle.SingleLineTextBox,
                "",
                "",
                new List<string>()
            );

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            LoadContextSponsor.UnloadAssembliesOnCloseProject();
            Assert.Empty(AssemblyLoadContextService.GetAssemblyLoadContext().Assemblies);
            foreach (Document document in VisioApplication.Documents)
            {
                document.Saved = true;
                document.Close();
            }
            VisioApplication.Quit();
        }
    }
}
