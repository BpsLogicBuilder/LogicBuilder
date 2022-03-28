using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
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
            XmlElement xmlElement = GetXmlElement(@"<text name=""stringProperty"">AAA</text>");
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
                                                                <literalListParameter name=""charArray"">
                                                                    <literalList literalType=""Char"" listType=""Array"" visibleText=""visibleText"">
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
                    ),
                    ["TestResponseB"] = new Constructor
                    (
                        "TestResponseB",
                        "Contoso.Test.Business.Responses.TestResponseB",
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
                            ),
                            new LiteralParameter
                            (
                                "intProperty",
                                false,
                                "",
                                LiteralParameterType.Integer,
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
                    ),
                    ["TestResponseC"] = new Constructor
                    (
                        "TestResponseC",
                        "Contoso.Test.Business.Responses.TestResponseC",
                        new List<ParameterBase>
                        {
                            new ObjectParameter
                            (
                                "objectProperty",
                                false,
                                "",
                                "System.Object",
                                true,
                                false,
                                true,
                                ContextProvider
                            )
                        },
                        new List<string>(),
                        "",
                        ContextProvider
                    ),
                    ["GenericResponse"] = new Constructor
                    (
                        "GenericResponse",
                        "Contoso.Test.Business.Responses.GenericResponse`2",
                        new List<ParameterBase>
                        {
                            new GenericParameter
                            (
                                "aProperty",
                                false,
                                "",
                                "A",
                                ContextProvider
                            ),
                            new GenericParameter
                            (
                                "bProperty",
                                false,
                                "",
                                "B",
                                ContextProvider
                            )
                        },
                        new List<string> { "A", "B" },
                        "",
                        ContextProvider
                    ),
                    ["TypeNotFoundConstructor"] = new Constructor
                    (
                        "TypeNotFoundConstructor",
                        "Contoso.Test.Business.Responses.TypeNotFoundConstructor",
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
                    ),
                    ["String"] = new Constructor
                    (
                        "String",
                        "System.String",
                        new List<ParameterBase>
                        {
                            new ListOfLiteralsParameter
                            (
                                "charArray",
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
                    ["GetString"] = new Function
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

            ConfigurationService.VariableList = new VariableList
            (
                new Dictionary<string, VariableBase>
                {
                    ["StringItem"] = new LiteralVariable
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
                        new List<string>(),
                        ContextProvider
                    ),
                    ["IntItem"] = new LiteralVariable
                    (
                        "IntItem",
                        "IntItem",
                        VariableCategory.StringKeyIndexer,
                        "",
                        "",
                        "flowManager.FlowDataCache.Items",
                        "Field.Property.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        "",
                        LiteralVariableType.Integer,
                        LiteralVariableInputStyle.SingleLineTextBox,
                        "",
                        "",
                        new List<string>(),
                        ContextProvider
                    ),
                    ["NullableIntItem"] = new LiteralVariable
                    (
                        "NullableIntItem",
                        "NullableIntItem",
                        VariableCategory.StringKeyIndexer,
                        "",
                        "",
                        "flowManager.FlowDataCache.Items",
                        "Field.Property.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        "",
                        LiteralVariableType.NullableInteger,
                        LiteralVariableInputStyle.SingleLineTextBox,
                        "",
                        "",
                        new List<string>(),
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
