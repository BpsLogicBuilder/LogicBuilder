using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TelerikLogicBuilder.IntegrationTests.Constants;
using Xunit;
using Application = ABIS.LogicBuilder.FlowBuilder.Configuration.Application;

namespace TelerikLogicBuilder.IntegrationTests.RulesGenerator.ShapeValidators
{
    public class ShapeValidatorTest : IClassFixture<ShapeValidatorFixture>
    {
        private readonly ShapeValidatorFixture _fixture;

        public ShapeValidatorTest(ShapeValidatorFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateShapeValidator()
        {
            //arrange
            IShapeValidator validator = _fixture.ServiceProvider.GetRequiredService<IShapeValidator>();

            //assert
            Assert.NotNull(validator);
        }

        [Fact]
        public void ShapeValidationSucceeds()
        {
            //arrange
            IShapeValidator validator = _fixture.ServiceProvider.GetRequiredService<IShapeValidator>();
            string sourceFile = GetFullSourceFilePath(nameof(ShapeValidationSucceeds));
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
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
                    return s.Master.NameU == UniversalMasterName.BEGINFLOW;
                }
            );
            List<ResultMessage> errors = new();

            //act
            validator.Validate
            (
                sourceFile,
                GetPage(visioDocument),
                shape,
                errors,
                applicationTypeInfo
            );

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Empty(errors);
        }

        [Fact]
        public void ShapeValidatorThrowsForInvalidShape()
        {
            //arrange
            IShapeValidator validator = _fixture.ServiceProvider.GetRequiredService<IShapeValidator>();
            string sourceFile = GetFullSourceFilePath(nameof(ShapeValidatorThrowsForInvalidShape));
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
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
                    return s.Master.NameU == "Process";
                }
            );
            List<ResultMessage> errors = new();

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>
            (
                () => validator.Validate
                (
                    sourceFile,
                    GetPage(visioDocument),
                    shape,
                    errors,
                    applicationTypeInfo
                )
            );

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{1847D564-79A4-49C0-8B82-DD7A91B3EA44}"),
                exception.Message
            );
        }

        private static string GetFullSourceFilePath(string fileNameNoExtension)
            => System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\{nameof(ShapeValidatorTest)}\{fileNameNoExtension}.vsdx");

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

    public class ShapeValidatorFixture : IDisposable
    {
        public ShapeValidatorFixture()
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
                        new WebApiDeployment("", "", "", "", ContextProvider),
                        ContextProvider
                    ),
                    ["app02"] = new Application
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
                        new WebApiDeployment("", "", "", "", ContextProvider),
                        ContextProvider
                    )
                },
                new HashSet<string>(),
                ContextProvider
            );

            VisioApplication = new InvisibleApp();
            LoadContextSponsor.LoadAssembiesIfNeeded();
        }

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

        internal InvisibleApp VisioApplication;
        internal IServiceProvider ServiceProvider;
        internal IConfigurationService ConfigurationService;
        internal IContextProvider ContextProvider;
        internal ILoadContextSponsor LoadContextSponsor;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
    }
}
