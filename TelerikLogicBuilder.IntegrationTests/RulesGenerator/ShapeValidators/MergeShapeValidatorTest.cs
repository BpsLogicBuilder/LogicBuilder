using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;
using Application = ABIS.LogicBuilder.FlowBuilder.Configuration.Application;

namespace TelerikLogicBuilder.IntegrationTests.RulesGenerator.ShapeValidators
{
    public class MergeShapeValidatorTest : IClassFixture<MergeShapeValidatorFixture>
    {
        private readonly MergeShapeValidatorFixture _fixture;

        public MergeShapeValidatorTest(MergeShapeValidatorFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void MergeShapeValidationSucceeds()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(MergeShapeValidationSucceeds));
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
                    if (s.Master.NameU != UniversalMasterName.MERGEOBJECT)
                        return false;

                    return GetIncomingRegularConnector() != null;
                    Shape? GetIncomingRegularConnector()
                    {
                        foreach(Connect fromConnect in s.FromConnects)
                        {
                            if (fromConnect.FromSheet.Master.NameU == UniversalMasterName.CONNECTOBJECT
                                & fromConnect.FromPart == (short)VisFromParts.visEnd)
                                return fromConnect.FromSheet;
                        }
                        return null;
                    }
                }
            );
            List<ResultMessage> errors = new();
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

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
        public void FailsValidationForUnecessaryOthersConnector()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForUnecessaryOthersConnector));
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
                    if (s.Master.NameU != UniversalMasterName.MERGEOBJECT)
                        return false;

                    return GetIncomingRegularConnector() != null;
                    Shape? GetIncomingRegularConnector()
                    {
                        foreach (Connect fromConnect in s.FromConnects)
                        {
                            if (fromConnect.FromSheet.Master.NameU == UniversalMasterName.CONNECTOBJECT
                                & fromConnect.FromPart == (short)VisFromParts.visEnd)
                                return fromConnect.FromSheet;
                        }
                        return null;
                    }
                }
            );
            List<ResultMessage> errors = new();
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

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
            Assert.Equal(Strings.othersConnectorInvalid, errors.First().Message);
        }

        [Fact]
        public void FailsValidationForIncomingAndOutGoingApplicationConnectors()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForIncomingAndOutGoingApplicationConnectors));
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
                    if (s.Master.NameU != UniversalMasterName.MERGEOBJECT)
                        return false;

                    return GetApp2Connector(VisFromParts.visEnd) != null 
                        && GetApp2Connector(VisFromParts.visBegin) != null;
                    Shape? GetApp2Connector(VisFromParts vispart)
                    {
                        foreach (Connect fromConnect in s.FromConnects)
                        {
                            if (fromConnect.FromSheet.Master.NameU == UniversalMasterName.APP02CONNECTOBJECT
                                & fromConnect.FromPart == (short)vispart)
                                return fromConnect.FromSheet;
                        }
                        return null;
                    }
                }
            );
            List<ResultMessage> errors = new();
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

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
            Assert.Equal(Strings.mergeHasInAndOutAppConnectors, errors.First().Message);
        }

        [Fact]
        public void FailsValidationForIncomingAndOutGoingRegularConnectors()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForIncomingAndOutGoingRegularConnectors));
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
                    if (s.Master.NameU != UniversalMasterName.MERGEOBJECT)
                        return false;

                    return GetApp2Connector(VisFromParts.visEnd) != null
                        && GetApp2Connector(VisFromParts.visBegin) != null;
                    Shape? GetApp2Connector(VisFromParts vispart)
                    {
                        foreach (Connect fromConnect in s.FromConnects)
                        {
                            if (fromConnect.FromSheet.Master.NameU == UniversalMasterName.CONNECTOBJECT
                                & fromConnect.FromPart == (short)vispart)
                                return fromConnect.FromSheet;
                        }
                        return null;
                    }
                }
            );
            List<ResultMessage> errors = new();
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

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
            Assert.Contains(Strings.mergeHasInAndOutNonAppConnectors, errors.Select(e => e.Message));
        }

        [Fact]
        public void FailsValidationForOutGoingRegularAndApplicationConnectors()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForOutGoingRegularAndApplicationConnectors));
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
                    if (s.Master.NameU != UniversalMasterName.MERGEOBJECT)
                        return false;

                    return GetApp2Connector(VisFromParts.visEnd) != null
                        && GetApp2Connector(VisFromParts.visBegin) != null;
                    Shape? GetApp2Connector(VisFromParts vispart)
                    {
                        foreach (Connect fromConnect in s.FromConnects)
                        {
                            if (fromConnect.FromSheet.Master.NameU == UniversalMasterName.CONNECTOBJECT
                                & fromConnect.FromPart == (short)vispart)
                                return fromConnect.FromSheet;
                        }
                        return null;
                    }
                }
            );
            List<ResultMessage> errors = new();
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

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
            Assert.Contains(Strings.mergeHasAppAndNonAppOutConnectors, errors.Select(e => e.Message));
        }

        [Fact]
        public void FailsValidationForIncomningRegularAndApplicationConnectors()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForIncomningRegularAndApplicationConnectors));
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
                    if (s.Master.NameU != UniversalMasterName.MERGEOBJECT)
                        return false;

                    return GetApp2Connector(VisFromParts.visEnd) != null
                        && GetApp2Connector(VisFromParts.visBegin) != null;
                    Shape? GetApp2Connector(VisFromParts vispart)
                    {
                        foreach (Connect fromConnect in s.FromConnects)
                        {
                            if (fromConnect.FromSheet.Master.NameU == UniversalMasterName.CONNECTOBJECT
                                & fromConnect.FromPart == (short)vispart)
                                return fromConnect.FromSheet;
                        }
                        return null;
                    }
                }
            );
            List<ResultMessage> errors = new();
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

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
            Assert.Contains(Strings.mergeHasAppAndNonAppInConnectors, errors.Select(e => e.Message));
        }

        [Fact]
        public void FailsValidationForDuplicateIncomingApplication()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForDuplicateIncomingApplication));
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
                    if (s.Master.NameU != UniversalMasterName.MERGEOBJECT)
                        return false;

                    return GetOutgoingRegularConnector() != null;
                    Shape? GetOutgoingRegularConnector()
                    {
                        foreach (Connect fromConnect in s.FromConnects)
                        {
                            if (fromConnect.FromSheet.Master.NameU == UniversalMasterName.CONNECTOBJECT
                                & fromConnect.FromPart == (short)VisFromParts.visBegin)
                                return fromConnect.FromSheet;
                        }
                        return null;
                    }
                }
            );
            List<ResultMessage> errors = new();
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

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
            Assert.Contains(Strings.duplicateIncomingConnector, errors.Select(e => e.Message));
        }

        [Fact]
        public void FailsValidationForDuplicateOutgoingApplication()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForDuplicateOutgoingApplication));
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
                    if (s.Master.NameU != UniversalMasterName.MERGEOBJECT)
                        return false;

                    return GetInomingRegularConnector() != null;
                    Shape? GetInomingRegularConnector()
                    {
                        foreach (Connect fromConnect in s.FromConnects)
                        {
                            if (fromConnect.FromSheet.Master.NameU == UniversalMasterName.CONNECTOBJECT
                                & fromConnect.FromPart == (short)VisFromParts.visEnd)
                                return fromConnect.FromSheet;
                        }
                        return null;
                    }
                }
            );
            List<ResultMessage> errors = new();
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

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
            Assert.Contains(Strings.duplicateOutgoingConnector, errors.Select(e => e.Message));
        }

        [Fact]
        public void FailsValidationForNotMergingOrSplitting()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForNotMergingOrSplitting));
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
                    return s.Master.NameU == UniversalMasterName.MERGEOBJECT;
                }
            );
            List<ResultMessage> errors = new();
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

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
            Assert.Contains(Strings.mergeAppConnectorComments, errors.Select(e => e.Message));
        }

        [Fact]
        public void FailsValidationForMergingWithoutOutgoingRegularConnector()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForMergingWithoutOutgoingRegularConnector));
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
                    if (s.Master.NameU != UniversalMasterName.MERGEOBJECT)
                        return false;

                    return GetInomingRegularConnector() == null;
                    Shape? GetInomingRegularConnector()
                    {
                        foreach (Connect fromConnect in s.FromConnects)
                        {
                            if (fromConnect.FromSheet.Master.NameU == UniversalMasterName.CONNECTOBJECT
                                & fromConnect.FromPart == (short)VisFromParts.visEnd)
                                return fromConnect.FromSheet;
                        }
                        return null;
                    }
                }
            );
            List<ResultMessage> errors = new();
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

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
            Assert.Contains(Strings.mergeMergingComments, errors.Select(e => e.Message));
        }

        [Fact]
        public void FailsValidationForMergingWithMultipleOutgoingRegularConnectors()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForMergingWithoutOutgoingRegularConnector));
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
                    if (s.Master.NameU != UniversalMasterName.MERGEOBJECT)
                        return false;

                    return GetInomingRegularConnector() == null;
                    Shape? GetInomingRegularConnector()
                    {
                        foreach (Connect fromConnect in s.FromConnects)
                        {
                            if (fromConnect.FromSheet.Master.NameU == UniversalMasterName.CONNECTOBJECT
                                & fromConnect.FromPart == (short)VisFromParts.visEnd)
                                return fromConnect.FromSheet;
                        }
                        return null;
                    }
                }
            );
            List<ResultMessage> errors = new();
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

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
            Assert.Contains(Strings.mergeMergingComments, errors.Select(e => e.Message));
        }

        [Fact]
        public void FailsValidationForSplittingWithoutIncomingRegularConnector()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForSplittingWithoutIncomingRegularConnector));
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
                    if (s.Master.NameU != UniversalMasterName.MERGEOBJECT)
                        return false;

                    return GetInomingConnector() == null;
                    Shape? GetInomingConnector()
                    {
                        foreach (Connect fromConnect in s.FromConnects)
                        {
                            if (fromConnect.FromPart == (short)VisFromParts.visEnd)
                                return fromConnect.FromSheet;
                        }
                        return null;
                    }
                }
            );
            List<ResultMessage> errors = new();
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

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
            Assert.Contains(Strings.mergeBranchingComments, errors.Select(e => e.Message));
        }

        [Fact]
        public void FailsValidationForSplittingWithApplicationUnaccountedFor()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForSplittingWithApplicationUnaccountedFor));
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            _fixture.ConfigurationService.ProjectProperties = ConfiguredWithThreeApplications;
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
                    if (s.Master.NameU != UniversalMasterName.MERGEOBJECT)
                        return false;

                    return GetIncomingRegularConnector() != null;
                    Shape? GetIncomingRegularConnector()
                    {
                        foreach (Connect fromConnect in s.FromConnects)
                        {
                            if (fromConnect.FromSheet.Master.NameU == UniversalMasterName.CONNECTOBJECT
                                & fromConnect.FromPart == (short)VisFromParts.visEnd)
                                return fromConnect.FromSheet;
                        }
                        return null;
                    }
                }
            );
            List<ResultMessage> errors = new();
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            rulesGeneratorFactory.GetShapeValidator
            (
                sourceFile,
                GetPage(visioDocument),
                new ShapeBag(shape),
                errors,
                applicationTypeInfo
            ).Validate();

            _fixture.ConfigurationService.ProjectProperties = ConfiguredWithTwoApplications;
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Contains
            (
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.applicationUnaccountedForFormat,
                    "App03",
                    nameof(FailsValidationForSplittingWithApplicationUnaccountedFor).ToLowerInvariant()
                ), 
                errors.Select(e => e.Message)
            );
        }

        [Fact]
        public void FailsValidationForMergingWithApplicationUnaccountedFor()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForMergingWithApplicationUnaccountedFor));
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            _fixture.ConfigurationService.ProjectProperties = ConfiguredWithThreeApplications;
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
                    if (s.Master.NameU != UniversalMasterName.MERGEOBJECT)
                        return false;

                    return GetOutgoingRegularConnector() != null;
                    Shape? GetOutgoingRegularConnector()
                    {
                        foreach (Connect fromConnect in s.FromConnects)
                        {
                            if (fromConnect.FromSheet.Master.NameU == UniversalMasterName.CONNECTOBJECT
                                & fromConnect.FromPart == (short)VisFromParts.visBegin)
                                return fromConnect.FromSheet;
                        }
                        return null;
                    }
                }
            );
            List<ResultMessage> errors = new();
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            rulesGeneratorFactory.GetShapeValidator
            (
                sourceFile,
                GetPage(visioDocument),
                new ShapeBag(shape),
                errors,
                applicationTypeInfo
            ).Validate();

            _fixture.ConfigurationService.ProjectProperties = ConfiguredWithTwoApplications;
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Contains
            (
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.applicationUnaccountedForFormat,
                    "App03",
                    nameof(FailsValidationForMergingWithApplicationUnaccountedFor).ToLowerInvariant()
                ),
                errors.Select(e => e.Message)
            );
        }

        private static string GetFullSourceFilePath(string fileNameNoExtension)
            => System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\{nameof(MergeShapeValidatorTest)}\{fileNameNoExtension}.vsdx");

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

        private ProjectProperties ConfiguredWithTwoApplications
        {
            get
            {
                var configurationItemFactory = _fixture.ServiceProvider.GetRequiredService<IConfigurationItemFactory>();
                IWebApiDeploymentItemFactory webApiDeploymentItemFactory = _fixture.ServiceProvider.GetRequiredService<IWebApiDeploymentItemFactory>();
                return configurationItemFactory.GetProjectProperties
                (
                    "Contoso",
                    @"C:\ProjectPath",
                    new Dictionary<string, Application>
                    {
                        ["app01"] = configurationItemFactory.GetApplication
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
                            webApiDeploymentItemFactory.GetWebApiDeployment("", "", "", "")
                        ),
                        ["app02"] = configurationItemFactory.GetApplication
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
                            webApiDeploymentItemFactory.GetWebApiDeployment("", "", "", "")
                        )
                    },
                    new HashSet<string>()
                );
            }
        }

        private ProjectProperties ConfiguredWithThreeApplications
        {
            get
            {
                var configurationItemFactory = _fixture.ServiceProvider.GetRequiredService<IConfigurationItemFactory>();
                IWebApiDeploymentItemFactory webApiDeploymentItemFactory = _fixture.ServiceProvider.GetRequiredService<IWebApiDeploymentItemFactory>();
                return configurationItemFactory.GetProjectProperties
                (
                    "Contoso",
                    @"C:\ProjectPath",
                    new Dictionary<string, Application>
                    {
                        ["app01"] = configurationItemFactory.GetApplication
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
                            webApiDeploymentItemFactory.GetWebApiDeployment("", "", "", "")
                        ),
                        ["app02"] = configurationItemFactory.GetApplication
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
                            webApiDeploymentItemFactory.GetWebApiDeployment("", "", "", "")
                        ),
                        ["app03"] = configurationItemFactory.GetApplication
                        (
                            "App03",
                            "App03",
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
                            webApiDeploymentItemFactory.GetWebApiDeployment("", "", "", "")
                        )
                    },
                    new HashSet<string>()
                );
            }
        }
    }

    public class MergeShapeValidatorFixture : IDisposable
    {
        public MergeShapeValidatorFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ConfigurationItemFactory = ServiceProvider.GetRequiredService<IConfigurationItemFactory>();
			WebApiDeploymentItemFactory = ServiceProvider.GetRequiredService<IWebApiDeploymentItemFactory>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
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
                    ["app02"] = ConfigurationItemFactory.GetApplication
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

            VisioApplication = new InvisibleApp();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            foreach (Document document in VisioApplication.Documents)
            {
                document.Saved = true;
                document.Close();
            }
            VisioApplication.Quit();
        }

        internal InvisibleApp VisioApplication;
        internal IServiceProvider ServiceProvider;
        internal IConfigurationItemFactory ConfigurationItemFactory;
		internal IWebApiDeploymentItemFactory WebApiDeploymentItemFactory;
        internal IConfigurationService ConfigurationService;
    }
}
