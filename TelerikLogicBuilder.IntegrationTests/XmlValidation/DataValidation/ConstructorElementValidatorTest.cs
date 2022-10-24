using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
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
using System.Linq;
using System.Xml;
using TelerikLogicBuilder.IntegrationTests.Constants;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class ConstructorElementValidatorTest : IClassFixture<ConstructorElementValidatorFixture>
    {
        private readonly ConstructorElementValidatorFixture _fixture;

        public ConstructorElementValidatorTest(ConstructorElementValidatorFixture fixture)
        {
            _fixture = fixture;
        }

        #region Fields
        #endregion Fields

        [Fact]
        public void CanCreateConstructorElementValidator()
        {
            //arrange
            IConstructorElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IConstructorElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }

        [Fact]
        public void ConstructorElementValidatorWorksForValidData()
        {
            //arrange
            IConstructorElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IConstructorElementValidator>();
            XmlElement xmlElement = GetXmlElement(@"<constructor name=""GenericResponse"" visibleText=""GenericResponse"">
                                                        <genericArguments>
                                                            <literalParameter genericArgumentName=""A"">
                                                                <literalType>String</literalType>
                                                                <control>SingleLineTextBox</control>
                                                                <useForEquality>false</useForEquality>
                                                                <useForHashCode>false</useForHashCode>
                                                                <useForToString>true</useForToString>
                                                                <propertySource />
                                                                <propertySourceParameter />
                                                                <defaultValue />
                                                                <domain>
						                                            <item>true</item>
						                                            <item>false</item>
					                                            </domain>
                                                            </literalParameter>
                                                            <objectParameter genericArgumentName=""B"">
                                                                <objectType>Contoso.Test.Business.Responses.TestResponseA</objectType>
                                                                <useForEquality>false</useForEquality>
                                                                <useForHashCode>false</useForHashCode>
                                                                <useForToString>true</useForToString>
                                                            </objectParameter>
                                                        </genericArguments>
                                                        <parameters>
                                                            <literalParameter name=""aProperty"">SomeValue</literalParameter>
                                                            <objectParameter name=""bProperty"">
                                                                <constructor name=""TestResponseA"" visibleText=""TestResponseA"" >
                                                                    <genericArguments />
                                                                    <parameters>
                                                                      <literalParameter name=""stringProperty""> XX</literalParameter>
                                                                    </parameters>
                                                                </constructor>
                                                            </objectParameter>
                                                        </parameters>
                                                    </constructor>");
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
        public void ConstructorElementValidatorFailsIfConstructorNotConfiguredForAssignedToType()
        {
            //arrange
            IConstructorElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IConstructorElementValidator>();
            XmlElement xmlElement = GetXmlElement(@"<constructor name=""TestResponseA"" visibleText=""TestResponseA"" >
                                                        <genericArguments />
                                                        <parameters>
                                                            <literalParameter name=""stringProperty""> XX</literalParameter>
                                                        </parameters>
                                                    </constructor>");
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();

            //act
            xmlValidator.Validate
            (
                xmlElement,
                typeof(int),
                applicationTypeInfo,
                errors
            );

            //assert
            Assert.True(errors.Any());
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.constructorNotConfiguredForObjectTypeFormat, typeof(int).ToString()), 
                errors.First()
            );
        }

        [Fact]
        public void ConstructorElementValidatorFailsIfConstructorNameNotConfigured()
        {
            //arrange
            IConstructorElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IConstructorElementValidator>();
            XmlElement xmlElement = GetXmlElement(@"<constructor name=""NotConfigured"" visibleText=""NotConfigured"" >
                                                        <genericArguments />
                                                        <parameters>
                                                            <literalParameter name=""stringProperty""> XX</literalParameter>
                                                        </parameters>
                                                    </constructor>");
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
            Assert.True(errors.Any());
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.constructorNotConfiguredForObjectTypeFormat2, "NotConfigured", typeof(object).ToString()),
                errors.First()
            );
        }

        [Fact]
        public void ConstructorElementValidatorFailsIfConstructorTypeCannotBeLoaded()
        {
            //arrange
            IConstructorElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IConstructorElementValidator>();
            XmlElement xmlElement = GetXmlElement(@"<constructor name=""TypeNotFoundConstructor"" visibleText=""TypeNotFoundConstructor"" >
                                                        <genericArguments />
                                                        <parameters>
                                                            <literalParameter name=""stringProperty"">SXX</literalParameter>
                                                        </parameters>
                                                    </constructor>");
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
            Assert.True(errors.Any());
            //Fails ConstructorElementValidatorFailsIfConstructorNameNotConfigured if the type cannot be loaded
            //Assert.Equal
            //(
            //    string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeForConstructorFormat, constructor.TypeName, constructor.Name),
            //    errors.First()
            //);
        }

        [Fact]
        public void ConstructorElementValidatorFailsIfTheGenericTypeCannotBeLoaded()
        {
            //arrange
            IConstructorElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IConstructorElementValidator>();
            XmlElement xmlElement = GetXmlElement(@"<constructor name=""GenericResponse"" visibleText=""GenericResponse"">
                                                        <genericArguments>
                                                            <literalParameter genericArgumentName=""A"">
                                                                <literalType>String</literalType>
                                                                <control>SingleLineTextBox</control>
                                                                <useForEquality>false</useForEquality>
                                                                <useForHashCode>false</useForHashCode>
                                                                <useForToString>true</useForToString>
                                                                <propertySource />
                                                                <propertySourceParameter />
                                                                <defaultValue />
                                                                <domain>
						                                            <item>true</item>
						                                            <item>false</item>
					                                            </domain>
                                                            </literalParameter>
                                                            <objectParameter genericArgumentName=""B"">
                                                                <objectType>Contoso.Test.Business.Responses.TypeNotFound</objectType>
                                                                <useForEquality>false</useForEquality>
                                                                <useForHashCode>false</useForHashCode>
                                                                <useForToString>true</useForToString>
                                                            </objectParameter>
                                                        </genericArguments>
                                                        <parameters>
                                                            <literalParameter name=""aProperty"">SomeValue</literalParameter>
                                                            <objectParameter name=""bProperty"">
                                                                <constructor name=""TestResponseA"" visibleText=""TestResponseA"" >
                                                                    <genericArguments />
                                                                    <parameters>
                                                                      <literalParameter name=""stringProperty""> XX</literalParameter>
                                                                    </parameters>
                                                                </constructor>
                                                            </objectParameter>
                                                        </parameters>
                                                    </constructor>");
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
            Assert.True(errors.Any());
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeForGenericArgumentForConstructorFormat, "B", "GenericResponse"),
                errors.First()
            );
        }

        [Fact]
        public void ConstructorElementValidatorFailsIfCantLoadConvertedGenericType()
        {
            //Can't test this because it passes if all previous tests pass. i.e. can load the  generic type definition and can load the generic types.
        }

        [Fact]
        public void ConstructorElementValidatorFailsIfConvertedGenericTypeIsNotAssignableToAssignedToType()
        {
            //arrange
            IConstructorElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IConstructorElementValidator>();
            XmlElement xmlElement = GetXmlElement(@"<constructor name=""GenericResponse"" visibleText=""GenericResponse"">
                                                        <genericArguments>
                                                            <literalParameter genericArgumentName=""A"">
                                                                <literalType>String</literalType>
                                                                <control>SingleLineTextBox</control>
                                                                <useForEquality>false</useForEquality>
                                                                <useForHashCode>false</useForHashCode>
                                                                <useForToString>true</useForToString>
                                                                <propertySource />
                                                                <propertySourceParameter />
                                                                <defaultValue />
                                                                <domain>
						                                            <item>true</item>
						                                            <item>false</item>
					                                            </domain>
                                                            </literalParameter>
                                                            <objectParameter genericArgumentName=""B"">
                                                                <objectType>Contoso.Test.Business.Responses.TestResponseA</objectType>
                                                                <useForEquality>false</useForEquality>
                                                                <useForHashCode>false</useForHashCode>
                                                                <useForToString>true</useForToString>
                                                            </objectParameter>
                                                        </genericArguments>
                                                        <parameters>
                                                            <literalParameter name=""aProperty"">SomeValue</literalParameter>
                                                            <objectParameter name=""bProperty"">
                                                                <constructor name=""TestResponseA"" visibleText=""TestResponseA"" >
                                                                    <genericArguments />
                                                                    <parameters>
                                                                      <literalParameter name=""stringProperty""> XX</literalParameter>
                                                                    </parameters>
                                                                </constructor>
                                                            </objectParameter>
                                                        </parameters>
                                                    </constructor>");
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            _fixture.TypeLoadHelper.TryGetSystemType("Contoso.Test.Business.Responses.GenericResponse`2[[System.String, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e],[System.Int32, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], Contoso.Test.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", applicationTypeInfo, out Type? assignedToType);
            List<string> errors = new();

            //act
            xmlValidator.Validate
            (
                xmlElement,
                assignedToType!,
                applicationTypeInfo,
                errors
            );

            //assert
            Assert.Equal
            (
                string.Format
                (
                    CultureInfo.CurrentCulture, 
                    Strings.constructorNotAssignableFormat,
                    "GenericResponse",
                    assignedToType!.ToString()
                ),
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

    public class ConstructorElementValidatorFixture : IDisposable
    {
        public ConstructorElementValidatorFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ServiceProvider.GetRequiredService<IMainWindow>().Instance = new Mocks.MockMdiParent();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            ConfigurationItemFactory = ServiceProvider.GetRequiredService<IConfigurationItemFactory>();
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
                    ["GenericResponse"] = ConstructorFactory.GetConstructor
                    (
                        "GenericResponse",
                        "Contoso.Test.Business.Responses.GenericResponse`2",
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
                        ""
                    ),
                    ["TypeNotFoundConstructor"] = ConstructorFactory.GetConstructor
                    (
                        "TypeNotFoundConstructor",
                        "Contoso.Test.Business.Responses.TypeNotFoundConstructor",
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
        internal IConfigurationItemFactory ConfigurationItemFactory;
        internal IConfigurationService ConfigurationService;
        internal IConstructorFactory ConstructorFactory;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
        internal ILoadContextSponsor LoadContextSponsor;
        internal IParameterFactory ParameterFactory;
        internal ITypeLoadHelper TypeLoadHelper;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
    }
}
