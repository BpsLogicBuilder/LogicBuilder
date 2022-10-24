using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using TelerikLogicBuilder.IntegrationTests.Constants;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class ObjectElementValidatorTest : IClassFixture<ObjectElementValidatorFixture>
    {
        private readonly ObjectElementValidatorFixture _fixture;

        public ObjectElementValidatorTest(ObjectElementValidatorFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateObjectElementValidator()
        {
            //arrange
            IObjectElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IObjectElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }

        [Fact]
        public void ObjectElementValidatorWorksForValidData()
        {
            //arrange
            IObjectElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IObjectElementValidator>();
            XmlElement xmlElement = GetXmlElement(@"<object>
                                                        <constructor name=""TestResponseA"" visibleText=""TestResponseA"" >
                                                            <genericArguments />
                                                            <parameters>
                                                                <literalParameter name=""stringProperty""> XX</literalParameter>
                                                            </parameters>
                                                        </constructor>
                                                    </object>");
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();

            //act
            xmlValidator.Validate
            (
                xmlElement,
                typeof(object),
                applicationTypeInfo,
                errors
            );

            //assert
            Assert.Empty(errors);
        }

        [Fact]
        public void ObjectElementValidatorThrowsForInvalidElementType()
        {
            //arrange
            IObjectElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IObjectElementValidator>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            XmlElement xmlElement = GetXmlElement(@"<literalListParameter name=""Includes"">
                                                        <literalList literalType=""String"" listType=""GenericList"" visibleText=""www"">
                                                        </literalList>
                                                      </literalListParameter>");
            List<string> errors = new();

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => xmlValidator.Validate(xmlElement, typeof(object), applicationTypeInfo, errors));
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{54007C41-31CD-49BD-99E6-03C198E8B6AB}"), 
                exception.Message
            );
        }

        [Fact]
        public void ObjectElementValidatorThrowsForMultipleChildElements()
        {
            //arrange
            IObjectElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IObjectElementValidator>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            XmlElement xmlElement = GetXmlElement(@"<object>
                                                        <constructor name=""TestResponseA"" visibleText=""TestResponseA"" >
                                                            <genericArguments />
                                                            <parameters>
                                                                <literalParameter name=""stringProperty""> XX</literalParameter>
                                                            </parameters>
                                                        </constructor>
                                                        <literalList literalType=""String"" listType=""GenericList"" visibleText=""www"">
                                                        </literalList>
                                                    </object>");
            List<string> errors = new();

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => xmlValidator.Validate(xmlElement, typeof(object), applicationTypeInfo, errors));
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{DB68CE86-4BA8-4ADF-B8FB-796E7FF73F40}"),
                exception.Message
            );
        }

        [Fact]
        public void ObjectElementValidatorThrowsForNoChildElements()
        {
            //arrange
            IObjectElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IObjectElementValidator>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            XmlElement xmlElement = GetXmlElement(@"<object>
                                                    </object>");
            List<string> errors = new();

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => xmlValidator.Validate(xmlElement, typeof(object), applicationTypeInfo, errors));
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{DB68CE86-4BA8-4ADF-B8FB-796E7FF73F40}"),
                exception.Message
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

    public class ObjectElementValidatorFixture : IDisposable
    {
        public ObjectElementValidatorFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ServiceProvider.GetRequiredService<IMainWindow>().Instance = new Mocks.MockMdiParent();
            ConfigurationItemFactory = ServiceProvider.GetRequiredService<IConfigurationItemFactory>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            ContextProvider = ServiceProvider.GetRequiredService<IContextProvider>();
            ConstructorFactory = ServiceProvider.GetRequiredService<IConstructorFactory>();
            AssemblyLoadContextService = ServiceProvider.GetRequiredService<IAssemblyLoadContextManager>();
            LoadContextSponsor = ServiceProvider.GetRequiredService<ILoadContextSponsor>();
            ParameterFactory = ServiceProvider.GetRequiredService<IParameterFactory>();
            TypeLoadHelper = ServiceProvider.GetRequiredService<ITypeLoadHelper>();
            ApplicationTypeInfoManager = ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>();

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
                        ConfigurationItemFactory.GetWebApiDeployment("", "", "", "")
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
        internal IConfigurationItemFactory ConfigurationItemFactory;
        internal IConfigurationService ConfigurationService;
        internal IContextProvider ContextProvider;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
        internal ILoadContextSponsor LoadContextSponsor;
        internal IParameterFactory ParameterFactory;
        internal IConstructorFactory ConstructorFactory;
        internal ITypeLoadHelper TypeLoadHelper;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
    }
}
