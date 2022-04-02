using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TelerikLogicBuilder.IntegrationTests.Constants;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class ParameterElementValidatorTest : IClassFixture<ParameterElementValidatorFixture>
    {
        private readonly ParameterElementValidatorFixture _fixture;

        public ParameterElementValidatorTest(ParameterElementValidatorFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanLoadParameterElementValidator()
        {
            //arrange
            IParameterElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IParameterElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }

        [Fact]
        public void CanValidateLiteralParameterElement()
        {
            //arrange
            IParameterElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IParameterElementValidator>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            XmlElement xmlElement = GetXmlElement(@"<literalParameter name=""stringProperty"">AAA</literalParameter>");
            List<string> errors = new();
            LiteralParameter parameter = new
            (
                "stringProperty",
                false,
                "",
                LiteralParameterType.String,
                LiteralParameterInputStyle.SingleLineTextBox,
                true,
                false,
                false,
                "",
                "",
                "",
                new List<string>(),
                _fixture.ContextProvider
            );

            //act
            xmlValidator.Validate(xmlElement, parameter, applicationTypeInfo, errors);

            //assert
            Assert.False(errors.Any());
        }

        [Fact]
        public void CanValidateObjectParameterElement()
        {
            //arrange
            IParameterElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IParameterElementValidator>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            XmlElement xmlElement = GetXmlElement(@"<objectParameter name=""objectProperty"">
                                                        <constructor name=""TestResponseA"" visibleText=""TestResponseA"" >
                                                            <genericArguments />
                                                            <parameters>
                                                                <literalParameter name=""stringProperty""> XX</literalParameter>
                                                            </parameters>
                                                        </constructor>
                                                    </objectParameter>");
            List<string> errors = new();
            ObjectParameter parameter = new
            (
                "objectProperty",
                false,
                "",
                "Contoso.Domain.Entities.DepartmentModel",
                true,
                false,
                false,
                _fixture.ContextProvider
            );

            //act
            xmlValidator.Validate(xmlElement, parameter, applicationTypeInfo, errors);

            //assert
            Assert.False(errors.Any());
        }

        [Fact]
        public void CanValidateListOfLiteralsParameterElement()
        {
            //arrange
            IParameterElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IParameterElementValidator>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            XmlElement xmlElement = GetXmlElement(@"<literalListParameter name=""Includes"">
                                                        <literalList literalType=""String"" listType=""GenericList"" visibleText=""www"">
                                                          <literal>Field1</literal>
                                                          <literal>Field2</literal>
                                                        </literalList>
                                                      </literalListParameter>");
            List<string> errors = new();
            ListOfLiteralsParameter parameter = new
            (
                "Includes",
                false,
                "",
                LiteralParameterType.String,
                ListType.GenericList,
                ListParameterInputStyle.HashSetForm,
                LiteralParameterInputStyle.SingleLineTextBox,
                "",
                "",
                new List<string>(),
                Array.Empty<char>(),
                new List<string>(),
                _fixture.ContextProvider
            );

            //act
            xmlValidator.Validate(xmlElement, parameter, applicationTypeInfo, errors);

            //assert
            Assert.False(errors.Any());
        }

        [Fact]
        public void CanValidateListOfObjectsParameterElement()
        {
            //arrange
            IParameterElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IParameterElementValidator>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            XmlElement xmlElement = GetXmlElement(@"<objectListParameter name=""myParamName"">
                                                <objectList objectType=""constructorType"" listType=""GenericList"" visibleText=""visibleText"">
                                                  <object>
                                                    <constructor name=""TestResponseA"" visibleText=""TestResponseA"" >
                                                        <genericArguments />
                                                        <parameters>
                                                            <literalParameter name=""stringProperty"">XX</literalParameter>
                                                        </parameters>
                                                    </constructor>
                                                  </object>
                                                  <object>
                                                    <constructor name=""TestResponseA"" visibleText=""TestResponseA"" >
                                                        <genericArguments />
                                                        <parameters>
                                                            <literalParameter name=""stringProperty"">YY</literalParameter>
                                                        </parameters>
                                                    </constructor>
                                                  </object>
                                                </objectList>
                                              </objectListParameter>");
            List<string> errors = new();
            ListOfObjectsParameter parameter = new
            (
                "objects",
                false,
                "",
                "Contoso.Test.Business.Responses.TestResponseA",
                ListType.GenericList,
                ListParameterInputStyle.HashSetForm,
                _fixture.ContextProvider
            );

            //act
            xmlValidator.Validate(xmlElement, parameter, applicationTypeInfo, errors);

            //assert
            Assert.False(errors.Any());
        }

        class InvalidParameterType : ParameterBase
        {
            public InvalidParameterType() : base("", false, "")
            {
            }

            internal override ParameterCategory ParameterCategory => throw new NotImplementedException();

            internal override string Description => throw new NotImplementedException();

            internal override string ToXml => throw new NotImplementedException();
        }

        [Fact]
        public void ValidateThrowsForInvalidParameterType()
        {
            //arrange
            IParameterElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IParameterElementValidator>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            XmlElement xmlElement = GetXmlElement(@"<literalParameter name=""stringProperty"">AAA</literalParameter>");
            List<string> errors = new();
            InvalidParameterType parameter = new();

            //act
            Assert.Throws<CriticalLogicBuilderException>(() => xmlValidator.Validate(xmlElement, parameter, applicationTypeInfo, errors));
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

    public class ParameterElementValidatorFixture : IDisposable
    {
        public ParameterElementValidatorFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            XmlElementValidator = ServiceProvider.GetRequiredService<IXmlElementValidator>();
            ContextProvider = ServiceProvider.GetRequiredService<IContextProvider>();
            AssemblyLoadContextService = ServiceProvider.GetRequiredService<IAssemblyLoadContextManager>();
            LoadContextSponsor = ServiceProvider.GetRequiredService<ILoadContextSponsor>();
            TypeLoadHelper = ServiceProvider.GetRequiredService<ITypeLoadHelper>();
            ApplicationTypeInfoManager = ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>();

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
                    ["TestResponseA"] = new Constructor
                    (
                        "TestResponseA",
                        "Contoso.Test.Business.Responses.TestResponseA",
                        new List<ParameterBase>
                        {
                            new LiteralParameter
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
        internal IXmlElementValidator XmlElementValidator;
        internal IContextProvider ContextProvider;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
        internal ILoadContextSponsor LoadContextSponsor;
        internal ITypeLoadHelper TypeLoadHelper;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
    }
}
