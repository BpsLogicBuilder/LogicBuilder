using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TelerikLogicBuilder.IntegrationTests.Constants;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class LiteralElementValidatorTest : IClassFixture<LiteralElementValidatorFixture>
    {
        private readonly LiteralElementValidatorFixture _fixture;

        public LiteralElementValidatorTest(LiteralElementValidatorFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateLiteralElementValidator()
        {
            //arrange
            ILiteralElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<ILiteralElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }

        [Fact]
        public void ValidateThrowsForInvalidElementType()
        {
            //arrange
            ILiteralElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<ILiteralElementValidator>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            XmlElement xmlElement = GetXmlElement(@"<someElement name=""stringProperty"">AAA</someElement>");
            List<string> errors = new();

            //act
            Assert.Throws<CriticalLogicBuilderException>(() => xmlValidator.Validate(xmlElement, typeof(string), applicationTypeInfo, errors));
        }

        [Theory]
        [InlineData(typeof(string), "yyy")]
        [InlineData(typeof(bool), "true")]
        [InlineData(typeof(DateTimeOffset), "2020-10-10 13:13:13 +01:00")]
        [InlineData(typeof(DateOnly), "2020-10-10")]
        [InlineData(typeof(DateTime), "2020-10-10 13:13:13")]
        [InlineData(typeof(Date), "2020-10-10")]
        [InlineData(typeof(TimeSpan), "13:13:13")]
        [InlineData(typeof(TimeOnly), "13:13:13")]
        [InlineData(typeof(TimeOfDay), "13:13:13")]
        [InlineData(typeof(Guid), "{2D64191A-C055-4E41-BF86-3781D775FA97}")]
        [InlineData(typeof(decimal), "3")]
        [InlineData(typeof(byte), "4")]
        [InlineData(typeof(short), "5")]
        [InlineData(typeof(int), "6")]
        [InlineData(typeof(long), "7")]
        [InlineData(typeof(float), "8")]
        [InlineData(typeof(double), "9")]
        [InlineData(typeof(char), "1")]
        [InlineData(typeof(sbyte), "2")]
        [InlineData(typeof(ushort), "3")]
        [InlineData(typeof(uint), "4")]
        [InlineData(typeof(ulong), "5")]
        [InlineData(typeof(bool?), "false")]
        [InlineData(typeof(DateTimeOffset?), "2020-10-10 13:13:13 +01:00")]
        [InlineData(typeof(DateOnly?), "2020-10-10")]
        [InlineData(typeof(DateTime?), "2020-10-10 13:13:13")]
        [InlineData(typeof(Date?), "2020-10-10")]
        [InlineData(typeof(TimeSpan?), "13:13:13")]
        [InlineData(typeof(TimeOnly?), "13:13:13")]
        [InlineData(typeof(TimeOfDay?), "13:13:13")]
        [InlineData(typeof(Guid?), "{2D64191A-C055-4E41-BF86-3781D775FA97}")]
        [InlineData(typeof(decimal?), "3")]
        [InlineData(typeof(byte?), "2")]
        [InlineData(typeof(short?), "3")]
        [InlineData(typeof(int?), "4")]
        [InlineData(typeof(long?), "5")]
        [InlineData(typeof(float?), "4")]
        [InlineData(typeof(double?), "3")]
        [InlineData(typeof(char?), "d")]
        [InlineData(typeof(sbyte?), "2")]
        [InlineData(typeof(ushort?), "99")]
        [InlineData(typeof(uint?), "99")]
        [InlineData(typeof(ulong?), "99")]
        public void SingleTextItemRSucceeds(Type assignedToType, string text)
        {
            //arrange
            ILiteralElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<ILiteralElementValidator>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            XmlElement xmlElement = GetXmlElement($"<literal>{text}</literal>");
            List<string> errors = new();

            //act
            xmlValidator.Validate(xmlElement, assignedToType, applicationTypeInfo, errors);

            //assert
            Assert.False(errors.Any());
        }

        [Theory]
        [InlineData(typeof(string), false)]
        [InlineData(typeof(bool), true)]
        [InlineData(typeof(DateTimeOffset), true)]
        [InlineData(typeof(DateOnly), true)]
        [InlineData(typeof(DateTime), true)]
        [InlineData(typeof(Date), true)]
        [InlineData(typeof(TimeSpan), true)]
        [InlineData(typeof(TimeOnly), true)]
        [InlineData(typeof(TimeOfDay), true)]
        [InlineData(typeof(Guid), true)]
        [InlineData(typeof(decimal), true)]
        [InlineData(typeof(byte), true)]
        [InlineData(typeof(short), true)]
        [InlineData(typeof(int), true)]
        [InlineData(typeof(long), true)]
        [InlineData(typeof(float), true)]
        [InlineData(typeof(double), true)]
        [InlineData(typeof(char), true)]
        [InlineData(typeof(sbyte), true)]
        [InlineData(typeof(ushort), true)]
        [InlineData(typeof(uint), true)]
        [InlineData(typeof(ulong), true)]
        [InlineData(typeof(bool?), true)]
        [InlineData(typeof(DateTimeOffset?), true)]
        [InlineData(typeof(DateOnly?), true)]
        [InlineData(typeof(DateTime?), true)]
        [InlineData(typeof(Date?), true)]
        [InlineData(typeof(TimeSpan?), true)]
        [InlineData(typeof(TimeOnly?), true)]
        [InlineData(typeof(TimeOfDay?), true)]
        [InlineData(typeof(Guid?), true)]
        [InlineData(typeof(decimal?), true)]
        [InlineData(typeof(byte?), true)]
        [InlineData(typeof(short?), true)]
        [InlineData(typeof(int?), true)]
        [InlineData(typeof(long?), true)]
        [InlineData(typeof(float?), true)]
        [InlineData(typeof(double?), true)]
        [InlineData(typeof(char?), true)]
        [InlineData(typeof(sbyte?), true)]
        [InlineData(typeof(ushort?), true)]
        [InlineData(typeof(uint?), true)]
        [InlineData(typeof(ulong?), true)]
        public void MixedXmlStringWithConstructorFunctionAndVariableReturnsTheExpectedResult(Type assignedToType, bool expectErrors)
        {
            //arrange
            ILiteralElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<ILiteralElementValidator>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            XmlElement xmlElement = GetXmlElement(@"<literal>AAA
                                                        <constructor name=""String"" visibleText=""String"" >
                                                            <genericArguments />
                                                            <parameters>
                                                                <literalListParameter name=""stringArray"">
                                                                    <literalList literalType=""String"" listType=""Array"" visibleText=""visibleText"">
                                                                      <literal>A</literal>
                                                                      <literal>B</literal>
                                                                    </literalList>
                                                                </literalListParameter>
                                                            </parameters>
                                                        </constructor>
                                                       BBB 
                                                        <function name=""GetString"" visibleText=""GetString"">
                                                            <genericArguments />
                                                            <parameters />
                                                        </function>
                                                        CCC
                                                        <variable name=""StringItem"" visibleText=""StringItem"" />
                                                    </literal>");
            List<string> errors = new();

            //act
            xmlValidator.Validate(xmlElement, assignedToType, applicationTypeInfo, errors);

            //assert
            Assert.Equal(expectErrors, errors.Any());
        }

        [Fact]
        public void SingleStringChildElementMatchingTheAssignedToTypeSucceeds()
        {
            //Single child element will be validated in the appropriate validator (constructor, function or variable)
        }

        [Theory]
        [InlineData(typeof(string), false)]
        [InlineData(typeof(bool), true)]
        [InlineData(typeof(DateTimeOffset), true)]
        [InlineData(typeof(DateOnly), true)]
        [InlineData(typeof(DateTime), true)]
        [InlineData(typeof(Date), true)]
        [InlineData(typeof(TimeSpan), true)]
        [InlineData(typeof(TimeOnly), true)]
        [InlineData(typeof(TimeOfDay), true)]
        [InlineData(typeof(Guid), true)]
        [InlineData(typeof(decimal), true)]
        [InlineData(typeof(byte), true)]
        [InlineData(typeof(short), true)]
        [InlineData(typeof(int), true)]
        [InlineData(typeof(long), true)]
        [InlineData(typeof(float), true)]
        [InlineData(typeof(double), true)]
        [InlineData(typeof(char), true)]
        [InlineData(typeof(sbyte), true)]
        [InlineData(typeof(ushort), true)]
        [InlineData(typeof(uint), true)]
        [InlineData(typeof(ulong), true)]
        [InlineData(typeof(bool?), false)]
        [InlineData(typeof(DateTimeOffset?), false)]
        [InlineData(typeof(DateOnly?), false)]
        [InlineData(typeof(DateTime?), false)]
        [InlineData(typeof(Date?), false)]
        [InlineData(typeof(TimeSpan?), false)]
        [InlineData(typeof(TimeOnly?), false)]
        [InlineData(typeof(TimeOfDay?), false)]
        [InlineData(typeof(Guid?), false)]
        [InlineData(typeof(decimal?), false)]
        [InlineData(typeof(byte?), false)]
        [InlineData(typeof(short?), false)]
        [InlineData(typeof(int?), false)]
        [InlineData(typeof(long?), false)]
        [InlineData(typeof(float?), false)]
        [InlineData(typeof(double?), false)]
        [InlineData(typeof(char?), false)]
        [InlineData(typeof(sbyte?), false)]
        [InlineData(typeof(ushort?), false)]
        [InlineData(typeof(uint?), false)]
        [InlineData(typeof(ulong?), false)]
        public void ElemenWithNoChildElemetsReturnsTheExpectedResult(Type assignedToType, bool expectErrors)
        {
            //arrange
            ILiteralElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<ILiteralElementValidator>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            XmlElement xmlElement = GetXmlElement(@"<literal></literal>");
            List<string> errors = new();

            //act
            xmlValidator.Validate(xmlElement, assignedToType, applicationTypeInfo, errors);

            //assert
            Assert.Equal(expectErrors, errors.Any());
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

    public class LiteralElementValidatorFixture : IDisposable
    {
        public LiteralElementValidatorFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ServiceProvider.GetRequiredService<IMainWindow>().Instance = new Mocks.MockMdiParent();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            ProjectPropertiesItemFactory = ServiceProvider.GetRequiredService<IProjectPropertiesItemFactory>();
			WebApiDeploymentItemFactory = ServiceProvider.GetRequiredService<IWebApiDeploymentItemFactory>();
            ConstructorFactory = ServiceProvider.GetRequiredService<IConstructorFactory>();
            AssemblyLoadContextService = ServiceProvider.GetRequiredService<IAssemblyLoadContextManager>();
            FunctionFactory = ServiceProvider.GetRequiredService<IFunctionFactory>();
            LoadContextSponsor = ServiceProvider.GetRequiredService<ILoadContextSponsor>();
            ParameterFactory = ServiceProvider.GetRequiredService<IParameterFactory>();
            ReturnTypeFactory = ServiceProvider.GetRequiredService<IReturnTypeFactory>();
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
                    ["String"] = ConstructorFactory.GetConstructor
                    (
                        "String",
                        "System.String",
                        new List<ParameterBase>
                        {
                            ParameterFactory.GetListOfLiteralsParameter
                            (
                                "stringArray",
                                false,
                                "",
                                LiteralParameterType.String,
                                ListType.Array,
                                ListParameterInputStyle.HashSetForm,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                "",
                                "",
                                new List<string>(),
                                new char[] { ',' },
                                new List<string>()
                            )
                        },
                        new List<string>(),
                        ""
                    )
                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>())
            );

            ConfigurationService.FunctionList = new FunctionList
            (
                new Dictionary<string, Function> 
                {
                    ["GetString"] = FunctionFactory.GetFunction
                    (
                        "GetString",
                        "GetString",
                        FunctionCategories.Standard,
                        "",
                        "flowManager.CustomActions",
                        "Field.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>(),
                        new List<string>(),
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.String),
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
                    ["StringItem"] = VariableFactory.GetLiteralVariable
                    (
                        "StringItem",
                        "StringItem",
                        VariableCategory.StringKeyIndexer,
                        "",
                        "",
                        "flowManager.FlowDataCache.Items",
                        "Field.Property.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        "",
                        LiteralVariableType.String,
                        LiteralVariableInputStyle.SingleLineTextBox,
                        "",
                        "",
                        new List<string>()
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
        internal IProjectPropertiesItemFactory ProjectPropertiesItemFactory;
		internal IWebApiDeploymentItemFactory WebApiDeploymentItemFactory;
        internal IConfigurationService ConfigurationService;
        internal IConstructorFactory ConstructorFactory;
        internal IFunctionFactory FunctionFactory;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
        internal ILoadContextSponsor LoadContextSponsor;
        internal IParameterFactory ParameterFactory;
        internal IReturnTypeFactory ReturnTypeFactory;
        internal ITypeLoadHelper TypeLoadHelper;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
        internal IVariableFactory VariableFactory;
    }
}
