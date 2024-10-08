﻿using ABIS.LogicBuilder.FlowBuilder;
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
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using TelerikLogicBuilder.IntegrationTests.Constants;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class AssertFunctionElementValidatorTest : IClassFixture<AssertFunctionElementValidatorFixture>
    {
        private readonly AssertFunctionElementValidatorFixture _fixture;

        public AssertFunctionElementValidatorTest(AssertFunctionElementValidatorFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateAssertFunctionElementValidator()
        {
            //arrange
            IAssertFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IAssertFunctionElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }

        public static List<object[]> FunctionElements_Data
        {
            get
            {
                return new List<object[]>
                {
                    new object[]
                    {
                        GetXmlElement(@$"<assertFunction name=""Set Variable"" visibleText=""visibleText"">
                                            <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.String)}Item"" visibleText=""visibleText"" />
                                            <variableValue>
                                              <literalVariable>CB</literalVariable>
                                            </variableValue>
                                          </assertFunction>")
                    },
                    new object[]
                    {
                        GetXmlElement(@$"<assertFunction name=""Set Variable"" visibleText=""visibleText"">
                                            <variable name=""System_Object"" visibleText=""System_Object"" />
                                            <variableValue>
                                                <objectVariable>
                                                    <constructor name=""TestResponseA"" visibleText=""TestResponseA"" >
                                                        <genericArguments />
                                                        <parameters>
                                                            <literalParameter name=""stringProperty"">XX</literalParameter>
                                                        </parameters>
                                                    </constructor>
                                                </objectVariable>
                                            </variableValue>
                                          </assertFunction>")
                    },
                    new object[]
                    {
                        GetXmlElement(@$"<assertFunction name=""Set Variable"" visibleText=""visibleText"">
                                            <variable name=""LiteralListVariable"" visibleText=""LiteralListVariable"" />
                                            <variableValue>
                                                <literalListVariable>
                                                    <literalList literalType=""String"" listType=""GenericList"" visibleText=""visibleText"">
                                                        <literal>Field1</literal>
                                                        <literal>Field2</literal>
                                                    </literalList>
                                                </literalListVariable>
                                            </variableValue>
                                          </assertFunction>")
                    },
                    new object[]
                    {
                        GetXmlElement(@$"<assertFunction name=""Set Variable"" visibleText=""visibleText"">
                                            <variable name=""ObjectListVariable"" visibleText=""ObjectListVariable"" />
                                            <variableValue>
                                                <objectListVariable>
                                                    <objectList objectType=""Contoso.Test.Business.Responses.TestResponseA"" listType=""GenericList"" visibleText=""visibleText"">
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
                                                </objectListVariable>
                                            </variableValue>
                                          </assertFunction>")
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(FunctionElements_Data))]
        public void AssertFunctionElementValidatorSucceedsForValidElements(XmlElement functionElement)
        {
            //arrange
            IAssertFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IAssertFunctionElementValidator>();
            var application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();

            //act
            xmlValidator.Validate(functionElement, application, errors);

            //assert
            Assert.Empty(errors);
        }

        [Fact]
        public void AssertFunctionElementValidatorThrowsForInvalidFunctionElement()
        {
            //arrange
            IAssertFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IAssertFunctionElementValidator>();
            var application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            XmlElement functionElement = GetXmlElement(@$"<function name=""Set Variable"" visibleText=""visibleText"">
                                                            <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.String)}Item"" visibleText=""TR Returned Data"" />
                                                            <variableValue>
                                                              <literalVariable>CB</literalVariable>
                                                            </variableValue>
                                                          </function>");

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => xmlValidator.Validate(functionElement, application, errors));

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{CAB00483-7D5A-43D1-A2D5-D6CD6FFF0954}"), 
                exception.Message
            );
        }

        [Fact]
        public void AssertFunctionElementValidatorThrowsForInvalidVariableType()
        {
            //arrange
            //throws in call to _typeLoadHelper.TryGetSystemType(VariableBase, app, out Type);
            //Does not reach our check at {C7F29579-F6BF-46E2-A2D4-95D141A03A66}
        }

        [Fact]
        public void AssertFunctionElementValidatorFailsForFunctionNotConfigured()
        {
            //arrange
            IAssertFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IAssertFunctionElementValidator>();
            var application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            XmlElement functionElement = GetXmlElement(@$"<assertFunction name=""Set Variable Function Not Configured"" visibleText=""visibleText"">
                                                            <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.String)}Item"" visibleText=""visibleText"" />
                                                            <variableValue>
                                                              <literalVariable>CB</literalVariable>
                                                            </variableValue>
                                                          </assertFunction>");

            //act
            xmlValidator.Validate(functionElement, application, errors);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.functionNotConfiguredFormat, "Set Variable Function Not Configured"),
                errors.First()
            );
        }

        [Fact]
        public void AssertFunctionElementValidatorFailsForIncorrectFunctionCategory()
        {
            //arrange
            IAssertFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IAssertFunctionElementValidator>();
            var application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            XmlElement functionElement = GetXmlElement(@$"<assertFunction name=""Set Variable Wrong Category"" visibleText=""visibleText"">
                                                            <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.String)}Item"" visibleText=""visibleText"" />
                                                            <variableValue>
                                                              <literalVariable>CB</literalVariable>
                                                            </variableValue>
                                                          </assertFunction>");
            Function function = _fixture.ConfigurationService.FunctionList.Functions["Set Variable Wrong Category"];
            //act
            xmlValidator.Validate(functionElement, application, errors);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.invalidFunctionCategoryFormat, function.Name, Enum.GetName(typeof(FunctionCategories), function.FunctionCategory), functionElement.Name),
                errors.First()
            );
        }

        [Fact]
        public void AssertFunctionElementValidatorFailsForVariableNotConfigured()
        {
            //arrange
            IAssertFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IAssertFunctionElementValidator>();
            var application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            XmlElement functionElement = GetXmlElement(@$"<assertFunction name=""Set Variable"" visibleText=""visibleText"">
                                                            <variable name=""garbage"" visibleText=""visibleText"" />
                                                            <variableValue>
                                                              <literalVariable>CB</literalVariable>
                                                            </variableValue>
                                                          </assertFunction>");
            //act
            xmlValidator.Validate(functionElement, application, errors);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.cannotEvaluateVariableFormat, "garbage"),
                errors.First()
            );
        }

        [Fact]
        public void AssertFunctionElementValidatorFailsIfVariableTypeCannotBeLoaded()
        {
            //arrange
            IAssertFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IAssertFunctionElementValidator>();
            var application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            XmlElement functionElement = GetXmlElement(@$"<assertFunction name=""Set Variable"" visibleText=""visibleText"">
                                                            <variable name=""VariableTypeNotFound"" visibleText=""visibleText"" />
                                                            <variableValue>
                                                                <objectVariable>
                                                                    <constructor name=""TestResponseA"" visibleText=""TestResponseA"" >
                                                                        <genericArguments />
                                                                        <parameters>
                                                                            <literalParameter name=""stringProperty"">XX</literalParameter>
                                                                        </parameters>
                                                                    </constructor>
                                                                </objectVariable>
                                                            </variableValue>
                                                          </assertFunction>");
            VariableBase variable = _fixture.ConfigurationService.VariableList.Variables["VariableTypeNotFound"];
            //act
            xmlValidator.Validate(functionElement, application, errors);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeForVariableFormat, variable.ObjectTypeString, variable.Name),
                errors.First()
            );
        }

        [Fact]
        public void AssertFunctionElementValidatorFailsIfConfiguredVariableTypeDoesnOtMatchTheElementType()
        {
            //arrange
            IAssertFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IAssertFunctionElementValidator>();
            var application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            XmlElement functionElement = GetXmlElement(@$"<assertFunction name=""Set Variable"" visibleText=""visibleText"">
                                                            <variable name=""System_Object"" visibleText=""visibleText"" />
                                                            <variableValue>
                                                              <literalVariable>CB</literalVariable>
                                                            </variableValue>
                                                          </assertFunction>");
            IEnumHelper enumHelper = _fixture.ServiceProvider.GetRequiredService<IEnumHelper>();
            VariableBase variable = _fixture.ConfigurationService.VariableList.Variables["System_Object"];

            //act
            xmlValidator.Validate(functionElement, application, errors);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.invalidVariableElementFormat, variable.Name, enumHelper.GetVisibleEnumText(variable.VariableTypeCategory)),
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

    public class AssertFunctionElementValidatorFixture : IDisposable
    {
        public AssertFunctionElementValidatorFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ServiceProvider.GetRequiredService<IMainWindow>().Instance = new Mocks.MockMdiParent();
            ProjectPropertiesItemFactory = ServiceProvider.GetRequiredService<IProjectPropertiesItemFactory>();
			WebApiDeploymentItemFactory = ServiceProvider.GetRequiredService<IWebApiDeploymentItemFactory>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            ConstructorFactory = ServiceProvider.GetRequiredService<IConstructorFactory>();
            EnumHelper = ServiceProvider.GetRequiredService<IEnumHelper>();
            TypeHelper = ServiceProvider.GetRequiredService<ITypeHelper>();
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
                    )
                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>())
            );

            ConfigurationService.FunctionList = new FunctionList
            (
                new Dictionary<string, Function>
                {
                    ["Set Variable"] = FunctionFactory.GetFunction
                    (
                        "Set Variable",
                        "assert",
                        FunctionCategories.Assert,
                        "",
                        "",
                        "",
                        "",
                        ReferenceCategories.None,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>(),
                        new List<string> { "A", "B" },
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Void),
                        ""
                    ),
                    ["Set Variable Wrong Category"] = FunctionFactory.GetFunction
                    (
                        "Set Variable Wrong Category",
                        "StaticMethod",
                        FunctionCategories.Standard,
                        "Contoso.Test.Business.StaticGenericClass`2",
                        "",
                        "",
                        "",
                        ReferenceCategories.Type,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>(),
                        new List<string> { "A", "B" },
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Boolean),
                        ""
                    )
                },
                new Dictionary<string, Function>(),
                new Dictionary<string, Function>(),
                new Dictionary<string, Function>(),
                new Dictionary<string, Function>(),
                new Dictionary<string, Function>(),
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
                    ["System_Object"] = VariableFactory.GetObjectVariable
                    (
                        "System_Object",
                        "System_Object",
                        VariableCategory.StringKeyIndexer,
                        "System.Object",
                        "",
                        "flowManager.FlowDataCache.Items",
                        "Field.Property.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        "",
                        "System.Object"
                    ),
                    ["VariableTypeNotFound"] = VariableFactory.GetObjectVariable
                    (
                        "VariableTypeNotFound",
                        "VariableTypeNotFound",
                        VariableCategory.StringKeyIndexer,
                        "",
                        "",
                        "flowManager.FlowDataCache.Items",
                        "Field.Property.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        "",
                        "VariableTypeNotFound"
                    ),
                    ["LiteralListVariable"] = VariableFactory.GetListOfLiteralsVariable
                    (
                        "LiteralListVariable",
                        "LiteralListVariable",
                        VariableCategory.StringKeyIndexer,
                        "System.Collections.Generic.IList`1[[System.String, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e",
                        "",
                        "flowManager.FlowDataCache.Items",
                        "Field.Property.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        "",
                        LiteralVariableType.String,
                        ListType.GenericList,
                        ListVariableInputStyle.ListForm,
                        LiteralVariableInputStyle.SingleLineTextBox,
                        "",
                        new List<string>(),
                        new List<string>()
                    ),
                    ["ObjectListVariable"] = VariableFactory.GetListOfObjectsVariable
                    (
                        "ObjectListVariable",
                        "ObjectListVariable",
                        VariableCategory.StringKeyIndexer,
                        "System.Collections.Generic.IList`1[[Contoso.Test.Business.Responses.TestResponseA, Contoso.Test.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e",
                        "",
                        "flowManager.FlowDataCache.Items",
                        "Field.Property.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        "",
                        "Contoso.Test.Business.Responses.TestResponseA",
                        ListType.GenericList,
                        ListVariableInputStyle.ListForm
                    ),
                    ["InvalidVariableType"] = new InvalidVariableType
                    (
                        ServiceProvider.GetRequiredService<IEnumHelper>(),
                        ServiceProvider.GetRequiredService<IStringHelper>()
                    )
                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>())
            );


            foreach (LiteralVariableType enumValue in Enum.GetValues<LiteralVariableType>())
            {
                string variableName = $"{Enum.GetName(typeof(LiteralVariableType), enumValue)}Item";
                ConfigurationService.VariableList.Variables.Add(variableName, GetLiteralVariable(variableName, enumValue));
            }

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
        }

        internal IServiceProvider ServiceProvider;
        internal IProjectPropertiesItemFactory ProjectPropertiesItemFactory;
		internal IWebApiDeploymentItemFactory WebApiDeploymentItemFactory;
        internal IConfigurationService ConfigurationService;
        internal IConstructorFactory ConstructorFactory;
        internal IEnumHelper EnumHelper;
        internal ITypeHelper TypeHelper;
        internal IFunctionFactory FunctionFactory;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
        internal ILoadContextSponsor LoadContextSponsor;
        internal IParameterFactory ParameterFactory;
        internal IReturnTypeFactory ReturnTypeFactory;
        internal ITypeLoadHelper TypeLoadHelper;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
        internal IVariableFactory VariableFactory;
    }

    class InvalidVariableType : VariableBase
    {
        public InvalidVariableType(IEnumHelper enumHelper, IStringHelper stringHelper)
            : base
            (
                enumHelper, 
                stringHelper,
                "name",
                "memberName",
                VariableCategory.Field,
                "~",
                "",
                "referenceName",
                "referenceDefinition",
                "~",
                ReferenceCategories.InstanceReference,
                "comments"
            )
        {
        }

        internal override string ToXml => throw new NotImplementedException();

        internal override VariableTypeCategory VariableTypeCategory => throw new NotImplementedException();

        internal override string ObjectTypeString => throw new NotImplementedException();
    }
}
