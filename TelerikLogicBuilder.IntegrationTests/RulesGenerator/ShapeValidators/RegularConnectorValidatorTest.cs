using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Linq;
using TelerikLogicBuilder.IntegrationTests.Constants;
using Xunit;
using Application = ABIS.LogicBuilder.FlowBuilder.Configuration.Application;

namespace TelerikLogicBuilder.IntegrationTests.RulesGenerator.ShapeValidators
{
    public class RegularConnectorValidatorTest : IClassFixture<RegularConnectorValidatorFixture>
    {
        private readonly RegularConnectorValidatorFixture _fixture;

        public RegularConnectorValidatorTest(RegularConnectorValidatorFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateRegularConnectorValidator()
        {
            //arrange
            IRegularConnectorValidator validator = _fixture.ServiceProvider.GetRequiredService<IRegularConnectorValidator>();

            //assert
            Assert.NotNull(validator);
        }

        [Fact]
        public void RegularConnectorValidationSucceeds()
        {
            //arrange
            IRegularConnectorValidator validator = _fixture.ServiceProvider.GetRequiredService<IRegularConnectorValidator>();
            string sourceFile = GetFullSourceFilePath(nameof(RegularConnectorValidationSucceeds));
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
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
                    if (s.Master.NameU != UniversalMasterName.CONNECTOBJECT)
                        return false;

                    Shape fromShape = shapeHelper.GetFromShape(s);
                    return fromShape.Master.NameU == UniversalMasterName.BEGINFLOW;
                }
            );
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
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
        public void FailsValidationIfConnectedToLessThanTwoShapes()
        {
            //arrange
            IRegularConnectorValidator validator = _fixture.ServiceProvider.GetRequiredService<IRegularConnectorValidator>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationIfConnectedToLessThanTwoShapes));
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
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
                    if (s.Master.NameU != UniversalMasterName.CONNECTOBJECT)
                        return false;

                    Shape fromShape = shapeHelper.GetFromShape(s);
                    return fromShape.Master.NameU == UniversalMasterName.BEGINFLOW;
                }
            );
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
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
            Assert.Equal(Strings.connectorRequires2Shapes, errors.First().Message);
        }

        [Fact]
        public void FailsValidationIfBothEndsAreConnectedToTheSameShape()
        {
            //arrange
            IRegularConnectorValidator validator = _fixture.ServiceProvider.GetRequiredService<IRegularConnectorValidator>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationIfBothEndsAreConnectedToTheSameShape));
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
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
                    if (s.Master.NameU != UniversalMasterName.CONNECTOBJECT)
                        return false;

                    Shape fromShape = shapeHelper.GetFromShape(s);
                    return fromShape.Master.NameU == UniversalMasterName.ACTION;
                }
            );
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
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
            Assert.Equal(Strings.shapeConnectedToBothEnds, errors.First().Message);
        }

        [Fact]
        public void FailsValidationForContainingADialogFunction()
        {
            //arrange
            IRegularConnectorValidator validator = _fixture.ServiceProvider.GetRequiredService<IRegularConnectorValidator>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForContainingADialogFunction));
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            IShapeXmlHelper shapeXmlHelper = _fixture.ServiceProvider.GetRequiredService<IShapeXmlHelper>();
            IConnectorDataParser connectorDataParser = _fixture.ServiceProvider.GetRequiredService<IConnectorDataParser>();
            IXmlDocumentHelpers xmlDocumentHelpers = _fixture.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>();
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
                    if (s.Master.NameU != UniversalMasterName.CONNECTOBJECT)
                        return false;

                    string xml = shapeXmlHelper.GetXmlString(s);
                    if (xml.Length == 0)
                        return false;

                    var data = connectorDataParser.Parse(xmlDocumentHelpers.ToXmlElement(xml));

                    return data.Index == 1;
                }
            );

            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
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
            Assert.Equal(Strings.dialogFunctionsInvalidConnector, errors.First().Message);
        }

        private static string GetFullSourceFilePath(string fileNameNoExtension)
            => System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\{nameof(RegularConnectorValidatorTest)}\{fileNameNoExtension}.vsdx");

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

    public class RegularConnectorValidatorFixture : IDisposable
    {
        public RegularConnectorValidatorFixture()
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

            ConfigurationService.ConstructorList = new ConstructorList
            (
                new Dictionary<string, Constructor>
                {
                    ["CommandButtonParameters"] = new Constructor
                    (
                        "CommandButtonParameters",
                        "Contoso.Forms.Parameters.CommandButtonParameters",
                        new List<ParameterBase>
                        {
                            new LiteralParameter
                            (
                                "command",
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
                            ),
                            new LiteralParameter
                            (
                                "buttonIcon",
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
                        new List<string>(),
                        "",
                        ContextProvider
                    )
                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>())
            );

            ConfigurationService.FunctionList = new FunctionList
            (
                new Dictionary<string, Function>
                {
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
                        new LiteralReturnType(LiteralFunctionReturnType.String, ContextProvider),
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
