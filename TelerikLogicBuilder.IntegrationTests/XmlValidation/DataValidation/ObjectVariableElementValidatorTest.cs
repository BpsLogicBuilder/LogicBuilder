﻿using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using TelerikLogicBuilder.IntegrationTests.Constants;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class ObjectVariableElementValidatorTest : IClassFixture<ObjectVariableElementValidatorFixture>
    {
        private readonly ObjectVariableElementValidatorFixture _fixture;

        public ObjectVariableElementValidatorTest(ObjectVariableElementValidatorFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateObjectVariableElementValidator()
        {
            //arrange
            IObjectVariableElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IObjectVariableElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }

        [Fact]
        public void ObjectVariableElementValidatorWorksForValidData()
        {
            //arrange
            IObjectVariableElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IObjectVariableElementValidator>();
            XmlElement xmlElement = GetXmlElement(@"<objectVariable>
                                                        <constructor name=""TestResponseA"" visibleText=""TestResponseA"" >
                                                            <genericArguments />
                                                            <parameters>
                                                                <literalParameter name=""stringProperty""> XX</literalParameter>
                                                            </parameters>
                                                        </constructor>
                                                    </objectVariable>");
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            ObjectVariable variable = _fixture.VariableFactory.GetObjectVariable
            (
                "Response",
                "Response",
                VariableCategory.Property,
                "",
                "",
                "parent",
                "Property",
                "",
                ReferenceCategories.InstanceReference,
                "",
                "Contoso.Test.Business.Responses.TestResponseA"
            );

            //act
            xmlValidator.Validate
            (
                xmlElement,
                variable,
                applicationTypeInfo,
                errors
            );

            //assert
            Assert.Empty(errors);
        }

        [Fact]
        public void ObjectVariableElementValidatorThrowsForInvalidElementType()
        {
            //arrange
            IObjectVariableElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IObjectVariableElementValidator>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            XmlElement xmlElement = GetXmlElement(@"<literalListParameter name=""Includes"">
                                                        <literalList literalType=""String"" listType=""GenericList"" visibleText=""www"">
                                                        </literalList>
                                                      </literalListParameter>");
            List<string> errors = new();
            ObjectVariable variable = _fixture.VariableFactory.GetObjectVariable
            (
                "Response",
                "Response",
                VariableCategory.Property,
                "",
                "",
                "parent",
                "Property",
                "",
                ReferenceCategories.InstanceReference,
                "",
                "Contoso.Test.Business.Responses.TestResponseA"
            );

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => xmlValidator.Validate(xmlElement, variable, applicationTypeInfo, errors));
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{D4A2A138-DEC9-4B91-B490-2C2A8D6DC1D4}"),
                exception.Message
            );
        }

        [Fact]
        public void ObjectVariableElementValidatorFailsIfObjectTypeCannotBeLoaded()
        {
            //arrange
            IObjectVariableElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IObjectVariableElementValidator>();
            XmlElement xmlElement = GetXmlElement(@"<objectVariable>
                                                        <constructor name=""TestResponseA"" visibleText=""TestResponseA"" >
                                                            <genericArguments />
                                                            <parameters>
                                                                <literalParameter name=""stringProperty""> XX</literalParameter>
                                                            </parameters>
                                                        </constructor>
                                                    </objectVariable>");
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            ObjectVariable variable = _fixture.VariableFactory.GetObjectVariable
            (
                "Response",
                "Response",
                VariableCategory.Property,
                "",
                "",
                "parent",
                "Property",
                "",
                ReferenceCategories.InstanceReference,
                "",
                "Contoso.Test.Business.Responses.TypeNotFound"
            );

            //act
            xmlValidator.Validate
            (
                xmlElement,
                variable,
                applicationTypeInfo,
                errors
            );

            //assert
            Assert.True(errors.Any());
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeForVariableFormat, variable.ObjectTypeString, variable.Name),
                errors.First()
            );
        }

        private static XmlElement GetXmlElement(string xmlString)
                => GetXmlDocument(xmlString).DocumentElement!;

        private static XmlDocument GetXmlDocument(string xmlString)
        {
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(xmlString);
            return xmlDocument;
        }
    }

    public class ObjectVariableElementValidatorFixture : IDisposable
    {
        public ObjectVariableElementValidatorFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ServiceProvider.GetRequiredService<IMainWindow>().Instance = new Mocks.MockMdiParent();
            ProjectPropertiesItemFactory = ServiceProvider.GetRequiredService<IProjectPropertiesItemFactory>();
			WebApiDeploymentItemFactory = ServiceProvider.GetRequiredService<IWebApiDeploymentItemFactory>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            ConstructorFactory = ServiceProvider.GetRequiredService<IConstructorFactory>();
            AssemblyLoadContextService = ServiceProvider.GetRequiredService<IAssemblyLoadContextManager>();
            LoadContextSponsor = ServiceProvider.GetRequiredService<ILoadContextSponsor>();
            ParameterFactory = ServiceProvider.GetRequiredService<IParameterFactory>();
            TypeLoadHelper = ServiceProvider.GetRequiredService<ITypeLoadHelper>();
            ApplicationTypeInfoManager = ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>();
            VariableFactory = ServiceProvider.GetRequiredService<IVariableFactory>();

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
                        $@"{TestFolders.TestAssembliesFolder}\Contoso.Test.Flow\bin\Debug\netstandard2.0",
                        ABIS.LogicBuilder.FlowBuilder.Enums.RuntimeType.NetCore,
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
                },
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
        internal IProjectPropertiesItemFactory ProjectPropertiesItemFactory;
		internal IWebApiDeploymentItemFactory WebApiDeploymentItemFactory;
        internal IConfigurationService ConfigurationService;
        internal IConstructorFactory ConstructorFactory;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
        internal ILoadContextSponsor LoadContextSponsor;
        internal IParameterFactory ParameterFactory;
        internal ITypeLoadHelper TypeLoadHelper;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
        internal IVariableFactory VariableFactory;
    }
}
